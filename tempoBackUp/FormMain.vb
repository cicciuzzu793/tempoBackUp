Imports System.IO
Imports System.Threading

Public Class FormMain
    Private Const MaxOutputCharacters As Integer = 200000
    Private Const OutputTrimToCharacters As Integer = 150000
    Private Const OutputFlushIntervalMs As Integer = 150

    Private ReadOnly _runner As New BackupRunner()
    Private ReadOnly _outputBuffer As New List(Of String)()
    Private ReadOnly _outputBufferLock As New Object()
    Private _config As BackupConfig
    Private _configPath As String
    Private _runCts As CancellationTokenSource
    Private _isRunning As Boolean
    Private _outputFlushTimer As System.Windows.Forms.Timer

    Public Sub New()
        InitializeComponent()
        _configPath = ConfigStore.GetUserConfigPath()
        _outputFlushTimer = New System.Windows.Forms.Timer With {.Interval = OutputFlushIntervalMs}
        AddHandler _outputFlushTimer.Tick, AddressOf OnOutputFlushTimerTick
        AddHandler _runner.OutputReceived, AddressOf OnRunnerOutputReceived
        AddHandler _runner.StatusChanged, AddressOf OnRunnerStatusChanged
        AddHandler _runner.ProgressChanged, AddressOf OnRunnerProgressChanged
        AddHandler btnRunBackup.Click, AddressOf btnRunBackup_Click
        AddHandler btnSimulate.Click, AddressOf btnSimulate_Click
        AddHandler btnStop.Click, AddressOf btnStop_Click
        AddHandler btnReloadConfig.Click, AddressOf btnReloadConfig_Click
        AddHandler btnEditConfig.Click, AddressOf btnEditConfig_Click
        AddHandler Me.FormClosing, AddressOf FormMain_FormClosing
        AddHandler Me.FormClosed, AddressOf FormMain_FormClosed
        LoadApplicationIcon()
        ApplyVersionLabels()
        ResetProgressUi()
        LoadConfiguration()
    End Sub

    Private Sub ApplyVersionLabels()
        lblTitle.Text = $"tempoBackUp  {AppVersion.DisplayVersion}"
        Text = $"tempoBackUp {AppVersion.DisplayVersion} - Backup manuale"
    End Sub

    Private Sub LoadApplicationIcon()
        Try
            Me.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
        Catch
        End Try
    End Sub

    Private Sub LoadConfiguration()
        Try
            If Not File.Exists(_configPath) Then
                Throw New FileNotFoundException("File config.json non trovato.", _configPath)
            End If

            _config = BackupConfig.LoadFromFile(_configPath)
            txtSource.Text = _config.SourceRoot
            txtDestination.Text = _config.DestinationRoot
            txtLogFolder.Text = _config.LogFolder
            lblStatus.Text = "Configurazione caricata"
            AppendOutput($"Configurazione caricata da: {_configPath}")
        Catch ex As Exception
            lblStatus.Text = "Errore configurazione"
            AppendOutput($"Errore caricamento configurazione: {ex.Message}")
            SetButtonsEnabled(canRun:=False)
        End Try
    End Sub

    Private Async Sub btnRunBackup_Click(sender As Object, e As EventArgs)
        Await StartRunAsync(simulation:=False)
    End Sub

    Private Async Sub btnSimulate_Click(sender As Object, e As EventArgs)
        Await StartRunAsync(simulation:=True)
    End Sub

    Private Async Function StartRunAsync(simulation As Boolean) As Task
        If _isRunning OrElse _config Is Nothing Then
            Return
        End If

        txtOutput.Clear()
        _isRunning = True
        ResetProgressUi()
        SetInteractiveControlsDuringRun(running:=True)
        SetButtonsEnabled(canRun:=False, running:=True)
        _runCts = New CancellationTokenSource()

        Try
            Dim modeLabel = If(simulation, "simulazione", "backup")
            lblStatus.Text = $"Avvio {modeLabel}..."
            AppendOutput($"Avvio {modeLabel} alle {DateTime.Now:HH:mm:ss}")

            Dim summary = Await _runner.RunAsync(_config, simulation, _runCts.Token).ConfigureAwait(True)

            FlushOutputBuffer(force:=True)

            If _runCts.IsCancellationRequested OrElse summary.Cancelled Then
                lblStatus.Text = BackupOutcome.Cancelled.ToString()
                AppendOutput("Operazione interrotta.")
            Else
                lblStatus.Text = summary.Outcome.ToString()
            End If

            For Each message In summary.Messages
                AppendOutput(message)
            Next

            If Not String.IsNullOrWhiteSpace(summary.LogFilePath) Then
                AppendOutput($"Log salvato in: {summary.LogFilePath}")
            End If

            FlushOutputBuffer(force:=True)
            SetProgressBarValue(100)
            lblProgressDetail.Text = "Operazione completata."
        Catch ex As OperationCanceledException
            lblStatus.Text = BackupOutcome.Cancelled.ToString()
            AppendOutput("Operazione annullata.")
            FlushOutputBuffer(force:=True)
        Catch ex As Exception
            lblStatus.Text = BackupOutcome.Failure.ToString()
            AppendOutput($"Errore applicativo: {ex.Message}")
            FlushOutputBuffer(force:=True)
        Finally
            _isRunning = False
            _runCts?.Dispose()
            _runCts = Nothing
            FlushOutputBuffer(force:=True)
            SetButtonsEnabled(canRun:=_config IsNot Nothing, running:=False)
            SetInteractiveControlsDuringRun(running:=False)
        End Try
    End Function

    Private Sub btnStop_Click(sender As Object, e As EventArgs)
        If Not _isRunning Then
            Return
        End If

        Dim confirm = MessageBox.Show(
            "Interrompere il processo Robocopy avviato da tempoBackUp?",
            "Conferma interruzione",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2)

        If confirm <> DialogResult.Yes Then
            Return
        End If

        _runner.RequestStop()
        _runCts?.Cancel()
        lblStatus.Text = "Interruzione in corso..."
        AppendOutput("Richiesta di interruzione inviata.")
    End Sub

    Private Sub btnEditConfig_Click(sender As Object, e As EventArgs)
        If _isRunning Then
            MessageBox.Show("Impossibile modificare la configurazione durante un'operazione in corso.", "tempoBackUp", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If _config Is Nothing Then
            MessageBox.Show("Configurazione non caricata.", "tempoBackUp", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using editor As New FormConfig(_configPath, _config.Clone())
            If editor.ShowDialog(Me) = DialogResult.OK Then
                LoadConfiguration()
                AppendOutput("Configurazione salvata da Impostazioni.")
            End If
        End Using
    End Sub

    Private Sub btnReloadConfig_Click(sender As Object, e As EventArgs)
        If _isRunning Then
            MessageBox.Show("Impossibile ricaricare la configurazione durante un'operazione in corso.", "tempoBackUp", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        LoadConfiguration()
    End Sub

    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs)
        FlushOutputBuffer(force:=True)

        If Not _isRunning Then
            Return
        End If

        Dim confirm = MessageBox.Show(
            "Un'operazione è ancora in corso. Uscire comunque?",
            "Conferma chiusura",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2)

        If confirm <> DialogResult.Yes Then
            e.Cancel = True
            Return
        End If

        _runCts?.Cancel()
        _runner.RequestStop()
    End Sub

    Private Sub FormMain_FormClosed(sender As Object, e As FormClosedEventArgs)
        _outputFlushTimer.Stop()
        _outputFlushTimer.Dispose()
    End Sub

    Private Sub OnRunnerOutputReceived(sender As Object, message As String)
        QueueOutput(message)
    End Sub

    Private Sub OnRunnerStatusChanged(sender As Object, message As String)
        If Not CanUpdateUi() Then
            Return
        End If

        If InvokeRequired Then
            BeginInvoke(New Action(Of String)(AddressOf UpdateStatus), message)
        Else
            UpdateStatus(message)
        End If
    End Sub

    Private Sub OnRunnerProgressChanged(sender As Object, e As BackupProgressEventArgs)
        If Not CanUpdateUi() Then
            Return
        End If

        Dim progressSnapshot As New BackupProgressEventArgs With {
            .PercentComplete = e.PercentComplete,
            .Message = e.Message,
            .IsFolderActive = e.IsFolderActive
        }

        If InvokeRequired Then
            BeginInvoke(New Action(Of BackupProgressEventArgs)(AddressOf UpdateProgressUi), progressSnapshot)
        Else
            UpdateProgressUi(progressSnapshot)
        End If
    End Sub

    Private Sub OnOutputFlushTimerTick(sender As Object, e As EventArgs)
        FlushOutputBuffer(force:=False)
    End Sub

    Private Function CanUpdateUi() As Boolean
        Return Not IsDisposed AndAlso IsHandleCreated
    End Function

    Private Sub SetInteractiveControlsDuringRun(running As Boolean)
        txtOutput.TabStop = Not running
        progressBarWork.TabStop = False
    End Sub

    Private Sub ResetProgressUi()
        progressBarWork.Style = ProgressBarStyle.Continuous
        progressBarWork.Minimum = 0
        progressBarWork.Maximum = 100
        progressBarWork.Value = 0
        lblProgressDetail.Text = "In attesa"
    End Sub

    Private Sub UpdateProgressUi(progress As BackupProgressEventArgs)
        If Not CanUpdateUi() Then
            Return
        End If

        lblProgressDetail.Text = progress.Message
        SetProgressBarValue(progress.PercentComplete)
    End Sub

    Private Sub SetProgressBarValue(value As Integer)
        progressBarWork.Style = ProgressBarStyle.Continuous
        Dim clamped = Math.Max(progressBarWork.Minimum, Math.Min(value, progressBarWork.Maximum))
        If progressBarWork.Value <> clamped Then
            progressBarWork.Value = clamped
        End If
    End Sub

    Private Sub UpdateStatus(message As String)
        lblStatus.Text = message
    End Sub

    Private Sub QueueOutput(message As String)
        SyncLock _outputBufferLock
            _outputBuffer.Add(message)
            If CanUpdateUi() AndAlso Not _outputFlushTimer.Enabled Then
                _outputFlushTimer.Start()
            End If
        End SyncLock
    End Sub

    Private Sub FlushOutputBuffer(force As Boolean)
        Dim batch As List(Of String) = Nothing

        SyncLock _outputBufferLock
            If _outputBuffer.Count = 0 Then
                If force Then
                    _outputFlushTimer.Stop()
                End If
                Return
            End If

            batch = New List(Of String)(_outputBuffer)
            _outputBuffer.Clear()
        End SyncLock

        If Not CanUpdateUi() Then
            Return
        End If

        If InvokeRequired Then
            BeginInvoke(New Action(Of List(Of String))(AddressOf ApplyOutputBatch), batch)
        Else
            ApplyOutputBatch(batch)
        End If

        If force Then
            _outputFlushTimer.Stop()
        End If
    End Sub

    Private Sub ApplyOutputBatch(lines As List(Of String))
        If Not CanUpdateUi() OrElse lines.Count = 0 Then
            Return
        End If

        txtOutput.AppendText(String.Join(Environment.NewLine, lines) & Environment.NewLine)

        If txtOutput.TextLength > MaxOutputCharacters Then
            txtOutput.Text = txtOutput.Text.Substring(txtOutput.TextLength - OutputTrimToCharacters)
        End If
    End Sub

    Private Sub AppendOutput(message As String)
        If Not CanUpdateUi() Then
            Return
        End If

        If InvokeRequired Then
            BeginInvoke(New Action(Of String)(AddressOf AppendOutput), message)
            Return
        End If

        ApplyOutputBatch(New List(Of String) From {message})
    End Sub

    Private Sub SetButtonsEnabled(canRun As Boolean, Optional running As Boolean = False)
        btnRunBackup.Enabled = canRun AndAlso Not running
        btnSimulate.Enabled = canRun AndAlso Not running
        btnEditConfig.Enabled = Not running
        btnReloadConfig.Enabled = Not running
        btnStop.Enabled = running
    End Sub
End Class
