<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MailReportForm
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
        Me.sendMail = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.bodyTxt = New System.Windows.Forms.TextBox
        Me.subjectTxt = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.mailToTxt = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.removeAttatchment = New System.Windows.Forms.Button
        Me.attatchmentList = New System.Windows.Forms.ListBox
        Me.addAttatchmentButton = New System.Windows.Forms.Button
        Me.attatchmentSelector = New System.Windows.Forms.OpenFileDialog
        Me.AlphaGradientPanel1.SuspendLayout()
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
        Me.AlphaGradientPanel1.Controls.Add(Me.sendMail)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label3)
        Me.AlphaGradientPanel1.Controls.Add(Me.bodyTxt)
        Me.AlphaGradientPanel1.Controls.Add(Me.subjectTxt)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label2)
        Me.AlphaGradientPanel1.Controls.Add(Me.mailToTxt)
        Me.AlphaGradientPanel1.Controls.Add(Me.Label1)
        Me.AlphaGradientPanel1.Controls.Add(Me.removeAttatchment)
        Me.AlphaGradientPanel1.Controls.Add(Me.attatchmentList)
        Me.AlphaGradientPanel1.Controls.Add(Me.addAttatchmentButton)
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
        Me.AlphaGradientPanel1.Image = Global.SpeechReportingToolKit.My.Resources.Resources.mail
        Me.AlphaGradientPanel1.ImageAlpha = 75
        Me.AlphaGradientPanel1.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AlphaGradientPanel1.ImagePosition = BlueActivity.Controls.ImagePosition.BottomRight
        Me.AlphaGradientPanel1.ImageSize = New System.Drawing.Size(48, 48)
        Me.AlphaGradientPanel1.Location = New System.Drawing.Point(0, 0)
        Me.AlphaGradientPanel1.Name = "AlphaGradientPanel1"
        Me.AlphaGradientPanel1.Rounded = True
        Me.AlphaGradientPanel1.Size = New System.Drawing.Size(610, 341)
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
        'sendMail
        '
        Me.sendMail.Image = Global.SpeechReportingToolKit.My.Resources.Resources.mail_out
        Me.sendMail.Location = New System.Drawing.Point(518, 184)
        Me.sendMail.Name = "sendMail"
        Me.sendMail.Size = New System.Drawing.Size(80, 80)
        Me.sendMail.TabIndex = 17
        Me.sendMail.Text = "Send E-mail"
        Me.sendMail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.sendMail.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 243)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Body:"
        '
        'bodyTxt
        '
        Me.bodyTxt.Location = New System.Drawing.Point(63, 243)
        Me.bodyTxt.Multiline = True
        Me.bodyTxt.Name = "bodyTxt"
        Me.bodyTxt.Size = New System.Drawing.Size(253, 67)
        Me.bodyTxt.TabIndex = 15
        '
        'subjectTxt
        '
        Me.subjectTxt.Location = New System.Drawing.Point(63, 217)
        Me.subjectTxt.Name = "subjectTxt"
        Me.subjectTxt.Size = New System.Drawing.Size(179, 20)
        Me.subjectTxt.TabIndex = 14
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 220)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Subject:"
        '
        'mailToTxt
        '
        Me.mailToTxt.Location = New System.Drawing.Point(63, 191)
        Me.mailToTxt.Name = "mailToTxt"
        Me.mailToTxt.Size = New System.Drawing.Size(179, 20)
        Me.mailToTxt.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 194)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Mail To:"
        '
        'removeAttatchment
        '
        Me.removeAttatchment.Image = Global.SpeechReportingToolKit.My.Resources.Resources.mail_delete
        Me.removeAttatchment.Location = New System.Drawing.Point(518, 98)
        Me.removeAttatchment.Name = "removeAttatchment"
        Me.removeAttatchment.Size = New System.Drawing.Size(80, 80)
        Me.removeAttatchment.TabIndex = 10
        Me.removeAttatchment.Text = "Remove Attatchment"
        Me.removeAttatchment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.removeAttatchment.UseVisualStyleBackColor = True
        '
        'attatchmentList
        '
        Me.attatchmentList.FormattingEnabled = True
        Me.attatchmentList.Location = New System.Drawing.Point(12, 12)
        Me.attatchmentList.Name = "attatchmentList"
        Me.attatchmentList.Size = New System.Drawing.Size(500, 173)
        Me.attatchmentList.TabIndex = 9
        '
        'addAttatchmentButton
        '
        Me.addAttatchmentButton.Image = Global.SpeechReportingToolKit.My.Resources.Resources.mail_add
        Me.addAttatchmentButton.Location = New System.Drawing.Point(518, 12)
        Me.addAttatchmentButton.Name = "addAttatchmentButton"
        Me.addAttatchmentButton.Size = New System.Drawing.Size(80, 80)
        Me.addAttatchmentButton.TabIndex = 8
        Me.addAttatchmentButton.Text = "Add Attatchment"
        Me.addAttatchmentButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.addAttatchmentButton.UseVisualStyleBackColor = True
        '
        'attatchmentSelector
        '
        Me.attatchmentSelector.FileName = "OpenFileDialog1"
        Me.attatchmentSelector.Title = "Select a Report:"
        '
        'MailReportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(610, 341)
        Me.Controls.Add(Me.AlphaGradientPanel1)
        Me.Name = "MailReportForm"
        Me.Text = "E-mail a Report"
        Me.AlphaGradientPanel1.ResumeLayout(False)
        Me.AlphaGradientPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AlphaGradientPanel1 As BlueActivity.Controls.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha1 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As BlueActivity.Controls.ColorWithAlpha
    Friend WithEvents attatchmentSelector As System.Windows.Forms.OpenFileDialog
    Friend WithEvents addAttatchmentButton As System.Windows.Forms.Button
    Friend WithEvents attatchmentList As System.Windows.Forms.ListBox
    Friend WithEvents removeAttatchment As System.Windows.Forms.Button
    Friend WithEvents mailToTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents bodyTxt As System.Windows.Forms.TextBox
    Friend WithEvents subjectTxt As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents sendMail As System.Windows.Forms.Button
End Class
