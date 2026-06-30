Imports System.IO
Imports System.Text.Json

Public Class BackupConfig
    Public Property SourceRoot As String = String.Empty
    Public Property DestinationRoot As String = String.Empty
    Public Property IncludedFolders As List(Of String) = New List(Of String)()
    Public Property ExcludedFolders As List(Of String) = New List(Of String)()
    Public Property ExcludedFiles As List(Of String) = New List(Of String)()
    Public Property AppDataMode As String = "Excluded"
    Public Property IncludedAppDataFolders As List(Of String) = New List(Of String)()
    Public Property LogFolder As String = String.Empty

    Public Shared Function LoadFromFile(path As String) As BackupConfig
        Dim json = File.ReadAllText(path)
        Dim config = JsonSerializer.Deserialize(Of BackupConfig)(json)
        If config Is Nothing Then
            Throw New InvalidOperationException("Il file di configurazione è vuoto o non valido.")
        End If

        If config.IncludedFolders Is Nothing Then config.IncludedFolders = New List(Of String)()
        If config.ExcludedFolders Is Nothing Then config.ExcludedFolders = New List(Of String)()
        If config.ExcludedFiles Is Nothing Then config.ExcludedFiles = New List(Of String)()
        If config.IncludedAppDataFolders Is Nothing Then config.IncludedAppDataFolders = New List(Of String)()
        Return config
    End Function

    Public Function ToLogSummary() As String
        Return $"SourceRoot={SourceRoot}" & Environment.NewLine &
               $"DestinationRoot={DestinationRoot}" & Environment.NewLine &
               $"IncludedFolders={String.Join(", ", IncludedFolders)}" & Environment.NewLine &
               $"ExcludedFolders={String.Join(", ", ExcludedFolders)}" & Environment.NewLine &
               $"ExcludedFiles={String.Join(", ", ExcludedFiles)}" & Environment.NewLine &
               $"AppDataMode={AppDataMode}" & Environment.NewLine &
               $"IncludedAppDataFolders={String.Join(", ", IncludedAppDataFolders)}" & Environment.NewLine &
               $"LogFolder={LogFolder}"
    End Function
End Class
