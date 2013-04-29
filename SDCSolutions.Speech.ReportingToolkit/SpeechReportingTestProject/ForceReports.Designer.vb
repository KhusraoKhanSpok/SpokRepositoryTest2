<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ForceReports
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
        Me.startDatePicker = New System.Windows.Forms.DateTimePicker
        Me.endDatePicker = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cancelButton = New System.Windows.Forms.Button
        Me.makeReports = New System.Windows.Forms.Button
        Me.transaction = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Channel = New System.Windows.Forms.CheckBox
        Me.transfer = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.email = New System.Windows.Forms.CheckBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.cmbSites = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.chkbxDebug = New System.Windows.Forms.CheckBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'startDatePicker
        '
        Me.startDatePicker.Location = New System.Drawing.Point(75, 18)
        Me.startDatePicker.Name = "startDatePicker"
        Me.startDatePicker.Size = New System.Drawing.Size(200, 20)
        Me.startDatePicker.TabIndex = 0
        '
        'endDatePicker
        '
        Me.endDatePicker.Location = New System.Drawing.Point(75, 44)
        Me.endDatePicker.Name = "endDatePicker"
        Me.endDatePicker.Size = New System.Drawing.Size(200, 20)
        Me.endDatePicker.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Start Date:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "End Date:"
        '
        'cancelButton
        '
        Me.cancelButton.Location = New System.Drawing.Point(283, 275)
        Me.cancelButton.Name = "cancelButton"
        Me.cancelButton.Size = New System.Drawing.Size(75, 23)
        Me.cancelButton.TabIndex = 5
        Me.cancelButton.Text = "Cancel"
        Me.cancelButton.UseVisualStyleBackColor = True
        '
        'makeReports
        '
        Me.makeReports.Location = New System.Drawing.Point(202, 275)
        Me.makeReports.Name = "makeReports"
        Me.makeReports.Size = New System.Drawing.Size(75, 23)
        Me.makeReports.TabIndex = 6
        Me.makeReports.Text = "Generate"
        Me.makeReports.UseVisualStyleBackColor = True
        '
        'transaction
        '
        Me.transaction.AutoSize = True
        Me.transaction.Checked = True
        Me.transaction.CheckState = System.Windows.Forms.CheckState.Checked
        Me.transaction.Location = New System.Drawing.Point(53, 19)
        Me.transaction.Name = "transaction"
        Me.transaction.Size = New System.Drawing.Size(82, 17)
        Me.transaction.TabIndex = 7
        Me.transaction.Text = "Transaction"
        Me.transaction.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Channel)
        Me.GroupBox1.Controls.Add(Me.transfer)
        Me.GroupBox1.Controls.Add(Me.transaction)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 155)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(346, 46)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Generate Which Reports?"
        '
        'Channel
        '
        Me.Channel.AutoSize = True
        Me.Channel.Location = New System.Drawing.Point(212, 19)
        Me.Channel.Name = "Channel"
        Me.Channel.Size = New System.Drawing.Size(92, 17)
        Me.Channel.TabIndex = 9
        Me.Channel.Text = "Calls/Channel"
        Me.Channel.UseVisualStyleBackColor = True
        '
        'transfer
        '
        Me.transfer.AutoSize = True
        Me.transfer.Checked = True
        Me.transfer.CheckState = System.Windows.Forms.CheckState.Checked
        Me.transfer.Location = New System.Drawing.Point(141, 19)
        Me.transfer.Name = "transfer"
        Me.transfer.Size = New System.Drawing.Size(65, 17)
        Me.transfer.TabIndex = 8
        Me.transfer.Text = "Transfer"
        Me.transfer.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.startDatePicker)
        Me.GroupBox2.Controls.Add(Me.endDatePicker)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(346, 75)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Select the date range for the reports:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.email)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 207)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(184, 40)
        Me.GroupBox3.TabIndex = 10
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Email Forced Reports:"
        '
        'email
        '
        Me.email.AutoSize = True
        Me.email.Location = New System.Drawing.Point(53, 17)
        Me.email.Name = "email"
        Me.email.Size = New System.Drawing.Size(79, 17)
        Me.email.TabIndex = 0
        Me.email.Text = "Send Email"
        Me.email.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.cmbSites)
        Me.GroupBox4.Controls.Add(Me.Label3)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 93)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(346, 56)
        Me.GroupBox4.TabIndex = 11
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Site"
        '
        'cmbSites
        '
        Me.cmbSites.FormattingEnabled = True
        Me.cmbSites.Location = New System.Drawing.Point(165, 19)
        Me.cmbSites.Name = "cmbSites"
        Me.cmbSites.Size = New System.Drawing.Size(175, 21)
        Me.cmbSites.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(153, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Select Site to force report from:"
        '
        'chkbxDebug
        '
        Me.chkbxDebug.AutoSize = True
        Me.chkbxDebug.Location = New System.Drawing.Point(53, 19)
        Me.chkbxDebug.Name = "chkbxDebug"
        Me.chkbxDebug.Size = New System.Drawing.Size(117, 17)
        Me.chkbxDebug.TabIndex = 12
        Me.chkbxDebug.Text = "Transaciton Debug"
        Me.chkbxDebug.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.chkbxDebug)
        Me.GroupBox5.Location = New System.Drawing.Point(12, 253)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(184, 48)
        Me.GroupBox5.TabIndex = 13
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Debug:"
        '
        'ForceReports
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(370, 310)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.makeReports)
        Me.Controls.Add(Me.cancelButton)
        Me.Name = "ForceReports"
        Me.Text = "Select Dates:"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents startDatePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents endDatePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cancelButton As System.Windows.Forms.Button
    Friend WithEvents makeReports As System.Windows.Forms.Button
    Friend WithEvents transaction As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Channel As System.Windows.Forms.CheckBox
    Friend WithEvents transfer As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents email As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbSites As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkbxDebug As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
End Class
