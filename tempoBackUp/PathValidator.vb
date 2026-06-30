Imports System.Text

Imports System.IO

Public Class PathValidationResult
    Public Property IsValid As Boolean
    Public Property ErrorMessage As String = String.Empty

    Public Shared Function Success() As PathValidationResult
        Return New PathValidationResult With {.IsValid = True}
    End Function

    Public Shared Function Fail(message As String) As PathValidationResult
        Return New PathValidationResult With {.IsValid = False, .ErrorMessage = message}
    End Function
End Class

Public Module PathValidator
    Private ReadOnly ValidAppDataModes As String() = {"Excluded", "Included", "Selective"}

    Public Function NormalizePath(path As String) As String
        Return IO.Path.GetFullPath(path.Trim())
    End Function

    Public Function PathsEqual(left As String, right As String) As Boolean
        Return String.Equals(
            NormalizePath(left).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
            NormalizePath(right).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
            StringComparison.OrdinalIgnoreCase)
    End Function

    Public Function IsPathContainedIn(child As String, parent As String) As Boolean
        Dim normalizedChild = NormalizePath(child).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
        Dim normalizedParent = NormalizePath(parent).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)

        If PathsEqual(normalizedChild, normalizedParent) Then
            Return True
        End If

        Dim parentPrefix = normalizedParent & Path.DirectorySeparatorChar
        Return normalizedChild.StartsWith(parentPrefix, StringComparison.OrdinalIgnoreCase)
    End Function

    Public Function ContainsInvalidPathCharacters(path As String) As Boolean
        If String.IsNullOrWhiteSpace(path) Then
            Return True
        End If

        Dim invalidChars = IO.Path.GetInvalidPathChars()
        Return path.IndexOfAny(invalidChars) >= 0
    End Function

    Public Function ContainsShellInjection(path As String) As Boolean
        If String.IsNullOrWhiteSpace(path) Then
            Return False
        End If

        Return path.Contains("""c") OrElse
               path.Contains("&") OrElse
               path.Contains("|") OrElse
               path.Contains(">") OrElse
               path.Contains("<") OrElse
               path.Contains("^")
    End Function

    Public Function RobocopyExists() As Boolean
        Return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "robocopy.exe"))
    End Function

    Public Function ValidateConfiguration(config As BackupConfig, simulation As Boolean) As PathValidationResult
        If config Is Nothing Then
            Return PathValidationResult.Fail("Configurazione non disponibile.")
        End If

        If String.IsNullOrWhiteSpace(config.SourceRoot) Then
            Return PathValidationResult.Fail("SourceRoot non specificato.")
        End If

        If String.IsNullOrWhiteSpace(config.DestinationRoot) Then
            Return PathValidationResult.Fail("DestinationRoot non specificato.")
        End If

        If String.IsNullOrWhiteSpace(config.LogFolder) Then
            Return PathValidationResult.Fail("LogFolder non specificato.")
        End If

        If Not ValidAppDataModes.Contains(config.AppDataMode, StringComparer.OrdinalIgnoreCase) Then
            Return PathValidationResult.Fail("AppDataMode non valido. Valori ammessi: Excluded, Included, Selective.")
        End If

        Dim source = config.SourceRoot
        Dim destination = config.DestinationRoot
        Dim logFolder = config.LogFolder

        For Each pathValue In New String() {source, destination, logFolder}
            If ContainsInvalidPathCharacters(pathValue) Then
                Return PathValidationResult.Fail($"Percorso non valido: {pathValue}")
            End If

            If ContainsShellInjection(pathValue) Then
                Return PathValidationResult.Fail($"Percorso con caratteri non consentiti: {pathValue}")
            End If
        Next

        Try
            source = NormalizePath(source)
            destination = NormalizePath(destination)
            logFolder = NormalizePath(logFolder)
        Catch ex As Exception
            Return PathValidationResult.Fail($"Impossibile normalizzare i percorsi: {ex.Message}")
        End Try

        If Not Directory.Exists(source) Then
            Return PathValidationResult.Fail($"La sorgente non esiste: {source}")
        End If

        If PathsEqual(source, destination) Then
            Return PathValidationResult.Fail("Sorgente e destinazione non possono coincidere.")
        End If

        If IsPathContainedIn(destination, source) Then
            Return PathValidationResult.Fail("La destinazione non può essere contenuta nella sorgente.")
        End If

        If IsPathContainedIn(source, destination) Then
            Return PathValidationResult.Fail("La sorgente non può essere contenuta nella destinazione.")
        End If

        Try
            Directory.CreateDirectory(destination)
        Catch ex As Exception
            Return PathValidationResult.Fail($"Impossibile creare la destinazione: {ex.Message}")
        End Try

        Try
            Directory.CreateDirectory(logFolder)
        Catch ex As Exception
            Return PathValidationResult.Fail($"Impossibile creare la cartella log: {ex.Message}")
        End Try

        Dim destinationRoot = Path.GetPathRoot(destination)
        If String.IsNullOrWhiteSpace(destinationRoot) Then
            Return PathValidationResult.Fail("Impossibile determinare l'unità della destinazione.")
        End If

        Dim driveInfo As New DriveInfo(destinationRoot)
        If Not driveInfo.IsReady Then
            Return PathValidationResult.Fail($"L'unità di destinazione non è disponibile: {destinationRoot}")
        End If

        If config.IncludedFolders Is Nothing OrElse config.IncludedFolders.Count = 0 Then
            Return PathValidationResult.Fail("IncludedFolders è vuoto.")
        End If

        For Each folder In config.IncludedFolders
            If String.IsNullOrWhiteSpace(folder) Then
                Return PathValidationResult.Fail("IncludedFolders contiene un valore vuoto.")
            End If

            Dim includedPath = Path.Combine(source, folder)
            If ContainsInvalidPathCharacters(includedPath) OrElse ContainsShellInjection(includedPath) Then
                Return PathValidationResult.Fail($"Percorso incluso non valido: {folder}")
            End If
        Next

        If Not RobocopyExists() Then
            Return PathValidationResult.Fail("Robocopy non è disponibile su questo sistema.")
        End If

        Return PathValidationResult.Success()
    End Function
End Module
