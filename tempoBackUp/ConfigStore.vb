Imports System.IO

Public Module ConfigStore
    Public Function GetUserConfigDirectory() As String
        Return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "QrcodeSrl",
            "tempoBackUp")
    End Function

    Public Function GetUserConfigPath() As String
        Return Path.Combine(GetUserConfigDirectory(), "config.json")
    End Function

    Public Sub EnsureUserConfig()
        Dim userConfigPath = GetUserConfigPath()
        If File.Exists(userConfigPath) Then
            Return
        End If

        Directory.CreateDirectory(GetUserConfigDirectory())

        Dim installConfigPath = Path.Combine(AppContext.BaseDirectory, "config.json")
        Dim exampleConfigPath = Path.Combine(AppContext.BaseDirectory, "config.example.json")

        If File.Exists(installConfigPath) Then
            File.Copy(installConfigPath, userConfigPath)
            Return
        End If

        If File.Exists(exampleConfigPath) Then
            File.Copy(exampleConfigPath, userConfigPath)
            Return
        End If

        Throw New FileNotFoundException(
            "Modello di configurazione non trovato accanto all'applicazione.",
            exampleConfigPath)
    End Sub
End Module
