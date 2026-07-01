Friend Module Program

    <STAThread()>
    Friend Sub Main(args As String())
        If Not DotNetRuntimeChecker.EnsureDesktopRuntime8Installed() Then
            Return
        End If

        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        ConfigStore.EnsureUserConfig()
        Application.Run(New FormMain())
    End Sub

End Module
