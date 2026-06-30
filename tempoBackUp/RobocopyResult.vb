Public Enum BackupOutcome
    NoChanges
    Success
    SuccessWithWarnings
    Failure
    FatalFailure
    Cancelled
End Enum

Public Class RobocopyResult
    Public Property ExitCode As Integer
    Public Property Outcome As BackupOutcome
    Public Property Message As String = String.Empty

    Public Shared Function InterpretExitCode(exitCode As Integer, cancelled As Boolean) As RobocopyResult
        If cancelled Then
            Return New RobocopyResult With {
                .ExitCode = exitCode,
                .Outcome = BackupOutcome.Cancelled,
                .Message = "Operazione interrotta dall'utente."
            }
        End If

        Dim outcome As BackupOutcome
        Dim message As String

        If exitCode >= 16 Then
            outcome = BackupOutcome.FatalFailure
            message = $"Errore grave Robocopy (exit code {exitCode})."
        ElseIf exitCode >= 8 Then
            outcome = BackupOutcome.Failure
            message = $"Errore Robocopy (exit code {exitCode})."
        ElseIf exitCode = 0 Then
            outcome = BackupOutcome.NoChanges
            message = "Nessuna modifica necessaria."
        ElseIf exitCode <= 1 Then
            outcome = BackupOutcome.Success
            message = $"Backup completato (exit code {exitCode})."
        Else
            outcome = BackupOutcome.SuccessWithWarnings
            message = $"Backup completato con avvisi (exit code {exitCode})."
        End If

        Return New RobocopyResult With {
            .ExitCode = exitCode,
            .Outcome = outcome,
            .Message = message
        }
    End Function

    Public ReadOnly Property IsCompleted As Boolean
        Get
            Return Outcome <> BackupOutcome.Failure AndAlso
                   Outcome <> BackupOutcome.FatalFailure AndAlso
                   Outcome <> BackupOutcome.Cancelled
        End Get
    End Property
End Class
