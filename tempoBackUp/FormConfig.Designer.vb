<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormConfig
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        tabConfig = New TabControl()
        tabPaths = New TabPage()
        btnBrowseLog = New Button()
        txtLogFolder = New TextBox()
        lblLogFolder = New Label()
        btnBrowseDestination = New Button()
        txtDestinationRoot = New TextBox()
        lblDestinationRoot = New Label()
        btnBrowseSource = New Button()
        txtSourceRoot = New TextBox()
        lblSourceRoot = New Label()
        lblNasHint = New Label()
        tabFolders = New TabPage()
        txtExcludedFiles = New TextBox()
        lblExcludedFiles = New Label()
        txtExcludedFolders = New TextBox()
        lblExcludedFolders = New Label()
        txtIncludedFolders = New TextBox()
        lblIncludedFolders = New Label()
        chkCopyAll = New CheckBox()
        tabAppData = New TabPage()
        txtIncludedAppDataFolders = New TextBox()
        lblIncludedAppDataFolders = New Label()
        cmbAppDataMode = New ComboBox()
        lblAppDataMode = New Label()
        tabNetwork = New TabPage()
        txtNetworkInfo = New TextBox()
        btnCancel = New Button()
        btnSave = New Button()
        tabConfig.SuspendLayout()
        tabPaths.SuspendLayout()
        tabFolders.SuspendLayout()
        tabAppData.SuspendLayout()
        tabNetwork.SuspendLayout()
        SuspendLayout()
        '
        'tabConfig
        '
        tabConfig.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        tabConfig.Controls.Add(tabPaths)
        tabConfig.Controls.Add(tabFolders)
        tabConfig.Controls.Add(tabAppData)
        tabConfig.Controls.Add(tabNetwork)
        tabConfig.Location = New Point(12, 12)
        tabConfig.Name = "tabConfig"
        tabConfig.SelectedIndex = 0
        tabConfig.Size = New Size(760, 480)
        tabConfig.TabIndex = 0
        '
        'tabPaths
        '
        tabPaths.Controls.Add(lblNasHint)
        tabPaths.Controls.Add(btnBrowseLog)
        tabPaths.Controls.Add(txtLogFolder)
        tabPaths.Controls.Add(lblLogFolder)
        tabPaths.Controls.Add(btnBrowseDestination)
        tabPaths.Controls.Add(txtDestinationRoot)
        tabPaths.Controls.Add(lblDestinationRoot)
        tabPaths.Controls.Add(btnBrowseSource)
        tabPaths.Controls.Add(txtSourceRoot)
        tabPaths.Controls.Add(lblSourceRoot)
        tabPaths.Location = New Point(4, 24)
        tabPaths.Name = "tabPaths"
        tabPaths.Padding = New Padding(8)
        tabPaths.Size = New Size(752, 452)
        tabPaths.TabIndex = 0
        tabPaths.Text = "Percorsi"
        tabPaths.UseVisualStyleBackColor = True
        '
        'lblSourceRoot
        '
        lblSourceRoot.AutoSize = True
        lblSourceRoot.Location = New Point(11, 16)
        lblSourceRoot.Name = "lblSourceRoot"
        lblSourceRoot.Size = New Size(52, 15)
        lblSourceRoot.TabIndex = 0
        lblSourceRoot.Text = "Sorgente"
        '
        'txtSourceRoot
        '
        txtSourceRoot.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtSourceRoot.Location = New Point(11, 34)
        txtSourceRoot.Name = "txtSourceRoot"
        txtSourceRoot.Size = New Size(646, 23)
        txtSourceRoot.TabIndex = 1
        '
        'btnBrowseSource
        '
        btnBrowseSource.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnBrowseSource.Location = New Point(663, 33)
        btnBrowseSource.Name = "btnBrowseSource"
        btnBrowseSource.Size = New Size(75, 25)
        btnBrowseSource.TabIndex = 2
        btnBrowseSource.Text = "Sfoglia..."
        btnBrowseSource.UseVisualStyleBackColor = True
        '
        'lblDestinationRoot
        '
        lblDestinationRoot.AutoSize = True
        lblDestinationRoot.Location = New Point(11, 72)
        lblDestinationRoot.Name = "lblDestinationRoot"
        lblDestinationRoot.Size = New Size(67, 15)
        lblDestinationRoot.TabIndex = 3
        lblDestinationRoot.Text = "Destinazione"
        '
        'txtDestinationRoot
        '
        txtDestinationRoot.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtDestinationRoot.Location = New Point(11, 90)
        txtDestinationRoot.Name = "txtDestinationRoot"
        txtDestinationRoot.Size = New Size(646, 23)
        txtDestinationRoot.TabIndex = 4
        '
        'btnBrowseDestination
        '
        btnBrowseDestination.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnBrowseDestination.Location = New Point(663, 89)
        btnBrowseDestination.Name = "btnBrowseDestination"
        btnBrowseDestination.Size = New Size(75, 25)
        btnBrowseDestination.TabIndex = 5
        btnBrowseDestination.Text = "Sfoglia..."
        btnBrowseDestination.UseVisualStyleBackColor = True
        '
        'lblLogFolder
        '
        lblLogFolder.AutoSize = True
        lblLogFolder.Location = New Point(11, 128)
        lblLogFolder.Name = "lblLogFolder"
        lblLogFolder.Size = New Size(66, 15)
        lblLogFolder.TabIndex = 6
        lblLogFolder.Text = "Cartella log"
        '
        'txtLogFolder
        '
        txtLogFolder.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtLogFolder.Location = New Point(11, 146)
        txtLogFolder.Name = "txtLogFolder"
        txtLogFolder.Size = New Size(646, 23)
        txtLogFolder.TabIndex = 7
        '
        'btnBrowseLog
        '
        btnBrowseLog.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnBrowseLog.Location = New Point(663, 145)
        btnBrowseLog.Name = "btnBrowseLog"
        btnBrowseLog.Size = New Size(75, 25)
        btnBrowseLog.TabIndex = 8
        btnBrowseLog.Text = "Sfoglia..."
        btnBrowseLog.UseVisualStyleBackColor = True
        '
        'lblNasHint
        '
        lblNasHint.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblNasHint.Location = New Point(11, 188)
        lblNasHint.Name = "lblNasHint"
        lblNasHint.Size = New Size(727, 80)
        lblNasHint.TabIndex = 9
        lblNasHint.Text = "Per un NAS o disco di rete usa un percorso UNC, ad esempio: \\NAS\Backup\Francesco" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "La condivisione deve essere già accessibile da Windows (credenziali salvate o rete locale)."
        '
        'tabFolders
        '
        tabFolders.Controls.Add(txtExcludedFiles)
        tabFolders.Controls.Add(lblExcludedFiles)
        tabFolders.Controls.Add(txtExcludedFolders)
        tabFolders.Controls.Add(lblExcludedFolders)
        tabFolders.Controls.Add(txtIncludedFolders)
        tabFolders.Controls.Add(lblIncludedFolders)
        tabFolders.Controls.Add(chkCopyAll)
        tabFolders.Location = New Point(4, 24)
        tabFolders.Name = "tabFolders"
        tabFolders.Padding = New Padding(8)
        tabFolders.Size = New Size(752, 452)
        tabFolders.TabIndex = 1
        tabFolders.Text = "Cartelle"
        tabFolders.UseVisualStyleBackColor = True
        '
        'chkCopyAll
        '
        chkCopyAll.AutoSize = True
        chkCopyAll.Location = New Point(11, 16)
        chkCopyAll.Name = "chkCopyAll"
        chkCopyAll.Size = New Size(330, 19)
        chkCopyAll.TabIndex = 0
        chkCopyAll.Text = "Copia tutto il contenuto della sorgente (senza elenco cartelle)"
        chkCopyAll.UseVisualStyleBackColor = True
        '
        'lblIncludedFolders
        '
        lblIncludedFolders.AutoSize = True
        lblIncludedFolders.Location = New Point(11, 44)
        lblIncludedFolders.Name = "lblIncludedFolders"
        lblIncludedFolders.Size = New Size(176, 15)
        lblIncludedFolders.TabIndex = 1
        lblIncludedFolders.Text = "Cartelle incluse (una per riga)"
        '
        'txtIncludedFolders
        '
        txtIncludedFolders.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtIncludedFolders.Location = New Point(11, 62)
        txtIncludedFolders.Multiline = True
        txtIncludedFolders.Name = "txtIncludedFolders"
        txtIncludedFolders.ScrollBars = ScrollBars.Vertical
        txtIncludedFolders.Size = New Size(727, 82)
        txtIncludedFolders.TabIndex = 2
        '
        'lblExcludedFolders
        '
        lblExcludedFolders.AutoSize = True
        lblExcludedFolders.Location = New Point(11, 154)
        lblExcludedFolders.Name = "lblExcludedFolders"
        lblExcludedFolders.Size = New Size(242, 15)
        lblExcludedFolders.TabIndex = 2
        lblExcludedFolders.Text = "Cartelle escluse (nome semplice es. node_modules, oppure percorso da SourceRoot)"
        '
        'txtExcludedFolders
        '
        txtExcludedFolders.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtExcludedFolders.Location = New Point(11, 172)
        txtExcludedFolders.Multiline = True
        txtExcludedFolders.Name = "txtExcludedFolders"
        txtExcludedFolders.ScrollBars = ScrollBars.Vertical
        txtExcludedFolders.Size = New Size(727, 90)
        txtExcludedFolders.TabIndex = 3
        '
        'lblExcludedFiles
        '
        lblExcludedFiles.AutoSize = True
        lblExcludedFiles.Location = New Point(11, 272)
        lblExcludedFiles.Name = "lblExcludedFiles"
        lblExcludedFiles.Size = New Size(153, 15)
        lblExcludedFiles.TabIndex = 4
        lblExcludedFiles.Text = "File esclusi (uno per riga)"
        '
        'txtExcludedFiles
        '
        txtExcludedFiles.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtExcludedFiles.Location = New Point(11, 290)
        txtExcludedFiles.Multiline = True
        txtExcludedFiles.Name = "txtExcludedFiles"
        txtExcludedFiles.ScrollBars = ScrollBars.Vertical
        txtExcludedFiles.Size = New Size(727, 149)
        txtExcludedFiles.TabIndex = 5
        '
        'tabAppData
        '
        tabAppData.Controls.Add(txtIncludedAppDataFolders)
        tabAppData.Controls.Add(lblIncludedAppDataFolders)
        tabAppData.Controls.Add(cmbAppDataMode)
        tabAppData.Controls.Add(lblAppDataMode)
        tabAppData.Location = New Point(4, 24)
        tabAppData.Name = "tabAppData"
        tabAppData.Padding = New Padding(8)
        tabAppData.Size = New Size(752, 452)
        tabAppData.TabIndex = 2
        tabAppData.Text = "AppData"
        tabAppData.UseVisualStyleBackColor = True
        '
        'lblAppDataMode
        '
        lblAppDataMode.AutoSize = True
        lblAppDataMode.Location = New Point(11, 16)
        lblAppDataMode.Name = "lblAppDataMode"
        lblAppDataMode.Size = New Size(79, 15)
        lblAppDataMode.TabIndex = 0
        lblAppDataMode.Text = "AppDataMode"
        '
        'cmbAppDataMode
        '
        cmbAppDataMode.DropDownStyle = ComboBoxStyle.DropDownList
        cmbAppDataMode.FormattingEnabled = True
        cmbAppDataMode.Items.AddRange(New Object() {"Excluded", "Included", "Selective"})
        cmbAppDataMode.Location = New Point(11, 34)
        cmbAppDataMode.Name = "cmbAppDataMode"
        cmbAppDataMode.Size = New Size(200, 23)
        cmbAppDataMode.TabIndex = 1
        '
        'lblIncludedAppDataFolders
        '
        lblIncludedAppDataFolders.AutoSize = True
        lblIncludedAppDataFolders.Location = New Point(11, 72)
        lblIncludedAppDataFolders.Name = "lblIncludedAppDataFolders"
        lblIncludedAppDataFolders.Size = New Size(318, 15)
        lblIncludedAppDataFolders.TabIndex = 2
        lblIncludedAppDataFolders.Text = "Cartelle AppData incluse (solo con Selective, una per riga)"
        '
        'txtIncludedAppDataFolders
        '
        txtIncludedAppDataFolders.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtIncludedAppDataFolders.Location = New Point(11, 90)
        txtIncludedAppDataFolders.Multiline = True
        txtIncludedAppDataFolders.Name = "txtIncludedAppDataFolders"
        txtIncludedAppDataFolders.ScrollBars = ScrollBars.Vertical
        txtIncludedAppDataFolders.Size = New Size(727, 349)
        txtIncludedAppDataFolders.TabIndex = 3
        '
        'tabNetwork
        '
        tabNetwork.Controls.Add(txtNetworkInfo)
        tabNetwork.Location = New Point(4, 24)
        tabNetwork.Name = "tabNetwork"
        tabNetwork.Padding = New Padding(8)
        tabNetwork.Size = New Size(752, 452)
        tabNetwork.TabIndex = 3
        tabNetwork.Text = "Rete"
        tabNetwork.UseVisualStyleBackColor = True
        '
        'txtNetworkInfo
        '
        txtNetworkInfo.Dock = DockStyle.Fill
        txtNetworkInfo.Location = New Point(8, 8)
        txtNetworkInfo.Multiline = True
        txtNetworkInfo.Name = "txtNetworkInfo"
        txtNetworkInfo.ReadOnly = True
        txtNetworkInfo.ScrollBars = ScrollBars.Vertical
        txtNetworkInfo.Size = New Size(736, 436)
        txtNetworkInfo.TabIndex = 0
        txtNetworkInfo.Text = "NAS / rete locale (disponibile ora)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Puoi usare Destinazione o Cartella log con percorso UNC, ad esempio:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "\\192.168.1.10\backup\Francesco" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Robocopy copia verso la condivisione se Windows riesce ad accedervi. Le credenziali vanno configurate in Windows (Esplora file → connetti unità di rete)." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "SFTP / SSH (prossima versione)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "La copia verso server SFTP o SSH richiede un motore diverso da Robocopy, nuovi campi nel config.json e aggiornamento del contratto di prodotto. Non è inclusa in questa versione." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Per motivi di sicurezza, user e password non verrebbero salvati in chiaro nel JSON senza una definizione approvata."
        '
        'btnCancel
        '
        btnCancel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(697, 504)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(75, 32)
        btnCancel.TabIndex = 2
        btnCancel.Text = "Annulla"
        btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        btnSave.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnSave.Location = New Point(616, 504)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(75, 32)
        btnSave.TabIndex = 1
        btnSave.Text = "Salva"
        btnSave.UseVisualStyleBackColor = True
        '
        'FormConfig
        '
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(784, 548)
        Controls.Add(btnCancel)
        Controls.Add(btnSave)
        Controls.Add(tabConfig)
        MinimumSize = New Size(800, 587)
        Name = "FormConfig"
        StartPosition = FormStartPosition.CenterParent
        Text = "Configurazione tempoBackUp"
        tabConfig.ResumeLayout(False)
        tabPaths.ResumeLayout(False)
        tabPaths.PerformLayout()
        tabFolders.ResumeLayout(False)
        tabFolders.PerformLayout()
        tabAppData.ResumeLayout(False)
        tabAppData.PerformLayout()
        tabNetwork.ResumeLayout(False)
        tabNetwork.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents tabConfig As TabControl
    Friend WithEvents tabPaths As TabPage
    Friend WithEvents tabFolders As TabPage
    Friend WithEvents tabAppData As TabPage
    Friend WithEvents tabNetwork As TabPage
    Friend WithEvents lblSourceRoot As Label
    Friend WithEvents txtSourceRoot As TextBox
    Friend WithEvents btnBrowseSource As Button
    Friend WithEvents lblDestinationRoot As Label
    Friend WithEvents txtDestinationRoot As TextBox
    Friend WithEvents btnBrowseDestination As Button
    Friend WithEvents lblLogFolder As Label
    Friend WithEvents txtLogFolder As TextBox
    Friend WithEvents btnBrowseLog As Button
    Friend WithEvents lblNasHint As Label
    Friend WithEvents chkCopyAll As CheckBox
    Friend WithEvents lblIncludedFolders As Label
    Friend WithEvents txtIncludedFolders As TextBox
    Friend WithEvents lblExcludedFolders As Label
    Friend WithEvents txtExcludedFolders As TextBox
    Friend WithEvents lblExcludedFiles As Label
    Friend WithEvents txtExcludedFiles As TextBox
    Friend WithEvents lblAppDataMode As Label
    Friend WithEvents cmbAppDataMode As ComboBox
    Friend WithEvents lblIncludedAppDataFolders As Label
    Friend WithEvents txtIncludedAppDataFolders As TextBox
    Friend WithEvents txtNetworkInfo As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
End Class
