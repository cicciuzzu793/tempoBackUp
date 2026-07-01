Imports System.IO

Public Class FormConfig
    Private ReadOnly _configPath As String

    Public Sub New(configPath As String, config As BackupConfig)
        InitializeComponent()
        _configPath = configPath
        LoadFromConfig(config)
        AddHandler btnSave.Click, AddressOf btnSave_Click
        AddHandler btnBrowseSource.Click, Sub() BrowseFolder(txtSourceRoot)
        AddHandler btnBrowseDestination.Click, Sub() BrowseFolder(txtDestinationRoot)
        AddHandler btnBrowseLog.Click, Sub() BrowseFolder(txtLogFolder)
        AddHandler cmbAppDataMode.SelectedIndexChanged, AddressOf UpdateAppDataFieldsState
        UpdateAppDataFieldsState()
    End Sub

    Private Sub LoadFromConfig(config As BackupConfig)
        txtSourceRoot.Text = config.SourceRoot
        txtDestinationRoot.Text = config.DestinationRoot
        txtLogFolder.Text = config.LogFolder
        txtIncludedFolders.Text = LinesFromList(config.IncludedFolders)
        txtExcludedFolders.Text = LinesFromList(config.ExcludedFolders)
        txtExcludedFiles.Text = LinesFromList(config.ExcludedFiles)
        txtIncludedAppDataFolders.Text = LinesFromList(config.IncludedAppDataFolders)

        Dim modeIndex = cmbAppDataMode.Items.IndexOf(config.AppDataMode)
        cmbAppDataMode.SelectedIndex = If(modeIndex >= 0, modeIndex, 0)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs)
        Dim config = BuildConfigFromForm()
        Dim validation = PathValidator.ValidateConfiguration(config, simulation:=True)

        If Not validation.IsValid Then
            MessageBox.Show(
                validation.ErrorMessage,
                "Configurazione non valida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If

        Try
            config.SaveToFile(_configPath)
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show(
                $"Impossibile salvare config.json:{Environment.NewLine}{ex.Message}",
                "Errore salvataggio",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function BuildConfigFromForm() As BackupConfig
        Return New BackupConfig With {
            .SourceRoot = txtSourceRoot.Text.Trim(),
            .DestinationRoot = txtDestinationRoot.Text.Trim(),
            .LogFolder = txtLogFolder.Text.Trim(),
            .IncludedFolders = ParseLines(txtIncludedFolders.Text),
            .ExcludedFolders = ParseLines(txtExcludedFolders.Text),
            .ExcludedFiles = ParseLines(txtExcludedFiles.Text),
            .AppDataMode = cmbAppDataMode.SelectedItem?.ToString(),
            .IncludedAppDataFolders = ParseLines(txtIncludedAppDataFolders.Text)
        }
    End Function

    Private Sub UpdateAppDataFieldsState()
        Dim isSelective = String.Equals(cmbAppDataMode.SelectedItem?.ToString(), "Selective", StringComparison.OrdinalIgnoreCase)
        txtIncludedAppDataFolders.Enabled = isSelective
        lblIncludedAppDataFolders.Enabled = isSelective
    End Sub

    Private Sub BrowseFolder(target As TextBox)
        Using dialog As New FolderBrowserDialog()
            If Not String.IsNullOrWhiteSpace(target.Text) AndAlso Not PathValidator.IsUncPath(target.Text) AndAlso Directory.Exists(target.Text) Then
                dialog.SelectedPath = target.Text
            End If

            If dialog.ShowDialog(Me) = DialogResult.OK Then
                target.Text = dialog.SelectedPath
            End If
        End Using
    End Sub

    Private Shared Function LinesFromList(items As IEnumerable(Of String)) As String
        If items Is Nothing Then
            Return String.Empty
        End If

        Return String.Join(Environment.NewLine, items.Where(Function(item) Not String.IsNullOrWhiteSpace(item)))
    End Function

    Private Shared Function ParseLines(text As String) As List(Of String)
        If String.IsNullOrWhiteSpace(text) Then
            Return New List(Of String)()
        End If

        Return text.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries).
            Select(Function(line) line.Trim()).
            Where(Function(line) Not String.IsNullOrWhiteSpace(line)).
            ToList()
    End Function
End Class
