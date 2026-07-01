<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
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
        lblTitle = New Label()
        lblSource = New Label()
        txtSource = New TextBox()
        lblDestination = New Label()
        txtDestination = New TextBox()
        lblLogFolder = New Label()
        txtLogFolder = New TextBox()
        lblStatus = New Label()
        txtOutput = New TextBox()
        btnRunBackup = New Button()
        btnSimulate = New Button()
        btnStop = New Button()
        btnEditConfig = New Button()
        btnReloadConfig = New Button()
        SuspendLayout()
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
        lblTitle.Location = New Point(12, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(118, 21)
        lblTitle.TabIndex = 0
        lblTitle.Text = "tempoBackUp"
        '
        'lblSource
        '
        lblSource.AutoSize = True
        lblSource.Location = New Point(12, 44)
        lblSource.Name = "lblSource"
        lblSource.Size = New Size(52, 15)
        lblSource.TabIndex = 1
        lblSource.Text = "Sorgente"
        '
        'txtSource
        '
        txtSource.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtSource.Location = New Point(12, 62)
        txtSource.Name = "txtSource"
        txtSource.ReadOnly = True
        txtSource.Size = New Size(860, 23)
        txtSource.TabIndex = 2
        '
        'lblDestination
        '
        lblDestination.AutoSize = True
        lblDestination.Location = New Point(12, 94)
        lblDestination.Name = "lblDestination"
        lblDestination.Size = New Size(67, 15)
        lblDestination.TabIndex = 3
        lblDestination.Text = "Destinazione"
        '
        'txtDestination
        '
        txtDestination.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtDestination.Location = New Point(12, 112)
        txtDestination.Name = "txtDestination"
        txtDestination.ReadOnly = True
        txtDestination.Size = New Size(860, 23)
        txtDestination.TabIndex = 4
        '
        'lblLogFolder
        '
        lblLogFolder.AutoSize = True
        lblLogFolder.Location = New Point(12, 144)
        lblLogFolder.Name = "lblLogFolder"
        lblLogFolder.Size = New Size(66, 15)
        lblLogFolder.TabIndex = 5
        lblLogFolder.Text = "Cartella log"
        '
        'txtLogFolder
        '
        txtLogFolder.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtLogFolder.Location = New Point(12, 162)
        txtLogFolder.Name = "txtLogFolder"
        txtLogFolder.ReadOnly = True
        txtLogFolder.Size = New Size(860, 23)
        txtLogFolder.TabIndex = 6
        '
        'lblStatus
        '
        lblStatus.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblStatus.Location = New Point(12, 196)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(860, 23)
        lblStatus.TabIndex = 7
        lblStatus.Text = "Pronto"
        '
        'txtOutput
        '
        txtOutput.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtOutput.Font = New Font("Consolas", 9.0F, FontStyle.Regular, GraphicsUnit.Point)
        txtOutput.Location = New Point(12, 228)
        txtOutput.Multiline = True
        txtOutput.Name = "txtOutput"
        txtOutput.ReadOnly = True
        txtOutput.ScrollBars = ScrollBars.Both
        txtOutput.Size = New Size(860, 361)
        txtOutput.TabIndex = 8
        txtOutput.WordWrap = False
        '
        'btnRunBackup
        '
        btnRunBackup.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnRunBackup.Location = New Point(12, 601)
        btnRunBackup.Name = "btnRunBackup"
        btnRunBackup.Size = New Size(120, 32)
        btnRunBackup.TabIndex = 9
        btnRunBackup.Text = "Esegui backup"
        btnRunBackup.UseVisualStyleBackColor = True
        '
        'btnSimulate
        '
        btnSimulate.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnSimulate.Location = New Point(138, 601)
        btnSimulate.Name = "btnSimulate"
        btnSimulate.Size = New Size(120, 32)
        btnSimulate.TabIndex = 10
        btnSimulate.Text = "Simulazione"
        btnSimulate.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        btnStop.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnStop.Enabled = False
        btnStop.Location = New Point(264, 601)
        btnStop.Name = "btnStop"
        btnStop.Size = New Size(120, 32)
        btnStop.TabIndex = 11
        btnStop.Text = "Interrompi"
        btnStop.UseVisualStyleBackColor = True
        '
        'btnEditConfig
        '
        btnEditConfig.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnEditConfig.Location = New Point(626, 601)
        btnEditConfig.Name = "btnEditConfig"
        btnEditConfig.Size = New Size(120, 32)
        btnEditConfig.TabIndex = 12
        btnEditConfig.Text = "Impostazioni"
        btnEditConfig.UseVisualStyleBackColor = True
        '
        'btnReloadConfig
        '
        btnReloadConfig.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnReloadConfig.Location = New Point(752, 601)
        btnReloadConfig.Name = "btnReloadConfig"
        btnReloadConfig.Size = New Size(120, 32)
        btnReloadConfig.TabIndex = 13
        btnReloadConfig.Text = "Ricarica config"
        btnReloadConfig.UseVisualStyleBackColor = True
        '
        'FormMain
        '
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(884, 645)
        Controls.Add(btnReloadConfig)
        Controls.Add(btnEditConfig)
        Controls.Add(btnStop)
        Controls.Add(btnSimulate)
        Controls.Add(btnRunBackup)
        Controls.Add(txtOutput)
        Controls.Add(lblStatus)
        Controls.Add(txtLogFolder)
        Controls.Add(lblLogFolder)
        Controls.Add(txtDestination)
        Controls.Add(lblDestination)
        Controls.Add(txtSource)
        Controls.Add(lblSource)
        Controls.Add(lblTitle)
        MinimumSize = New Size(900, 684)
        Name = "FormMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "tempoBackUp - Backup manuale"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblSource As Label
    Friend WithEvents txtSource As TextBox
    Friend WithEvents lblDestination As Label
    Friend WithEvents txtDestination As TextBox
    Friend WithEvents lblLogFolder As Label
    Friend WithEvents txtLogFolder As TextBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents txtOutput As TextBox
    Friend WithEvents btnRunBackup As Button
    Friend WithEvents btnSimulate As Button
    Friend WithEvents btnStop As Button
    Friend WithEvents btnEditConfig As Button
    Friend WithEvents btnReloadConfig As Button
End Class
