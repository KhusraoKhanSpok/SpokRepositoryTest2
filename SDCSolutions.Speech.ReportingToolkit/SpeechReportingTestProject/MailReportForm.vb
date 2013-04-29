
Imports ICSharpCode.SharpZipLib
Imports System.Text.RegularExpressions
Imports Amcom.SDC.Speech.ReportBuilder

Public Class MailReportForm
    Dim configs As New ReportConfigReader

    Private Sub addAttatchmentButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addAttatchmentButton.Click
        Me.attatchmentSelector.ShowDialog()
    End Sub

    Private Sub attatchmentSelector_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles attatchmentSelector.FileOk
        Me.attatchmentList.Items.Add(Me.attatchmentSelector.FileName)
    End Sub

    Private Sub MailReportForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        attatchmentSelector.InitialDirectory = configs.renderedLocation
        attatchmentSelector.DefaultExt = ".pdf"
    End Sub

    Private Sub removeAttatchment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles removeAttatchment.Click
        If attatchmentList.SelectedIndex = -1 Then Exit Sub
        attatchmentList.Items.RemoveAt(attatchmentList.SelectedIndex)
        attatchmentList.Select()
    End Sub

    Private Sub sendEmail()
        'Constructs and sends an email to recipients based on config settings.

        Try
            Dim email As New System.Net.Mail.MailMessage()

            configs.LoadSiteConfig(configs.sites(1)) 'Load the first site, and use that information to send e-mail.

            email.To.Add(Me.mailToTxt.Text)

            Dim fromAddress As New Net.Mail.MailAddress(configs.mailFromAddress, "Report Services Tool-Kit")
            email.From = fromAddress

            email.Subject = Me.subjectTxt.Text
            email.Body = Me.bodyTxt.Text

            'Add our attachments!
            'For Each report As String In Me.attatchmentList.Items
            '    Dim reportAttatch As New Net.Mail.Attachment(report)
            '    email.Attachments.Add(reportAttatch)
            'Next
            email.Attachments.Add(zipFiles(Me.attatchmentList.Items))

            'Setup our sender and credentials!
            Dim emailSender As New System.Net.Mail.SmtpClient(configs.mailServer, configs.mailPort)
            Dim credentials As New System.Net.NetworkCredential(configs.mailUser, configs.mailPass)

            emailSender.Credentials = credentials
            'emailSender.UseDefaultCredentials = True

            'Annnnd send our message!
            emailSender.Send(email)

            Me.Close() 'Close the form. 


        Catch ex As Net.Mail.SmtpException
            Try
                MsgBox("An error occured when sending email: " & ex.InnerException.Message, MsgBoxStyle.Exclamation, "Error during E-mail")
            Catch
                MsgBox("An error occured when sending email: " & ex.Message, MsgBoxStyle.Exclamation, "Error during E-mail")
            End Try
        Catch ex As Exception
            MsgBox("An error occured when sending email: " & ex.Message, MsgBoxStyle.Exclamation, "Error during E-mail")
        End Try
    End Sub

    Function zipFiles(ByVal reports As ListBox.ObjectCollection) As System.Net.Mail.Attachment
        'Zips up the report files and returns the file path of were the zip resides. 
        Dim fileLoc As String = ""

        'Set the zip file name.
        fileLoc = My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & configs.selectedSite & "." & Now.Day & "." & Now.Month & "." & Now.Year & ".zip"

        'Set the name for the temporary directory we will be zipping up. 
        Dim tempReportDir As String = My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & configs.selectedSite & "_Reports"

        'Create the temporary directory
        FileIO.FileSystem.CreateDirectory(tempReportDir)

        'For each file, copy that file into the temp directory. 
        Try
            For Each file As String In reports
                Dim sFileName As String = Regex.Match(file, "\w+\.[a-zA-Z]{3}").Value
                FileIO.FileSystem.CopyFile(file, tempReportDir & "\" & sFileName, True)
            Next
        Catch ex As Exception
        End Try

        Dim zipper As New ICSharpCode.SharpZipLib.Zip.FastZip

        zipper.CreateZip(fileLoc, tempReportDir, False, "") 'Create the zip file. 

        'Then destroy the temp directory. 
        FileIO.FileSystem.DeleteDirectory(tempReportDir, FileIO.DeleteDirectoryOption.DeleteAllContents)

        'Create an attachment for mail
        Dim mailAttachment As New Net.Mail.Attachment(fileLoc)

        'Return the attachment. 
        Return mailAttachment

    End Function

    Private Sub sendMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sendMail.Click
        sendEmail()
    End Sub
End Class