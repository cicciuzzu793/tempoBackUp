Public Class BackupProgressEventArgs
    Inherits EventArgs

    Public Property PercentComplete As Integer
    Public Property Message As String = String.Empty
    Public Property IsFolderActive As Boolean
End Class
