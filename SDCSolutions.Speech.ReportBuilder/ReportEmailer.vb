Imports System.Net.mail
Imports Amcom.SDC.BaseServices
Imports System.Text.RegularExpressions

'Zip library import
Imports ICSharpCode.SharpZipLib
Imports System.Collections.ObjectModel
Public Class ReportEmailer
    Dim configs As New ReportConfigReader




    'Dim reportFiles As New Collection(Of System.Net.Mail.Attachment) 'Collection that will hold attatchments.
    Dim reportFileStrings As New Collection(Of String) 'Collection that holds the string files for zipping reports. 

    Sub AddReportFile(ByVal path As String)
        'Dim attatchment As New System.Net.Mail.Attachment(path)
        reportFileStrings.Add(path)
        App.TraceLog(TraceLevel.Verbose, "VERBOSE-AddReportFile(): Adding " & path & " to attachement list.")
        'reportFiles.Add(attatchment) 'PROBLEM
    End Sub

    Function zipFiles(ByVal reports As Collection(Of String)) As System.Net.Mail.Attachment
        'Zips up the report files and returns the file path of were the zip resides. 
        Dim fileLoc As String = ""

        'Set the zip file name.
        'We do not want 'default' to show if we can help it, so change the zip file to use the customer name and not the site name if needed.
        If configs.selectedSite = "default" Then
            fileLoc = configs.renderedLocation & configs.Customer_Name
        Else
            fileLoc = configs.renderedLocation & configs.selectedSite
        End If

        'Set up the rest of the zip name. 
        fileLoc = fileLoc & "." & Now.Month & "." & Now.Day & "." & Now.Year & ".zip"

        'Set the name for the temporary directory we will be zipping up. 
        Dim tempReportDir As String = configs.renderedLocation & configs.selectedSite & "_Reports"

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


        ' Lets try to copy the reports into the archive directory This should get the files uploaded to Amcom as a back to SMTP
        Try
            Dim archiveFolder As String
            Dim fileName As String
            fileName = fileLoc.Substring(fileLoc.LastIndexOf("\"))
            archiveFolder = configs.renderedLocation.Replace("IS_Reports\", "SpeechServices\Archive\" & fileName)
            FileIO.FileSystem.CopyFile(fileLoc, archiveFolder, True)

        Catch ex As Exception
            App.ExceptionLog(New AmcomException("Could not copy report zips to SpeechServices\Archive\ Folder Exception:", ex.InnerException))
        End Try

        'Then destroy the temp directory. 
        FileIO.FileSystem.DeleteDirectory(tempReportDir, FileIO.DeleteDirectoryOption.DeleteAllContents)

        'Create an attachment for mail
        Dim mailAttachment As New Net.Mail.Attachment(fileLoc)

        'Return the attachment. 
        Return mailAttachment

    End Function

    Sub SendMail(ByVal reportConfig As Speech.ReportBuilder.ReportConfigReader)
        'Constructs and sends an email to recipients based on config settings.

        configs = reportConfig 'sets our current config to what the report generater has passed to us. 

        App.TraceLog(TraceLevel.Info, "INFO-SendMail(): Preparing to send e-mail...")

        Dim email As New System.Net.Mail.MailMessage()

        email.To.Add(configs.mailToAddress)
        Dim fromAddress As New Net.Mail.MailAddress(configs.mailFromAddress)
        email.From = fromAddress

        email.Subject = configs.mailSubject
        email.Body = configs.mailBody

        'Add our attachments!
        App.TraceLog(TraceLevel.Verbose, "VERBOSE-SendMail(): Adding attatchements to e-mail object...")

        email.Attachments.Add(zipFiles(reportFileStrings)) 'Zip the attachment instead of just importing them raw. 
        'For Each report As System.Net.Mail.Attachment In reportFiles
        '    email.Attachments.Add(report)
        'Next

        'Setup our sender and credentials!
        Dim emailSender As New System.Net.Mail.SmtpClient(configs.mailServer, configs.mailPort)
        Dim credentials As New System.Net.NetworkCredential(configs.mailUser, configs.mailPass)

        emailSender.Credentials = credentials
        'emailSender.UseDefaultCredentials = True

        'Annnnd send our message!
        App.TraceLog(TraceLevel.Info, "INFO-SendMail(): Sending mail NOW!")

        Try
            emailSender.Send(email)
        Catch ex As Net.Mail.SmtpException
            App.TraceLog(TraceLevel.Error, "ERROR - Failure sending mail: " & ex.ToString)
        Catch abortEx As Threading.ThreadAbortException
            App.TraceLog(TraceLevel.Warning, "WARNING - Mail send aborted - the thread is closing.")
        Finally
            email.Dispose()
            'reportFiles.Clear()
            reportFileStrings.Clear()
        End Try



    End Sub



End Class
