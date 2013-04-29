Public Class ForceReports
    Dim configs As New Amcom.SDC.Speech.ReportBuilder.ReportConfigReader

    Private Sub cancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelButton.Click
        Me.Close()
    End Sub

    Private Sub makeReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles makeReports.Click
        If Me.cmbSites.SelectedItem Is Nothing Then Exit Sub
        If Me.transaction.Checked Or Me.transfer.Checked Or Me.Channel.Checked Then

            configs.LoadSiteConfig(cmbSites.SelectedItem)

            Dim generator As New Amcom.SDC.Speech.ReportBuilder.SpeechReportController()
            generator.ForceReports(Me.startDatePicker.Value.ToString("d"), Me.endDatePicker.Value.ToString("d"), Me.transaction.Checked, Me.transfer.Checked, Me.Channel.Checked, Me.email.Checked, Me.chkbxDebug.Checked, configs)

            Me.Close()
        Else
            MsgBox("Please select at least one report to process.", MsgBoxStyle.MsgBoxHelp, "No reports selected!")
        End If

    End Sub

    Private Sub ForceReports_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.startDatePicker.Value = Me.startDatePicker.Value.AddDays(-7)
        configs.Load()
        For Each site As String In configs.sites
            Me.cmbSites.Items.Add(site)
        Next

    End Sub
End Class