
Imports Amcom.SDC.BaseServices
Imports Amcom.SDC.Speech.ReportBuilder

Public Class ConfigViewer
    Dim configs As New ReportConfigReader

    Private Sub ConfigViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        configs.Load()
        For Each site As String In configs.sites
            Me.cmbSites.Items.Add(site)
        Next


        configs.LoadSiteConfig(configs.sites.Item(1)) 'Select a default site to start with. 
        loadSiteValues()

        'Select the first item in the list. 
        Me.cmbSites.SelectedIndex = 0

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles showFullCon.Click
        fullContext.Show()
    End Sub

    Private Sub cmbSites_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSites.SelectedIndexChanged
        configs.LoadSiteConfig(cmbSites.SelectedItem)
        loadSiteValues()
    End Sub
    Private Sub loadSiteValues()
        'Set the speech database readouts. 
        Me.tbSpeechConnString.Text = App.ConnectionString("speech")

        'Set the desk database readouts.
        Me.tbDeskConnectionString.Text = App.ConnectionString("desk")

        'Set the SMTP readouts.
        Me.smtpSrvTxt.Text = configs.mailServer
        Me.smtpUserTxt.Text = configs.mailUser
        Me.smtpPassTxt.Text = configs.mailPass
        Me.smtpPortTxt.Text = configs.mailPort
        Me.smtpSendTo.Text = configs.mailToAddress
        Me.smtpSendFrom.Text = configs.mailFromAddress
        Me.smtpSubject.Text = configs.mailSubject
        Me.smtpMessage.Text = configs.mailBody

        'Set the flag readouts.

        'Convert the integer of the day we run flag into something more meaningful. 
        Dim intDay As Integer
        intDay = Integer.Parse(configs.dayToRun)
        Me.flagsDay.Text = WeekdayName(intDay + 1) & " (" & intDay & ")"

        Me.flagsHour.Text = configs.hourToRun & ":00"
        Me.flagsTemplateFolder.Text = configs.reportLocation
        Me.flagsRenderedFolder.Text = configs.renderedLocation
        Me.flagsCallsChannels.Text = configs.reportChannels
    End Sub

    Private Sub smtpSrvTxt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smtpSrvTxt.Click
        getNewValue("repSmtpHost")
    End Sub


    Private Sub getNewValue(ByVal fieldName As String)
        'Shows a dialog allowing a user to change values for reports in the database. 

        'Set the values
        ChangeDBDialog.changeField = fieldName
        ChangeDBDialog.changeCustomer = cmbSites.SelectedItem

        'Show the dialog
        ChangeDBDialog.ShowDialog()

        'Reload the values into the form.
        configs.LoadSiteConfig(cmbSites.SelectedItem) 'Reload configs from DB
        loadSiteValues() 'Reload values to form.
    End Sub

    Private Sub smtpUserTxt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smtpUserTxt.Click
        getNewValue("repSmtpUsername")
    End Sub

    Private Sub smtpPassTxt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smtpPassTxt.Click
        getNewValue("repSmtpPassword")
    End Sub

    Private Sub smtpPortTxt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smtpPortTxt.Click
        getNewValue("repSmtpPort")
    End Sub

    Private Sub smtpSendTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smtpSendTo.Click
        getNewValue("repSmtpTo")
    End Sub

    Private Sub smtpSendFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smtpSendFrom.Click
        getNewValue("repSmtpFrom")
    End Sub

    Private Sub smtpSubject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smtpSubject.Click
        getNewValue("repSmtpSubject")
    End Sub

    Private Sub smtpMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smtpMessage.Click
        getNewValue("repSmtpMessage")
    End Sub

    Private Sub flagsDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flagsDay.Click
        getNewValue("repDayOfWeek")
    End Sub

    Private Sub flagsHour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flagsHour.Click
        getNewValue("repTimeOfDay")
    End Sub

    Private Sub flagsTemplateFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flagsTemplateFolder.Click
        getNewValue("repReportPath")
    End Sub

    Private Sub flagsRenderedFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flagsRenderedFolder.Click
        getNewValue("repRenderedPath")
    End Sub

    Private Sub flagsCallsChannels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flagsCallsChannels.Click
        getNewValue("repMakeChannelReport")
    End Sub



    Private Sub btnAddSite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSite.Click
        'Creates a new customer site entry in the SDC_Config table. 
        AddNewSiteDialog.ShowDialog()

        'Refresh the site listing. 
        configs = New ReportConfigReader
        configs.Load()
        Me.cmbSites.Items.Clear()
        For Each site As String In configs.sites
            Me.cmbSites.Items.Add(site)
        Next


    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        getNewValue("monthlyReport")
    End Sub

    Private Sub MonthlyReport_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MonthlyReport.CheckedChanged
        App.Config.GetString(ConfigSection.modules, "//sdcSpeechReporting//monthlyEnabled", False, True)
    End Sub
End Class