# CONTRACT.md — tempoBackUp (Manual Backup)

## 1. Scopo del prodotto

**tempoBackUp** è un'applicazione desktop Windows 11 che consente all'utente di eseguire **manualmente** il backup di cartelle del profilo utente verso una destinazione locale, usando **Robocopy** come unico motore di copia.

Il prodotto **non** elimina, sposta o sincronizza in modo distruttivo. Non è uno scheduler e non resta in esecuzione in background.

## 2. Stack tecnico

| Elemento | Valore |
|----------|--------|
| Linguaggio | VB.NET |
| UI | Windows Forms |
| Framework | .NET 8 (`net8.0-windows`) |
| OS | Windows 11 |
| Motore copia | `robocopy.exe` (System32) |
| Configurazione | `config.json` + `System.Text.Json` |

## 3. Configurazione JSON

Schema obbligatorio (nomi proprietà immutabili):

```json
{
  "SourceRoot": "C:\\Users\\Francesco",
  "DestinationRoot": "D:\\Backup\\Francesco",
  "IncludedFolders": ["Desktop", "Documents", "Downloads", "Pictures", "Videos", "Music"],
  "CopyAll": false,
  "ExcludedFolders": ["AppData\\Local\\Temp", "AppData\\Local\\Microsoft\\Windows\\INetCache"],
  "ExcludedFiles": ["*.tmp", "*.temp", "thumbs.db", "desktop.ini"],
  "AppDataMode": "Excluded",
  "IncludedAppDataFolders": [],
  "LogFolder": "D:\\Backup\\Logs"
}
```

### Semantica

- **SourceRoot**: radice del profilo utente da cui derivano le cartelle incluse.
- **DestinationRoot**: radice di destinazione; per ogni cartella inclusa si copia `SourceRoot\Nome` → `DestinationRoot\Nome`.
- **IncludedFolders**: elenco relativo di cartelle sotto `SourceRoot` (ignorato se `CopyAll` è `true`).
- **CopyAll**: se `true`, copia l’intero contenuto di `SourceRoot` verso `DestinationRoot` in un’unica passata Robocopy, senza elencare le sottocartelle. Le esclusioni (`ExcludedFolders`, `AppDataMode`) restano attive.
- **ExcludedFolders**: esclusioni tramite `/XD`. Un **nome semplice** (es. `node_modules`) esclude ogni cartella con quel nome a qualsiasi profondità sotto la cartella in copia. Un **percorso relativo a SourceRoot** (es. `AppData\Local\Temp`) esclude quel percorso quando ricade sotto la cartella inclusa in esecuzione.
- **ExcludedFiles**: pattern file esclusi tramite `/XF`.
- **AppDataMode**: `Excluded` | `Included` | `Selective`.
- **IncludedAppDataFolders**: usato solo con `Selective`; percorsi relativi sotto `AppData`.
- **LogFolder**: cartella dove scrivere i log di esecuzione.

## 4. Parametri Robocopy

Parametri base (sempre applicati):

```
/E /XO /FFT /COPY:DAT /DCOPY:T /R:2 /W:2 /XJ /Z /TEE /NP
```

In **simulazione** si aggiunge `/L`.

**Vietati** senza approvazione: `/MIR`, `/PURGE`, `/MOVE`, `/MOV`, `/IS`, `/IT`.

## 5. Esclusioni hardcoded

Directory (sempre escluse con `/XD`):

- `C:\Windows`
- `C:\Program Files`
- `C:\Program Files (x86)`
- `C:\ProgramData`
- `C:\Recovery`
- `C:\PerfLogs`
- `C:\$Recycle.Bin`
- `C:\System Volume Information`

File (sempre esclusi con `/XF`):

- `pagefile.sys`, `hiberfil.sys`, `swapfile.sys`, `DumpStack.log`, `DumpStack.log.tmp`

## 6. Exit code

- `ExitCode < 8` → operazione **completata** (con possibili avvisi)
- `ExitCode >= 8` → **errore**

Interpretazione applicativa:

| Esito | Condizione |
|-------|------------|
| NoChanges | exit 0, non annullato |
| Success | exit 1 |
| SuccessWithWarnings | exit 2–7 |
| Failure | exit 8–15 |
| FatalFailure | exit >= 16 |
| Cancelled | interruzione utente |

## 7. Validazione percorsi

Prima di ogni esecuzione:

1. sorgente esistente
2. destinazione creabile
3. sorgente ≠ destinazione
4. nessuna inclusione ricorsiva sorgente/destinazione
5. caratteri percorso validi
6. nessuna iniezione shell
7. disco destinazione disponibile
8. cartella log creabile
9. `AppDataMode` valido
10. Robocopy presente

Gli argomenti devono usare `ProcessStartInfo.ArgumentList`.

## 8. Interfaccia utente

Form principale: `FormMain`

Controlli:

- `txtSource`, `txtDestination`, `txtLogFolder` (sola lettura, da config)
- `btnRunBackup` — esecuzione reale
- `btnSimulate` — simulazione (`/L`)
- `btnStop` — interrompe **solo** il processo avviato dall'app (con conferma)
- `btnReloadConfig` — ricarica `config.json`
- `lblStatus` — stato corrente
- `lblProgressDetail` — dettaglio avanzamento (cartella corrente)
- `progressBarWork` — barra di progresso dell'operazione
- `txtOutput` — output live

Le operazioni lunghe sono asincrone; il form non deve bloccarsi.

## 9. Logging

Ogni esecuzione crea un file **nuovo** in `LogFolder`:

- `backup_YYYY-MM-DD_HHmmss.log`
- `simulation_YYYY-MM-DD_HHmmss.log`

Contenuto minimo: configurazione, comando, stdout, stderr, exit code, esito, eccezioni.

## 10. Architettura

```
tempoBackUp/
├── tempoBackUp.vbproj
├── FormMain.vb
├── FormMain.Designer.vb
├── BackupConfig.vb
├── BackupRunner.vb
├── RobocopyCommandBuilder.vb
├── RobocopyResult.vb
├── PathValidator.vb
├── HardcodedExclusions.vb
├── config.json
├── CONTRACT.md
├── AGENTS.md
└── README.md
```

Un solo processo Robocopy alla volta per istanza applicazione.

## 11. Cosa il prodotto non fa

- Nessuna cancellazione in destinazione per file rimossi dalla sorgente
- Nessun backup automatico o pianificato
- Nessun servizio Windows
- Nessuna modifica permessi/proprietà file
- Nessuna dipendenza NuGet aggiuntiva senza approvazione
