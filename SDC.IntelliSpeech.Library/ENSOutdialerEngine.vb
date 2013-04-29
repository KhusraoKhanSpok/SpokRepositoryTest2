Imports System.Xml
Imports System.Threading
Imports Newtonsoft.Json
Imports Amcom.SDC.BaseServices.SDCThreading
Imports Amcom.SDC.BaseServices
Public Class ENSOutdialerEngine
    'Emergency Notification Outdialer Engine
    Public mLBLock As Object
    Private mOutDialer As OutboundDialerService.OutboundDialerService
    Private mTransactions As ENSTransactions
    Private _SODSQuery As SODSData
    Private mStop As Boolean = False
    Private mRunning As Boolean = False
    Private mDefaultSpeechURI As String = ""
    Private mLastCheck As Object = Nothing
    Private mLoadBalancer As CallLoadBalancer
    Private mCheckCallTimer As System.Timers.Timer
    Private mODBusy As Boolean = False
    Private _CallProcessorThread As SafeThread = Nothing
    Public Event AbortService As EventHandler
    Public Sub New()
        App.TraceLog(TraceLevel.Info, "Reading configuration...")


        Dim wsURL As String = App.Config.GetString(ConfigSection.modules, "sdcEmergencyNotificationSystem/webServiceURL")
        'Dim dbServer As String = App.ConnectionString("speech")
        'Dim dbServerPort As Integer = App.Config.GetInteger(ConfigSection.configuration, "databaseConnections/database[@application='speech']/serverPort")
        'If dbServerPort <> 0 Then
        '    dbServer = dbServer & ":" & dbServerPort.ToString
        'End If
        'Dim dbName As String = App.Config.GetString(ConfigSection.configuration, "databaseConnections/database[@application='e']/dbName")
        'Dim dbWinSecurity As Boolean = App.Config.GetBoolean(ConfigSection.configuration, "databaseConnections/database[@application='enterprise']/integratedSecurity")
        'Dim dbUserName As String = App.Config.GetString(ConfigSection.configuration, "databaseConnections/database[@application='enterprise']/userName")
        'Dim dbPassword As String = App.Config.GetString(ConfigSection.configuration, "databaseConnections/database[@application='enterprise']/password")

        mOutDialer = New OutboundDialerService.OutboundDialerService
        mOutDialer.Timeout = 1000
        mOutDialer.Url = wsURL

        CheckForWebServiceAvailability()
        mLBLock = New Object
        mTransactions = New ENSTransactions
        For Each tranType As XmlNode In App.Config.GetNodeList(ConfigSection.modules, "sdcEmergencyNotificationSystem/sdcOutboundServiceTypes/service")
            mTransactions.Add(New ENSTransaction(tranType.SelectSingleNode("dbTransactionType").InnerText, CInt(tranType.SelectSingleNode("priority").InnerText)))
        Next
        App.TraceLog(TraceLevel.Info, "Creating database monitor thread...")
        _SODSQuery = New SODSData(App.ConnectionString("desk"), mTransactions)
        AddHandler _SODSQuery.NewOutBoundCall, AddressOf OnNewOutboundCall
        mLoadBalancer = New CallLoadBalancer(_SODSQuery, _SODSQuery.RetryCount)

        'mCheckCallTimer = New System.Timers.Timer
        'AddHandler mCheckCallTimer.Elapsed, AddressOf ProcessOutboundCalls
        'mCheckCallTimer.Interval = 100
        'mCheckCallTimer.Enabled = True
        StartProcessOutboundCallsThread()
    End Sub

    Private Sub StartProcessOutboundCallsThread()
        _CallProcessorThread = New SafeThread(New ThreadStart(AddressOf ProcessOutboundCalls))
        AddHandler _CallProcessorThread.ThreadException, AddressOf OnProcessOutboundCallsThreadException
        _CallProcessorThread.Tag = New ThreadExceptionDetail
        _CallProcessorThread.IsBackground = True
        _CallProcessorThread.Name = "ProcessOutboundCalls"
        _CallProcessorThread.ShouldReportThreadAbort = True
        _CallProcessorThread.Start()
    End Sub
    Private Sub OnProcessOutboundCallsThreadException(ByVal t As SafeThread, ByVal ex As System.Exception)
        If TypeOf ex Is ThreadAbortException Then
            App.TraceLog(TraceLevel.Info, "ProcessOutboundCalls thread is exiting")
        Else
            With DirectCast(_CallProcessorThread.Tag, ThreadExceptionDetail)
                .Add(ex)
                If .Count > 3 And .TimeDifference(Now) < 5 Then
                    App.TraceLog(TraceLevel.Error, "ProcessOutboundCalls EXCESSIVE EXCEPTIONS - terminating service.")
                    RaiseEvent AbortService(Me, New System.EventArgs)
                Else
                    'restart the thread
                    StartProcessOutboundCallsThread()
                End If
            End With
        End If
    End Sub
    Private Sub CheckCallStatus()
        Dim outCallList As String = ""
        App.TraceLog(TraceLevel.Info, "Checking call status...")
        If mDefaultSpeechURI.Length = 0 Then
            CheckForWebServiceAvailability()
            If mDefaultSpeechURI.Length = 0 Then
                App.TraceLog(TraceLevel.Error, "Can not check call Status - Web Service Unavailable!")
                Exit Sub
            End If
        End If
        Try
            outCallList = mOutDialer.getOutboundCallList
        Catch ex As Exception
            App.TraceLog(TraceLevel.Error, "Error calling Outdialing Webservice: " & ex.Message)
            Exit Sub
        End Try
        Dim retries As Integer = 0
        Dim sc As New SpeechCalls(outCallList)
        'kill any killed calls
        Dim killcalls As Dictionary(Of String, DialoutCall) = _SODSQuery.GetCurrentCalls(DialoutCall.CallStatus.callKilled)
        For Each kc As DialoutCall In killcalls.Values
            Try
                outCallList = mOutDialer.cancelOutboundCallByForeignKey(kc.DBRecordID)
            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "Error killing call " & kc.PhoneNumber)
            Finally
                App.TraceLog(TraceLevel.Verbose, "Killed call to " & kc.PhoneNumber)
                _SODSQuery.SetCallStatus(kc, DialoutCall.CallStatus.callKillComplete)
            End Try
        Next
        If killcalls.Count Then
            'refresh from webservice
            Try
                outCallList = mOutDialer.getOutboundCallList
            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "Error calling Outdialing Webservice: " & ex.Message)
                Exit Sub
            End Try
            retries = 0
            sc = New SpeechCalls(outCallList)
        End If
        For Each l As Dictionary(Of String, Parameter) In sc.RetryCalls.Values
            'remove the call from the queue
            If mDefaultSpeechURI.Length = 0 Then
                CheckForWebServiceAvailability()
                If mDefaultSpeechURI.Length = 0 Then
                    App.TraceLog(TraceLevel.Error, "Can not remove call from outdialer -- Web Service Unavailable!")
                    Continue For
                End If
            End If
            Try
                mOutDialer.cancelOutboundCallByForeignKey(l("foreignkey").Value)
                App.TraceLog(TraceLevel.Verbose, "Call to " & l("foreignKey").Value & " removed from outdial queue")
            Catch ex As Exception
                '  App.TraceLog(TraceLevel.Error, "Failed to cancel retry call to " & d.PhoneNumber)
            End Try
            Dim d As DialoutCall = _SODSQuery.GetCall(l("foreignkey").Value)
            If d Is Nothing Then Continue For
            Select Case d.Status
                Case DialoutCall.CallStatus.inprocess
                    If retries = 0 Then
                        retries = _SODSQuery.RetryCount
                    End If
                    If d.RetryCount + 1 > retries Then
                        _SODSQuery.FailCall(d)
                        Continue For
                    End If
                    SyncLock mLBLock
                        _SODSQuery.UpdateRetryCount(d)
                        mLoadBalancer.Add(d)
                    End SyncLock
                Case DialoutCall.CallStatus.newcall
                    mLoadBalancer.Add(d)
                Case Else
                    Continue For
            End Select
        Next

        Dim calls As Dictionary(Of String, DialoutCall) = _SODSQuery.GetCurrentCalls(DialoutCall.CallStatus.inprocess Or DialoutCall.CallStatus.inProcessRetryWebServiceDown)
        If calls.Count > 0 Then
            App.TraceLog(TraceLevel.Verbose, "Found " & calls.Count & " in-process calls")
            For Each d As DialoutCall In calls.Values
                If Not sc.DoesCallExist(CStr(d.DBRecordID)) Then
                    If retries = 0 Then
                        retries = _SODSQuery.RetryCount
                    End If
                    If d.RetryCount + 1 > retries Then
                        d.Status = DialoutCall.CallStatus.callFail
                        _SODSQuery.FailCall(d)
                        Continue For
                    End If
                    'retry call
                    If mDefaultSpeechURI.Length = 0 Then
                        CheckForWebServiceAvailability()
                        If mDefaultSpeechURI.Length = 0 Then
                            App.TraceLog(TraceLevel.Error, "Can not make retry outbound call -- Web Service Unavailable!")
                            Exit Sub
                        End If
                    End If
                    SyncLock mLBLock
                        d.RetryCount += 1
                        d.Status = DialoutCall.CallStatus.inprocessRetry
                        mLoadBalancer.Add(d)
                    End SyncLock
                End If
            Next
        End If

        'calls = mDataThread.GetCurrentCalls(DialoutCall.CallStatus.newcall)
        'If calls.Count = 0 Then Exit Sub
        'App.TraceLog(TraceLevel.Verbose, "Found " & calls.Count & " new calls")
        'For Each d As DialoutCall In calls.Values
        '    SyncLock mLBLock
        '        If d.RetryCount > 0 Then
        '            mDataThread.SetCallStatus(d, DialoutCall.CallStatus.callFail)
        '        Else
        '            mDataThread.SetCallStatus(d, DialoutCall.CallStatus.preprocess)
        '            mDataThread.UpdateRetryHistory(d)
        '            mLoadBalancer.Add(d)
        '        End If
        '    End SyncLock
        'Next
    End Sub
    Private Sub CheckForWebServiceAvailability()
        App.TraceLog(TraceLevel.Info, "Initializing Web Service...")
        Try
            mDefaultSpeechURI = mOutDialer.getDefaultBrowserURI
            App.TraceLog(TraceLevel.Info, "Web Service Initialized.")
        Catch ex As Net.WebException
            App.TraceLog(TraceLevel.Info, "Failed to retrieve Default Browser URI: " & ex.Message)
        End Try

    End Sub
    Public ReadOnly Property Running()
        Get
            Return mRunning
        End Get
    End Property
    Public Sub Start()
        mRunning = True
        _SODSQuery.Start()
        StartwaitThread()
    End Sub
    Private Sub StartwaitThread()
        Dim t As New Thread(AddressOf WaitForExit)
        t.IsBackground = True
        t.Start()
    End Sub
    Private Sub WaitForExit()

        Do Until mStop = True
            Try
                If mLastCheck Is Nothing Then
                    mLastCheck = Now
                    CheckCallStatus()
                Else
                    If Math.Abs(DateDiff(DateInterval.Second, mLastCheck, Now)) >= 30 Then
                        CheckCallStatus()
                        mLastCheck = Now
                    End If
                End If
                Thread.Sleep(100)
            Catch ex As ThreadInterruptedException
                App.TraceLog(TraceLevel.Info, "ENSOutdialerEngine Thread Stopped")
                mStop = True
                mRunning = False
                Exit Do

            Catch ex As ThreadAbortException
                App.TraceLog(TraceLevel.Info, "ENSOutdialerEngine Thread Stopped")
                mStop = True
                mRunning = False
                Exit Do
            End Try
        Loop
        mRunning = False
    End Sub

    Public Sub [Stop]()
        App.TraceLog(TraceLevel.Info, "Stopping ENSOutdialerEngine")
        If _CallProcessorThread IsNot Nothing Then _CallProcessorThread.Abort()
        _SODSQuery.Stop()
        mStop = True
    End Sub
    Private Sub OnNewOutboundCall(ByVal sender As Object, ByVal e As NewOutboundCallEventArgs)
        SyncLock mLBLock
            _SODSQuery.SetCallStatus(e.Call, DialoutCall.CallStatus.preprocess)
            mLoadBalancer.Add(e.Call)
        End SyncLock

    End Sub
    Private Sub ProcessOutboundCalls()
        App.TraceLog(TraceLevel.Info, "ProcessOutboundCalls Thread running")
        Do
            If mStop Then Exit Do
            Try
                Thread.Sleep(100)
                If mODBusy Then
                    App.TraceLog(TraceLevel.Info, "ProcessOutboundCalls Busy")
                    Continue Do
                End If
                mODBusy = True
                Dim nextCall As DialoutCall = Nothing
                SyncLock mLBLock
                    nextCall = mLoadBalancer.NextCall
                End SyncLock
                If nextCall Is Nothing Then
                    mODBusy = False
                    Continue Do
                End If
                If mDefaultSpeechURI.Length = 0 Then
                    CheckForWebServiceAvailability()
                    If mDefaultSpeechURI.Length = 0 Then
                        App.TraceLog(TraceLevel.Error, "Can not make outbound call -- Web Service Unavailable!")
                        _SODSQuery.SetCallStatus(nextCall, DialoutCall.CallStatus.inProcessRetryWebServiceDown)
                        mODBusy = False
                        Continue Do
                    End If
                End If
                Dim defretry As Integer = mOutDialer.getDefaultRetryIntervalMinutes
                'FIX: Switch AddOutboundCallWithRetry till after set in process, and check for
                'result from SetCallStatus.  Prevents two or more outbound dialers from getting the
                'same call.
                If _SODSQuery.SetCallStatus(nextCall, DialoutCall.CallStatus.inprocess) = 0 Then
                    'if we cannot set status to inprocess, it means another outdialer has the call.
                    mODBusy = False
                    Exit Sub
                End If
                Try
                    mOutDialer.addOutboundCallWithForeignKey(mDefaultSpeechURI, nextCall.ApplicationURL, nextCall.PhoneNumber, defretry, nextCall.Priority, nextCall.DBRecordID.ToString)
                Catch ex As Exception
                    'Set call back to preprocess if outdialer failed.
                    _SODSQuery.SetCallStatus(nextCall, DialoutCall.CallStatus.preprocess)
                End Try
                Try
                    _SODSQuery.UpdateRetryCount(nextCall)
                    If nextCall.RetryCount > 0 Then
                        _SODSQuery.UpdateRetryHistory(nextCall)
                    End If
                    App.TraceLog(TraceLevel.Info, "Outdialing Call: " & nextCall.DBRecordID)
                Catch ex As Exception
                    App.TraceLog(TraceLevel.Error, "Cannot Make Outbound Call! Error: " & ex.Message)
                    _SODSQuery.SetCallStatus(nextCall, DialoutCall.CallStatus.inProcessRetryWebServiceDown)
                End Try
            Catch ex As ThreadAbortException
                App.TraceLog(TraceLevel.Info, "ProcessOutboundCalls thread is exiting")
                Exit Do
            End Try
            mODBusy = False
        Loop
    End Sub
End Class
