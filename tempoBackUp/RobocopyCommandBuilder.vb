Imports System.IO

Public Class RobocopyCommandBuilder
    Private ReadOnly BaseParameters As String() = {
        "/E",
        "/XO",
        "/FFT",
        "/COPY:DAT",
        "/DCOPY:T",
        "/R:2",
        "/W:2",
        "/XJ",
        "/Z",
        "/TEE",
        "/NP"
    }

    Public Function BuildArgumentList(source As String, destination As String, config As BackupConfig, simulation As Boolean) As List(Of String)
        Dim args As New List(Of String) From {
            PathValidator.NormalizePath(source),
            PathValidator.NormalizePath(destination)
        }

        args.AddRange(BaseParameters)

        If simulation Then
            args.Add("/L")
        End If

        For Each excludedDirectory In GetExcludedDirectories(config, source)
            args.Add("/XD")
            args.Add(excludedDirectory)
        Next

        For Each excludedFile In GetExcludedFiles(config)
            args.Add("/XF")
            args.Add(excludedFile)
        Next

        Return args
    End Function

    Public Function BuildCommandLineForLog(args As IEnumerable(Of String)) As String
        Return "robocopy " & String.Join(" ", args.Select(AddressOf QuoteForLog))
    End Function

    Private Function GetExcludedDirectories(config As BackupConfig, sourceRoot As String) As IEnumerable(Of String)
        Dim directories As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

        For Each directory In HardcodedExclusions.ExcludedDirectories
            directories.Add(PathValidator.NormalizePath(directory))
        Next

        For Each relativeFolder In config.ExcludedFolders
            directories.Add(PathValidator.NormalizePath(Path.Combine(sourceRoot, relativeFolder)))
        Next

        If String.Equals(config.AppDataMode, "Excluded", StringComparison.OrdinalIgnoreCase) Then
            directories.Add(PathValidator.NormalizePath(Path.Combine(sourceRoot, "AppData")))
        End If

        Return directories
    End Function

    Private Function GetExcludedFiles(config As BackupConfig) As IEnumerable(Of String)
        Dim files As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

        For Each fileName In HardcodedExclusions.ExcludedFiles
            files.Add(fileName)
        Next

        For Each pattern In config.ExcludedFiles
            files.Add(pattern)
        Next

        Return files
    End Function

    Private Shared Function QuoteForLog(value As String) As String
        If value.Contains(" "c) OrElse value.Contains(""""c) Then
            Return """" & value.Replace("""", "\""") & """"
        End If

        Return value
    End Function
End Class
