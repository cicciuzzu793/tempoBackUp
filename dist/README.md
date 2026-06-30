# Installer tempoBackUp

File generato da `tempoBackUp.Setup` (Release | x64).

Per rigenerarlo in locale:

```powershell
dotnet build tempoBackUp.Setup\tempoBackUp.Setup.wixproj -c Release
Copy-Item tempoBackUp.Setup\bin\x64\Release\tempoBackUp.msi dist\tempoBackUp.msi -Force
```

Prerequisito sul PC di destinazione: **.NET 8 Desktop Runtime (x64)**  
https://dotnet.microsoft.com/download/dotnet/8.0
