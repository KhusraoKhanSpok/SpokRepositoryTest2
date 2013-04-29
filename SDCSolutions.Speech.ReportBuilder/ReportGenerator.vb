Imports Amcom.SDC.BaseServices

Public Class ReportGenerator

    'Datasets
    Dim altNameTable As New DataSet 'The alt name table from sdcintw DB.
    Dim callLogTable As New DataSet 'The callLog table from Intellispeech4 DB.
    Dim licenseTable As New DataSet 'The licensure table from intellispeech4 DB.
    Dim is4AltNameTable As New DataSet 'The alt name table from IntelliSpeech4 DB.

    'Report Data Sources
    Dim transDataSource As New Microsoft.Reporting.WinForms.ReportDataSource("SanCherryTest")
    Dim chanDataSource As New Microsoft.Reporting.WinForms.ReportDataSource("CallLog")
    Dim furDataSource As New Microsoft.Reporting.WinForms.ReportDataSource("CallLog")

    'Variables used to hold the refnum amounts from the alt name table. 
    Dim dirCount As Integer 'Holds how many unique names are in the directory. 
    Dim altNameCount As Integer 'Holds how many total alternate names are in the directory.

    'Var to hold the customer name we are working with. 
    'Dim customerName As String

    'Config reader and email sender classes. 
    Dim configs As New ReportConfigReader
    Dim emailer As New ReportEmailer()

    'Date strings used in all queries. 
    Dim dateStart As String
    Dim dateEnd As String
    Dim generateMonthly As Boolean

    'Used for passing as parameters in reports. 
    Dim dateStartString As String
    Dim dateEndString As String

    'Debug variables
    Dim police As Boolean = False

    'Report objects, used in properties for the report viewer to grab.
    Dim transReport As New Microsoft.Reporting.WinForms.LocalReport
    Dim furReport As New Microsoft.Reporting.WinForms.LocalReport
    Dim chanReport As New Microsoft.Reporting.WinForms.LocalReport

    'Events
    Public Event fileMade(ByVal file As FileCreatedEventArgs)
    Public Event finished()



    'Now, properties we will expose to the outside world, for a report reader to grab. 
    Public ReadOnly Property transaSource() As Microsoft.Reporting.WinForms.ReportDataSource
        Get
            Return transDataSource
        End Get
    End Property
    Public ReadOnly Property transfSource() As Microsoft.Reporting.WinForms.ReportDataSource
        Get
            Return furDataSource
        End Get
    End Property
    Public ReadOnly Property chHrSource() As Microsoft.Reporting.WinForms.ReportDataSource
        Get
            Return chanDataSource
        End Get
    End Property
    Public Property currentConfig() As Amcom.SDC.Speech.ReportBuilder.ReportConfigReader

        Get
            Return configs
        End Get
        Set(ByVal value As Amcom.SDC.Speech.ReportBuilder.ReportConfigReader)
            configs = value
        End Set
    End Property




    'Step 1: Create Dataset object from sdcintw
    '1a: Make connection object for database sdcintw
    '1b: Create adapters etc. 
    '1c: Create SQL query - find distinct ref nums in alt name table, and count them. 
    '1d: store the count of how many distinct refnums there are in a variable(A). 
    '1e: store the count of how many alternate names total there are in the alt name table in a variable(B)

    'Step 2: Update the Licensure table of the Intellispeech 4 database. 
    '2a: Create db connection objects for Intellispeech 4 database. 
    '2b: create a dataset of the licensure table
    '2c: update the names used column with variable(A) from step 1. 
    '2d: update the alternate names used column with variable(B) from step 1. 
    '2e: commit updates to intellispeech4 database.

    'Step 3: Create dataset object for Call_Log of intellispeech4. 
    '3a: Create db connection objects
    '3b: Create query - decide date logic here. Query should find dates in the range between tomarrow and seven days ago.
    '3c: Fill dataset object with data from call_log

    'Step 4: Generate Transaction Report

    'Step 5: Sync IS4db altname table with sdcintw altname
    '5a: Fill a sdcintw dataset with the alt name table (Should already be done via getaltnametable()
    '5b: Fill a is4 dataset with the alt name table from the is4 database. 
    '5c: assign the is4 dataset the value of the sdcint dataset. is4AltNameTable = altNameTable 
    '5d: Create a data command object and update the Is4 database. 

    'Step 6: Generate transfer Report. 
    'Step 7: Generate Calls Per Channel per hour report. 

    'The next two subs are the control subs. You should call this sub or another sub from this class... NEVER call individual subs!
    Sub makeReports(ByVal siteConfig As ReportConfigReader)
        'This overload is used if we are making reports according to our regular schedule. 
        App.TraceLog(TraceLevel.Info, "INFO - makeReports(1): Scheduled report run, making reports according to config.")

        'Set our config to the config sent by the controller. 
        configs = siteConfig

        'Check and see if reports have already been made today. If they have, exit the sub.

        If reportAlreadyExists(configs.selectedSite) Then
            App.TraceLog(TraceLevel.Info, "INFO - makeReports(): Reports already exist for today for " & configs.selectedSite & ". Aborting generation.")
            Exit Sub
        End If

        dateStartString = getSundayReportStartDate(getLastSaturday).ToString("d")
        dateEndString = getLastSaturday.ToString("d")

        Try

            dateEnd = getEndDate(getLastSaturday())
            dateStart = getStartDate(getSundayReportStartDate(getLastSaturday()))

            App.TraceLog(TraceLevel.Verbose, "VERBOSE - makeReports(1): Report date ranges will be: " & dateStart & " - " & dateEnd & ".")

            getAltNameTable()
            getLicenseTable()
            getCallLogTable()
            GenerateTransaction()


            If (My.Computer.FileSystem.FileExists(App.LogPath & "\lastSync.txt")) Then
                Dim lastSync As Date
                lastSync = Convert.ToDateTime(My.Computer.FileSystem.ReadAllText(App.LogPath & "\lastSync.txt"))
                ' add an hour to the lastsync time then compare it. If its still older then now run the Sync otherwise it should be up to date
                If (Not lastSync.AddHours(1) > Now) Then
                    My.Computer.FileSystem.WriteAllText(App.LogPath & "\lastSync.txt", Now, False)
                    syncIs4AltName()
                End If
            Else
                My.Computer.FileSystem.WriteAllText(App.LogPath & "\lastSync.txt", Now, False)
                syncIs4AltName()
            End If


            GenerateTransfer()

            If configs.reportChannels Then GenerateChReport()
            emailer.SendMail(configs)

            'Move report files and zip from TMP folder you will create and put in regular folder.
            'KEEP check for files in the original directoy, so that way we test only for a succesfull run. 
            'DELETE the TMP folder and re-create at the begining of each run. 
            '   Normally, this will be empty. If not,we were aborted before finishing - 
            '   there should not be a handle on any files, though - as it should be a different process alltogether. 
        Catch ex As OleDb.OleDbException
            'Error logging goes here. 
            App.TraceLog(TraceLevel.Error, "ERROR - SpeechReporting.MakeReports(1):Database Error: " & ex.Message)
        Catch ex As badCallLogEx
            App.TraceLog(TraceLevel.Error, "ERROR - Call Log bad exception caught. Aborting Execution.")
        Catch ex As Microsoft.Reporting.WinForms.LocalProcessingException
            'A report is bad. 
            App.TraceLog(TraceLevel.Error, "ERROR - SpeechReporting.MakeReports(1):Error in report: " & ex.Message)
        Catch ex As IO.IOException
            'The file we are trying to write to is open or in use!
            App.TraceLog(TraceLevel.Error, "ERROR - SpeechReporting.MakeReports(1):Error writing to PDF file: " & ex.Message)
        Catch ex As AmcomException
            'Getting the config failed!
            App.TraceLog(TraceLevel.Error, "ERROR - SpeechReporting.MakeReports(1):Error with loading Config: " & ex.Message)
        Catch abortEx As Threading.ThreadAbortException
            'The thread is being aborted. Clear out everything from memory. 
            App.TraceLog(TraceLevel.Error, "INFO - SpeechReporting.MakeReports(1):Thread abort detected! Cleaning up!")


            transReport.Dispose()
            furReport.Dispose()
            chanReport.Dispose()

            Exit Sub 'Exit the sub. NOW.

        Finally
            'Tell the main sub that we are finished with this report set.
            transReport.Dispose()
            furReport.Dispose()
            chanReport.Dispose()
            RaiseEvent finished()
        End Try

        App.TraceLog(TraceLevel.Info, "INFO - makeReports(1): Done report processing.")

    End Sub

    Sub makeReports(ByVal start_Date As Date, ByVal end_Date As Date, ByVal transaction As Boolean, ByVal transfer As Boolean, ByVal channel As Boolean, ByVal email As Boolean, ByVal debug As Boolean, ByVal siteConfig As ReportConfigReader, Optional ByVal syncAlternateNames As Boolean = True)
        'This overload is used if we are forcing reports.

        configs = siteConfig

        App.TraceLog(TraceLevel.Info, "INFO - makeReports(2): FORCED report run, making reports according to parameters!")

        'If reports already exist and we are running a forced report, then as if we should overwrite reports. 

        If reportAlreadyExists(configs.selectedSite) Then

            Dim result As MsgBoxResult = MsgBox("It appears reports have already run for today. Are you sure you want to overwrite the existing reports.", MsgBoxStyle.YesNo, "Overwrite existing reports?")

            If result = MsgBoxResult.No Then
                MsgBox("Report Generation aborted.", MsgBoxStyle.Information, "Report Generation:")
                Exit Sub
            End If

        End If

        dateStartString = start_Date.ToString("d")
        dateEndString = end_Date.ToString("d")


        Try

            police = debug 'Set the debugging flag for transaction reports. 
            dateStart = getStartDate(start_Date)
            dateEnd = getEndDate(end_Date)
            App.TraceLog(TraceLevel.Verbose, "VERBOSE - makeReports(2): Report date ranges will be: " & dateStart & " - " & dateEnd & ".")

            getAltNameTable()
            getLicenseTable()
            getCallLogTable()

            If transaction Then GenerateTransaction()
            If transfer Then
                If (syncAlternateNames) Then
                    syncIs4AltName()
                End If
                GenerateTransfer()
            End If
            If channel Then GenerateChReport()
            'If email Then emailer.SendMail(configs)

        Catch ex As OleDb.OleDbException
            MsgBox("There was an error attempting to connect to a database:" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Database Error!")
        Catch ex As Microsoft.Reporting.WinForms.LocalProcessingException
            MsgBox("There was an error processing a report:" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Local Report Processing Error!")
        Catch ex As badCallLogEx
            MsgBox("There was an error with the call log data:" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Call Log Data Error!")
        Catch ex As IO.IOException
            MsgBox("A .PDF file the program is trying to write to is not accessible:" & vbCrLf & ex.Message & vbCrLf & "If you have a report open in adobe, close it before trying to make a new one!", MsgBoxStyle.Critical, "File output Error!")
        Catch ex As AmcomException
            MsgBox("An error occured while reading the sdc_config file:" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "SDC-Config error!")
        Catch ex As Net.Mail.SmtpException
            MsgBox("An error occured while attempting to email reports:" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "SMTP Error!")
        Catch ex As FormatException
            MsgBox("An error occured while attempting to parse email addresses:" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "SMTP Error!")
        Finally
            'Do nothing, just exit the sub. Log that you are exiting the sub. 
        End Try

        App.TraceLog(TraceLevel.Info, "INFO - makeReports(2): Done report processing.")

    End Sub


    Private Sub getAltNameTable()
        'Connects to the SDCINTW database, and fills the altname dataset, then populates the two vars we will need from the sdcintw altnames table.  

        App.TraceLog(TraceLevel.Verbose, "VERBOSE - getAltNameTable(): Grabbing the altname table from intelliDesk DB.")

        Dim sdcIntWConn As New SqlClient.SqlConnection(App.ConnectionString("desk"))
        Dim sqlStatement As String

        If (configs.getIspeechMode = "SDC") Then
            sqlStatement = "SELECT distinct ASRAlternateName.DirRefNum FROM [ASRAlternateName] left join directory on ASRAlternateName.DirRefNum=directory.refnum  where directory.SS_Enable=1 and directory.SS_Flag=1"
        ElseIf (configs.getIspeechMode = "XTEND") Then
            sqlStatement = "select distinct userid from asralternatename inner join directory on asralternatename.dirrefnum=directory.refnum"
        Else
            sqlStatement = "SELECT distinct ASRAlternateName.DirRefNum FROM [ASRAlternateName] left join directory on ASRAlternateName.DirRefNum=directory.refnum  where directory.SS_Enable=1 and directory.SS_Flag=1"
        End If

        'sqlStatement = "SELECT distinct [DirRefNum] FROM [ASRAlternateName]"
        App.TraceLog(TraceLevel.Info, "SQL STATEMENT: " & sqlStatement)

        sdcIntWConn.Open()

        'Fill the dataset table with the count of how many unique dir ref nums there are. 
        Dim da As New SqlClient.SqlDataAdapter(sqlStatement, sdcIntWConn)
        da.Fill(altNameTable, "altNameTable")

        dirCount = altNameTable.Tables(0).Rows.Count

        'Fill the dataset with how many total alt names there are in the AltNameTable. 
        sqlStatement = "SELECT [ID],[DirRefNum],[AlternateName],[PreferredName],[ASR_AlternateName],[Source],[DirtyBit],[Date_Entered],[AGM_DirtyBit] FROM [ASRAlternateName]"

        da = New SqlClient.SqlDataAdapter(sqlStatement, sdcIntWConn)
        altNameTable = New DataSet
        da.Fill(altNameTable, "altNameTable")

        altNameCount = altNameTable.Tables(0).Rows.Count

        sdcIntWConn.Close()

    End Sub

    Private Sub syncIs4AltName()



        'Connects to the Intellispeech database, and fills the is4AltNameTable dataset. Then syncs it to the sdcintw database, which should be populated already. 

        App.TraceLog(TraceLevel.Verbose, "VERBOSE - syncIs4AltName(): Replacing the IntelliSpeech Altname table with the IntelliDesk Altname table.")

        Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("speech"))
        Dim sqlStatement As String

        sqlStatement = "SELECT * FROM [ASRAlternateName]"

        is4Conn.Open()

        'Fill the dataset table with the count of how many unique dir ref nums there are. 
        Dim da As New SqlClient.SqlDataAdapter(sqlStatement, is4Conn)
        da.Fill(is4AltNameTable, "altNameTable")

        Dim k As Integer = 0

        'Clear old entries from table. 
        Dim delCommand As New SqlClient.SqlCommand("DELETE FROM [ASRAlternateName]", is4Conn)

        Try
            delCommand.ExecuteNonQuery()
        Catch ex As Exception
            App.TraceLog(TraceLevel.Error, ex.Message)
        End Try

        'Dim is4Command As New (da)
        'Try
        '    da.Update(is4AltNameTable, "altNameTable")
        'Catch ex As Exception
        'End Try

        'Now make new entries from sdcintw table. 

        Dim j As Integer = 0
        'Dim aRow As DataRow

        'For all of the entries in sdcintw, import that row into the IntelliSpeech alt name table. 
        For j = 0 To altNameTable.Tables(0).Rows.Count - 1

            'aRow = is4AltNameTable.Tables(0).NewRow

            'Assign this row to be the same as the equivilent row in the sdcintw table. 
            'aRow.Item(0) = altNameTable.Tables(0).Rows(j).Item(0)
            'aRow.Item(1) = altNameTable.Tables(0).Rows(j).Item(1)
            'aRow.Item(2) = altNameTable.Tables(0).Rows(j).Item(2)
            'aRow.Item(3) = altNameTable.Tables(0).Rows(j).Item(3)
            'aRow.Item(4) = altNameTable.Tables(0).Rows(j).Item(4)
            'aRow.Item(5) = altNameTable.Tables(0).Rows(j).Item(5)
            'aRow.Item(6) = altNameTable.Tables(0).Rows(j).Item(6)
            'aRow.Item(7) = altNameTable.Tables(0).Rows(j).Item(7)
            'aRow.Item(8) = altNameTable.Tables(0).Rows(j).Item(8)

            'New SQL based commands to insert these rows on the fly, without an adapter. 
            With altNameTable.Tables(0).Rows(j)
                Dim sqlInsertString As String = "INSERT INTO [ASRAlternateName] " & _
                                "([DirRefNum],[AlternateName],[PreferredName],[ASR_AlternateName],[Source] " & _
                                ",[DirtyBit],[Date_Entered],[AGM_DirtyBit]) " & _
                                "VALUES (" & .Item(1) & ",'" & .Item(2) & _
                                "','" & .Item(3) & "','" & .Item(4) & "','" & .Item(5) & "','" & _
                                .Item(6) & "','" & .Item(7) & "','" & .Item(8) & "')"
                Dim insertComm As New SqlClient.SqlCommand(sqlInsertString, is4Conn)

                Try
                    insertComm.ExecuteNonQuery()
                Catch ex As Exception
                    App.TraceLog(TraceLevel.Error, "syncIs4AltName - " & ex.Message)
                End Try

            End With





            'is4AltNameTable.Tables(0).Rows.Add(aRow)
        Next

        'Commit our changes!
        'Try
        '    da.Update(is4AltNameTable, "altNameTable")
        '    Dim this As New SqlClient.SqlCommand()
        '    this.Parameters.Add(New SqlClient.SqlParameter(
        'Catch ex As Exception
        'End Try

        is4Conn.Close()

    End Sub

    Private Sub getLicenseTable()
        'Gets the licensure table from the Intellispeech4 database, and updates it. Call this AFTER getAltNameTable.
        'Per larry's request on 9/2/2008, we should now clear and re-populate the license table - so that it is self setting...

        'START HERE TUESDAY!

        'Step 2: gather all data needed.
        'Step 3: Insert the new row.
        App.TraceLog(TraceLevel.Verbose, "VERBOSE - getLicenseTable(): Getting the License table from the IntelliSpeech db and updating it.")

        Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("speech"))
        Dim sqlStatement As String
        Dim sqlUpdater As New SqlClient.SqlCommand

        is4Conn.Open()

        'Step 1: Remove the row that holds the current site's information.
        sqlStatement = "DELETE FROM [ASR_License] WHERE [Customer_Name] = '" & _
                            configs.selectedSite & "'"
        sqlUpdater = New SqlClient.SqlCommand(sqlStatement, is4Conn)

        Try
            Dim numAffected As Integer
            numAffected = sqlUpdater.ExecuteNonQuery()
            App.TraceLog(TraceLevel.Verbose, "VERBOSE - License Table Update: delete affected " & numAffected & " rows.")
        Catch ex As SqlClient.SqlException
            App.TraceLog(TraceLevel.Error, "ERROR - There was an error trying to remove rows from the license table: " & ex.ToString)
        End Try

        Dim rand As New Random(Now.Second)
        Dim dirOver As Integer = 0
        Dim PoNum As String = Now.Ticks.ToString.Substring(Now.Ticks.ToString.Length - 6, 6)

        'Set the dir over to true if they have too many names. 
        If dirCount > configs.maxLicensedNames Then dirOver = 1

        sqlStatement = "INSERT INTO [ASR_License] " & _
            "([Customer_Name] " & _
           ",[Dir_Lic] " & _
           ",[Email_Lic] " & _
           ",[Dir_Count] " & _
           ",[Email_Count] " & _
           ",[Alt_Name_Count] " & _
           ",[Dir_Over] " & _
           ",[Email_Over] " & _
           ",[PO] " & _
           ",[ENS_Ports] " & _
           ",[Speech_Ports]) " & _
           " VALUES ('" & configs.selectedSite & "'," & configs.maxLicensedNames & ",0," & dirCount & ",0," & altNameCount & "," & dirOver & ",0," & PoNum & "," & configs.ensPortsAvail & "," & configs.speechPortsAvail & ")"


        Try
            Dim numAffected As Integer = 0
            sqlUpdater = New SqlClient.SqlCommand(sqlStatement, is4Conn)
            numAffected = sqlUpdater.ExecuteNonQuery() 'Execute the insert. 
            App.TraceLog(TraceLevel.Verbose, "VERBOSE - License Table Update: delete affected " & numAffected & " rows.")
        Catch ex As Exception
            App.TraceLog(TraceLevel.Error, "ERROR - getLicenseTable - " & ex.Message)
            is4Conn.Close()
        End Try

        is4Conn.Close()
    End Sub

    Private Sub getCallLogTable()
        'Fills the calllog table that will be passed to our report. Also calls the startdate and enddate functions from within the query. 
        'You might need to have a different port number here. 

        App.TraceLog(TraceLevel.Verbose, "VERBOSE - getCallLogTable(): Getting the callLog table data AND license table data from the IntelliSpeech DB.")
        Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("speech"))
        Dim sqlStatement As String

        sqlStatement = "SELECT     ASR_Call_Log.Call_ID, ASR_Call_Log.Machine, ASR_Call_Log.Channel, ASR_Call_Log.Call_Date, ASR_Call_Log.AA, ASR_Call_Log.AA_Refnum, " & _
                        "ASR_Call_Log.AA_Destination, ASR_Call_Log.PA, ASR_Call_Log.PA_Refnum, ASR_Call_Log.GI, ASR_Call_Log.Operator, ASR_Call_Log.DTMF," & _
                        "ASR_Call_Log.DTMF_Digits, ASR_Call_Log.Command, ASR_Call_Log.Patient, ASR_Call_Log.Email, ASR_Call_Log.msrepl_tran_version," & _
                        "ASR_Call_Log.Call_Duration, ASR_Call_Log.Authenticate, ASR_Call_Log.Hangup, ASR_Call_Log.Status_Code, ASR_Call_Log.noInput," & _
                        "ASR_Call_Log.noMatch, ASR_Call_Log.VRU, ASR_Call_Log.ID,ASR_Call_Log.CASLOC, ASR_License.Dir_Lic, ASR_License.Dir_Count, ASR_License.Alt_Name_Count," & _
                        "ASR_License.Customer_Name,ASR_License.ENS_Ports, ASR_License.Speech_Ports, ASR_Call_Log.Paged, ASR_Call_Log.Transfered " & _
                        "FROM ASR_Call_Log CROSS JOIN ASR_License " & _
                        "WHERE     ASR_Call_Log.Call_Date >= (cast(convert(varchar(8),getdate() " & dateStart & ",1) as datetime)) " & _
                            " AND ASR_Call_Log.Call_Date < (cast(convert(varchar(8),getdate() " & dateEnd & ",1) as datetime)) " & _
                            " AND (NOT (ASR_Call_Log.Call_ID = N'Unitialized')) " & _
                            " AND (NOT (ASR_Call_Log.Call_ID = N'999AAA888BBB777CCC666DDD555EEE')) " & _
                            " AND ASR_Call_Log.CASLOC = '" & configs.selectedSite & "'" & _
                            " AND ASR_License.Customer_Name = '" & configs.selectedSite & "'"

        'Debug for outputting what the query was after all computations. This is to track down the month change bug. 
        App.TraceLog(TraceLevel.Info, "SQL STATEMENT: " & sqlStatement)
        'TODO: Remove the following BEFORE BUILD!
        'MsgBox(sqlStatement)


        is4Conn.Open()

        Dim da As New SqlClient.SqlDataAdapter(sqlStatement, is4Conn)

        da.Fill(callLogTable, "CallLogTable")

        If Not sanityCheck() Then
            dealWithInsanity(sqlStatement)
            Dim badCallLogEx As New badCallLogEx("Call log error detected. Please view log file directory for xml file of call log and SqlStatement.")
            Throw badCallLogEx

        End If


        'If police Then
        Dim policer As New CallLogPolicer

        callLogTable = policer.PoliceCalls(callLogTable)

        callLogTable.WriteXml(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\CallLog.xml")

        'End If

        'Sets the table data source that will be used by the report. 
        transDataSource.Value = callLogTable.Tables(0)
        is4Conn.Close()

        'TEST CODE
        'Dim testSelector As String = "SELECT * FROM [ASR_Call_Log_MATT_TEST]"
        'Dim testDs As New DataSet

        'da = New OleDb.OleDbDataAdapter(testSelector, is4Conn)

        'da.Fill(testDs)

        'testDs.Tables(0).Clear()

        'Dim commander As New OleDb.OleDbCommandBuilder(da)
        'da.Update(testDs)

        'testDs = New DataSet

        'testDs = callLogTable

        'da.Update(testDs.Tables(0))

        ''END TEST CODE

        'is4Conn.Close()
    End Sub

    Private Sub GenerateTransaction()
        'Creates the transaction report and renders it to pdf. 
        App.TraceLog(TraceLevel.Verbose, "VERBOSE - GenerateTransaction(): Creating Transaction Report.")
        Dim transReport As New Microsoft.Reporting.WinForms.LocalReport

        'Set the report path. 
        transReport.ReportPath = configs.reportLocation & "Transaction_Report.rdlc"

        'Add the report data source that contains the datatable we need. 
        transReport.DataSources.Add(transDataSource)

        'Add parameters for the text representation of the date range. 
        Dim reportParams(3) As Microsoft.Reporting.WinForms.ReportParameter



        reportParams(0) = New Microsoft.Reporting.WinForms.ReportParameter("Start_Date", dateStartString, False)
        reportParams(1) = New Microsoft.Reporting.WinForms.ReportParameter("End_Date", dateEndString, False)
        reportParams(2) = New Microsoft.Reporting.WinForms.ReportParameter("Customer", configs.Customer_Name)
        reportParams(3) = New Microsoft.Reporting.WinForms.ReportParameter("Casloc", configs.selectedSite)


        transReport.SetParameters(reportParams)



        'Code to render and write the code to a file. 
        Dim deviceInfo As String = ""

        Dim mimeType As String = ""

        Dim encoding As String = ""

        Dim fileNameExtension As String = ""

        Dim streams() As String = Nothing

        Dim warnings() As Microsoft.Reporting.WinForms.Warning = Nothing

        Dim bytes() As Byte

        'Dim file As System.IO.FileStream

        Dim fileLocation As String


        bytes = transReport.Render("PDF", deviceInfo, mimeType, encoding, fileNameExtension, streams, warnings)


        'Set up fileLocation so that the filename does not resolve to 'default' if it is only one site - use customer name instead. 

        fileLocation = configs.renderedLocation & "TMP\" & DateHeader() & "-"

        If configs.selectedSite = "default" Then
            fileLocation = fileLocation & configs.Customer_Name
        Else
            fileLocation = fileLocation & configs.selectedSite
        End If

        fileLocation = fileLocation & "-TransactionReport.pdf"
        RaiseEvent fileMade(New FileCreatedEventArgs(fileLocation))

        'file = New System.IO.FileStream(fileLocation, IO.FileMode.Create) 'Make this a variable, use that to pass to email. 

        'file.Write(bytes, 0, bytes.Length)

        'file.Close()

        My.Computer.FileSystem.WriteAllBytes(fileLocation, bytes, False)

        emailer.AddReportFile(fileLocation)

        App.TraceLog(TraceLevel.Verbose, "VERBOSE - GenerateTransaction(): Done. Report rendered to: " & fileLocation)
    End Sub
    Private Sub GenerateTransfer()

        checkTransferReportView()
        'Generates the Transfer Report from a specificaly made call log table joined with the ASR_altname table. Call after the Alt name sync for up-to-date results!

        App.TraceLog(TraceLevel.Verbose, "VERBOSE - GenerateTransfer(): Getting combined call log and license table data and creating Transfer report.")

        Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("speech"))

        'Set the SQL statement the report will use. 


        Dim sqlStatement As String
        'sqlStatement = "SELECT     ASR_Call_Log.ID, ASR_Call_Log.Call_ID, ASR_Call_Log.Machine, ASR_Call_Log.Channel, ASR_Call_Log.Call_Date, ASR_Call_Log.AA, " & _
        '                "ASR_Call_Log.AA_Refnum, ASR_Call_Log.AA_Destination, ASR_Call_Log.CASLOC, " & _
        '                "CASE WHEN ASRAlternateName.AlternateName IS null THEN AA_Destination " & _
        '                "ELSE ASRAlternateName.AlternateName END AS AlternateName,ASRAlternateName.DirRefNum " & _
        '                "FROM ASR_Call_Log LEFT JOIN " & _
        '                            "ASRAlternateName ON ASR_Call_Log.AA_Refnum = ASRAlternateName.DirRefNum " & _
        '                "WHERE     (ASR_Call_Log.AA_Destination <> N'') AND (ASR_Call_Log.Call_Date >= (getDate()  " & dateStart & ")) AND (ASR_Call_Log.Call_Date < getDate() " & dateEnd & ")" & _
        '                "AND (ASR_Call_Log.CASLOC = '" & configs.selectedSite & "')" & _
        '                "ORDER BY ASR_Call_Log.Call_Date"

        'sqlStatement = "SELECT     ASR_Call_Log.ID, ASR_Call_Log.Call_ID, ASR_Call_Log.Machine, ASR_Call_Log.Channel, ASR_Call_Log.Call_Date, ASR_Call_Log.AA, " & _
        '                          "ASR_Call_Log.AA_Refnum, ASR_Call_Log.AA_Destination, ASR_Call_Log.CASLOC, ASRAlternateName.AlternateName, ASRAlternateName.DirRefNum " & _
        '              "FROM ASR_Call_Log LEFT OUTER JOIN " & _
        '                          "ASRAlternateName ON ASR_Call_Log.AA_Refnum = ASRAlternateName.DirRefNum " & _
        '              "WHERE     (ASR_Call_Log.AA_Destination <> N'') AND (ASR_Call_Log.Call_Date >= (getDate()  " & dateStart & ")) AND (ASR_Call_Log.Call_Date < getDate() " & dateEnd & ")" & _
        '              "AND (ASR_Call_Log.CASLOC = '" & configs.selectedSite & "') and asralternateName.preferredName=1 " & _
        '              "ORDER BY ASR_Call_Log.Call_Date"

        sqlStatement = "Select ID,call_ID,Machine,Channel,CAll_date,AA,AA_Refnum,AA_Destination,CASLOC,AlternateName,Dirrefnum" & _
                        " FROM TransferReportView where  (AA_Destination <> N'') AND (Call_Date >= (DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE() " & dateStart & ")))) " & _
                        " AND (Call_Date < DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE() " & dateEnd & "))) AND (CASLOC = '" & configs.selectedSite & "') and (PreferredName=1 or PreferredName is null)" & _
                        " ORDER BY Call_Date desc "

        App.TraceLog(TraceLevel.Info, "SQL STATEMENT: " & sqlStatement)

        Dim da As New SqlClient.SqlDataAdapter(sqlStatement, is4Conn)
        Dim ds As New DataSet
        da.Fill(ds, "CallLog")

        'We are finished with the database.
        is4Conn.Close()


        'Set the report data source. This is the interface between the report and our local datasets. 
        furDataSource = New Microsoft.Reporting.WinForms.ReportDataSource("CallLog")
        furDataSource.Value = ds.Tables(0)


        'Now we generate the report!
        Dim furReport As New Microsoft.Reporting.WinForms.LocalReport

        'Set the report path. 
        furReport.ReportPath = configs.reportLocation & "TransferReport.rdlc"

        'Add the report data source that contains the datatable we need. 
        furReport.DataSources.Add(furDataSource)

        'Add parameters for the text representation of the date range. 
        Dim reportParams As New List(Of Microsoft.Reporting.WinForms.ReportParameter)
        reportParams.Add(New Microsoft.Reporting.WinForms.ReportParameter("Start_Date", dateStartString))
        reportParams.Add(New Microsoft.Reporting.WinForms.ReportParameter("End_Date", dateEndString))
        furReport.SetParameters(reportParams)

        'Code to render and write the code to a file. 
        Dim deviceInfo As String = ""

        Dim mimeType As String = ""

        Dim encoding As String = ""

        Dim fileNameExtension As String = ""

        Dim streams() As String = Nothing

        Dim warnings() As Microsoft.Reporting.WinForms.Warning = Nothing

        Dim bytes() As Byte

        Dim fileLocation As String

        bytes = furReport.Render("PDF", deviceInfo, mimeType, encoding, fileNameExtension, streams, warnings)

        fileLocation = configs.renderedLocation & "TMP\" & DateHeader() & "-"

        If configs.selectedSite = "default" Then
            fileLocation = fileLocation & configs.Customer_Name
        Else
            fileLocation = fileLocation & configs.selectedSite
        End If

        fileLocation = fileLocation & "-TransferReport.pdf"
        RaiseEvent fileMade(New FileCreatedEventArgs(fileLocation))

        My.Computer.FileSystem.WriteAllBytes(fileLocation, bytes, False)
        'file = New System.IO.FileStream(fileLocation, IO.FileMode.Create) 'Make this a variable, use that to pass to email. 

        'file.Write(bytes, 0, bytes.Length)

        'file.Close()

        'file.Dispose()

        emailer.AddReportFile(fileLocation)

        App.TraceLog(TraceLevel.Verbose, "VERBOSE - GenerateTransfer(): Done. Report rendered to: " & fileLocation)

    End Sub
    Private Sub checkTransferReportView()

        Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("speech"))
        Dim sqlStatement As String
        sqlStatement = "IF not EXISTS(select * FROM sys.views where name = 'TransferReportView')  " & _
        " BEGIN" & _
        " exec ('CREATE VIEW [dbo].[TransferReportView] AS " & _
        " SELECT     dbo.ASR_Call_Log.ID, dbo.ASR_Call_Log.Call_ID, dbo.ASR_Call_Log.Machine, dbo.ASR_Call_Log.Channel, dbo.ASR_Call_Log.Call_Date, " & _
        "                       CASE WHEN dbo.ASR_Call_Log.Command = 1 AND dbo.ASR_Call_Log.AA_Refnum = - 1 THEN - 888 ELSE dbo.asr_call_log.AA_Refnum END AS AA_Refnum, " & _
        "                       dbo.ASR_Call_Log.AA_Destination, dbo.ASR_Call_Log.CASLOC, dbo.ASRAlternateName.AlternateName, dbo.ASRAlternateName.DirRefNum," & _
        "                       dbo.ASRAlternateName.PreferredName, dbo.ASR_Call_Log.AA" & _
        " FROM          dbo.ASR_Call_Log LEFT OUTER JOIN" & _
        "                        dbo.ASRAlternateName ON dbo.ASR_Call_Log.AA_Refnum = dbo.ASRAlternateName.DirRefNum" & _
        " WHERE(dbo.ASR_Call_Log.AA_Refnum <> 0) And (dbo.ASR_Call_Log.Status_Code < 4000) ') " & _
        " End"

        Dim cmd As New SqlClient.SqlCommand(sqlStatement, is4Conn)
        is4Conn.Open()
        cmd.ExecuteNonQuery()
        is4Conn.Close()

    End Sub
    Private Sub GenerateChReport()
        'Generates the channel report. Pulls down a limited call log table. 

        App.TraceLog(TraceLevel.Verbose, "VERBOSE - GenerateChReport(): Getting call log table with specific data and generating Calls Per Channel Report.")

        Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("speech"))

        'Set the SQL statement the report will use. 
        Dim sqlStatement As String
        'sqlStatement = "SELECT     ASR_Call_Log.ID, ASR_Call_Log.Call_ID, ASR_Call_Log.Machine, ASR_Call_Log.Channel, ASR_Call_Log.Call_Date, ASR_Call_Log.Call_Duration, " & _
        '                            "ASR_License.Customer_Name,ASR_Call_Log.CASLOC " & _
        '                "FROM   ASR_Call_Log CROSS JOIN ASR_License " & _
        '                "WHERE     (ASR_Call_Log.Call_Date >= (getDate() " & dateStart & ")) AND (ASR_Call_Log.Call_Date < getDate() " & dateEnd & ") AND (NOT (ASR_Call_Log.Call_ID = N'Unitialized')) AND (NOT (ASR_Call_Log.Call_ID = N'999AAA888BBB777CCC666DDD555EEE')) AND ASR_CAll_Log.CASLOC = '" & configs.selectedSite & "'" & _
        '                "ORDER BY ASR_Call_Log.Call_Date"
        sqlStatement = "SELECT     ASR_Call_Log.ID, ASR_Call_Log.Call_ID, ASR_Call_Log.Machine, ASR_Call_Log.Channel, ASR_Call_Log.Call_Date, ASR_Call_Log.Call_Duration, " & _
                                    "ASR_License.Customer_Name,ASR_Call_Log.CASLOC " & _
                        "FROM   ASR_Call_Log inner join ASR_License on asr_license.Customer_Name=asr_call_log.casloc " & _
                        "WHERE     (ASR_Call_Log.Call_Date >= (getDate() " & dateStart & ")) AND (ASR_Call_Log.Call_Date < getDate() " & dateEnd & ") AND (NOT (ASR_Call_Log.Call_ID = N'Unitialized')) AND (NOT (ASR_Call_Log.Call_ID = N'999AAA888BBB777CCC666DDD555EEE')) AND ASR_CAll_Log.CASLOC = '" & configs.selectedSite & "'" & _
                        "ORDER BY ASR_Call_Log.Call_Date"

        App.TraceLog(TraceLevel.Info, "SQL STATEMENT: " & sqlStatement)

        Dim da As New SqlClient.SqlDataAdapter(sqlStatement, is4Conn)
        Dim ds As New DataSet
        da.Fill(ds, "CallLog")

        'We are finished with the database.
        is4Conn.Close()

        'Set the report data source. This is the interface between the report and our local datasets. 
        chanDataSource = New Microsoft.Reporting.WinForms.ReportDataSource("CallLog")
        chanDataSource.Value = ds.Tables(0)

        'Now we generate the report!
        Dim chanReport As New Microsoft.Reporting.WinForms.LocalReport

        'Set the report path. 
        chanReport.ReportPath = configs.reportLocation & "Call Channel Report.rdlc"

        'Add the report data source that contains the datatable we need. 
        chanReport.DataSources.Add(chanDataSource)

        'Add parameters for the text representation of the date range. 
        Dim reportParams As New List(Of Microsoft.Reporting.WinForms.ReportParameter)
        reportParams.Add(New Microsoft.Reporting.WinForms.ReportParameter("Start_Date", dateStartString))
        reportParams.Add(New Microsoft.Reporting.WinForms.ReportParameter("End_Date", dateEndString))
        chanReport.SetParameters(reportParams)



        'Code to render and write the code to a file. 
        Dim deviceInfo As String = ""

        Dim mimeType As String = ""

        Dim encoding As String = ""

        Dim fileNameExtension As String = ""

        Dim streams() As String = Nothing

        Dim warnings() As Microsoft.Reporting.WinForms.Warning = Nothing

        Dim bytes() As Byte

        Dim fileLocation As String



        bytes = chanReport.Render("PDF", deviceInfo, mimeType, encoding, fileNameExtension, streams, warnings)



        fileLocation = configs.renderedLocation & "TMP\" & DateHeader() & "-"

        If configs.selectedSite = "default" Then
            fileLocation = fileLocation & configs.Customer_Name
        Else
            fileLocation = fileLocation & configs.selectedSite
        End If

        fileLocation = fileLocation & "-Call Channel Report.pdf"
        RaiseEvent fileMade(New FileCreatedEventArgs(fileLocation)) 'Raised in order to alert the controller that we are getting ready to delete a file. 

        'file = New System.IO.FileStream(fileLocation, IO.FileMode.Create) 'Make this a variable, use that to pass to email. 

        'file.Write(bytes, 0, bytes.Length)

        'file.Close()

        My.Computer.FileSystem.WriteAllBytes(fileLocation, bytes, False)

        emailer.AddReportFile(fileLocation)

        App.TraceLog(TraceLevel.Verbose, "VERBOSE - GenerateChReport(): Done. Report rendered to: " & fileLocation)

    End Sub

    Private Function AWeekAgo(ByVal day As Date) As String

        Dim formattedDate As String
        Dim sevenDaysAgo As New Date
        'Dim rightNow As New Date

        'rightNow = Today
        sevenDaysAgo = day.AddDays(-7)

        formattedDate = Format(sevenDaysAgo, "MM/dd/yyyy")


        Return formattedDate

    End Function

    Private Function EndDate(ByVal day As Date) As String
        Return Format(day.AddDays(1), "MM/dd/yyyy")
    End Function

    Private Function DateHeader() As String
        Return Format(Today, "MM-dd-yyyy")
    End Function
    Private Function getLastSaturday() As DateTime
        Dim day As DateTime = Now.Date
        'Finds last saturdy
        While Not day.DayOfWeek = DayOfWeek.Saturday
            day = day.AddDays(-1)
        End While
        Return day
    End Function

    Private Function getSundayReportStartDate(ByVal endingSaturday As DateTime) As DateTime
        Dim day As DateTime = endingSaturday
        While Not day.DayOfWeek = DayOfWeek.Sunday
            day = day.AddDays(-1)
        End While
        Return day
    End Function

    Private Function getStartDate(ByVal day As Date) As String
        Dim daysAgo As TimeSpan
        Dim returnDay As String = ""

        ' check if date is earlier than today
        If (day < Now.Date) Then
            ' Find out how many days ago day was
            daysAgo = day - Now.Date
            returnDay = daysAgo.Days
            ' check if day is today
        ElseIf (day = Now.Date) Then
            returnDay = ""
        Else
            Dim AE = New AmcomException("Generate Reports Exception: Start Date must occur prior to today " & Now.Date.ToString())            
            App.ExceptionLog(AE)
        End If

        Return returnDay
    End Function
    Private Function getEndDate(ByVal day As Date) As String

        Dim daysAgo As TimeSpan
        Dim returnDay As String = ""
        ' check if date selected is in the future
        If (day > Now.Date) Then
            daysAgo = day - Now.Date
            returnDay = "+" & daysAgo.Days + 1

        ElseIf (day = Now.Date) Then
            returnDay = "+1"

        ElseIf (day < Now.Date) Then
            daysAgo = day - Now.Date
            If (daysAgo.Days < -1) Then
                returnDay = daysAgo.Days + 1
            ElseIf (daysAgo.Days = -1) Then
                returnDay = ""
            End If           
        End If

        Return returnDay
    End Function


    Private Function reportAlreadyExists(ByVal site As String) As Boolean
        Dim fileExists As Boolean = False

        If site = "default" Then
            If (FileIO.FileSystem.FileExists(configs.renderedLocation & DateHeader() & "-" & configs.Customer_Name & "-TransactionReport.pdf")) Then
                fileExists = True
            End If
        Else
            If (FileIO.FileSystem.FileExists(configs.renderedLocation & DateHeader() & "-" & configs.selectedSite & "-TransactionReport.pdf")) Then
                fileExists = True
            End If
        End If

        Return fileExists
    End Function

    Private Function sanityCheck() As Boolean
        Dim sanity As Boolean = False

        Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("speech"))

        'Set the SQL statement the report will use. 
        Dim sqlStatement As String
        sqlStatement = "SELECT COUNT(*) FROM [ASR_Call_Log] " & _
                        "WHERE     ASR_Call_Log.Call_Date >= (cast(convert(varchar(8),getdate() " & dateStart & ",1) as datetime)) " & _
                            " AND ASR_Call_Log.Call_Date < (cast(convert(varchar(8),getdate() " & dateEnd & ",1) as datetime)) " & _
                            " AND (NOT (ASR_Call_Log.Call_ID = N'Unitialized')) " & _
                            " AND (NOT (ASR_Call_Log.Call_ID = N'999AAA888BBB777CCC666DDD555EEE')) " & _
                            " AND ASR_Call_Log.CASLOC = '" & configs.selectedSite & "'"


        App.TraceLog(TraceLevel.Verbose, "Sanity Check SQL: " & sqlStatement)

        is4Conn.Open()

        Dim sqlCountCommand As New SqlClient.SqlCommand(sqlStatement, is4Conn)

        Dim numberOfRows As Integer = sqlCountCommand.ExecuteScalar

        App.TraceLog(TraceLevel.Verbose, "Site: " & configs.selectedSite & " should have " & numberOfRows & " rows in the dataset.")
        App.TraceLog(TraceLevel.Verbose, "Site: " & configs.selectedSite & " actualy has " & callLogTable.Tables(0).Rows.Count & " rows in the dataset.")

        If numberOfRows = callLogTable.Tables(0).Rows.Count Then
            sanity = True
        End If

        App.TraceLog(TraceLevel.Verbose, "SQL Sanity check result for site " & configs.selectedSite & ": " & sanity.ToString)

        Return sanity

    End Function

    'Sends an alert to home when we find that the call log is not what it should be. Has a hard-coded e-mail in it. 
    Private Sub dealWithInsanity(ByVal queryText As String)
        App.TraceLog(TraceLevel.Error, "ERROR - Invalid Call_Log detected! The call log count for the date range was lower than the rows in the dataset - query did not return expected data. Exiting report generation!")

        App.TraceLog(TraceLevel.Verbose, "Writing contents of call log and query to: " & App.LogPath)
        callLogTable.Tables(0).WriteXml(App.LogPath & "Bad_Call_Log_" & DateHeader())
        My.Computer.FileSystem.WriteAllText(App.LogPath & "Bad_Call_Log_Query_" & DateHeader(), queryText, False)

        'Sending the e-mail here. If an error occurs inside this block, it's logged but the code doesn't stop. 
        Try

            App.TraceLog(TraceLevel.Verbose, "Attempting to send alert to Development...")
            Dim warningMail As New Net.Mail.MailMessage(configs.mailFromAddress, "mstudley@sdcsolutions.com")
            warningMail.Subject = "Call Log concurrency check failure."

            warningMail.Body = "The call log for customer: " & configs.Customer_Name & " failed a concurrency check. Please troubleshoot this site." & _
                                vbCrLf & "SQL Statement:" & vbCrLf & queryText

            Dim emailSender As New System.Net.Mail.SmtpClient(configs.mailServer, configs.mailPort)
            Dim credentials As New System.Net.NetworkCredential(configs.mailUser, configs.mailPass)

            emailSender.Credentials = credentials

            emailSender.Send(warningMail)

            App.TraceLog(TraceLevel.Verbose, "...Alert sent.")

        Catch ex As Net.Mail.SmtpException
            App.TraceLog(TraceLevel.Warning, "...Could not send warning to development. Reason: " & ex.ToString)
        End Try

    End Sub


End Class
