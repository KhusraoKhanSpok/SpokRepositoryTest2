<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigViewer
    Inherits System.Windows.Forms.Form

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
        Me.AlphaGradientPanel1 = New BlueActivity.Controls.AlphaGradientPanel
        Me.ColorWithAlpha1 = New BlueActivity.Controls.ColorWithAlpha
        Me.ColorWithAlpha2 = New BlueActivity.Controls.ColorWithAlpha
        Me.btnAddSite = New System.Windows.Forms.Button
        Me.cmbSites = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.showFullCon = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.reportFlagsTab = New System.Windows.Forms.TabControl
        Me.speechdb = New System.Windows.Forms.TabPage
        Me.AlphaGradientPanel2 = New BlueActivity.Controls.AlphaGradientPanel
        Me.ColorWithAlpha3 = New BlueActivity.Controls.ColorWithAlpha
        Me.AlphaGradientPanel4 = New BlueActivity.Controls.AlphaGradientPanel
        Me.ColorWithAlpha4 = New BlueActivity.Controls.ColorWithAlpha
        Me.smtpMessage = New System.Windows.Forms.Label
        Me.smtpSubject = New System.Windows.Forms.Label
        Me.smtpSendFrom = New System.Windows.Forms.Label
        Me.smtpSendTo = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.smtpPortTxt = New System.Windows.Forms.Label
        Me.smtpPassTxt = New System.Windows.Forms.Label
        Me.smtpUserTxt = New System.Windows.Forms.Label
        Me.smtpSrvTxt = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label27 = New System.Windows.Forms.Label
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.tbSpeechConnString = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.deskdb = New System.Windows.Forms.TabPage
        Me.AlphaGradientPanel3 = New BlueActivity.Controls.AlphaGradientPanel
        Me.tbDeskConnectionString = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.smtp = New System.Windows.Forms.TabPage
        Me.reportDetails = New System.Windows.Forms.TabPage
        Me.AlphaGradientPanel5 = New BlueActivity.Controls.AlphaGradientPanel
        Me.Label11 = New System.Windows.Forms.Label
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.flagsCallsChannels = New System.Windows.Forms.Label
        Me.flagsRenderedFolder = New System.Windows.Forms.Label
        Me.flagsTemplateFolder = New System.Windows.Forms.Label
        Me.flagsHour = New System.Windows.Forms.Label
        Me.flagsDay = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label34 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.Label36 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.Label38 = New System.Windows.Forms.Label
        Me.MonthlyReportEnabled = New System.Windows.Forms.Label
        Me.MonthlyReport = New System.Windows.Forms.CheckBox
        Me.AlphaGradientPanel1.SuspendLayout()
        Me.reportFlagsTab.SuspendLayout()
        Me.speechdb.SuspendLayout()
        Me.AlphaGradientPanel2.SuspendLayout()
        Me.AlphaGradientPanel4.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.deskdb.SuspendLayout()
        Me.AlphaGradientPanel3.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.smtp.SuspendLayout()
        Me.reportDetails.SuspendLayout()
        Me.AlphaGradientPanel5.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AlphaGradientPanel1
        '
        Me.AlphaGradientPanel1.BackColor = System.Drawing.Color.Transparent
        Me.AlphaGradientPanel1.Border = True
        Me.AlphaGradientPanel1.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AlphaGradientPanel1.Colors.Add(Me.ColorWithAlpha1)
        Me.AlphaGradientPanel1.Colors.Add(Me.ColorWithAlpha2)
        Me.AlphaGradientPanel1.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AlphaGradientPanel1.Controls.Add(Me.btnAddSite)
        Me.AlphaGradientPanel1.Controls.Add(Me.cmbSites)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label12)
        Me.AlphaGradientPanel1.Controls.Add(Me.showFullCon)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label9)
        Me.AlphaGradientPanel1.Controls.Add(Me.reportFlagsTab)
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
        Me.AlphaGradientPanel1.Size = New System.Drawing.Size(705, 412)
        Me.AlphaGradientPanel1.TabIndex = 0
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
        'btnAddSite
        '
        Me.btnAddSite.Image = Global.SpeechReportingToolKit.My.Resources.Resources.gear_add
        Me.btnAddSite.Location = New System.Drawing.Point(664, 4)
        Me.btnAddSite.Name = "btnAddSite"
        Me.btnAddSite.Size = New System.Drawing.Size(30, 30)
        Me.btnAddSite.TabIndex = 5
        Me.btnAddSite.UseVisualStyleBackColor = True
        '
        'cmbSites
        '
        Me.cmbSites.FormattingEnabled = True
        Me.cmbSites.Location = New System.Drawing.Point(530, 8)
        Me.cmbSites.Name = "cmbSites"
        Me.cmbSites.Size = New System.Drawing.Size(121, 21)
        Me.cmbSites.TabIndex = 4
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(495, 13)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(28, 13)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Site:"
        '
        'showFullCon
        '
        Me.showFullCon.Image = Global.SpeechReportingToolKit.My.Resources.Resources.information
        Me.showFullCon.Location = New System.Drawing.Point(451, 1)
        Me.showFullCon.Name = "showFullCon"
        Me.showFullCon.Size = New System.Drawing.Size(34, 31)
        Me.showFullCon.TabIndex = 2
        Me.showFullCon.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(356, 13)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(91, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "View Full Context:"
        '
        'reportFlagsTab
        '
        Me.reportFlagsTab.Controls.Add(Me.speechdb)
        Me.reportFlagsTab.Controls.Add(Me.deskdb)
        Me.reportFlagsTab.Controls.Add(Me.smtp)
        Me.reportFlagsTab.Controls.Add(Me.reportDetails)
        Me.reportFlagsTab.Location = New System.Drawing.Point(12, 12)
        Me.reportFlagsTab.Name = "reportFlagsTab"
        Me.reportFlagsTab.SelectedIndex = 0
        Me.reportFlagsTab.Size = New System.Drawing.Size(646, 388)
        Me.reportFlagsTab.TabIndex = 0
        '
        'speechdb
        '
        Me.speechdb.Controls.Add(Me.AlphaGradientPanel2)
        Me.speechdb.Location = New System.Drawing.Point(4, 22)
        Me.speechdb.Name = "speechdb"
        Me.speechdb.Padding = New System.Windows.Forms.Padding(3)
        Me.speechdb.Size = New System.Drawing.Size(638, 362)
        Me.speechdb.TabIndex = 0
        Me.speechdb.Text = "Speech Database"
        Me.speechdb.UseVisualStyleBackColor = True
        '
        'AlphaGradientPanel2
        '
        Me.AlphaGradientPanel2.BackColor = System.Drawing.Color.Transparent
        Me.AlphaGradientPanel2.Border = True
        Me.AlphaGradientPanel2.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AlphaGradientPanel2.Colors.Add(Me.ColorWithAlpha3)
        Me.AlphaGradientPanel2.Colors.Add(Me.ColorWithAlpha4)
        Me.AlphaGradientPanel2.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AlphaGradientPanel2.Controls.Add(Me.tbSpeechConnString)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label7)
        Me.AlphaGradientPanel2.Controls.Add(Me.PictureBox1)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label2)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label1)
        Me.AlphaGradientPanel2.CornerRadius = 20
        Me.AlphaGradientPanel2.Corners = CType((((BlueActivity.Controls.Corner.TopLeft Or BlueActivity.Controls.Corner.TopRight) _
                    Or BlueActivity.Controls.Corner.BottomLeft) _
                    Or BlueActivity.Controls.Corner.BottomRight), BlueActivity.Controls.Corner)
        Me.AlphaGradientPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AlphaGradientPanel2.Gradient = True
        Me.AlphaGradientPanel2.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AlphaGradientPanel2.GradientOffset = 1.0!
        Me.AlphaGradientPanel2.GradientSize = New System.Drawing.Size(0, 0)
        Me.AlphaGradientPanel2.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AlphaGradientPanel2.Grayscale = False
        Me.AlphaGradientPanel2.Image = Global.SpeechReportingToolKit.My.Resources.Resources.data_copy
        Me.AlphaGradientPanel2.ImageAlpha = 75
        Me.AlphaGradientPanel2.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AlphaGradientPanel2.ImagePosition = BlueActivity.Controls.ImagePosition.BottomRight
        Me.AlphaGradientPanel2.ImageSize = New System.Drawing.Size(48, 48)
        Me.AlphaGradientPanel2.Location = New System.Drawing.Point(3, 3)
        Me.AlphaGradientPanel2.Name = "AlphaGradientPanel2"
        Me.AlphaGradientPanel2.Rounded = True
        Me.AlphaGradientPanel2.Size = New System.Drawing.Size(632, 356)
        Me.AlphaGradientPanel2.TabIndex = 0
        '
        'ColorWithAlpha3
        '
        Me.ColorWithAlpha3.Alpha = 255
        Me.ColorWithAlpha3.Color = System.Drawing.Color.White
        Me.ColorWithAlpha3.Parent = Me.AlphaGradientPanel5
        '
        'AlphaGradientPanel4
        '
        Me.AlphaGradientPanel4.BackColor = System.Drawing.Color.Transparent
        Me.AlphaGradientPanel4.Border = True
        Me.AlphaGradientPanel4.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AlphaGradientPanel4.Colors.Add(Me.ColorWithAlpha3)
        Me.AlphaGradientPanel4.Colors.Add(Me.ColorWithAlpha4)
        Me.AlphaGradientPanel4.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AlphaGradientPanel4.Controls.Add(Me.smtpMessage)
        Me.AlphaGradientPanel4.Controls.Add(Me.smtpSubject)
        Me.AlphaGradientPanel4.Controls.Add(Me.smtpSendFrom)
        Me.AlphaGradientPanel4.Controls.Add(Me.smtpSendTo)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label29)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label21)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label20)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label10)
        Me.AlphaGradientPanel4.Controls.Add(Me.smtpPortTxt)
        Me.AlphaGradientPanel4.Controls.Add(Me.smtpPassTxt)
        Me.AlphaGradientPanel4.Controls.Add(Me.smtpUserTxt)
        Me.AlphaGradientPanel4.Controls.Add(Me.smtpSrvTxt)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label22)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label23)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label24)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label25)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label26)
        Me.AlphaGradientPanel4.Controls.Add(Me.Label27)
        Me.AlphaGradientPanel4.Controls.Add(Me.PictureBox3)
        Me.AlphaGradientPanel4.CornerRadius = 20
        Me.AlphaGradientPanel4.Corners = CType((((BlueActivity.Controls.Corner.TopLeft Or BlueActivity.Controls.Corner.TopRight) _
                    Or BlueActivity.Controls.Corner.BottomLeft) _
                    Or BlueActivity.Controls.Corner.BottomRight), BlueActivity.Controls.Corner)
        Me.AlphaGradientPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AlphaGradientPanel4.Gradient = True
        Me.AlphaGradientPanel4.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AlphaGradientPanel4.GradientOffset = 1.0!
        Me.AlphaGradientPanel4.GradientSize = New System.Drawing.Size(0, 0)
        Me.AlphaGradientPanel4.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AlphaGradientPanel4.Grayscale = False
        Me.AlphaGradientPanel4.Image = Global.SpeechReportingToolKit.My.Resources.Resources.mail
        Me.AlphaGradientPanel4.ImageAlpha = 75
        Me.AlphaGradientPanel4.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AlphaGradientPanel4.ImagePosition = BlueActivity.Controls.ImagePosition.BottomRight
        Me.AlphaGradientPanel4.ImageSize = New System.Drawing.Size(48, 48)
        Me.AlphaGradientPanel4.Location = New System.Drawing.Point(0, 0)
        Me.AlphaGradientPanel4.Name = "AlphaGradientPanel4"
        Me.AlphaGradientPanel4.Rounded = True
        Me.AlphaGradientPanel4.Size = New System.Drawing.Size(638, 362)
        Me.AlphaGradientPanel4.TabIndex = 2
        '
        'ColorWithAlpha4
        '
        Me.ColorWithAlpha4.Alpha = 255
        Me.ColorWithAlpha4.Color = System.Drawing.SystemColors.ControlDark
        Me.ColorWithAlpha4.Parent = Me.AlphaGradientPanel5
        '
        'smtpMessage
        '
        Me.smtpMessage.AutoSize = True
        Me.smtpMessage.Location = New System.Drawing.Point(394, 81)
        Me.smtpMessage.Name = "smtpMessage"
        Me.smtpMessage.Size = New System.Drawing.Size(65, 13)
        Me.smtpMessage.TabIndex = 20
        Me.smtpMessage.Text = "<Unknown>"
        '
        'smtpSubject
        '
        Me.smtpSubject.AutoSize = True
        Me.smtpSubject.Location = New System.Drawing.Point(394, 68)
        Me.smtpSubject.Name = "smtpSubject"
        Me.smtpSubject.Size = New System.Drawing.Size(65, 13)
        Me.smtpSubject.TabIndex = 19
        Me.smtpSubject.Text = "<Unknown>"
        '
        'smtpSendFrom
        '
        Me.smtpSendFrom.AutoSize = True
        Me.smtpSendFrom.Location = New System.Drawing.Point(394, 55)
        Me.smtpSendFrom.Name = "smtpSendFrom"
        Me.smtpSendFrom.Size = New System.Drawing.Size(65, 13)
        Me.smtpSendFrom.TabIndex = 18
        Me.smtpSendFrom.Text = "<Unknown>"
        '
        'smtpSendTo
        '
        Me.smtpSendTo.AutoSize = True
        Me.smtpSendTo.Location = New System.Drawing.Point(394, 42)
        Me.smtpSendTo.Name = "smtpSendTo"
        Me.smtpSendTo.Size = New System.Drawing.Size(65, 13)
        Me.smtpSendTo.TabIndex = 17
        Me.smtpSendTo.Text = "<Unknown>"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(15, 81)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(29, 13)
        Me.Label29.TabIndex = 16
        Me.Label29.Text = "Port:"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(314, 81)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(53, 13)
        Me.Label21.TabIndex = 14
        Me.Label21.Text = "Message:"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(314, 68)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(44, 13)
        Me.Label20.TabIndex = 13
        Me.Label20.Text = "Subect:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(15, 224)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 13)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "Context:"
        '
        'smtpPortTxt
        '
        Me.smtpPortTxt.AutoSize = True
        Me.smtpPortTxt.Location = New System.Drawing.Point(100, 81)
        Me.smtpPortTxt.Name = "smtpPortTxt"
        Me.smtpPortTxt.Size = New System.Drawing.Size(65, 13)
        Me.smtpPortTxt.TabIndex = 10
        Me.smtpPortTxt.Text = "<Unknown>"
        '
        'smtpPassTxt
        '
        Me.smtpPassTxt.AutoSize = True
        Me.smtpPassTxt.Location = New System.Drawing.Point(100, 68)
        Me.smtpPassTxt.Name = "smtpPassTxt"
        Me.smtpPassTxt.Size = New System.Drawing.Size(65, 13)
        Me.smtpPassTxt.TabIndex = 8
        Me.smtpPassTxt.Text = "<Unknown>"
        '
        'smtpUserTxt
        '
        Me.smtpUserTxt.AutoSize = True
        Me.smtpUserTxt.Location = New System.Drawing.Point(100, 55)
        Me.smtpUserTxt.Name = "smtpUserTxt"
        Me.smtpUserTxt.Size = New System.Drawing.Size(65, 13)
        Me.smtpUserTxt.TabIndex = 7
        Me.smtpUserTxt.Text = "<Unknown>"
        '
        'smtpSrvTxt
        '
        Me.smtpSrvTxt.AutoSize = True
        Me.smtpSrvTxt.Location = New System.Drawing.Point(100, 42)
        Me.smtpSrvTxt.Name = "smtpSrvTxt"
        Me.smtpSrvTxt.Size = New System.Drawing.Size(65, 13)
        Me.smtpSrvTxt.TabIndex = 6
        Me.smtpSrvTxt.Text = "<Unknown>"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(314, 55)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(61, 13)
        Me.Label22.TabIndex = 5
        Me.Label22.Text = "Send From:"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(314, 42)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(51, 13)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "Send To:"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(15, 68)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(56, 13)
        Me.Label24.TabIndex = 3
        Me.Label24.Text = "Password:"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(15, 55)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(63, 13)
        Me.Label25.TabIndex = 2
        Me.Label25.Text = "User Name:"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(15, 42)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(41, 13)
        Me.Label26.TabIndex = 1
        Me.Label26.Text = "Server:"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(15, 15)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(197, 13)
        Me.Label27.TabIndex = 0
        Me.Label27.Text = "SMTP (E-mail) Configuration Information:"
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.SpeechReportingToolKit.My.Resources.Resources.SMTPcontext
        Me.PictureBox3.Location = New System.Drawing.Point(18, 240)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(427, 119)
        Me.PictureBox3.TabIndex = 11
        Me.PictureBox3.TabStop = False
        '
        'tbSpeechConnString
        '
        Me.tbSpeechConnString.Location = New System.Drawing.Point(18, 58)
        Me.tbSpeechConnString.Multiline = True
        Me.tbSpeechConnString.Name = "tbSpeechConnString"
        Me.tbSpeechConnString.Size = New System.Drawing.Size(611, 102)
        Me.tbSpeechConnString.TabIndex = 13
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 218)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Context:"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.SpeechReportingToolKit.My.Resources.Resources.SpeechDbContext
        Me.PictureBox1.Location = New System.Drawing.Point(18, 234)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(427, 119)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Connection String:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(238, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IntelliSPEECH Database Connection Information:"
        '
        'deskdb
        '
        Me.deskdb.Controls.Add(Me.AlphaGradientPanel3)
        Me.deskdb.Location = New System.Drawing.Point(4, 22)
        Me.deskdb.Name = "deskdb"
        Me.deskdb.Padding = New System.Windows.Forms.Padding(3)
        Me.deskdb.Size = New System.Drawing.Size(638, 362)
        Me.deskdb.TabIndex = 1
        Me.deskdb.Text = "Desk Database"
        Me.deskdb.UseVisualStyleBackColor = True
        '
        'AlphaGradientPanel3
        '
        Me.AlphaGradientPanel3.BackColor = System.Drawing.Color.Transparent
        Me.AlphaGradientPanel3.Border = True
        Me.AlphaGradientPanel3.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AlphaGradientPanel3.Colors.Add(Me.ColorWithAlpha3)
        Me.AlphaGradientPanel3.Colors.Add(Me.ColorWithAlpha4)
        Me.AlphaGradientPanel3.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AlphaGradientPanel3.Controls.Add(Me.tbDeskConnectionString)
        Me.AlphaGradientPanel3.Controls.Add(Me.Label8)
        Me.AlphaGradientPanel3.Controls.Add(Me.Label18)
        Me.AlphaGradientPanel3.Controls.Add(Me.Label19)
        Me.AlphaGradientPanel3.Controls.Add(Me.PictureBox2)
        Me.AlphaGradientPanel3.CornerRadius = 20
        Me.AlphaGradientPanel3.Corners = CType((((BlueActivity.Controls.Corner.TopLeft Or BlueActivity.Controls.Corner.TopRight) _
                    Or BlueActivity.Controls.Corner.BottomLeft) _
                    Or BlueActivity.Controls.Corner.BottomRight), BlueActivity.Controls.Corner)
        Me.AlphaGradientPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AlphaGradientPanel3.Gradient = True
        Me.AlphaGradientPanel3.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AlphaGradientPanel3.GradientOffset = 1.0!
        Me.AlphaGradientPanel3.GradientSize = New System.Drawing.Size(0, 0)
        Me.AlphaGradientPanel3.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AlphaGradientPanel3.Grayscale = False
        Me.AlphaGradientPanel3.Image = Global.SpeechReportingToolKit.My.Resources.Resources.data_copy
        Me.AlphaGradientPanel3.ImageAlpha = 75
        Me.AlphaGradientPanel3.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AlphaGradientPanel3.ImagePosition = BlueActivity.Controls.ImagePosition.BottomRight
        Me.AlphaGradientPanel3.ImageSize = New System.Drawing.Size(48, 48)
        Me.AlphaGradientPanel3.Location = New System.Drawing.Point(3, 3)
        Me.AlphaGradientPanel3.Name = "AlphaGradientPanel3"
        Me.AlphaGradientPanel3.Rounded = True
        Me.AlphaGradientPanel3.Size = New System.Drawing.Size(632, 356)
        Me.AlphaGradientPanel3.TabIndex = 1
        '
        'tbDeskConnectionString
        '
        Me.tbDeskConnectionString.Location = New System.Drawing.Point(18, 58)
        Me.tbDeskConnectionString.Multiline = True
        Me.tbDeskConnectionString.Name = "tbDeskConnectionString"
        Me.tbDeskConnectionString.Size = New System.Drawing.Size(611, 102)
        Me.tbDeskConnectionString.TabIndex = 14
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(15, 150)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Context:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(15, 42)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(94, 13)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "Connection String:"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(15, 15)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(224, 13)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "IntelliDESK Database Connection Information:"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.SpeechReportingToolKit.My.Resources.Resources.DeskDbContext
        Me.PictureBox2.Location = New System.Drawing.Point(18, 166)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(427, 187)
        Me.PictureBox2.TabIndex = 11
        Me.PictureBox2.TabStop = False
        '
        'smtp
        '
        Me.smtp.Controls.Add(Me.AlphaGradientPanel4)
        Me.smtp.Location = New System.Drawing.Point(4, 22)
        Me.smtp.Name = "smtp"
        Me.smtp.Size = New System.Drawing.Size(638, 362)
        Me.smtp.TabIndex = 2
        Me.smtp.Text = "SMTP (E-mail)"
        Me.smtp.UseVisualStyleBackColor = True
        '
        'reportDetails
        '
        Me.reportDetails.Controls.Add(Me.AlphaGradientPanel5)
        Me.reportDetails.Location = New System.Drawing.Point(4, 22)
        Me.reportDetails.Name = "reportDetails"
        Me.reportDetails.Padding = New System.Windows.Forms.Padding(3)
        Me.reportDetails.Size = New System.Drawing.Size(638, 362)
        Me.reportDetails.TabIndex = 3
        Me.reportDetails.Text = "Report Details"
        Me.reportDetails.UseVisualStyleBackColor = True
        '
        'AlphaGradientPanel5
        '
        Me.AlphaGradientPanel5.BackColor = System.Drawing.Color.Transparent
        Me.AlphaGradientPanel5.Border = True
        Me.AlphaGradientPanel5.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AlphaGradientPanel5.Colors.Add(Me.ColorWithAlpha3)
        Me.AlphaGradientPanel5.Colors.Add(Me.ColorWithAlpha4)
        Me.AlphaGradientPanel5.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AlphaGradientPanel5.Controls.Add(Me.MonthlyReport)
        Me.AlphaGradientPanel5.Controls.Add(Me.MonthlyReportEnabled)
        Me.AlphaGradientPanel5.Controls.Add(Me.Label11)
        Me.AlphaGradientPanel5.Controls.Add(Me.PictureBox4)
        Me.AlphaGradientPanel5.Controls.Add(Me.flagsCallsChannels)
        Me.AlphaGradientPanel5.Controls.Add(Me.flagsRenderedFolder)
        Me.AlphaGradientPanel5.Controls.Add(Me.flagsTemplateFolder)
        Me.AlphaGradientPanel5.Controls.Add(Me.flagsHour)
        Me.AlphaGradientPanel5.Controls.Add(Me.flagsDay)
        Me.AlphaGradientPanel5.Controls.Add(Me.Label33)
        Me.AlphaGradientPanel5.Controls.Add(Me.Label34)
        Me.AlphaGradientPanel5.Controls.Add(Me.Label35)
        Me.AlphaGradientPanel5.Controls.Add(Me.Label36)
        Me.AlphaGradientPanel5.Controls.Add(Me.Label37)
        Me.AlphaGradientPanel5.Controls.Add(Me.Label38)
        Me.AlphaGradientPanel5.CornerRadius = 20
        Me.AlphaGradientPanel5.Corners = CType((((BlueActivity.Controls.Corner.TopLeft Or BlueActivity.Controls.Corner.TopRight) _
                    Or BlueActivity.Controls.Corner.BottomLeft) _
                    Or BlueActivity.Controls.Corner.BottomRight), BlueActivity.Controls.Corner)
        Me.AlphaGradientPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AlphaGradientPanel5.Gradient = True
        Me.AlphaGradientPanel5.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AlphaGradientPanel5.GradientOffset = 1.0!
        Me.AlphaGradientPanel5.GradientSize = New System.Drawing.Size(0, 0)
        Me.AlphaGradientPanel5.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AlphaGradientPanel5.Grayscale = False
        Me.AlphaGradientPanel5.Image = Global.SpeechReportingToolKit.My.Resources.Resources.gear_run
        Me.AlphaGradientPanel5.ImageAlpha = 75
        Me.AlphaGradientPanel5.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AlphaGradientPanel5.ImagePosition = BlueActivity.Controls.ImagePosition.BottomRight
        Me.AlphaGradientPanel5.ImageSize = New System.Drawing.Size(48, 48)
        Me.AlphaGradientPanel5.Location = New System.Drawing.Point(3, 3)
        Me.AlphaGradientPanel5.Name = "AlphaGradientPanel5"
        Me.AlphaGradientPanel5.Rounded = True
        Me.AlphaGradientPanel5.Size = New System.Drawing.Size(632, 356)
        Me.AlphaGradientPanel5.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(3, 202)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(46, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Context:"
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.SpeechReportingToolKit.My.Resources.Resources.FlagsContext
        Me.PictureBox4.Location = New System.Drawing.Point(18, 218)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(551, 135)
        Me.PictureBox4.TabIndex = 11
        Me.PictureBox4.TabStop = False
        '
        'flagsCallsChannels
        '
        Me.flagsCallsChannels.AutoSize = True
        Me.flagsCallsChannels.Location = New System.Drawing.Point(167, 94)
        Me.flagsCallsChannels.Name = "flagsCallsChannels"
        Me.flagsCallsChannels.Size = New System.Drawing.Size(65, 13)
        Me.flagsCallsChannels.TabIndex = 10
        Me.flagsCallsChannels.Text = "<Unknown>"
        '
        'flagsRenderedFolder
        '
        Me.flagsRenderedFolder.AutoSize = True
        Me.flagsRenderedFolder.Location = New System.Drawing.Point(167, 81)
        Me.flagsRenderedFolder.Name = "flagsRenderedFolder"
        Me.flagsRenderedFolder.Size = New System.Drawing.Size(65, 13)
        Me.flagsRenderedFolder.TabIndex = 9
        Me.flagsRenderedFolder.Text = "<Unknown>"
        '
        'flagsTemplateFolder
        '
        Me.flagsTemplateFolder.AutoSize = True
        Me.flagsTemplateFolder.Location = New System.Drawing.Point(167, 68)
        Me.flagsTemplateFolder.Name = "flagsTemplateFolder"
        Me.flagsTemplateFolder.Size = New System.Drawing.Size(65, 13)
        Me.flagsTemplateFolder.TabIndex = 8
        Me.flagsTemplateFolder.Text = "<Unknown>"
        '
        'flagsHour
        '
        Me.flagsHour.AutoSize = True
        Me.flagsHour.Location = New System.Drawing.Point(167, 55)
        Me.flagsHour.Name = "flagsHour"
        Me.flagsHour.Size = New System.Drawing.Size(65, 13)
        Me.flagsHour.TabIndex = 7
        Me.flagsHour.Text = "<Unknown>"
        '
        'flagsDay
        '
        Me.flagsDay.AutoSize = True
        Me.flagsDay.Location = New System.Drawing.Point(167, 42)
        Me.flagsDay.Name = "flagsDay"
        Me.flagsDay.Size = New System.Drawing.Size(65, 13)
        Me.flagsDay.TabIndex = 6
        Me.flagsDay.Text = "<Unknown>"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(15, 95)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(128, 13)
        Me.Label33.TabIndex = 5
        Me.Label33.Text = "Calls Per Channel Report:"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(15, 81)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(89, 13)
        Me.Label34.TabIndex = 4
        Me.Label34.Text = "Rendered Folder:"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(15, 68)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(86, 13)
        Me.Label35.TabIndex = 3
        Me.Label35.Text = "Template Folder:"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(15, 55)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(68, 13)
        Me.Label36.TabIndex = 2
        Me.Label36.Text = "Hour to Run:"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(15, 42)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(68, 13)
        Me.Label37.TabIndex = 1
        Me.Label37.Text = "Day To Run:"
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(15, 15)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(217, 13)
        Me.Label38.TabIndex = 0
        Me.Label38.Text = "Report and Timing Configuration Information:"
        '
        'MonthlyReportEnabled
        '
        Me.MonthlyReportEnabled.AutoSize = True
        Me.MonthlyReportEnabled.Location = New System.Drawing.Point(15, 121)
        Me.MonthlyReportEnabled.Name = "MonthlyReportEnabled"
        Me.MonthlyReportEnabled.Size = New System.Drawing.Size(124, 13)
        Me.MonthlyReportEnabled.TabIndex = 13
        Me.MonthlyReportEnabled.Text = "Monthly Report Enabled:"
        Me.MonthlyReportEnabled.Visible = False
        '
        'MonthlyReport
        '
        Me.MonthlyReport.AutoSize = True
        Me.MonthlyReport.Location = New System.Drawing.Point(147, 122)
        Me.MonthlyReport.Name = "MonthlyReport"
        Me.MonthlyReport.Size = New System.Drawing.Size(15, 14)
        Me.MonthlyReport.TabIndex = 15
        Me.MonthlyReport.UseVisualStyleBackColor = True
        Me.MonthlyReport.Visible = False
        '
        'ConfigViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(705, 412)
        Me.Controls.Add(Me.AlphaGradientPanel1)
        Me.Name = "ConfigViewer"
        Me.Text = "ConfigViewer"
        Me.AlphaGradientPanel1.ResumeLayout(False)
        Me.AlphaGradientPanel1.PerformLayout()
        Me.reportFlagsTab.ResumeLayout(False)
        Me.speechdb.ResumeLayout(False)
        Me.AlphaGradientPanel2.ResumeLayout(False)
        Me.AlphaGradientPanel2.PerformLayout()
        Me.AlphaGradientPanel4.ResumeLayout(False)
        Me.AlphaGradientPanel4.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.deskdb.ResumeLayout(False)
        Me.AlphaGradientPanel3.ResumeLayout(False)
        Me.AlphaGradientPanel3.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.smtp.ResumeLayout(False)
        Me.reportDetails.ResumeLayout(False)
        Me.AlphaGradientPanel5.ResumeLayout(False)
        Me.AlphaGradientPanel5.PerformLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AlphaGradientPanel1 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha1 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents reportFlagsTab As System.Windows.Forms.TabControl
    Friend WithEvents speechdb As System.Windows.Forms.TabPage
    Friend WithEvents AlphaGradientPanel2 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha3 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents deskdb As System.Windows.Forms.TabPage
    Friend WithEvents ColorWithAlpha4 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents smtp As System.Windows.Forms.TabPage
    Friend WithEvents reportDetails As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AlphaGradientPanel3 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents AlphaGradientPanel4 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents smtpPortTxt As System.Windows.Forms.Label
    Friend WithEvents smtpPassTxt As System.Windows.Forms.Label
    Friend WithEvents smtpUserTxt As System.Windows.Forms.Label
    Friend WithEvents smtpSrvTxt As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents smtpMessage As System.Windows.Forms.Label
    Friend WithEvents smtpSubject As System.Windows.Forms.Label
    Friend WithEvents smtpSendFrom As System.Windows.Forms.Label
    Friend WithEvents smtpSendTo As System.Windows.Forms.Label
    Friend WithEvents AlphaGradientPanel5 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents flagsCallsChannels As System.Windows.Forms.Label
    Friend WithEvents flagsRenderedFolder As System.Windows.Forms.Label
    Friend WithEvents flagsTemplateFolder As System.Windows.Forms.Label
    Friend WithEvents flagsHour As System.Windows.Forms.Label
    Friend WithEvents flagsDay As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents showFullCon As System.Windows.Forms.Button
    Friend WithEvents cmbSites As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnAddSite As System.Windows.Forms.Button
    Friend WithEvents tbSpeechConnString As System.Windows.Forms.TextBox
    Friend WithEvents tbDeskConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents MonthlyReportEnabled As System.Windows.Forms.Label
    Friend WithEvents MonthlyReport As System.Windows.Forms.CheckBox
End Class
