Imports System.ServiceProcess
Imports System.Threading
Imports Amcom.SDC.Speech.ReportBuilder
Imports Amcom.SDC.BaseServices.SDCThreading
Imports Amcom.SDC.BaseServices
Imports Amcom.SDC.IntelliSpeech.Library

Public Class IntelliSpeechService

    Private mServiceManager As ServiceManager = Nothing
    Private mENSOutdialerEngine As ENSOutdialerEngine = Nothing
    Private mReportController As SpeechReportController = Nothing
    Private mStartTime As Date = Nothing
    Private mReportRunning As Boolean = False
    Private mReportThread As SafeThread = Nothing
    Private mOutdialerThread As SafeThread = Nothing
    Private mServiceManagerThread As SafeThread = Nothing
    Protected Overrides Sub OnStart(ByVal args() As String)

        mStartTime = Now
        App.Initialize(ApplicationType.WindowsService, My.Settings)
        App.TraceLog(TraceLevel.Info, "Starting Service")
        Thread.CurrentThread.Name = "IntelliSpeech Service"

        'StartServiceMonitor()
        StartReportThread()
        StartOutDialerThread()
        startServiceManager()

    End Sub
    Private Sub startServiceManager()
        mServiceManagerThread = New SafeThread(New ThreadStart(AddressOf StartController))
        AddHandler mServiceManagerThread.ThreadException, AddressOf OnThreadException
        mServiceManagerThread.IsBackground = True
        mServiceManagerThread.Name = "Service Manager"
        mServiceManagerThread.Start()
    End Sub
    Private Sub StartController()
        mServiceManager = New ServiceManager
        mServiceManager.Start(True)
    End Sub
    'Protected Overrides Sub OnStop()
    '    App.TraceLog(TraceLevel.Info, "Service has requested STOP")
    '    mServiceManager.Stop()
    '    mENSOutdialerEngine.Stop()
    '    mServiceManagerThread.Abort()

    '    If Not mOutdialerThread IsNot Nothing Then mOutdialerThread.Abort()
    '    StopReportThread()
    '    If mOutdialerThread IsNot Nothing Then mOutdialerThread.Abort()
    '    mReportThread.Abort()
    '    mServiceMonitorClient.Stop()
    '    App.TraceLog(TraceLevel.Info, "Service has completed STOP")
    'End Sub
    Protected Overrides Sub OnStop()
        App.TraceLog(TraceLevel.Info, "Service has requested STOP")
        mServiceManager.Stop()
        If mENSOutdialerEngine IsNot Nothing Then mENSOutdialerEngine.Stop()
        If mOutdialerThread IsNot Nothing Then mOutdialerThread.Abort()
        StopReportThread()
        'mServiceMonitorClient.Stop()
        App.TraceLog(TraceLevel.Info, "Service has completed STOP")
    End Sub
    Private Sub StartOutDialerThread()
        Dim enabled As Boolean = App.Config.GetBoolean(ConfigSection.modules, "sdcSpeechOutdialer/enabled")
        If Not enabled Then
            App.TraceLog(TraceLevel.Info, "Speech Outdialer has been disabled by configuration.")
            Exit Sub
        End If
        mOutdialerThread = New SafeThread(New ThreadStart(AddressOf OutDialerThread))
        AddHandler mOutdialerThread.ThreadException, AddressOf OnThreadException
        mOutdialerThread.IsBackground = True
        mOutdialerThread.Name = "Outdialer"
        mOutdialerThread.Start()

    End Sub
    Private Sub StartReportThread()
        mReportThread = New SafeThread(New ThreadStart(AddressOf ReportThread))
        AddHandler mReportThread.ThreadException, AddressOf OnThreadException
        mReportThread.IsBackground = True
        mReportThread.Name = "SpeechReporting"
        mReportThread.Start()
    End Sub
    Private Sub OnThreadException(ByVal t As SafeThread, ByVal ex As System.Exception)
        App.TraceLog(TraceLevel.Error, "An exception has occurred in thread: " & t.Name)

        With DirectCast(t.Tag, ThreadExceptionDetail)
            .Add(ex)
            If .Count > 3 And .TimeDifference(Now) < 5 Then
                App.TraceLog(TraceLevel.Error, "Aborting Service -- Thread " & t.Name & " EXCEPTIONing more than three times in a 5-second period.  Check logs.")
                OnStop()
                Exit Sub
            End If
        End With

        App.TraceLog(TraceLevel.Error, "Attempting to restart thread " & t.Name & "...")
        Select Case t.Name.ToString.ToLower
            Case "speechreporting"
                mReportThread.Abort()
                StartReportThread()
            Case "outdialer"
                mOutdialerThread.Abort()
                StartOutDialerThread()
            Case "service manager"
                mServiceManagerThread.Abort()
                startServiceManager()
        End Select
    End Sub
    'Private Sub StartServiceMonitor()
    '    App.TraceLog(TraceLevel.Info, "Initializing Service Monitor connection...")
    '    Try
    '        Dim primaryMonitor As String = App.Config.GetString(ConfigSection.configuration, "serviceMonitor/primaryServer")
    '        Dim secondaryMonitor As String = App.Config.GetString(ConfigSection.configuration, "serviceMonitor/secondaryServer")
    '        Dim serverPort As Integer = App.Config.GetInteger(ConfigSection.configuration, "serviceMonitor/port")
    '        If primaryMonitor.Length = 0 Then
    '            App.TraceLog(TraceLevel.Info, "Service Monitor connection disabled, no primaryServer found in configuration.")
    '            Exit Sub
    '        End If
    '        If serverPort = 0 Then
    '            App.TraceLog(TraceLevel.Info, "Service Monitor connection disabled, no primaryServer port number found in configuration.")
    '            Exit Sub
    '        End If
    '        mServiceMonitorClient = New Connector("SDCSolutions.IntelliSpeech.Service", primaryMonitor, serverPort)

    '        mServiceMonitorClient.AddProperty("Command1", "Enable Outdialer")
    '        mServiceMonitorClient.AddExtendedProperty("Command1", "Description", "Starts the ENS Outdialer subsystem.   Database will be watched for outgoing ENS transactions.")
    '        mServiceMonitorClient.AddProperty("Command2", "Disable Outdialer")
    '        mServiceMonitorClient.AddExtendedProperty("Command2", "Description", "Stops the ENS Outdialer subsystem.   Existing calls in queue will complete.")
    '        mServiceMonitorClient.AddProperty("Command3", "Enable Grammar Compiler")
    '        mServiceMonitorClient.AddExtendedProperty("Command3", "Description", "Starts the Speech Grammar Compiler subsystem.")
    '        mServiceMonitorClient.AddProperty("Command4", "Disable Grammar Compiler")
    '        mServiceMonitorClient.AddExtendedProperty("Command4", "Description", "Stops the Speech Grammar Compiler subsystem.")
    '        mServiceMonitorClient.AddProperty("Command5", "Enable Report Generator")
    '        mServiceMonitorClient.AddExtendedProperty("Command5", "Description", "Starts the Speech Report Generator subsystem.")
    '        mServiceMonitorClient.AddProperty("Command6", "Disable Report Generator")
    '        mServiceMonitorClient.AddExtendedProperty("Command6", "Description", "Stops the Speech Report Generator subsystem.")

    '        mServiceMonitorClient.AddProperty("Service Uptime", App.UpTime())
    '        '   mServiceMonitorClient.AddProperty("Grammar Compiler Configuration Path", mServiceManager.GetConfigPath)
    '        mServiceMonitorClient.AddExtendedProperty("Service Uptime", "IsDynamic", "True")
    '        AddHandler mServiceMonitorClient.CommandReceived, AddressOf OnSMCommand
    '        AddHandler mServiceMonitorClient.PropertyChanged, AddressOf OnPropertyChanged
    '        AddHandler mServiceMonitorClient.PropertyValueRequest, AddressOf OnPropertyValueRequest
    '    Catch ex As Exception
    '        App.TraceLog(TraceLevel.Error, "Did not start Service Monitor due to error")
    '    End Try
    '    mServiceMonitorClient.Start()
    '    App.TraceLog(TraceLevel.Verbose, "Service Monitor thread started.")
    'End Sub

    'Private Sub OnPropertyValueRequest(ByVal sender As Object, ByVal e As ServiceMonitorClientPropertyRequestArgs)
    '    Select Case e.PropertyID
    '        Case "service uptime"
    '            mServiceMonitorClient.UpdateProperty(e.PropertyID, App.UpTime)

    '    End Select
    'End Sub
    'Private Sub OnSMCommand(ByVal sender As Object, ByVal e As ServiceMonitorClientCommandArgs)
    '    Select Case e.CommandName
    '        Case "Enable OutDialer"
    '            If mENSOutdialerEngine.Running Then Exit Sub
    '            mENSOutdialerEngine.Start()
    '        Case "Disable OutDialer"
    '            mENSOutdialerEngine.Stop()
    '        Case "Enable Grammar Compiler"
    '            If mServiceManager.Running Then Exit Sub
    '            mServiceManager.Start(False)
    '        Case "Disable Grammar Compiler"
    '            mServiceManager.Stop()
    '        Case "Enable Report Generator"
    '            If mReportRunning Then Exit Sub
    '            'StartReportController()
    '        Case "Disable Report Generator"
    '            StopReportThread()
    '    End Select
    'End Sub
    Private Sub StopReportThread()

        If mReportThread IsNot Nothing Then
            mReportThread.Abort()
            mReportRunning = False
        End If

    End Sub
    Private Sub ReportThread()
        mReportController = New SpeechReportController
        mReportRunning = True
        Do
            Try
                App.Config.CheckForChanges()
                If App.Config.GetBoolean(ConfigSection.modules, "sdcSpeechReporting/enabled") Then
                    mReportController.MakeWeeklyReports()
                Else
                    App.TraceLog(TraceLevel.Verbose, "Speech Reporting is currently disabled")
                End If
            Catch ex As ConfigException
                App.TraceLog(TraceLevel.Error, ex.Message)
            End Try
            Try
                Thread.Sleep(60000)
            Catch ex As ThreadAbortException
                mReportRunning = False
                Exit Do

            End Try
            If Not mReportRunning Then Exit Do
        Loop
    End Sub
    Private Sub OutDialerThread()
        mENSOutdialerEngine = New ENSOutdialerEngine
        mENSOutdialerEngine.Start()
    End Sub


End Class
