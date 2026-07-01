# tempoBackUp

Applicazione Windows Forms in VB.NET (.NET 8) per backup manuale di cartelle utente tramite Robocopy.

## Requisiti

- Windows 11
- Visual Studio 2022 con carico di lavoro **Sviluppo di applicazioni desktop .NET**
- .NET 8 SDK

## Apertura in Visual Studio 2022

1. Apri `tempoBackUp.sln`
2. Copia `tempoBackUp\config.example.json` in `tempoBackUp\config.json` solo per sviluppo locale (opzionale)
3. Avvia con F5

La configurazione usata dall'app installata è salvata in:

`%LocalAppData%\QrcodeSrl\tempoBackUp\config.json`

(modificabile da **Impostazioni** senza permessi amministratore)

## Comandi

```powershell
dotnet restore tempoBackUp.sln
dotnet build tempoBackUp.sln
```

## Documentazione

- `CONTRACT.md` — contratto funzionale del prodotto
- `AGENTS.md` — regole per sviluppatori e agenti AI

## Icona

- Sorgente: `icona.png` (root repository)
- Icona Windows/MSI: `tempoBackUp\Assets\tempoBackUp.ico` (16–256 px)

Per rigenerare l'icona dopo aver modificato `icona.png`:

```powershell
dotnet run --project tools\IconConverter\IconConverter\IconConverter.csproj -- icona.png tempoBackUp\Assets\tempoBackUp.ico
```

## Setup MSI

L'installer pronto all'uso è nel repository:

**`dist/tempoBackUp.msi`**

Scaricalo da GitHub (pulsante *Download* o *Raw* nella cartella `dist`) e avvialo sul PC di destinazione.

> Nota: anche la cartella `bin/` resta ignorata da Git; solo `dist/tempoBackUp.msi` viene versionato per distribuzione.

In alternativa, dopo ogni push su `main`:

1. **GitHub Actions** → workflow *Build MSI* → artifact `tempoBackUp-msi`
2. **GitHub Releases** → allegato `tempoBackUp.msi` (tag `v1.0.0`, ecc.)

### Prerequisito sul PC di destinazione

- **Microsoft .NET 8 Desktop Runtime (x64)**  
  Download: https://dotnet.microsoft.com/download/dotnet/8.0

L'installer MSI **blocca l'installazione** se .NET 8 Desktop non è presente e mostra il link di download.  
L'applicazione, all'avvio, fa lo stesso controllo e propone di aprire la pagina di download.

### Build MSI in locale

Il progetto `tempoBackUp.Setup` (WiX Toolset) produce l'installer con:

- controllo prerequisito .NET 8 Desktop
- icona in **Programmi e funzionalità**
- collegamenti su **Menu Start** e **Desktop** con la stessa icona
- eseguibile con icona incorporata

### Build MSI da Visual Studio 2022

1. Apri `tempoBackUp.sln`
2. Se richiesto, installa l'estensione **HeatWave for VS** (WiX) o usa solo `dotnet build`
3. Imposta configurazione **Release | x64**
4. Compila il progetto **tempoBackUp.Setup**

### Build MSI da riga di comando

```powershell
dotnet build tempoBackUp.Setup\tempoBackUp.Setup.wixproj -c Release
```

L'MSI generato si trova in:

`tempoBackUp.Setup\bin\x64\Release\tempoBackUp.msi`

## Comportamento

- Backup **solo manuale** (nessuno scheduler)
- Motore: **Robocopy** integrato in Windows
- Nessuna cancellazione di file in destinazione o sorgente
- Exit code Robocopy `< 8` = completato
- Ogni esecuzione produce un log con timestamp in `LogFolder`
