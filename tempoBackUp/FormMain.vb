Imports System.IO
Imports System.Threading

Public Class FormMain
    Private ReadOnly _runner As New BackupRunner()
    Private _config As BackupConfig
    Private _configPath As String
    Private _runCts As CancellationTokenSource
    Private _isRunning As Boolean

    Public Sub New()
        InitializeComponent()
        _configPath = ConfigStore.GetUserConfigPath()
        AddHandler _runner.OutputReceived, AddressOf OnRunnerOutputReceived
        AddHandler _runner.StatusChanged, AddressOf OnRunnerStatusChanged
        AddHandler _runner.ProgressChanged, AddressOf OnRunnerProgressChanged
        AddHandler btnRunBackup.Click, AddressOf btnRunBackup_Click
        AddHandler btnSimulate.Click, AddressOf btnSimulate_Click
        AddHandler btnStop.Click, AddressOf btnStop_Click
        AddHandler btnReloadConfig.Click, AddressOf btnReloadConfig_Click
        AddHandler btnEditConfig.Click, AddressOf btnEditConfig_Click
        AddHandler Me.FormClosing, AddressOf FormMain_FormClosing
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
        SetButtonsEnabled(canRun:=False, running:=True)
        _runCts = New CancellationTokenSource()

        Try
            Dim modeLabel = If(simulation, "simulazione", "backup")
            lblStatus.Text = $"Avvio {modeLabel}..."
            AppendOutput($"Avvio {modeLabel} alle {DateTime.Now:HH:mm:ss}")

            Dim summary = Await _runner.RunAsync(_config, simulation, _runCts.Token).ConfigureAwait(True)

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
        Catch ex As OperationCanceledException
            lblStatus.Text = BackupOutcome.Cancelled.ToString()
            AppendOutput("Operazione annullata.")
        Catch ex As Exception
            lblStatus.Text = BackupOutcome.Failure.ToString()
            AppendOutput($"Errore applicativo: {ex.Message}")
        Finally
            _isRunning = False
            _runCts?.Dispose()
            _runCts = Nothing
            SetButtonsEnabled(canRun:=_config IsNot Nothing, running:=False)
            If progressBarWork.Style = ProgressBarStyle.Marquee Then
                progressBarWork.Style = ProgressBarStyle.Continuous
            End If
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

    Private Sub OnRunnerOutputReceived(sender As Object, message As String)
        AppendOutput(message)
    End Sub

    Private Sub OnRunnerStatusChanged(sender As Object, message As String)
        If InvokeRequired Then
            BeginInvoke(New Action(Of String)(AddressOf UpdateStatus), message)
        Else
            UpdateStatus(message)
        End If
    End Sub

    Private Sub OnRunnerProgressChanged(sender As Object, e As BackupProgressEventArgs)
        If InvokeRequired Then
            BeginInvoke(New Action(Of BackupProgressEventArgs)(AddressOf UpdateProgressUi), e)
        Else
            UpdateProgressUi(e)
        End If
    End Sub

    Private Sub ResetProgressUi()
        progressBarWork.Style = ProgressBarStyle.Continuous
        progressBarWork.Minimum = 0
        progressBarWork.Maximum = 100
        progressBarWork.Value = 0
        lblProgressDetail.Text = "In attesa"
    End Sub

    Private Sub UpdateProgressUi(progress As BackupProgressEventArgs)
        lblProgressDetail.Text = progress.Message

        If progress.IsFolderActive Then
            progressBarWork.Style = ProgressBarStyle.Continuous
            progressBarWork.Value = Math.Max(progressBarWork.Minimum, Math.Min(progress.PercentComplete, progressBarWork.Maximum))
            progressBarWork.Style = ProgressBarStyle.Marquee
            Return
        End If

        progressBarWork.Style = ProgressBarStyle.Continuous
        progressBarWork.Value = Math.Max(progressBarWork.Minimum, Math.Min(progress.PercentComplete, progressBarWork.Maximum))
    End Sub

    Private Sub UpdateStatus(message As String)
        lblStatus.Text = message
    End Sub

    Private Sub AppendOutput(message As String)
        If InvokeRequired Then
            BeginInvoke(New Action(Of String)(AddressOf AppendOutput), message)
            Return
        End If

        txtOutput.AppendText(message & Environment.NewLine)
    End Sub

    Private Sub SetButtonsEnabled(canRun As Boolean, Optional running As Boolean = False)
        btnRunBackup.Enabled = canRun AndAlso Not running
        btnSimulate.Enabled = canRun AndAlso Not running
        btnEditConfig.Enabled = Not running
        btnReloadConfig.Enabled = Not running
        btnStop.Enabled = running
    End Sub
End Class
