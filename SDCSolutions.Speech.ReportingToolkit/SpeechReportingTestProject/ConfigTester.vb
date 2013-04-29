Imports Amcom.SDC.Speech.ReportBuilder
Imports Amcom.SDC.BaseServices


Public Class ConfigTester
    'Contains a series of tests that will show an at-a-glance status of the report system.
    Dim configs As New ReportConfigReader
    Dim errors As New Collection

    Public ReadOnly Property errorList() As Collection
        Get
            Return errors
        End Get
    End Property


    Function testRegistry() As Boolean
        'Test our registry values to see if they are there. 
        Dim configFilePath As String
        'Dim configFile As String

        configFilePath = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Amcom Software\IntelliSPEECH\", "Config_Path", "FAIL")
        'configFile = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\SDC Solutions\IntelliSPEECH\", "Config_Report", "FAIL")

        If configFilePath = "FAIL" Then
            errors.Add("Registry keys not found. Check 'HKEY_LOCAL_MACHINE\SOFTWARE\Amcom Software\IntelliSPEECH\' for integrity.")
            Return False
        Else
            Return True
        End If

    End Function
    Function testSdcConfig() As Boolean
        'Test to make sure the registry values are pointed at the right location and that the config file is actualy there. 
        'Dim configFilePath As String
        'Dim configFile As String
        'Dim filePath As String

        'configFilePath = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\SDC Solutions\IntelliSPEECH\", "Config_Path", "FAIL")
        'configFile = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\SDC Solutions\IntelliSPEECH\", "Config_Report", "FAIL")

        'filePath = configFilePath & "\" & configFile




        Try
            App.Initialize(ApplicationType.Windows, My.Settings)
            Return True
        Catch ex As ConfigException
            errors.Add("SDC_Config file is not valid: " & ex.ToString)
            Return False
            Exit Function
        Catch ex As AmcomException
            errors.Add("SDC_Config file is not valid: " & ex.ToString)
            Return False
            Exit Function
        End Try


    End Function
    Function testSpeechDb() As Boolean
        'Tests the speech database to make sure we can connect to it. 
        Try
            Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("speech"))
            Dim sqlStatement As String
            Dim insertString As String
            Dim deleteString As String

            sqlStatement = "SELECT top 1 *  FROM [ASR_Call_Log] CROSS JOIN [ASR_License] CROSS JOIN [ASRAlternateName]"
            insertString = "INSERT INTO [ASRAlternateName]" & _
                " ([DirRefNum],[AlternateName],[PreferredName],[ASR_AlternateName],[Source],[DirtyBit],[Date_Entered],[AGM_DirtyBit]) " & _
                "VALUES (999999,'TEST',0,'TEST','TEST',0,1/1/5050,0) "

            deleteString = "DELETE FROM [ASRAlternateName] WHERE [DirRefNum] = 999999"

            Dim da As New SqlClient.SqlDataAdapter(sqlStatement, is4Conn)
            Dim ds As New DataSet
            Dim sqlcommand As New SqlClient.SqlCommand(insertString, is4Conn)

            is4Conn.Open() 'Try to open the connection
            'Test one - try a sample query that our reports would pull..
            da.Fill(ds, "table") 'Pull down ONE record, a hybrid of all three tables used. 

            'Test two - see if we have rights to write a record to asraltname. 
            Dim rowsAffected As Integer

            rowsAffected = sqlcommand.ExecuteNonQuery()

            'if we didn't affect any rows, then something isn't right. 
            If rowsAffected = 0 Then
                errors.Add("Problem inserting record into IntelliSpeech Database - AsrAlternateName table; SQL command returned 0 rows. Check sdc account permissions.")
                Return False

                Exit Function
            End If


            'Test three - see if we can delete a record from the asraltname table. 
            sqlcommand.CommandText = deleteString
            rowsAffected = sqlcommand.ExecuteNonQuery()

            'if we didn't affect any rows, then something isn't right. 
            If rowsAffected = 0 Then
                errors.Add("Problem deleting record from IntelliSpeech Database - AsrAlternateName table; SQL command returned 0 rows. Check sdc account permissions.")
                Return False
                Exit Function
            End If


            is4Conn.Close()
            Return True

        Catch ex As AmcomException
            errors.Add("Speech DB: A database exception occured: " & ex.Message)
            Return False
        Catch ex As Exception
            errors.Add("Speech DB: A database exception occured: " & ex.Message)
            Return False
        End Try




    End Function

    Function testDeskDb() As Boolean
        Try
            'Tests the desk database
            Dim sdcIntWConn As New SqlClient.SqlConnection(App.ConnectionString("desk"))
            Dim sqlStatement As String
            sqlStatement = "SELECT TOP 1 * FROM [ASRAlternateName]"
            Dim da As New SqlClient.SqlDataAdapter(sqlStatement, sdcIntWConn)
            Dim ds As New DataSet


            sdcIntWConn.Open()
            'Test if we can actualy see the table we need to. If we can, we are good. 
            da.Fill(ds)
            sdcIntWConn.Close()
            Return True
        Catch ex As Exception
            errors.Add("Desk DB: A database exception occured: " & ex.Message)
            Return False
        End Try

    End Function

    Function testSMTP(ByVal configs As ReportConfigReader) As Boolean
        'Sends a test e-mail in order to make sure SMTP is working. 
        Try
            Dim email As New System.Net.Mail.MailMessage()

            email.To.Add("mstudley@sdcsolutions.com")
            Dim fromAddress As New Net.Mail.MailAddress(configs.mailFromAddress)
            email.From = fromAddress

            email.Subject = "Test E-mail from IntelliSPEECH Report Services Toolkit."
            email.Body = "This is a simple test of the SMTP system at a customer's site. If you are reading this message, the customer's SMTP systems are working properly."

            'Setup our sender and credentials!
            Dim emailSender As New System.Net.Mail.SmtpClient(configs.mailServer, configs.mailPort)
            Dim credentials As New System.Net.NetworkCredential(configs.mailUser, configs.mailPass)

            emailSender.Credentials = credentials
            'emailSender.UseDefaultCredentials = True

            'Annnnd send our message!
            emailSender.Send(email)
            Return True
        Catch ex As Net.Mail.SmtpException
            Try
                errors.Add("There is a problem with SMTP (E-mail): " & ex.InnerException.Message)
            Catch
                errors.Add("There is a problem with SMTP (E-mail): " & ex.Message)
            End Try
        Catch ex As Exception
            errors.Add("There is a problem with SMTP (E-mail): " & ex.Message)
            Return False
        End Try

    End Function

    Function testReportPath(ByVal configs As ReportConfigReader) As Boolean
        'Tests the report path that contains the .rdlc files. 
        Try
            If FileIO.FileSystem.DirectoryExists(configs.reportLocation) Then
                Return True
            Else
                errors.Add("The path holding the microsoft report files does not exist: " & configs.reportLocation)
                Return False
            End If
        Catch ex As AmcomException
            errors.Add("Error accessing config: " & ex.ToString)
            Return False
        Catch ex As Exception
            errors.Add("The path holding the microsoft report files does not exist: " & configs.reportLocation)
            Return False
        End Try
    End Function
    Function testRenderedPath(ByVal configs As ReportConfigReader) As Boolean
        'Tests the rendered report path, where the rendered reports are stored. 
        Try
            If FileIO.FileSystem.DirectoryExists(configs.renderedLocation) Then
                Return True
            Else
                errors.Add("The path holding the rendered-PDF report files does not exist: " & configs.renderedLocation)
                Return False
            End If
        Catch ex As AmcomException
            errors.Add("Error accessing config: " & ex.ToString)
            Return False
        End Try
    End Function

    Sub clearErrors()
        errors.Clear()
    End Sub
End Class
