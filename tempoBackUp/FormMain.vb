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
        _configPath = Path.Combine(AppContext.BaseDirectory, "config.json")
        AddHandler _runner.OutputReceived, AddressOf OnRunnerOutputReceived
        AddHandler _runner.StatusChanged, AddressOf OnRunnerStatusChanged
        AddHandler btnRunBackup.Click, AddressOf btnRunBackup_Click
        AddHandler btnSimulate.Click, AddressOf btnSimulate_Click
        AddHandler btnStop.Click, AddressOf btnStop_Click
        AddHandler btnReloadConfig.Click, AddressOf btnReloadConfig_Click
        AddHandler Me.FormClosing, AddressOf FormMain_FormClosing
        LoadApplicationIcon()
        LoadConfiguration()
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
        SetButtonsEnabled(canRun:=False, running:=True)
        _runCts = New CancellationTokenSource()

        Try
            Dim modeLabel = If(simulation, "simulazione", "backup")
            lblStatus.Text = $"Avvio {modeLabel}..."
            AppendOutput($"Avvio {modeLabel} alle {DateTime.Now:HH:mm:ss}")

            Dim summary = Await _runner.RunAsync(_config, simulation, _runCts.Token)
            lblStatus.Text = summary.Outcome.ToString()

            For Each message In summary.Messages
                AppendOutput(message)
            Next

            If Not String.IsNullOrWhiteSpace(summary.LogFilePath) Then
                AppendOutput($"Log salvato in: {summary.LogFilePath}")
            End If

            If summary.Cancelled Then
                AppendOutput("Operazione interrotta.")
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

        _runCts?.Cancel()
        lblStatus.Text = "Interruzione in corso..."
        AppendOutput("Richiesta di interruzione inviata.")

        If _runner.RequestStop() Then
            AppendOutput("Processo Robocopy terminato.")
        End If
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
        btnReloadConfig.Enabled = Not running
        btnStop.Enabled = running
    End Sub
End Class
