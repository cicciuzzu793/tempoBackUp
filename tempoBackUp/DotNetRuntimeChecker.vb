Imports Microsoft.Win32

Public Module DotNetRuntimeChecker
    Public Const DownloadUrl As String = "https://dotnet.microsoft.com/download/dotnet/8.0"

    Private ReadOnly RegistryPaths As String() = {
        "SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.WindowsDesktop.App",
        "SOFTWARE\WOW6432Node\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.WindowsDesktop.App"
    }

    Public Function IsDesktopRuntime8Installed() As Boolean
        For Each registryPath In RegistryPaths
            Try
                Using key = Registry.LocalMachine.OpenSubKey(registryPath)
                    If key Is Nothing Then
                        Continue For
                    End If

                    For Each versionName In key.GetValueNames()
                        If versionName.StartsWith("8.", StringComparison.Ordinal) Then
                            Return True
                        End If
                    Next
                End Using
            Catch
            End Try
        Next

        Return False
    End Function

    Public Function EnsureDesktopRuntime8Installed() As Boolean
        If IsDesktopRuntime8Installed() Then
            Return True
        End If

        Dim message =
            "tempoBackUp richiede Microsoft .NET 8 Desktop Runtime (x64)." & Environment.NewLine & Environment.NewLine &
            "Scaricalo da:" & Environment.NewLine & DownloadUrl & Environment.NewLine & Environment.NewLine &
            "Aprire la pagina di download ora?"

        Dim answer = MessageBox.Show(
            message,
            "Runtime .NET mancante",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button1)

        If answer = DialogResult.Yes Then
            Try
                Process.Start(New ProcessStartInfo(DownloadUrl) With {.UseShellExecute = True})
            Catch ex As Exception
                MessageBox.Show(
                    $"Impossibile aprire il browser.{Environment.NewLine}{Environment.NewLine}{DownloadUrl}",
                    "tempoBackUp",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)
            End Try
        End If

        Return False
    End Function
End Module
