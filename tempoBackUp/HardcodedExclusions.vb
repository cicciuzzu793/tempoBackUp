Public Module HardcodedExclusions
    Public ReadOnly ExcludedDirectories As String() = {
        "C:\Windows",
        "C:\Program Files",
        "C:\Program Files (x86)",
        "C:\ProgramData",
        "C:\Recovery",
        "C:\PerfLogs",
        "C:\$Recycle.Bin",
        "C:\System Volume Information"
    }

    Public ReadOnly ExcludedFiles As String() = {
        "pagefile.sys",
        "hiberfil.sys",
        "swapfile.sys",
        "DumpStack.log",
        "DumpStack.log.tmp"
    }
End Module
