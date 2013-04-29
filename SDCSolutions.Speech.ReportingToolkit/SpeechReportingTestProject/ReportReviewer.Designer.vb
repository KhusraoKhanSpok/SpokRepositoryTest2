<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReportReviewer
    Inherits System.Windows.Forms.Form

    Friend WithEvents reportViewer As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents reportSelecter As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents genReportButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents endDatePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents startDatePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents reportSelector As System.Windows.Forms.ComboBox
    Friend WithEvents generateButton As System.Windows.Forms.Button
    Friend WithEvents AlphaGradientPanel1 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha1 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents reportLocLabel As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSites As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReportReviewer))
        Me.reportViewer = New Microsoft.Reporting.WinForms.ReportViewer
        Me.reportSelector = New System.Windows.Forms.ComboBox
        Me.endDatePicker = New System.Windows.Forms.DateTimePicker
        Me.startDatePicker = New System.Windows.Forms.DateTimePicker
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
        Me.reportSelecter = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.AlphaGradientPanel1 = New BlueActivity.Controls.AlphaGradientPanel
        Me.ColorWithAlpha1 = New BlueActivity.Controls.ColorWithAlpha
        Me.ColorWithAlpha2 = New BlueActivity.Controls.ColorWithAlpha
        Me.syncAlts = New System.Windows.Forms.CheckBox
        Me.chkbxDebug = New System.Windows.Forms.CheckBox
        Me.cmbSites = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.generateButton = New System.Windows.Forms.Button
        Me.reportLocLabel = New System.Windows.Forms.Label
        Me.genReportButton = New System.Windows.Forms.ToolStripButton
        Me.DateTips = New System.Windows.Forms.ToolTip(Me.components)
        Me.AlphaGradientPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'reportViewer
        '
        Me.reportViewer.AutoScroll = True
        Me.reportViewer.AutoSize = True
        Me.reportViewer.Location = New System.Drawing.Point(15, 90)
        Me.reportViewer.Name = "reportViewer"
        Me.reportViewer.Size = New System.Drawing.Size(850, 900)
        Me.reportViewer.TabIndex = 0
        Me.reportViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.FullPage
        '
        'reportSelector
        '
        Me.reportSelector.FormattingEnabled = True
        Me.reportSelector.Items.AddRange(New Object() {"Transaction", "Transfer", "Channel"})
        Me.reportSelector.Location = New System.Drawing.Point(102, 52)
        Me.reportSelector.Name = "reportSelector"
        Me.reportSelector.Size = New System.Drawing.Size(121, 21)
        Me.reportSelector.TabIndex = 5
        '
        'endDatePicker
        '
        Me.endDatePicker.Location = New System.Drawing.Point(312, 53)
        Me.endDatePicker.Name = "endDatePicker"
        Me.endDatePicker.Size = New System.Drawing.Size(200, 20)
        Me.endDatePicker.TabIndex = 3
        Me.DateTips.SetToolTip(Me.endDatePicker, "End Date=The date in which the data should report to." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Time is automatically calc" & _
                "ulated to 11:59pm")
        '
        'startDatePicker
        '
        Me.startDatePicker.Location = New System.Drawing.Point(312, 25)
        Me.startDatePicker.MaxDate = New Date(9998, 12, 9, 0, 0, 0, 0)
        Me.startDatePicker.Name = "startDatePicker"
        Me.startDatePicker.Size = New System.Drawing.Size(200, 20)
        Me.startDatePicker.TabIndex = 1
        Me.DateTips.SetToolTip(Me.startDatePicker, "State Date= The date in which the reporting should begin." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Time is automatically " & _
                "set to 00:00 hours")
        Me.startDatePicker.Value = New Date(2011, 12, 9, 0, 0, 0, 0)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(85, 22)
        Me.ToolStripLabel1.Text = "Select a Report:"
        '
        'reportSelecter
        '
        Me.reportSelecter.Items.AddRange(New Object() {"Transaction", "Transfer", "Channel"})
        Me.reportSelecter.Name = "reportSelecter"
        Me.reportSelecter.Size = New System.Drawing.Size(121, 23)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'AlphaGradientPanel1
        '
        Me.AlphaGradientPanel1.AutoSize = True
        Me.AlphaGradientPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AlphaGradientPanel1.BackColor = System.Drawing.Color.Transparent
        Me.AlphaGradientPanel1.Border = True
        Me.AlphaGradientPanel1.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AlphaGradientPanel1.Colors.Add(Me.ColorWithAlpha1)
        Me.AlphaGradientPanel1.Colors.Add(Me.ColorWithAlpha2)
        Me.AlphaGradientPanel1.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AlphaGradientPanel1.Controls.Add(Me.syncAlts)
        Me.AlphaGradientPanel1.Controls.Add(Me.chkbxDebug)
        Me.AlphaGradientPanel1.Controls.Add(Me.cmbSites)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label4)
        Me.AlphaGradientPanel1.Controls.Add(Me.startDatePicker)
        Me.AlphaGradientPanel1.Controls.Add(Me.endDatePicker)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label3)
        Me.AlphaGradientPanel1.Controls.Add(Me.reportSelector)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label2)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label1)
        Me.AlphaGradientPanel1.Controls.Add(Me.generateButton)
        Me.AlphaGradientPanel1.Controls.Add(Me.reportLocLabel)
        Me.AlphaGradientPanel1.Controls.Add(Me.reportViewer)
        Me.AlphaGradientPanel1.CornerRadius = 20
        Me.AlphaGradientPanel1.Corners = CType((((BlueActivity.Controls.Corner.TopLeft Or BlueActivity.Controls.Corner.TopRight) _
                    Or BlueActivity.Controls.Corner.BottomLeft) _
                    Or BlueActivity.Controls.Corner.BottomRight), BlueActivity.Controls.Corner)
        Me.AlphaGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AlphaGradientPanel1.Gradient = True
        Me.AlphaGradientPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AlphaGradientPanel1.GradientOffset = 1.0!
        Me.AlphaGradientPanel1.GradientSize = New System.Drawing.Size(0, 0)
        Me.AlphaGradientPanel1.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AlphaGradientPanel1.Grayscale = False
        Me.AlphaGradientPanel1.Image = Nothing
        Me.AlphaGradientPanel1.ImageAlpha = 75
        Me.AlphaGradientPanel1.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AlphaGradientPanel1.ImagePosition = BlueActivity.Controls.ImagePosition.BottomRight
        Me.AlphaGradientPanel1.ImageSize = New System.Drawing.Size(48, 48)
        Me.AlphaGradientPanel1.Location = New System.Drawing.Point(0, 0)
        Me.AlphaGradientPanel1.Name = "AlphaGradientPanel1"
        Me.AlphaGradientPanel1.Rounded = True
        Me.AlphaGradientPanel1.Size = New System.Drawing.Size(867, 912)
        Me.AlphaGradientPanel1.TabIndex = 9
        '
        'ColorWithAlpha1
        '
        Me.ColorWithAlpha1.Alpha = 255
        Me.ColorWithAlpha1.Color = System.Drawing.Color.White
        Me.ColorWithAlpha1.Parent = Me.AlphaGradientPanel1
        '
        'ColorWithAlpha2
        '
        Me.ColorWithAlpha2.Alpha = 255
        Me.ColorWithAlpha2.Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ColorWithAlpha2.Parent = Me.AlphaGradientPanel1
        '
        'syncAlts
        '
        Me.syncAlts.AutoSize = True
        Me.syncAlts.Checked = True
        Me.syncAlts.CheckState = System.Windows.Forms.CheckState.Checked
        Me.syncAlts.Location = New System.Drawing.Point(639, 53)
        Me.syncAlts.Name = "syncAlts"
        Me.syncAlts.Size = New System.Drawing.Size(128, 17)
        Me.syncAlts.TabIndex = 11
        Me.syncAlts.Text = "Sync AlternateNames"
        Me.DateTips.SetToolTip(Me.syncAlts, resources.GetString("syncAlts.ToolTip"))
        Me.syncAlts.UseVisualStyleBackColor = True
        Me.syncAlts.Visible = False
        '
        'chkbxDebug
        '
        Me.chkbxDebug.AutoSize = True
        Me.chkbxDebug.Location = New System.Drawing.Point(639, 26)
        Me.chkbxDebug.Name = "chkbxDebug"
        Me.chkbxDebug.Size = New System.Drawing.Size(117, 17)
        Me.chkbxDebug.TabIndex = 10
        Me.chkbxDebug.Text = "Transaction Debug"
        Me.chkbxDebug.UseVisualStyleBackColor = True
        '
        'cmbSites
        '
        Me.cmbSites.FormattingEnabled = True
        Me.cmbSites.Location = New System.Drawing.Point(102, 25)
        Me.cmbSites.Name = "cmbSites"
        Me.cmbSites.Size = New System.Drawing.Size(121, 21)
        Me.cmbSites.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Select Site:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(251, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "End Date:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(250, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Start Date:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Select Report:"
        '
        'generateButton
        '
        Me.generateButton.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.generateButton.Image = Global.SpeechReportingToolKit.My.Resources.Resources.gear_run
        Me.generateButton.Location = New System.Drawing.Point(546, 18)
        Me.generateButton.Name = "generateButton"
        Me.generateButton.Size = New System.Drawing.Size(75, 56)
        Me.generateButton.TabIndex = 7
        Me.generateButton.Text = "Generate"
        Me.generateButton.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.generateButton.UseVisualStyleBackColor = True
        '
        'reportLocLabel
        '
        Me.reportLocLabel.AutoSize = True
        Me.reportLocLabel.Location = New System.Drawing.Point(229, 4)
        Me.reportLocLabel.Name = "reportLocLabel"
        Me.reportLocLabel.Size = New System.Drawing.Size(0, 13)
        Me.reportLocLabel.TabIndex = 0
        '
        'genReportButton
        '
        Me.genReportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.genReportButton.Image = Global.SpeechReportingToolKit.My.Resources.Resources.gear_run
        Me.genReportButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.genReportButton.Name = "genReportButton"
        Me.genReportButton.Size = New System.Drawing.Size(23, 22)
        Me.genReportButton.Text = "GenerateReport"
        '
        'DateTips
        '
        Me.DateTips.AutomaticDelay = 200
        Me.DateTips.AutoPopDelay = 5000
        Me.DateTips.InitialDelay = 200
        Me.DateTips.ReshowDelay = 40
        Me.DateTips.ShowAlways = True
        Me.DateTips.ToolTipTitle = "ReportTips"
        '
        'ReportReviewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(867, 912)
        Me.Controls.Add(Me.AlphaGradientPanel1)
        Me.Name = "ReportReviewer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Report Reviewer"
        Me.AlphaGradientPanel1.ResumeLayout(False)
        Me.AlphaGradientPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkbxDebug As System.Windows.Forms.CheckBox
    Friend WithEvents syncAlts As System.Windows.Forms.CheckBox
    Friend WithEvents DateTips As System.Windows.Forms.ToolTip
    'Friend WithEvents reportViewer As Microsoft.Reporting.WinForms.ReportViewer
    'Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    'Friend WithEvents reportSelecter As System.Windows.Forms.ToolStripComboBox
    'Friend WithEvents genReportButton As System.Windows.Forms.ToolStripButton
    'Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    'Friend WithEvents endDatePicker As System.Windows.Forms.DateTimePicker
    'Friend WithEvents startDatePicker As System.Windows.Forms.DateTimePicker
    'Friend WithEvents reportSelector As System.Windows.Forms.ComboBox
    'Friend WithEvents generateButton As System.Windows.Forms.Button
    'Friend WithEvents AlphaGradientPanel1 As BlueActivity.Controls.AlphaGradientPanel
    'Friend WithEvents ColorWithAlpha1 As BlueActivity.Controls.ColorWithAlpha
    'Friend WithEvents ColorWithAlpha2 As BlueActivity.Controls.ColorWithAlpha
    'Friend WithEvents reportLocLabel As System.Windows.Forms.Label
    'Friend WithEvents Label3 As System.Windows.Forms.Label
    'Friend WithEvents Label2 As System.Windows.Forms.Label
    'Friend WithEvents Label1 As System.Windows.Forms.Label
    'Friend WithEvents cmbSites As System.Windows.Forms.ComboBox
    'Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
