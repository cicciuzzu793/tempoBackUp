# AGENTS.md

## 1. Scopo del file

Questo file definisce le regole obbligatorie per qualunque sviluppatore o agente AI che lavori sul progetto **tempoBackUp** (Manual Backup).

Il riferimento funzionale principale è: **CONTRACT.md**

In caso di conflitto tra implementazione e contratto, prevale **CONTRACT.md**.

Non modificare il comportamento del prodotto per interpretazione personale.

## 2. Regola fondamentale

Il progetto deve restare una piccola applicazione **VB.NET Windows Forms** per backup manuale di file tramite **Robocopy**.

Non trasformare il progetto in:

- servizio Windows
- applicazione residente
- scheduler
- sincronizzatore automatico
- clone disco
- sistema di imaging
- backup cloud
- motore proprietario di copia

## 3. Prima di modificare il codice

Prima di applicare qualunque modifica:

1. leggere `CONTRACT.md`
2. leggere questo file
3. verificare la struttura attuale del progetto
4. individuare classi, controlli e funzioni già esistenti
5. riutilizzare il codice esistente quando equivalente
6. evitare nuove astrazioni non necessarie
7. verificare che la modifica non alteri comportamenti osservabili
8. indicare eventuali punti non coperti dal contratto

Non introdurre modifiche architetturali senza approvazione esplicita.

## 4. Divieti di sicurezza

Non aggiungere mai automaticamente i seguenti parametri Robocopy:

- `/MIR`
- `/PURGE`
- `/MOVE`
- `/MOV`
- `/IS`
- `/IT`

Non aggiungere codice che:

- elimina file dalla destinazione
- elimina file dalla sorgente
- sposta file dalla sorgente
- formatta dischi
- modifica partizioni
- modifica il registro di Windows
- installa servizi
- crea attività pianificate
- acquisisce proprietà di file
- cambia permessi
- disattiva protezioni Windows
- esegue comandi PowerShell non necessari
- esegue comandi shell arbitrari provenienti dal JSON
- consente l'iniezione di argomenti nella riga di comando

Ogni funzione distruttiva richiede approvazione esplicita prima dell'implementazione.

## 5. Contratti da non modificare

Non modificare senza autorizzazione:

- nomi dei campi JSON
- struttura del JSON
- semantica dei campi
- modalità `AppDataMode`
- nomi dei controlli UI già concordati
- comportamento degli exit code Robocopy
- esclusioni hardcoded
- regola `ExitCode < 8 = completato`
- assenza di cancellazioni
- esecuzione esclusivamente manuale
- motore Robocopy
- piattaforma Windows 11
- linguaggio VB.NET
- framework Windows Forms

## 6–26

Vedi `AGENTS.pdf` nella root del repository per il testo completo delle sezioni 6–26 (configurazione JSON, validazione, Robocopy, logging, test, ecc.).
