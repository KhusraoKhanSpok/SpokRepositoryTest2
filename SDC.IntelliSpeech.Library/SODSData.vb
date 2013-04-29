Imports System.Data.SqlClient
Imports System.Threading
Imports Amcom.SDC.BaseServices
Public Class SODSData
    Private mDb As String = ""
    Private mServer As String = ""
    Private mUser As String = ""
    Private mPass As String = ""
    Private mConnectionString As String = ""
    Private mUseIntegratedSecuity As Boolean
    Private mPollInterval As Integer = 10
    Private mStop As Boolean = False
    Private mTransactions As ENSTransactions
    Private mPagerTimeoutPeriod As Integer = 7
    Private mCampaignMaxResponders As Dictionary(Of String, responder)
    Public Event NewOutBoundCall As EventHandler(Of NewOutboundCallEventArgs)
    Public Event CampaignCompleted As EventHandler(Of CampaignCompletedEventArgs)
    Private _pollthread As Thread = Nothing
    Public Enum InProcessCallState As Integer
        newCall
        preProcess
        inProcess
        complete
        failed
    End Enum
    Public Enum CompaignCompleteReason
        finished
        waskilled
        maxRespondersReached
    End Enum
    Public Sub New(ByVal connectionString As String, ByVal transactions As ENSTransactions)
        'mServer = server
        'mDb = database
        'mUser = userName
        'mPass = password
        mTransactions = transactions
        ' mUseIntegratedSecuity = useIntegratedSecuity
        mConnectionString = connectionString
        mCampaignMaxResponders = New Dictionary(Of String, Responder)
    End Sub
    Public Sub New(ByVal connectionString As String)
        mConnectionString = connectionString
    End Sub
    Public Sub Start()
        _pollthread = New Thread(AddressOf PollThread)
        _pollthread.IsBackground = True
        _pollthread.Name = "SODS DB Poller"
        _pollthread.Start()
        mPagerTimeoutPeriod = GetPagerTimeoutPeriod()
    End Sub
    Public Sub [Stop]()
        App.TraceLog(TraceLevel.Info, "Stopping SODS Data monitor")
        mStop = True
        _pollthread.Abort()
    End Sub
    Private Sub PollThread()
        Do While mStop = False
            Try
                PollDatabase()
                Threading.Thread.Sleep(mPollInterval * 1000)
            Catch ex As ThreadAbortException
                App.TraceLog(TraceLevel.Info, "SODS Data Monitor stopped")
                mStop = True
                Exit Do
            End Try
        Loop
    End Sub

    Public Sub FailCall(ByVal c As DialoutCall)
        App.TraceLog(TraceLevel.Verbose, "Failing call to " & c.PhoneNumber)
        Dim calls As New Dictionary(Of String, DialoutCall)
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during FailCall(): " & ex.Message)
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "UPDATE INTTRAN SET TRNPARM5='Maximum Retries exceeded',TRNDONE='E',TRNEBY='SDCISS',TRNDBY='SPEECH',TRNDDAT=getDate() WHERE TRNREFNUM = " & c.DBRecordID
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "Failed to update database during FailCall(): " & ex.Message)

            End Try
        End Using


    End Sub
    Public ReadOnly Property RetryCount() As Integer
        Get
            Using conn As New SqlConnection(Me.dbConnectString)
                Try
                    conn.Open()
                Catch ex As SqlException
                    App.TraceLog(TraceLevel.Error, "Failed to connect to database during UpdateRetryCount(): " & ex.Message)
                End Try
                Dim cmd As New SqlCommand
                cmd.Connection = conn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "Select OutboundRetryAttempts from ENSCONFIG"
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader
                    If Not reader.HasRows Then
                        Return 3
                    Else
                        reader.Read()
                        Return reader!OutboundRetryAttempts
                    End If
                Catch ex As InvalidOperationException
                    Return 3
                Catch ex As SqlException
                    Return 3
                End Try
            End Using
        End Get
    End Property
    Public Sub UpdateRetryCount(ByRef c As DialoutCall)
        App.TraceLog(TraceLevel.Verbose, "Updating Retry Count for " & c.PhoneNumber & " to " & c.RetryCount)
        Dim calls As New Dictionary(Of String, DialoutCall)
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during UpdateRetryCount(): " & ex.Message)
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            Dim ret As String = ""
            If c.RetryCount = 0 Then
                ret = Format(Now, "hh:mm tt") & " Initial Call"
            Else
                ret = Format(Now, "hh:mm tt") & " Retry " & c.RetryCount
            End If
            cmd.CommandText = "UPDATE INTTRAN SET TRNPARM5='" & ret & "' WHERE TRNREFNUM = " & c.DBRecordID
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "Failed to update database during UpdateRetryCount(): " & ex.Message)

            End Try
        End Using

    End Sub
    Public Function GetCall(ByVal id As String) As DialoutCall
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during GetCall(): " & ex.Message)
                Return Nothing
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            Dim criteria As String = ""
            cmd.CommandText = "Select INTTRAN.*, (select TRNDESC from INTTRAN as INTTRAN2 Where INTTRAN2.TRNREFNUM=INTTRAN.EVENT_ID) as EventDesc from Inttran where TRNREFNUM = " & id
            Try
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If Not reader.HasRows Then Return Nothing
                    While reader.Read

                        If IsDBNull(reader!TRNTYP) Then
                            App.TraceLog(TraceLevel.Error, "TRNTYP is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".   The call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!Event_ID) Then
                            App.TraceLog(TraceLevel.Error, "EVENT_ID is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".  The call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!TRNPARM3) Then
                            App.TraceLog(TraceLevel.Error, "VXML Application URL is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".  The call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!TRNPARM1) Then
                            App.TraceLog(TraceLevel.Error, "Phone Number is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".  The call will be ignored.  Check database.")
                            Continue While
                        End If
                        Dim tmp As String = reader!TRNPARM5 & ""
                        If tmp.ToUpper.Contains("INITIAL") Then
                            retryCount = 0
                        Else
                            If IsNumeric(Right(tmp, 1)) Then
                                retryCount = CInt(Mid(tmp, InStrRev(tmp, " ") + 1))
                            Else
                                retryCount = 0
                            End If
                        End If
                        Dim d As DialoutCall = New DialoutCall(reader!TRNREFNUM, reader!EVENT_ID, reader!EventDesc & "", retryCount, reader!TRNPARM3, reader!TRNPARM1, mTransactions.ProirityFromTransactionType(reader!TRNTYP))
                        If IsDBNull(reader!TRNDONE) Then
                            d.Status = DialoutCall.CallStatus.newcall
                        Else
                            Select Case reader!TRNDONE
                                Case ""
                                    d.Status = DialoutCall.CallStatus.newcall
                                Case "PP"
                                    d.Status = DialoutCall.CallStatus.preprocess
                                Case "P"
                                    d.Status = DialoutCall.CallStatus.inprocess
                                Case "D"
                                    d.Status = DialoutCall.CallStatus.callComplete
                                Case "F"
                                    d.Status = DialoutCall.CallStatus.callFail
                                Case "R"
                                    'an R call is really a Q call that should be retried
                                    d.Status = DialoutCall.CallStatus.inprocess
                            End Select
                        End If
                        Return d
                    End While
                End Using
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Error reading INTTRAN during GetCall: " & ex.Message)
            End Try
        End Using
        Return Nothing

    End Function
    Public Sub UpdateRetryHistory(ByVal d As DialoutCall)
        'EXEC([dbo].[SDC_AddIntTranHist])
        '    @TRNEBY = N'SDCISS',
        '    @TRNTYP = N'ECALLOUT',
        '    @TRNMEMO = Retry attempt number
        '    @TRNREFNUM = TRNREFNUM
        App.TraceLog(TraceLevel.Info, "Updating retry history...")
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during Updateretryhistory(): " & ex.Message)
                Exit Sub
            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "SDC_AddIntTranHist"
            Dim p As New SqlParameter("@TRNEBY", SqlDbType.VarChar, 50, ParameterDirection.Input)
            p.Value = "SDCISS"
            cmd.Parameters.Add(p)
            p = New SqlParameter("@TRNTYP", SqlDbType.VarChar, 10, ParameterDirection.Input)
            p.Value = "ECALLOUT"
            cmd.Parameters.Add(p)
            p = New SqlParameter("@TRNMEMO", SqlDbType.NVarChar, 1000, ParameterDirection.Input)
            If d.RetryCount = 0 Then
                p.Value = "Initial Dial"
            Else
                p.Value = "Retry " & d.RetryCount
            End If
            cmd.Parameters.Add(p)
            p = New SqlParameter("@TRNREFNUM", SqlDbType.BigInt, 8, ParameterDirection.Input)
            p.Value = d.DBRecordID
            cmd.Parameters.Add(p)
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "Failed to update retry history: " & ex.Message)
            End Try
        End Using
    End Sub
    Public Function GetPagerTimeoutPeriod() As Integer
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                Throw New AmcomException("Issue getting pagerTimeoutPeriod from database", ex)
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during GetPagerTimeoutPeriod(): " & ex.Message & "returning default value")
                Return App.Config.GetString(ConfigSection.modules, "sdcEmergencyNotificationSystem/pagedDaysAgoFailover", "7")

            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandText = "Select ENSPagedTransDaysAgo as Period From ENSConfig"
            cmd.CommandType = CommandType.Text
            Try
                Dim reader As SqlDataReader = cmd.ExecuteReader
                If Not reader.HasRows Then
                    App.TraceLog(TraceLevel.Error, "Failed fetching recordset for configuration GetPagerTimeoutPeriod() returning default value")
                    Return App.Config.GetString(ConfigSection.modules, "sdcEmergencyNotificationSystem/pagedDaysAgoFailover", "7")
                Else
                    reader.Read()
                    Return reader!Period
                End If
            Catch ex As Exception
                Throw New AmcomException("Issue getting pagerTimeoutPeriod from database", ex)
                App.TraceLog(TraceLevel.Error, "Error occurred reading GetPagerTimeoutPeriod: " & ex.Message)
            End Try
        End Using
    End Function
    Public Function GetMaxNumberOfResponders() As Dictionary(Of String, Responder)
        Dim d As New Dictionary(Of String, Responder)
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during GetMaxNumberOfResponders(): " & ex.Message)
                Return d
            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandText = "Select TRNPARM2 as MaxResponders, TRNREFNUM from INTTRAN WHERE TRNTYP='EMERGCALL' AND TRNPARM2 IS NOT NULL AND TRNPARM2<>'' AND TRNPARM2 <>0 AND TRNDONE<>'D'"
            cmd.CommandType = CommandType.Text
            Try
                Dim reader As SqlDataReader = cmd.ExecuteReader
                If Not reader.HasRows Then
                    Return d
                Else
                    While reader.Read
                        App.TraceLog(TraceLevel.Info, "GetMaxResponders():TRNREFNUM:" & reader!TRNREFNUM & "MaxResponders:" & reader!MaxResponders)
                        d.Add(reader!TRNREFNUM & "", New Responder(reader!TRNREFNUM & "", CInt(reader!MaxResponders & "")))
                    End While
                End If
                Return d
            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "Error occurred reading MaxNumberOfResponders: " & ex.Message)
            End Try
            Return d
        End Using
    End Function

    Public Sub SetCampaignCompleted(ByVal campaignID As String, ByVal reason As CompaignCompleteReason)
        App.TraceLog(TraceLevel.Verbose, "Setting Campaign " & campaignID & " completed... ")
        Dim calls As New Dictionary(Of String, DialoutCall)
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during SetCampaignCompleted(): " & ex.Message)
                Exit Sub
            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            'Select Case reason
            '    Case CompaignCompleteReason.maxRespondersReached
            '        cmd.CommandText = "UPDATE INTTRAN Set TRNDBY='SDCISS', TRNPARM2 = '', TRNDONE='D', TRNPARM5='Max Responders Reached' WHERE TRNREFNUM = " & campaignID
            '    Case CompaignCompleteReason.finished
            '        cmd.CommandText = "UPDATE INTTRAN Set TRNDBY='SDCISS', TRNPARM2 = '', TRNDONE='D', TRNPARM5='Campaign completed successfully' WHERE TRNREFNUM = " & campaignID
            '    Case CompaignCompleteReason.waskilled
            '        cmd.CommandText = "UPDATE INTTRAN Set TRNDBY='SDCISS', TRNPARM2 = '', TRNDONE='D', TRNPARM5='Campaign Forcibly Killed' WHERE  TRNREFNUM = " & campaignID
            'End Select
            'Fixed 1/19/09 GBH for LarryK -- Do not set TRNPARM2 to blank!
            Select Case reason
                Case CompaignCompleteReason.maxRespondersReached
                    cmd.CommandText = "UPDATE INTTRAN Set TRNDBY='SDCISS', TRNDONE='D',TRNDDAT=getDate(), TRNPARM5='Max Responders Reached' WHERE TRNREFNUM = " & campaignID
                    updateEventLog(campaignID, "D", "Max Responders Reached")
                Case CompaignCompleteReason.finished
                    cmd.CommandText = "UPDATE INTTRAN Set TRNDBY='SDCISS', TRNDONE='D',TRNDDAT=getDate(), TRNPARM5='Campaign completed successfully' WHERE TRNREFNUM = " & campaignID
                    updateEventLog(campaignID, "D", "Campaign Completed Successfully")
                Case CompaignCompleteReason.waskilled
                    cmd.CommandText = "UPDATE INTTRAN Set TRNDBY='SDCISS', TRNDONE='D',TRNDDAT=getDate(), TRNPARM5='Campaign Forcibly Killed' WHERE  TRNREFNUM = " & campaignID
                    killOutstandingPages(campaignID)
                    updateEventLog(campaignID, "K", "Campaign Forcibly Killed")
            End Select

            cmd.CommandType = CommandType.Text
            Try
                Dim res As Integer = cmd.ExecuteNonQuery()
                Debug.Print(res)
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "SQL Error while performing SetCampaignCompleted(): " & ex.Message)
            End Try

            cmd = New SqlCommand
            cmd.Connection = conn
            cmd.CommandText = "UPDATE INTTRAN Set TRNDBY='SDCISS', TRNDONE='D',TRNDDAT=getDate() WHERE TRNTYP='EMERGCALL' AND TRNREFNUM = " & campaignID & " AND TRNDONE<> 'K'"
            cmd.CommandType = CommandType.Text
            Try
                cmd.ExecuteNonQuery()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "SQL Error while performing SetCampaignCompleted(): " & ex.Message)
            End Try
            For Each dc As DialoutCall In GetCurrentCalls(DialoutCall.CallStatus.inprocess Or _
                            DialoutCall.CallStatus.inprocessRetry Or _
                            DialoutCall.CallStatus.initialized Or _
                            DialoutCall.CallStatus.inProcessRetryWebServiceDown Or _
                            DialoutCall.CallStatus.newcall Or DialoutCall.CallStatus.preprocess, campaignID).Values
                SetCallStatus(dc, DialoutCall.CallStatus.callKilled)
            Next
        End Using
    End Sub
    Public Function GetListOfCompletedCampaigns() As List(Of Integer)
        Dim l As New List(Of Integer)
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during GetListOfCompletedCampaigns(): " & ex.Message)
                Return l
            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            Dim sql As String = "SELECT trnrefnum AS CampaignID from inttran where TRNTYP='EMERGCALL' and TRNDONE<>'D' and TRNREFNUM not in (SELECT DISTINCT EVENT_ID AS [Events] " & _
                                " FROM Inttran where ((TRNDONE<>'D' and TRNDONE<>'K' and TRNDONE<>'F') and TRNTYP='ECALLOUT') or " & _
                                " (TRNTYP='PAGSRV' and (TRNPARM2='N' or TRNPARM2='A') and TRNDONE<>'F') " & _
                                " GROUP BY Inttran.EVENT_ID HAVING Count(Inttran.EVENT_ID)>0)"
            ' removed from query ?" and (EVENT_ID is not null or EVENT_ID<>'0') " & _


            'Dim sql As String = "SELECT DISTINCT  EVENT_ID AS CampaignID" & _
            '                   " FROM INTTRAN " & _
            '                  "WHERE  (NOT (TRNDONE IN (' ', 'Q', 'P')) OR " & _
            '                         "TRNDONE IS NULL) AND (EVENT_ID IS NOT NULL) AND (EVENT_ID <> 0) AND " & _
            '                        "((SELECT     TRNDONE " & _
            '                       "FROM  INTTRAN AS INTTRAN_1 " & _
            '                      "WHERE     (TRNREFNUM = INTTRAN.EVENT_ID and (TRNPARM2='0' or TRNPARM2='-1'))) <> 'D')"


            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text
            Try
                Using reader As SqlDataReader = cmd.ExecuteReader
                    While reader.Read()
                        l.Add(reader!CampaignID)

                    End While
                End Using

            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "SQL Error processing GetListOfCompletedCampaigns(): " & ex.Message)
                Return l
            End Try
        End Using
        Return l

    End Function

    ' Func used to check if any transactions for a campaign exists

    Public Function IsCampaignComplete(ByVal campaign As Integer) As Boolean
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during CloseExpiredOutstandingPages(): " & ex.Message)
                Exit Function
            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            'Dim sql As String = "SELECT Count(*) as total from INTTRAN WHERE (TRNDONE IN (' ','Q','P') OR TRNDONE IS NULL) AND (TRNPARM2<>'A' or TRNPARM2<>'N') AND EVENT_ID = " & campaign
            Dim sql As String = "SELECT Count(*) as total from INTTRAN WHERE ((TRNDONE IN (' ','Q','P') OR TRNDONE IS NULL) OR (TRNTYP='PAGSRV' and (TRNPARM2<>'A' or TRNPARM2<>'N'))) AND EVENT_ID = " & campaign
            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text
            Try
                Using reader As SqlDataReader = cmd.ExecuteReader
                    reader.Read()
                    If reader!total = 0 Then Return True
                    Return False
                End Using

            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "SQL Error processing CloseExpiredOutstandingPages(): " & ex.Message)
            End Try
        End Using

    End Function
    Public Sub CloseExpiredOutstandingPages()
        'Set pager status to 'F' for expired campaings
        App.TraceLog(TraceLevel.Info, "Checking for Expired Page Transactions")
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during CloseExpiredOutstandingPages(): " & ex.Message)
                Exit Sub
            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            Dim sql As String = "UPDATE INTTRAN " & _
                                "SET TRNDONE = 'F',TRNPARM5='Callback Expired @'+CONVERT(varchar,GETDATE(),100) " & _
                                "FROM INTTRAN AS INTTRAN_1 RIGHT OUTER JOIN " & _
                                "INTTRAN ON INTTRAN_1.TRNREFNUM = INTTRAN.EVENT_ID " & _
                                "WHERE (INTTRAN.TRNTYP = 'PAGSRV') AND " & _
                                "(INTTRAN.TRNDONE IN (' ', 'Q', 'P','D') OR INTTRAN.TRNDONE IS NULL) AND " & _
                                "(INTTRAN.TRNPARM2='N' or INTTRAN.TRNPARM2='A') AND " & _
                                "(DateDiff(Day, INTTRAN_1.TRNDAT, GETDATE()) >= " & mPagerTimeoutPeriod & ")"

            cmd.CommandText = sql
            cmd.CommandType = CommandType.Text
            Try
                Dim res As Integer = cmd.ExecuteNonQuery()
                If res > 0 Then App.TraceLog(TraceLevel.Verbose, "Updated " & res & " pager records to complete.")
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "SQL Error processing CloseExpiredOutstandingPages(): " & ex.Message)
            End Try
        End Using
    End Sub
    Public Sub CheckforCompletedCampaigns()

        App.TraceLog(TraceLevel.Verbose, "Checking for Completed Campaigns")
        Dim calls As New Dictionary(Of String, DialoutCall)
        mCampaignMaxResponders = GetMaxNumberOfResponders()
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during CheckforCompletedCampaigns(): " & ex.Message)
                Exit Sub
            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            'cmd.CommandText = "Select Count(*) as ResponderCount, EVENT_ID from INTTRAN Where TRNPARM2='true' and TRNDONE='D'  GROUP BY EVENT_ID ORDER BY EVENT_ID "
            cmd.CommandText = "Select Count(*) as ResponderCount, EVENT_ID from INTTRAN Where TRNPARM2='true' and TRNDONE='D' and EVENT_ID in (select trnrefnum from inttran where trntyp='EMERGCALL' and TRNDONE<>'D')  GROUP BY EVENT_ID ORDER BY EVENT_ID"
            cmd.CommandType = CommandType.Text
            Dim reader As SqlDataReader = Nothing
            Try
                reader = cmd.ExecuteReader
            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "SQL Error while performing CheckForCompleteCampaigns(): " & ex.Message)
                Exit Sub
            End Try
            If Not reader.HasRows Then
                App.TraceLog(TraceLevel.Verbose, "Found no max responders.")
            Else
                While reader.Read
                    Dim count As Integer = reader!ResponderCount
                    Dim campaign As String = reader!Event_ID & ""
                    App.TraceLog(TraceLevel.Info, "Responder Campaign:" & campaign & " was count:" & count)

                    If mCampaignMaxResponders.ContainsKey(campaign) Then
                        App.TraceLog(TraceLevel.Info, "Responder count for Campaign:" & campaign & " running count:" & count & " Campaign requires:" & mCampaignMaxResponders(campaign).MaxResponders)
                        If count >= mCampaignMaxResponders(campaign).MaxResponders AndAlso mCampaignMaxResponders(campaign).MaxResponders <> -1 Then
                            mCampaignMaxResponders(campaign).Completed = True
                        End If
                    End If
                End While
            End If
            reader.Close()
            'now check for campaigns killed directly
            cmd = New SqlCommand
            cmd.Connection = conn
            cmd.CommandText = "Select TRNREFNUM From INTTRAN Where TRNTYP='EMERGCALL' AND TRNDONE='K'"
            cmd.CommandType = CommandType.Text
            reader = Nothing
            Try
                reader = cmd.ExecuteReader
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to check for Killed Campaigns due to SQL Error: " & ex.Message)
            Finally
                If reader.HasRows Then
                    While reader.Read
                        Dim campaign As String = reader!TRNREFNUM & ""
                        App.TraceLog(TraceLevel.Info, "Campaign " & campaign & " was forcibly killed")
                        If mCampaignMaxResponders.ContainsKey(campaign) Then
                            mCampaignMaxResponders(campaign).Completed = True
                            mCampaignMaxResponders(campaign).CampaignKilled = True
                        Else
                            Dim r As New Responder(campaign, 0)
                            r.Completed = True
                            mCampaignMaxResponders.Add(campaign, r)
                        End If
                    End While
                End If
            End Try
        End Using
        For Each campaign As String In mCampaignMaxResponders.Keys
            Dim maxResReached As Boolean = IsCampaignComplete(campaign)
            If mCampaignMaxResponders(campaign).Completed OrElse maxResReached Then
                If mCampaignMaxResponders(campaign).CampaignKilled Then
                    SetCampaignCompleted(campaign, CompaignCompleteReason.waskilled)
                ElseIf maxResReached Then
                    SetCampaignCompleted(campaign, CompaignCompleteReason.maxRespondersReached)
                Else
                    SetCampaignCompleted(campaign, CompaignCompleteReason.finished)
                End If
                App.TraceLog(TraceLevel.Info, "Campaign " & campaign & " completed.")
            End If
        Next
        ' removed this was producing campaign completion when campaign has not been completed
        For Each CampaignID As Integer In GetListOfCompletedCampaigns()
            SetCampaignCompleted(CStr(CampaignID), CompaignCompleteReason.finished)
        Next


    End Sub

    Public Function GetCurrentCalls(ByVal callStatus As DialoutCall.CallStatus) As Dictionary(Of String, DialoutCall)
        Return GetCurrentCalls(callStatus, 0)
    End Function
    Public Function GetCurrentCalls(ByVal callStatus As DialoutCall.CallStatus, ByVal forCampain As Integer) As Dictionary(Of String, DialoutCall)
        App.TraceLog(TraceLevel.Info, "Polling Database for new outbound calls...")
        Dim calls As New Dictionary(Of String, DialoutCall)
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during InProcessCalls(): " & ex.Message)
                Return calls
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            Dim criteria As New Text.StringBuilder
            Dim isNewCall As Boolean = False

            If (callStatus And DialoutCall.CallStatus.callKilled) = DialoutCall.CallStatus.callKilled Then
                If criteria.Length > 0 Then criteria.Append(" OR ")
                criteria.Append("(TRNPARM2<>'K' AND TRNDONE='K')")
            End If
            If (callStatus And DialoutCall.CallStatus.newcall) = DialoutCall.CallStatus.newcall Then
                If criteria.Length > 0 Then criteria.Append(" OR ")
                criteria.Append("(TRNDONE IS NULL OR TRNDONE='')")
                isNewCall = True
            End If
            If (callStatus And DialoutCall.CallStatus.preprocess) = DialoutCall.CallStatus.preprocess Then
                If criteria.Length > 0 Then criteria.Append(" OR ")
                criteria.Append("TRNDONE = 'Q'")
            End If
            If (callStatus And DialoutCall.CallStatus.inprocess) = DialoutCall.CallStatus.inprocess Then
                If criteria.Length > 0 Then criteria.Append(" OR ")
                criteria.Append("TRNDONE = 'P'")
            End If
            If (callStatus And DialoutCall.CallStatus.callComplete) = DialoutCall.CallStatus.callComplete Then
                If criteria.Length > 0 Then criteria.Append(" OR ")
                criteria.Append("TRNDONE = 'D'")
            End If
            If (DialoutCall.CallStatus.callFail And callStatus) = DialoutCall.CallStatus.callFail Then
                If criteria.Length > 0 Then criteria.Append(" OR ")
                criteria.Append("TRNDONE = 'F'")
            End If
            If (DialoutCall.CallStatus.inProcessRetryWebServiceDown And callStatus) = DialoutCall.CallStatus.inProcessRetryWebServiceDown Then
                If criteria.Length > 0 Then criteria.Append(" OR ")
                criteria.Append("TRNDONE = 'R'")
            End If
            cmd.CommandText = "Select INTTRAN.*, (select TRNDESC from INTTRAN as INTTRAN2 Where INTTRAN2.TRNREFNUM=INTTRAN.EVENT_ID) as EventDesc, (select TRNPARM2 from INTTRAN as INTTRAN3 Where INTTRAN3.TRNREFNUM=INTTRAN.EVENT_ID) as MaxResponders from INTTRAN WHERE ((" & mTransactions.SQLWhere & ")" & IIf(forCampain <> 0, " AND EVENT_ID=" & forCampain, "") & ") and (" & criteria.ToString & ")"
            Try
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If Not reader.HasRows Then Return calls
                    While reader.Read

                        If IsDBNull(reader!TRNTYP) Then
                            App.TraceLog(TraceLevel.Error, "TRNTYP is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".   The call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!Event_ID) Then
                            App.TraceLog(TraceLevel.Error, "EVENT_ID is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".  The call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!TRNPARM3) Then
                            App.TraceLog(TraceLevel.Error, "VXML Application URL is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".  Call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!TRNPARM1) Then
                            App.TraceLog(TraceLevel.Error, "Phone Number is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ". Call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!TRNPARM5) Then
                            retryCount = 0
                        Else
                            Dim tmp As String = reader!TRNPARM5 & ""
                            If tmp.ToUpper.Contains("INITIAL") Then
                                retryCount = 0
                            Else
                                If IsNumeric(Right(tmp, 1)) Then
                                    retryCount = CInt(Mid(tmp, InStrRev(tmp, " ") + 1))
                                Else
                                    retryCount = 0
                                End If
                            End If
                        End If
                        Dim eventId As Integer = 0
                        If Not IsDBNull(reader!Event_ID) Then
                            eventId = reader!event_id
                        End If
                        Dim d As New DialoutCall(reader!TRNREFNUM, eventId, reader!EventDesc & "", retryCount, reader!TRNPARM3, reader!TRNPARM1, mTransactions.ProirityFromTransactionType(reader!TRNTYP))
                        d.InitialDate = reader!TRNDAT & ""
                        calls.Add(CStr(reader!TRNREFNUM), d)
                        'If Not IsDBNull(reader!Maxresponders) Then
                        '    If mCampaignMaxResponders.ContainsKey(reader!EVENT_ID & "") Then
                        '        If mCampaignMaxResponders(reader!EVENT_ID & "").MaxResponders < CInt(reader!MaxResponders) Then
                        '            mCampaignMaxResponders.Remove(reader!EVENT_ID & "")
                        '            mCampaignMaxResponders.Add(reader!EVENT_ID & "", New Responder(reader!EventID & "", CInt(reader!MaxResponders)))
                        '        End If
                        '    Else
                        '        mCampaignMaxResponders.Add(reader!EVENT_ID & "", New Responder(reader!EventID & "", CInt(reader!MaxResponders)))
                        '    End If
                        'End If
                    End While
                End Using
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Error reading INTTRAN during GetCurrentCalls(): " & ex.Message)
            End Try
        End Using
        Return calls
    End Function
    Public Function IsCallStatus(ByVal c As DialoutCall, ByVal status As DialoutCall.CallStatus) As Boolean
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "IsCallStatus: " & ex.Message)
                Exit Function
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            Dim test As String = ""
            Select Case status
                Case DialoutCall.CallStatus.callKilled
                    test = "(TRNDONE = 'K')"
                Case DialoutCall.CallStatus.callFail
                    test = "(TRNDONE = 'F')"
                Case DialoutCall.CallStatus.inprocess
                    test = "(TRNDONE = 'P')"
                Case DialoutCall.CallStatus.preprocess
                    test = "(TRNDONE = 'Q')"
                Case DialoutCall.CallStatus.newcall
                    test = "(TRNDONE IS NULL OR TRNDONE='')"
                Case DialoutCall.CallStatus.callComplete
                    test = "(TRNDONE = 'D')"
            End Select
            cmd.CommandText = "Select Count(*) as InState from INTTRAN WHERE TRNREFNUM = " & c.DBRecordID & " and " & test
            Try
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If Not reader.HasRows Then Return False
                    reader.Read()
                    Return reader!InState > 0
                End Using
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "SQL Error during IsCallStatus(): " & ex.Message)
                Return False
            End Try
        End Using
    End Function

    Public Function IsCallStillOpen(ByVal c As DialoutCall) As Boolean
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "IsCallStillOpen: " & ex.Message)
                Exit Function
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Select Count(*) as StillOpen from INTTRAN WHERE TRNREFNUM = " & c.DBRecordID & " and TRNDONE='Q'"
            Try
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If Not reader.HasRows Then Return False
                    reader.Read()
                    Return reader!StillOpen > 0
                End Using
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "SQL Error during IsCallStillOpen(): " & ex.Message)
                Return False
            End Try
        End Using
    End Function
    Public Sub PollDatabase()
        App.TraceLog(TraceLevel.Info, "Polling Database for new outbound calls...")
        Dim calls As New List(Of DialoutCall)
        Me.CloseExpiredOutstandingPages()
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during PollDatabase(): " & ex.Message)
                Exit Sub
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Select INTTRAN.*, (select TRNDESC from INTTRAN as INTTRAN2 Where INTTRAN2.TRNREFNUM=INTTRAN.EVENT_ID) as EventDesc from INTTRAN WHERE (" & mTransactions.SQLWhere & ") and (TRNDONE IS NULL OR TRNDONE='')"
            Try
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If Not reader.HasRows Then
                        CheckforCompletedCampaigns()
                        Exit Sub
                    End If
                    While reader.Read
                        If IsDBNull(reader!TRNTYP) Then
                            App.TraceLog(TraceLevel.Error, "TRNTYP is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".   The call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!Event_ID) Then
                            App.TraceLog(TraceLevel.Error, "EVENTID is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ".  The call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!TRNPARM3) Then
                            App.TraceLog(TraceLevel.Error, "VXML Application URL is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ". Call will be ignored.  Check database.")
                            Continue While
                        End If
                        If IsDBNull(reader!TRNPARM1) Then
                            App.TraceLog(TraceLevel.Error, "Phone Number is NULL for for TRNREFNUM=" & reader!TRNREFNUM & ". Call will be ignored.  Check database")
                            Continue While
                        End If
                        If IsDBNull(reader!TRNPARM5) Then
                            retryCount = 0
                        Else
                            Dim tmp As String = reader!TRNPARM5
                            If tmp.ToUpper.Contains("INITIAL") Then
                                retryCount = 0
                            Else
                                If IsNumeric(Right(tmp, 1)) Then
                                    retryCount = CInt(Mid(tmp, InStrRev(tmp, " ") + 1))
                                Else
                                    retryCount = 0
                                End If
                            End If
                        End If
                        calls.Add(New DialoutCall(reader!TRNREFNUM, reader!EVENT_ID, reader!EventDesc & "", retryCount, reader!TRNPARM3, reader!TRNPARM1, mTransactions.ProirityFromTransactionType(reader!TRNTYP)))
                    End While
                End Using
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed query database during PollDatabase(): " & ex.Message)
            End Try
        End Using
        'process the calls!
        For Each c As DialoutCall In calls
            RaiseEvent NewOutBoundCall(Me, New NewOutboundCallEventArgs(c))
            'SetCallStatusInProcess(c)
        Next
        Me.CheckforCompletedCampaigns()

        'LK Moved to top of the routine
        'Me.CloseExpiredOutstandingPages()

    End Sub
    Public Function SetCallStatus(ByRef c As DialoutCall, ByVal status As DialoutCall.CallStatus) As Boolean
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to connect to database during SetCallStatusInProcess(): " & ex.Message)
                Exit Function
            End Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            Dim criteria As String = ""
            Dim updatedValue As String = ""
            Select Case status
                Case DialoutCall.CallStatus.callKillComplete
                    criteria = "TRNPARM2='K'"
                Case DialoutCall.CallStatus.callKilled
                    criteria = "TRNDONE='K'"
                Case DialoutCall.CallStatus.newcall
                    criteria = "TRNDONE=''"
                Case DialoutCall.CallStatus.preprocess
                    criteria = "TRNDONE = 'Q'"
                Case DialoutCall.CallStatus.inprocess
                    criteria = "TRNDONE = 'P'"
                Case DialoutCall.CallStatus.callComplete
                    criteria = "TRNDONE = 'D'"
                Case DialoutCall.CallStatus.callFail
                    criteria = "TRNDONE = 'E'"
                Case DialoutCall.CallStatus.inProcessRetryWebServiceDown
                    criteria = "TRNDONE = 'R'"
            End Select
            c.Status = status
            cmd.CommandText = "UPDATE INTTRAN SET TRNEBY='SDCISS',TRNDDAT=getDate(),TRNDBY='SDCISS', " & criteria & " WHERE TRNREFNUM = " & c.DBRecordID

            Try
                Dim result As Integer = cmd.ExecuteNonQuery()
                Return result
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to update database during SetCallStatus(): " & ex.Message)
                Return 0
            End Try
        End Using
    End Function
    Public Function updateEventLog(ByVal campaign As Integer, ByVal status As String, ByVal reason As String) As Boolean
        Dim EventLogID As Integer
        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "updateEventLog: " & ex.Message)
                Exit Function
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Select EVENT_ID from INTTRAN WHERE TRNREFNUM = " & campaign
            Try
                Using reader As SqlDataReader = cmd.ExecuteReader
                    If Not reader.HasRows Then Return False
                    reader.Read()
                    EventLogID = reader!Event_ID
                End Using
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "SQL Error during IsCallStillOpen(): " & ex.Message)
                Return False
            End Try
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "update EVENTLOG set status='" & status & "',Closed_Date=getDate(),closed_By='SDCISS',Closed_Data='" & reason & "' where ID=" & EventLogID

            Try
                Dim result As Integer = cmd.ExecuteNonQuery()
                App.TraceLog(TraceLevel.Info, "Updating EventLog record:" & EventLogID & "with Status:" & status & " Reason: " & reason)
                Return True
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to update database during SetCallStatus(): " & ex.Message)
                Return False
            End Try
        End Using
    End Function
    ' New killOutstandingPages 

    Public Function killOutstandingPages(ByVal campaignID As Integer) As Integer

        Using conn As New SqlConnection(Me.dbConnectString)
            Try
                conn.Open()
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "killOutstandingPages: " & ex.Message)
                Exit Function
            End Try
            Dim retryCount As Integer = 0
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Update inttran set TRNDONE='K'  where TRNTYP='PAGSRV' and (TRNPARM2<>'N' or TRNPARM2<>'A') and EVENT_ID='" & campaignID & "'"

            Try
                Dim result As Integer = cmd.ExecuteNonQuery()
                App.TraceLog(TraceLevel.Info, "Killed " & result & "outstanding pages do to campaign cancellation")
                Return result
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to update database during killOutstandingPages(): " & ex.Message)
                Return 0
            End Try
        End Using


    End Function


    Private ReadOnly Property dbConnectString() As String

        Get
            'Dim builder As New SqlConnectionStringBuilder
            'builder.DataSource = mServer
            'builder.InitialCatalog = mDb
            'If mUseIntegratedSecuity Then
            '    builder.IntegratedSecurity = True
            'Else
            'builder.IntegratedSecurity = False
            '    builder.UserID = mUser
            '    builder.Password = mPass
            'End If
            'Return builder.ConnectionString
            Return mConnectionString
        End Get
    End Property
End Class
