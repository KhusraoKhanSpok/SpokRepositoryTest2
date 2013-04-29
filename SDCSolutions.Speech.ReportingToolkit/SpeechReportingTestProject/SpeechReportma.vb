
Imports Amcom.SDC.BaseServices
Imports Amcom.SDC.Speech.ReportBuilder


Public Class SpeechReportma

    Dim problems As New Collection 'This is the list of errors that is accessed through a property to the error viewer form.
    Public ReadOnly Property reportErrors() As Collection
        Get
            Return problems
        End Get
    End Property

    Private Sub goButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim reports As New SpeechReportController

        reports.MakeWeeklyReports()

    End Sub

    Private Sub force_Reports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles force_Reports.Click
        ForceReports.Show()
    End Sub

    Private Sub viewReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewReports.Click
        ReportReviewer.Show()
    End Sub

    Private Sub reTestConfigsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reTestConfigsButton.Click
        runTest()
    End Sub

    Private Sub SpeechReportma_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        Startup.Show()
        Try
            'Try to start. If we fail, we will run our test anyways, which will catch the error.
            App.Initialize(ApplicationType.Windows, My.Settings)
            loadsites()
            Me.VersionLabel.Text = "Version " & My.Application.Info.Version.ToString
        Catch ex As AmcomException
            'Do nothing.
        Catch ex As Exception
            'Do nothing. 
        Finally
            runTest()
        End Try

        Startup.Close()

    End Sub

    Private Sub loadsites()
        'Loads the name of sites from the config. 
        Try
            Dim configs As New ReportConfigReader

            For Each site As String In configs.sites
                Me.cbSiteSelect.Items.Add(site)
            Next

            If Me.cbSiteSelect.Items.Count > 0 Then
                Me.cbSiteSelect.SelectedIndex = 0
            End If

        Catch ex As AmcomException
            'Do nothing, no sites found. 
        Catch ex As Exception
            'Do nothing, no sites found.
        End Try
    End Sub
    Private Sub runTest()
        'runs a test procedure on the speech reporting systems. 
        Dim tester As New ConfigTester
        Dim configs As New ReportConfigReader


        'Set all things to unknown, first of all. 
        Me.statusImageReg.Image = My.Resources.unknown
        Me.statusImageConfigs.Image = My.Resources.unknown
        Me.statusImageSpeechDb.Image = My.Resources.unknown
        Me.statusImageIntelliDeskDb.Image = My.Resources.unknown
        Me.statusImageReportSource.Image = My.Resources.unknown
        Me.statusImageRenderedReports.Image = My.Resources.unknown
        Me.statusImageSMTP.Image = My.Resources.unknown

        'Set any disabled buttons to enabled.
        Me.force_Reports.Enabled = True
        Me.viewReports.Enabled = True
        Me.Button1.Enabled = True 'this is the smtp email button. oops. 
        Me.viewConfig.Enabled = True

        'Hide the view errors button and label.
        Me.viewErrorsImg.Visible = False
        Me.viewErrorslbl.Visible = False

        Me.pleaseWaitLabel.Visible = True

        Try
            If Me.cbSiteSelect.Items.Count > 0 Then
                configs.LoadSiteConfig(Me.cbSiteSelect.Text) 'Load the site config we wish to test. 
            End If
        Catch ex As AmcomException
            Me.statusImageConfigs.Image = My.Resources._error
            Me.force_Reports.Enabled = False
            Me.viewReports.Enabled = False
            Me.Button1.Enabled = False
            Me.viewConfig.Enabled = False
            Me.pleaseWaitLabel.Visible = False
            Dim errors As New Collection
            errors.Add("Error loading config: " & ex.Message)
            writeErrors(errors)
            Exit Sub
        End Try


        'Registry Test
        If tester.testRegistry Then
            Me.statusImageReg.Image = My.Resources.check
        Else
            Me.statusImageReg.Image = My.Resources._error
            Me.force_Reports.Enabled = False
            Me.viewReports.Enabled = False
            Me.Button1.Enabled = False
            Me.viewConfig.Enabled = False
            Me.pleaseWaitLabel.Visible = False
            writeErrors(tester.errorList)
            Exit Sub
        End If

        'Test SDC Configs. 
        If tester.testSdcConfig Then
            Me.statusImageConfigs.Image = My.Resources.check
        Else
            Me.statusImageConfigs.Image = My.Resources._error
            Me.force_Reports.Enabled = False
            Me.viewReports.Enabled = False
            Me.Button1.Enabled = False
            Me.viewConfig.Enabled = False
            Me.pleaseWaitLabel.Visible = False
            writeErrors(tester.errorList)
            Exit Sub
        End If

        'Test Speech DB. 
        If tester.testSpeechDb Then
            Me.statusImageSpeechDb.Image = My.Resources.check
        Else
            Me.statusImageSpeechDb.Image = My.Resources._error
            Me.force_Reports.Enabled = False
            Me.viewReports.Enabled = False
        End If

        'Test Desk DB. 
        If tester.testDeskDb Then
            Me.statusImageIntelliDeskDb.Image = My.Resources.check
        Else
            Me.statusImageIntelliDeskDb.Image = My.Resources._error
            Me.force_Reports.Enabled = False
            Me.viewReports.Enabled = False
        End If

        'Test Report Source. 
        If tester.testReportPath(configs) Then
            Me.statusImageReportSource.Image = My.Resources.check
        Else
            Me.statusImageReportSource.Image = My.Resources._error
            Me.pleaseWaitLabel.Visible = False
            Me.force_Reports.Enabled = False
            Me.viewReports.Enabled = False
            writeErrors(tester.errorList)
            Exit Sub
        End If

        'Test Rendered Report Path. 
        If tester.testRenderedPath(configs) Then
            Me.statusImageRenderedReports.Image = My.Resources.check
        Else
            Me.statusImageRenderedReports.Image = My.Resources._error
            Me.pleaseWaitLabel.Visible = False
            Me.force_Reports.Enabled = False
            Me.viewReports.Enabled = False
            writeErrors(tester.errorList)
            Exit Sub
        End If

        'Test SMTP. 
        If tester.testSMTP(configs) Then
            Me.statusImageSMTP.Image = My.Resources.check
        Else
            Me.statusImageSMTP.Image = My.Resources._error
            Me.Button1.Enabled = False
        End If

        Me.pleaseWaitLabel.Visible = False
        writeErrors(tester.errorList)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        MailReportForm.Show()
    End Sub

    Private Sub quit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles quit.Click
        Me.Close()
    End Sub
    Private Sub writeErrors(ByVal errors As Collection)

        'If there are no errors, don't bother. 
        If errors.Count = 0 Then Exit Sub
        'set the errors for the error viewer form to grab. 
        problems = errors

        'Show the view errors button and label.
        Me.viewErrorsImg.Visible = True
        Me.viewErrorslbl.Visible = True


    End Sub

    Private Sub viewErrorsImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewErrorsImg.Click
        ErrorViewer.Show()
    End Sub

    Private Sub viewConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewConfig.Click
        ConfigViewer.Show()
    End Sub

    Private Sub startupTestThread_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        runTest()
    End Sub


    Private Sub statusImageSMTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusImageSMTP.Click
        ErrorViewer.Show()
    End Sub

    Private Sub statusImageReg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusImageReg.Click
        ErrorViewer.Show()
    End Sub

    Private Sub statusImageConfigs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusImageConfigs.Click
        ErrorViewer.Show()
    End Sub

    Private Sub statusImageSpeechDb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusImageSpeechDb.Click
        ErrorViewer.Show()
    End Sub

    Private Sub statusImageIntelliDeskDb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusImageIntelliDeskDb.Click
        ErrorViewer.Show()
    End Sub

    Private Sub statusImageReportSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusImageReportSource.Click
        ErrorViewer.Show()
    End Sub

    Private Sub statusImageRenderedReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles statusImageRenderedReports.Click
        ErrorViewer.Show()
    End Sub

   
End Class
