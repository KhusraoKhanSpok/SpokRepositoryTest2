
Imports Amcom.SDC.Speech.ReportBuilder

Public Class ReportReviewer
    Dim configs As New Amcom.SDC.Speech.ReportBuilder.ReportConfigReader
    Private Sub ReportReviewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.startDatePicker.Value = Today.AddDays(-7)
        Me.endDatePicker.Value = Today.AddDays(0)

        Me.startDatePicker.MaxDate = Today.Date
        Me.endDatePicker.MaxDate = Today.Date

        configs.Load()
        For Each site As String In configs.sites
            Me.cmbSites.Items.Add(site)
        Next
        cmbSites.SelectedItem = SpeechReportma.cbSiteSelect.SelectedItem
        reportSelector.SelectedItem = "Transaction"

        Me.reportViewer.RefreshReport()
    End Sub


    Private Sub generateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles generateButton.Click

        generateButton.Enabled = False
        If ((Me.startDatePicker.Value > Me.endDatePicker.Value)) Then
            MsgBox("Start Date must occur prior to the End Date selected", MsgBoxStyle.OkOnly)
            generateButton.Enabled = True
            Exit Sub
        ElseIf (Me.startDatePicker.Value > Now.Date) Then
            MsgBox("Start Date must either be today or earlier (Cannot have a start date in the future)", MsgBoxStyle.OkOnly)
            generateButton.Enabled = True
            Exit Sub
        End If


        configs.LoadSiteConfig(Me.cmbSites.SelectedItem)
        Dim generator As New Amcom.SDC.Speech.ReportBuilder.ReportGenerator

        Dim selectedReport As String = reportSelector.SelectedItem
        Dim arraySize As Integer = 1
        Dim reportPath As String
        Dim transa As Boolean = False
        Dim transf As Boolean = False
        Dim chan As Boolean = False
        'generate report selected here. use a case statement.

        Select Case selectedReport
            Case "Transfer"
                transf = True
                reportPath = configs.reportLocation & "TransferReport.rdlc"
            Case "Channel"
                chan = True
                reportPath = configs.reportLocation & "Call Channel Report.rdlc"
            Case Else
                arraySize = 3
                transa = True
                reportPath = configs.reportLocation & "Transaction_Report.rdlc"

        End Select

        generator.makeReports(Me.startDatePicker.Value, Me.endDatePicker.Value, transa, transf, chan, False, Me.chkbxDebug.Checked, configs, Me.syncAlts.Checked)

        'Clear out any old report
        reportViewer.Reset()
        reportViewer.RefreshReport()

        'Set the datasets and create a new report. 
        reportViewer.LocalReport.DataSources.Add(generator.transaSource)
        reportViewer.LocalReport.DataSources.Add(generator.transfSource)
        reportViewer.LocalReport.DataSources.Add(generator.chHrSource)
        reportViewer.LocalReport.ReportPath = reportPath

        'Set the report params

        Dim reportParams(arraySize) As Microsoft.Reporting.WinForms.ReportParameter

        reportParams(0) = New Microsoft.Reporting.WinForms.ReportParameter("Start_Date", Me.startDatePicker.Value.ToString("d"), False)
        reportParams(1) = New Microsoft.Reporting.WinForms.ReportParameter("End_Date", Me.endDatePicker.Value.ToString("d"), False)

        If transa Then
            reportParams(2) = New Microsoft.Reporting.WinForms.ReportParameter("Customer", configs.Customer_Name)
            reportParams(3) = New Microsoft.Reporting.WinForms.ReportParameter("Casloc", configs.selectedSite)
        End If

        reportViewer.LocalReport.SetParameters(reportParams)


        reportViewer.RefreshReport()
        generateButton.Enabled = True
    End Sub


    Private Sub reportSelector_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reportSelector.SelectedIndexChanged

        If (reportSelector.SelectedItem = "Transfer") Then
            syncAlts.Visible = True
        Else
            syncAlts.Visible = False
        End If


    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim SRC As New SpeechReportController

        SRC.MakeWeeklyReports(True)

    End Sub

End Class