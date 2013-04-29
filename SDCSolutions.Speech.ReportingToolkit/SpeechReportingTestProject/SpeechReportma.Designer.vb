<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpeechReportma
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SpeechReportma))
        Me.ColorWithAlpha1 = New BlueActivity.Controls.ColorWithAlpha
        Me.AlphaGradientPanel1 = New BlueActivity.Controls.AlphaGradientPanel
        Me.ColorWithAlpha2 = New BlueActivity.Controls.ColorWithAlpha
        Me.Label9 = New System.Windows.Forms.Label
        Me.cbSiteSelect = New System.Windows.Forms.ComboBox
        Me.AlphaGradientPanel2 = New BlueActivity.Controls.AlphaGradientPanel
        Me.ColorWithAlpha3 = New BlueActivity.Controls.ColorWithAlpha
        Me.ColorWithAlpha4 = New BlueActivity.Controls.ColorWithAlpha
        Me.viewErrorslbl = New System.Windows.Forms.Label
        Me.pleaseWaitLabel = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.quit = New System.Windows.Forms.Button
        Me.viewConfig = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.reTestConfigsButton = New System.Windows.Forms.Button
        Me.viewErrorsImg = New System.Windows.Forms.PictureBox
        Me.statusImageSMTP = New System.Windows.Forms.PictureBox
        Me.statusImageRenderedReports = New System.Windows.Forms.PictureBox
        Me.statusImageReportSource = New System.Windows.Forms.PictureBox
        Me.statusImageIntelliDeskDb = New System.Windows.Forms.PictureBox
        Me.statusImageSpeechDb = New System.Windows.Forms.PictureBox
        Me.statusImageConfigs = New System.Windows.Forms.PictureBox
        Me.statusImageReg = New System.Windows.Forms.PictureBox
        Me.force_Reports = New System.Windows.Forms.Button
        Me.viewReports = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.VersionLabel = New System.Windows.Forms.Label
        Me.AlphaGradientPanel1.SuspendLayout()
        Me.AlphaGradientPanel2.SuspendLayout()
        CType(Me.viewErrorsImg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.statusImageSMTP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.statusImageRenderedReports, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.statusImageReportSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.statusImageIntelliDeskDb, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.statusImageSpeechDb, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.statusImageConfigs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.statusImageReg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ColorWithAlpha1
        '
        Me.ColorWithAlpha1.Alpha = 255
        Me.ColorWithAlpha1.Color = System.Drawing.Color.White
        Me.ColorWithAlpha1.Parent = Me.AlphaGradientPanel1
        '
        'AlphaGradientPanel1
        '
        Me.AlphaGradientPanel1.BackColor = System.Drawing.Color.Transparent
        Me.AlphaGradientPanel1.Border = True
        Me.AlphaGradientPanel1.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AlphaGradientPanel1.Colors.Add(Me.ColorWithAlpha1)
        Me.AlphaGradientPanel1.Colors.Add(Me.ColorWithAlpha2)
        Me.AlphaGradientPanel1.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AlphaGradientPanel1.Controls.Add(Me.VersionLabel)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label10)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label9)
        Me.AlphaGradientPanel1.Controls.Add(Me.quit)
        Me.AlphaGradientPanel1.Controls.Add(Me.cbSiteSelect)
        Me.AlphaGradientPanel1.Controls.Add(Me.viewConfig)
        Me.AlphaGradientPanel1.Controls.Add(Me.Button1)
        Me.AlphaGradientPanel1.Controls.Add(Me.reTestConfigsButton)
        Me.AlphaGradientPanel1.Controls.Add(Me.AlphaGradientPanel2)
        Me.AlphaGradientPanel1.Controls.Add(Me.force_Reports)
        Me.AlphaGradientPanel1.Controls.Add(Me.viewReports)
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
        Me.AlphaGradientPanel1.ImageSize = New System.Drawing.Size(208, 126)
        Me.AlphaGradientPanel1.Location = New System.Drawing.Point(0, 0)
        Me.AlphaGradientPanel1.Name = "AlphaGradientPanel1"
        Me.AlphaGradientPanel1.Rounded = True
        Me.AlphaGradientPanel1.Size = New System.Drawing.Size(503, 423)
        Me.AlphaGradientPanel1.TabIndex = 4
        '
        'ColorWithAlpha2
        '
        Me.ColorWithAlpha2.Alpha = 255
        Me.ColorWithAlpha2.Color = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.ColorWithAlpha2.Parent = Me.AlphaGradientPanel1
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Location = New System.Drawing.Point(313, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(133, 13)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Site Select for Config Test:"
        '
        'cbSiteSelect
        '
        Me.cbSiteSelect.FormattingEnabled = True
        Me.cbSiteSelect.Location = New System.Drawing.Point(316, 31)
        Me.cbSiteSelect.Name = "cbSiteSelect"
        Me.cbSiteSelect.Size = New System.Drawing.Size(166, 21)
        Me.cbSiteSelect.TabIndex = 10
        '
        'AlphaGradientPanel2
        '
        Me.AlphaGradientPanel2.BackColor = System.Drawing.Color.Transparent
        Me.AlphaGradientPanel2.Border = True
        Me.AlphaGradientPanel2.BorderColor = System.Drawing.SystemColors.ActiveBorder
        Me.AlphaGradientPanel2.Colors.Add(Me.ColorWithAlpha3)
        Me.AlphaGradientPanel2.Colors.Add(Me.ColorWithAlpha4)
        Me.AlphaGradientPanel2.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AlphaGradientPanel2.Controls.Add(Me.viewErrorslbl)
        Me.AlphaGradientPanel2.Controls.Add(Me.viewErrorsImg)
        Me.AlphaGradientPanel2.Controls.Add(Me.pleaseWaitLabel)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label5)
        Me.AlphaGradientPanel2.Controls.Add(Me.statusImageSMTP)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label8)
        Me.AlphaGradientPanel2.Controls.Add(Me.statusImageRenderedReports)
        Me.AlphaGradientPanel2.Controls.Add(Me.statusImageReportSource)
        Me.AlphaGradientPanel2.Controls.Add(Me.statusImageIntelliDeskDb)
        Me.AlphaGradientPanel2.Controls.Add(Me.statusImageSpeechDb)
        Me.AlphaGradientPanel2.Controls.Add(Me.statusImageConfigs)
        Me.AlphaGradientPanel2.Controls.Add(Me.statusImageReg)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label7)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label6)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label4)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label3)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label2)
        Me.AlphaGradientPanel2.Controls.Add(Me.Label1)
        Me.AlphaGradientPanel2.CornerRadius = 20
        Me.AlphaGradientPanel2.Corners = CType((((BlueActivity.Controls.Corner.TopLeft Or BlueActivity.Controls.Corner.TopRight) _
                    Or BlueActivity.Controls.Corner.BottomLeft) _
                    Or BlueActivity.Controls.Corner.BottomRight), BlueActivity.Controls.Corner)
        Me.AlphaGradientPanel2.Gradient = True
        Me.AlphaGradientPanel2.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AlphaGradientPanel2.GradientOffset = 1.0!
        Me.AlphaGradientPanel2.GradientSize = New System.Drawing.Size(0, 0)
        Me.AlphaGradientPanel2.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AlphaGradientPanel2.Grayscale = False
        Me.AlphaGradientPanel2.Image = Nothing
        Me.AlphaGradientPanel2.ImageAlpha = 75
        Me.AlphaGradientPanel2.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AlphaGradientPanel2.ImagePosition = BlueActivity.Controls.ImagePosition.BottomRight
        Me.AlphaGradientPanel2.ImageSize = New System.Drawing.Size(48, 48)
        Me.AlphaGradientPanel2.Location = New System.Drawing.Point(12, 12)
        Me.AlphaGradientPanel2.Name = "AlphaGradientPanel2"
        Me.AlphaGradientPanel2.Rounded = True
        Me.AlphaGradientPanel2.Size = New System.Drawing.Size(298, 399)
        Me.AlphaGradientPanel2.TabIndex = 4
        '
        'ColorWithAlpha3
        '
        Me.ColorWithAlpha3.Alpha = 255
        Me.ColorWithAlpha3.Color = System.Drawing.SystemColors.Control
        Me.ColorWithAlpha3.Parent = Me.AlphaGradientPanel2
        '
        'ColorWithAlpha4
        '
        Me.ColorWithAlpha4.Alpha = 255
        Me.ColorWithAlpha4.Color = System.Drawing.SystemColors.ControlDark
        Me.ColorWithAlpha4.Parent = Me.AlphaGradientPanel2
        '
        'viewErrorslbl
        '
        Me.viewErrorslbl.AutoSize = True
        Me.viewErrorslbl.Location = New System.Drawing.Point(68, 77)
        Me.viewErrorslbl.Name = "viewErrorslbl"
        Me.viewErrorslbl.Size = New System.Drawing.Size(93, 13)
        Me.viewErrorslbl.TabIndex = 18
        Me.viewErrorslbl.Text = "View Error Details:"
        Me.viewErrorslbl.Visible = False
        '
        'pleaseWaitLabel
        '
        Me.pleaseWaitLabel.AutoSize = True
        Me.pleaseWaitLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pleaseWaitLabel.ForeColor = System.Drawing.Color.Orange
        Me.pleaseWaitLabel.Location = New System.Drawing.Point(58, 49)
        Me.pleaseWaitLabel.Name = "pleaseWaitLabel"
        Me.pleaseWaitLabel.Size = New System.Drawing.Size(170, 17)
        Me.pleaseWaitLabel.TabIndex = 16
        Me.pleaseWaitLabel.Text = "-Testing, Please Wait-"
        Me.pleaseWaitLabel.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(57, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(173, 20)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "-Report Engine Status-"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 259)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(100, 13)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "SMTP Mail Access:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 148)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(104, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "IS Configuration File:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 111)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(108, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "SDC Registry Entries:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 333)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(137, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Rendered Report Directory:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 296)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(124, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Report Source Directory:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 222)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "IntelliDESK Database:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 185)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IntelliSPEECH Database:"
        '
        'quit
        '
        Me.quit.BackColor = System.Drawing.Color.Transparent
        Me.quit.Image = Global.SpeechReportingToolKit.My.Resources.Resources.delete2
        Me.quit.Location = New System.Drawing.Point(402, 253)
        Me.quit.Name = "quit"
        Me.quit.Size = New System.Drawing.Size(80, 94)
        Me.quit.TabIndex = 9
        Me.quit.Text = "Quit!"
        Me.quit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.quit.UseVisualStyleBackColor = False
        '
        'viewConfig
        '
        Me.viewConfig.BackColor = System.Drawing.Color.Transparent
        Me.viewConfig.Image = Global.SpeechReportingToolKit.My.Resources.Resources.gears_view
        Me.viewConfig.Location = New System.Drawing.Point(316, 253)
        Me.viewConfig.Name = "viewConfig"
        Me.viewConfig.Size = New System.Drawing.Size(80, 94)
        Me.viewConfig.TabIndex = 8
        Me.viewConfig.Text = "View Config Values"
        Me.viewConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.viewConfig.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Transparent
        Me.Button1.Image = Global.SpeechReportingToolKit.My.Resources.Resources.mail
        Me.Button1.Location = New System.Drawing.Point(402, 167)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 94)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "E-mail Reports"
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button1.UseVisualStyleBackColor = False
        '
        'reTestConfigsButton
        '
        Me.reTestConfigsButton.BackColor = System.Drawing.Color.Transparent
        Me.reTestConfigsButton.Image = Global.SpeechReportingToolKit.My.Resources.Resources.gear_find
        Me.reTestConfigsButton.Location = New System.Drawing.Point(316, 167)
        Me.reTestConfigsButton.Name = "reTestConfigsButton"
        Me.reTestConfigsButton.Size = New System.Drawing.Size(80, 94)
        Me.reTestConfigsButton.TabIndex = 6
        Me.reTestConfigsButton.Text = "Re-Test Configs"
        Me.reTestConfigsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.reTestConfigsButton.UseVisualStyleBackColor = False
        '
        'viewErrorsImg
        '
        Me.viewErrorsImg.Image = Global.SpeechReportingToolKit.My.Resources.Resources.gear_warning
        Me.viewErrorsImg.Location = New System.Drawing.Point(182, 69)
        Me.viewErrorsImg.Name = "viewErrorsImg"
        Me.viewErrorsImg.Size = New System.Drawing.Size(32, 31)
        Me.viewErrorsImg.TabIndex = 17
        Me.viewErrorsImg.TabStop = False
        Me.viewErrorsImg.Visible = False
        '
        'statusImageSMTP
        '
        Me.statusImageSMTP.Image = CType(resources.GetObject("statusImageSMTP.Image"), System.Drawing.Image)
        Me.statusImageSMTP.Location = New System.Drawing.Point(182, 259)
        Me.statusImageSMTP.Name = "statusImageSMTP"
        Me.statusImageSMTP.Size = New System.Drawing.Size(32, 31)
        Me.statusImageSMTP.TabIndex = 14
        Me.statusImageSMTP.TabStop = False
        '
        'statusImageRenderedReports
        '
        Me.statusImageRenderedReports.Image = CType(resources.GetObject("statusImageRenderedReports.Image"), System.Drawing.Image)
        Me.statusImageRenderedReports.Location = New System.Drawing.Point(182, 333)
        Me.statusImageRenderedReports.Name = "statusImageRenderedReports"
        Me.statusImageRenderedReports.Size = New System.Drawing.Size(32, 31)
        Me.statusImageRenderedReports.TabIndex = 12
        Me.statusImageRenderedReports.TabStop = False
        '
        'statusImageReportSource
        '
        Me.statusImageReportSource.Image = CType(resources.GetObject("statusImageReportSource.Image"), System.Drawing.Image)
        Me.statusImageReportSource.Location = New System.Drawing.Point(182, 296)
        Me.statusImageReportSource.Name = "statusImageReportSource"
        Me.statusImageReportSource.Size = New System.Drawing.Size(32, 31)
        Me.statusImageReportSource.TabIndex = 11
        Me.statusImageReportSource.TabStop = False
        '
        'statusImageIntelliDeskDb
        '
        Me.statusImageIntelliDeskDb.Image = CType(resources.GetObject("statusImageIntelliDeskDb.Image"), System.Drawing.Image)
        Me.statusImageIntelliDeskDb.Location = New System.Drawing.Point(182, 222)
        Me.statusImageIntelliDeskDb.Name = "statusImageIntelliDeskDb"
        Me.statusImageIntelliDeskDb.Size = New System.Drawing.Size(32, 31)
        Me.statusImageIntelliDeskDb.TabIndex = 10
        Me.statusImageIntelliDeskDb.TabStop = False
        '
        'statusImageSpeechDb
        '
        Me.statusImageSpeechDb.Image = CType(resources.GetObject("statusImageSpeechDb.Image"), System.Drawing.Image)
        Me.statusImageSpeechDb.Location = New System.Drawing.Point(182, 185)
        Me.statusImageSpeechDb.Name = "statusImageSpeechDb"
        Me.statusImageSpeechDb.Size = New System.Drawing.Size(32, 31)
        Me.statusImageSpeechDb.TabIndex = 9
        Me.statusImageSpeechDb.TabStop = False
        '
        'statusImageConfigs
        '
        Me.statusImageConfigs.Image = CType(resources.GetObject("statusImageConfigs.Image"), System.Drawing.Image)
        Me.statusImageConfigs.Location = New System.Drawing.Point(182, 148)
        Me.statusImageConfigs.Name = "statusImageConfigs"
        Me.statusImageConfigs.Size = New System.Drawing.Size(32, 31)
        Me.statusImageConfigs.TabIndex = 8
        Me.statusImageConfigs.TabStop = False
        '
        'statusImageReg
        '
        Me.statusImageReg.Image = CType(resources.GetObject("statusImageReg.Image"), System.Drawing.Image)
        Me.statusImageReg.Location = New System.Drawing.Point(182, 111)
        Me.statusImageReg.Name = "statusImageReg"
        Me.statusImageReg.Size = New System.Drawing.Size(32, 31)
        Me.statusImageReg.TabIndex = 7
        Me.statusImageReg.TabStop = False
        '
        'force_Reports
        '
        Me.force_Reports.BackColor = System.Drawing.Color.Transparent
        Me.force_Reports.Image = Global.SpeechReportingToolKit.My.Resources.Resources.gears_run
        Me.force_Reports.Location = New System.Drawing.Point(316, 81)
        Me.force_Reports.Name = "force_Reports"
        Me.force_Reports.Size = New System.Drawing.Size(80, 94)
        Me.force_Reports.TabIndex = 2
        Me.force_Reports.Text = "Force Reports"
        Me.force_Reports.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.force_Reports.UseVisualStyleBackColor = False
        '
        'viewReports
        '
        Me.viewReports.BackColor = System.Drawing.Color.Transparent
        Me.viewReports.Image = Global.SpeechReportingToolKit.My.Resources.Resources.find
        Me.viewReports.Location = New System.Drawing.Point(402, 81)
        Me.viewReports.Name = "viewReports"
        Me.viewReports.Size = New System.Drawing.Size(80, 94)
        Me.viewReports.TabIndex = 3
        Me.viewReports.Text = "View Reports"
        Me.viewReports.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.viewReports.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.viewReports.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(326, 376)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(0, 13)
        Me.Label10.TabIndex = 20
        '
        'VersionLabel
        '
        Me.VersionLabel.AutoSize = True
        Me.VersionLabel.Location = New System.Drawing.Point(342, 376)
        Me.VersionLabel.Name = "VersionLabel"
        Me.VersionLabel.Size = New System.Drawing.Size(0, 13)
        Me.VersionLabel.TabIndex = 21
        '
        'SpeechReportma
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(503, 423)
        Me.Controls.Add(Me.AlphaGradientPanel1)
        Me.Name = "SpeechReportma"
        Me.Text = "Amcom Speech Reporting Services Toolkit"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.AlphaGradientPanel1.ResumeLayout(False)
        Me.AlphaGradientPanel1.PerformLayout()
        Me.AlphaGradientPanel2.ResumeLayout(False)
        Me.AlphaGradientPanel2.PerformLayout()
        CType(Me.viewErrorsImg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.statusImageSMTP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.statusImageRenderedReports, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.statusImageReportSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.statusImageIntelliDeskDb, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.statusImageSpeechDb, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.statusImageConfigs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.statusImageReg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents force_Reports As System.Windows.Forms.Button
    Friend WithEvents viewReports As System.Windows.Forms.Button
    Friend WithEvents AlphaGradientPanel1 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha1 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents AlphaGradientPanel2 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha3 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents ColorWithAlpha4 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents statusImageReg As System.Windows.Forms.PictureBox
    Friend WithEvents statusImageConfigs As System.Windows.Forms.PictureBox
    Friend WithEvents statusImageSpeechDb As System.Windows.Forms.PictureBox
    Friend WithEvents statusImageReportSource As System.Windows.Forms.PictureBox
    Friend WithEvents statusImageIntelliDeskDb As System.Windows.Forms.PictureBox
    Friend WithEvents statusImageRenderedReports As System.Windows.Forms.PictureBox
    Friend WithEvents statusImageSMTP As System.Windows.Forms.PictureBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents reTestConfigsButton As System.Windows.Forms.Button
    Friend WithEvents pleaseWaitLabel As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents viewConfig As System.Windows.Forms.Button
    Friend WithEvents quit As System.Windows.Forms.Button
    Friend WithEvents viewErrorsImg As System.Windows.Forms.PictureBox
    Friend WithEvents viewErrorslbl As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cbSiteSelect As System.Windows.Forms.ComboBox
    Friend WithEvents VersionLabel As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label

End Class
