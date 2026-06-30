Imports System.IO
Imports System.Text
Imports System.Threading

Public Class BackupRunSummary
    Public Property Outcome As BackupOutcome = BackupOutcome.Success
    Public Property LogFilePath As String = String.Empty
    Public Property Cancelled As Boolean
    Public Property Messages As New List(Of String)()
End Class

Public Class BackupRunner
    Private ReadOnly _commandBuilder As New RobocopyCommandBuilder()
    Private _currentProcess As Process
    Private _cancellationRequested As Boolean

    Public Event OutputReceived As EventHandler(Of String)
    Public Event StatusChanged As EventHandler(Of String)

    Public ReadOnly Property IsRunning As Boolean
        Get
            Return _currentProcess IsNot Nothing AndAlso Not _currentProcess.HasExited
        End Get
    End Property

    Public Async Function RunAsync(config As BackupConfig, simulation As Boolean, cancellationToken As CancellationToken) As Task(Of BackupRunSummary)
        _cancellationRequested = False
        Dim summary As New BackupRunSummary()
        Dim validation = PathValidator.ValidateConfiguration(config, simulation)

        If Not validation.IsValid Then
            summary.Outcome = BackupOutcome.Failure
            summary.Messages.Add(validation.ErrorMessage)
            Return summary
        End If

        Dim logPrefix = If(simulation, "simulation", "backup")
        Dim logFileName = $"{logPrefix}_{DateTime.Now:yyyy-MM-dd_HHmmss}.log"
        Dim logFilePath = Path.Combine(PathValidator.NormalizePath(config.LogFolder), logFileName)
        summary.LogFilePath = logFilePath

        Using logWriter As New StreamWriter(logFilePath, append:=False, Encoding.UTF8)
            Await WriteLogLineAsync(logWriter, $"=== tempoBackUp {If(simulation, "simulazione", "backup")} ===")
            Await WriteLogLineAsync(logWriter, $"Avviato: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
            Await WriteLogLineAsync(logWriter, "Configurazione:")
            Await WriteLogLineAsync(logWriter, config.ToLogSummary())
            Await WriteLogLineAsync(logWriter, String.Empty)

            Dim sourceRoot = PathValidator.NormalizePath(config.SourceRoot)
            Dim destinationRoot = PathValidator.NormalizePath(config.DestinationRoot)
            Dim foldersToProcess = BuildFolderList(config, sourceRoot)
            Dim overallOutcome = BackupOutcome.NoChanges

            For Each folder In foldersToProcess
                If IsCancellationRequested(cancellationToken) Then
                    summary.Cancelled = True
                    summary.Outcome = BackupOutcome.Cancelled
                    Await WriteLogLineAsync(logWriter, "Operazione interrotta dall'utente.")
                    Exit For
                End If

                Dim sourcePath = Path.Combine(sourceRoot, folder)
                Dim destinationPath = Path.Combine(destinationRoot, folder)

                If Not Directory.Exists(sourcePath) Then
                    Dim skipMessage = $"Cartella sorgente assente, salto: {sourcePath}"
                    summary.Messages.Add(skipMessage)
                    RaiseOutput(skipMessage)
                    Await WriteLogLineAsync(logWriter, skipMessage)
                    Continue For
                End If

                Directory.CreateDirectory(destinationPath)

                Dim args = _commandBuilder.BuildArgumentList(sourcePath, destinationPath, config, simulation)
                Dim commandLine = _commandBuilder.BuildCommandLineForLog(args)

                RaiseStatus($"Esecuzione: {folder}")
                RaiseOutput(commandLine)
                Await WriteLogLineAsync(logWriter, $"--- Cartella: {folder} ---")
                Await WriteLogLineAsync(logWriter, commandLine)

                Dim folderResult = Await RunSingleRobocopyAsync(args, logWriter, cancellationToken)
                summary.Messages.Add($"{folder}: {folderResult.Message}")
                overallOutcome = MergeOutcome(overallOutcome, folderResult.Outcome)

                If folderResult.Outcome = BackupOutcome.Cancelled Then
                    summary.Cancelled = True
                    summary.Outcome = BackupOutcome.Cancelled
                    Exit For
                End If

                If IsCancellationRequested(cancellationToken) Then
                    summary.Cancelled = True
                    summary.Outcome = BackupOutcome.Cancelled
                    Await WriteLogLineAsync(logWriter, "Operazione interrotta dall'utente.")
                    Exit For
                End If

                If folderResult.Outcome = BackupOutcome.Failure OrElse folderResult.Outcome = BackupOutcome.FatalFailure Then
                    Exit For
                End If
            Next

            If summary.Cancelled Then
                overallOutcome = BackupOutcome.Cancelled
            End If

            summary.Outcome = overallOutcome
            Await WriteLogLineAsync(logWriter, String.Empty)
            Await WriteLogLineAsync(logWriter, $"Risultato finale: {summary.Outcome}")
            Await WriteLogLineAsync(logWriter, $"Terminato: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
        End Using

        Return summary
    End Function

    Public Function RequestStop() As Boolean
        _cancellationRequested = True

        If _currentProcess Is Nothing OrElse _currentProcess.HasExited Then
            Return False
        End If

        Try
            _currentProcess.Kill(entireProcessTree:=True)
            Return True
        Catch ex As Exception
            RaiseOutput($"Impossibile interrompere Robocopy: {ex.Message}")
            Return False
        End Try
    End Function

    Private Sub KillCurrentProcessIfRunning()
        If _currentProcess Is Nothing OrElse _currentProcess.HasExited Then
            Return
        End If

        Try
            _currentProcess.Kill(entireProcessTree:=True)
        Catch
        End Try
    End Sub

    Private Function IsCancellationRequested(cancellationToken As CancellationToken) As Boolean
        Return _cancellationRequested OrElse cancellationToken.IsCancellationRequested
    End Function

    Private Async Function RunSingleRobocopyAsync(args As List(Of String), logWriter As StreamWriter, cancellationToken As CancellationToken) As Task(Of RobocopyResult)
        Dim robocopyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "robocopy.exe")
        Dim outputBuilder As New StringBuilder()
        Dim errorBuilder As New StringBuilder()

        Using process As New Process()
            _currentProcess = process
            process.StartInfo.FileName = robocopyPath
            process.StartInfo.UseShellExecute = False
            process.StartInfo.RedirectStandardOutput = True
            process.StartInfo.RedirectStandardError = True
            process.StartInfo.CreateNoWindow = True
            process.StartInfo.ArgumentList.Clear()

            For Each argument In args
                process.StartInfo.ArgumentList.Add(argument)
            Next

            AddHandler process.OutputDataReceived,
                Sub(sender, e)
                    If e.Data IsNot Nothing Then
                        outputBuilder.AppendLine(e.Data)
                        RaiseOutput(e.Data)
                    End If
                End Sub

            AddHandler process.ErrorDataReceived,
                Sub(sender, e)
                    If e.Data IsNot Nothing Then
                        errorBuilder.AppendLine(e.Data)
                        RaiseOutput(e.Data)
                    End If
                End Sub

            If Not process.Start() Then
                Throw New InvalidOperationException("Avvio di Robocopy non riuscito.")
            End If

            process.BeginOutputReadLine()
            process.BeginErrorReadLine()

            Await Task.Run(Sub()
                               While Not process.HasExited
                                   If IsCancellationRequested(cancellationToken) Then
                                       Exit While
                                   End If
                                   Thread.Sleep(100)
                               End While
                           End Sub, cancellationToken)

            If IsCancellationRequested(cancellationToken) Then
                KillCurrentProcessIfRunning()
            End If

            If Not process.HasExited Then
                process.WaitForExit(5000)
            End If

            Dim exitCode = If(process.HasExited, process.ExitCode, -1)
            Dim cancelled = IsCancellationRequested(cancellationToken)
            Dim result = RobocopyResult.InterpretExitCode(exitCode, cancelled)

            Await WriteLogLineAsync(logWriter, String.Empty)
            Await WriteLogLineAsync(logWriter, "--- Output standard ---")
            Await WriteLogLineAsync(logWriter, outputBuilder.ToString())
            Await WriteLogLineAsync(logWriter, "--- Output errori ---")
            Await WriteLogLineAsync(logWriter, errorBuilder.ToString())
            Await WriteLogLineAsync(logWriter, $"Exit code: {exitCode}")
            Await WriteLogLineAsync(logWriter, $"Risultato: {result.Outcome} - {result.Message}")

            _currentProcess = Nothing
            Return result
        End Using
    End Function

    Private Shared Function BuildFolderList(config As BackupConfig, sourceRoot As String) As List(Of String)
        Dim folders As New List(Of String)()

        For Each folder In config.IncludedFolders
            If Not String.IsNullOrWhiteSpace(folder) Then
                folders.Add(folder)
            End If
        Next

        If String.Equals(config.AppDataMode, "Included", StringComparison.OrdinalIgnoreCase) Then
            If Not folders.Any(Function(f) String.Equals(f, "AppData", StringComparison.OrdinalIgnoreCase)) Then
                folders.Add("AppData")
            End If
        ElseIf String.Equals(config.AppDataMode, "Selective", StringComparison.OrdinalIgnoreCase) Then
            For Each appDataFolder In config.IncludedAppDataFolders
                If String.IsNullOrWhiteSpace(appDataFolder) Then
                    Continue For
                End If

                Dim relativePath = If(appDataFolder.StartsWith("AppData\", StringComparison.OrdinalIgnoreCase),
                                      appDataFolder,
                                      Path.Combine("AppData", appDataFolder))
                folders.Add(relativePath)
            Next
        End If

        Return folders.Distinct(StringComparer.OrdinalIgnoreCase).ToList()
    End Function

    Private Shared Function MergeOutcome(current As BackupOutcome, nextOutcome As BackupOutcome) As BackupOutcome
        If nextOutcome = BackupOutcome.Cancelled OrElse nextOutcome = BackupOutcome.FatalFailure Then
            Return nextOutcome
        End If

        If nextOutcome = BackupOutcome.Failure Then
            Return BackupOutcome.Failure
        End If

        If nextOutcome = BackupOutcome.SuccessWithWarnings Then
            If current = BackupOutcome.NoChanges Then
                Return BackupOutcome.SuccessWithWarnings
            End If
        End If

        If nextOutcome = BackupOutcome.Success AndAlso current = BackupOutcome.NoChanges Then
            Return BackupOutcome.Success
        End If

        If nextOutcome = BackupOutcome.NoChanges Then
            Return current
        End If

        Return nextOutcome
    End Function

    Private Shared Async Function WriteLogLineAsync(writer As StreamWriter, line As String) As Task
        Await writer.WriteLineAsync(line).ConfigureAwait(False)
    End Function

    Private Sub RaiseOutput(message As String)
        RaiseEvent OutputReceived(Me, message)
    End Sub

    Private Sub RaiseStatus(message As String)
        RaiseEvent StatusChanged(Me, message)
    End Sub
End Class
