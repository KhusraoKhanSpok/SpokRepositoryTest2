Imports System.Threading
Imports Amcom.SDC.Speech.ReportBuilder
Imports Amcom.SDC.BaseServices.SDCThreading
Imports Amcom.SDC.IntelliSpeech.Library
Imports Amcom.SDC.BaseServices
Imports Amcom.SDC.ServiceMonitorConnector


Module CSModule
    Private mServiceManager As ServiceManager = Nothing
    Private mENSOutdialerEngine As ENSOutdialerEngine = Nothing
    Private mReportController As SpeechReportController = Nothing
    Private mServiceMonitorClient As ServiceMonitorClient
    Private mStartTime As Date = Nothing
    Private mReportRunning As Boolean = False
    Private mReportThread As SafeThread = Nothing
    Private mOutdialerThread As SafeThread = Nothing
    Private mServiceManagerThread As SafeThread = Nothing
#Region "Entry Point"
    Sub Main()

        ShowConsoleBanner()
        OnStart()
        System.Console.ReadKey()
        OnStop()
    End Sub
#End Region
#Region "Service Methods"
    Sub OnStart()

        mStartTime = Now
        App.Initialize(ApplicationType.Console, My.Settings)
        App.TraceLog(TraceLevel.Info, "Starting Service")
        App.TraceLog(TraceLevel.Info, My.Application.Info.ProductName & " " & My.Application.Info.Version.ToString)

        Thread.CurrentThread.Name = "IntelliSpeech Service"

        'StartServiceMonitor()
        StartReportThread()
        StartOutDialerThread()
        StartServiceManager()
    End Sub
    Private Sub StartServiceManager()
        App.TraceLog(TraceLevel.Info, "Starting Service Manager Thread")

        mServiceManagerThread = New SafeThread(New ThreadStart(AddressOf StartController))
        AddHandler mServiceManagerThread.ThreadException, AddressOf OnThreadException
        mServiceManagerThread.Tag = New ThreadExceptionDetail
        mServiceManagerThread.IsBackground = True
        mServiceManagerThread.Name = "Service Manager"
        mServiceManagerThread.Start()
    End Sub
    Private Sub StartController()
        mServiceManager = New ServiceManager
        AddHandler mServiceManager.AbortService, AddressOf OnAbortService
        mServiceManager.Start(True)
    End Sub
    Private Sub OnAbortService(ByVal sender As Object, ByVal e As System.EventArgs)
        App.TraceLog(TraceLevel.Error, "Shutting down service due to excessive Thread Exceptions")
        OnStop()
    End Sub
    Private Sub OnStop()
        App.TraceLog(TraceLevel.Info, "Service has requested STOP")
        mServiceManager.Stop()
        If mENSOutdialerEngine IsNot Nothing Then mENSOutdialerEngine.Stop()
        If mOutdialerThread IsNot Nothing Then mOutdialerThread.Abort()
        StopReportThread()
        'mServiceMonitorClient.Stop()
        App.TraceLog(TraceLevel.Info, "Service has completed STOP")
    End Sub
    Private Sub StartOutDialerThread()

        App.TraceLog(TraceLevel.Info, "Starting Outdialer Thread")
        mOutdialerThread = New SafeThread(New ThreadStart(AddressOf OutDialerThread))
        AddHandler mOutdialerThread.ThreadException, AddressOf OnThreadException
        mOutdialerThread.IsBackground = True
        mOutdialerThread.Name = "Outdialer"
        mOutdialerThread.Tag = New ThreadExceptionDetail
        mOutdialerThread.Start()

    End Sub
    Private Sub StartReportThread()
        App.TraceLog(TraceLevel.Info, "Starting Reporting Thread")
        mReportThread = New SafeThread(New ThreadStart(AddressOf ReportThread))
        AddHandler mReportThread.ThreadException, AddressOf OnThreadException
        mReportThread.IsBackground = True
        mReportThread.Tag = New ThreadExceptionDetail
        mReportThread.Name = "SpeechReporting"
        mReportThread.Start()
    End Sub

    Private Sub OnThreadException(ByVal t As SafeThread, ByVal ex As System.Exception)
        App.TraceLog(TraceLevel.Error, "An exception has occurred in thread: " & t.Name)

        With DirectCast(t.Tag, ThreadExceptionDetail)
            .Add(ex)
            If .Count > 3 And .TimeDifference(Now) < 5 Then
                App.TraceLog(TraceLevel.Error, "Aborting Service -- Thread " & t.Name & " EXCEPTIONing more than once in a 5-second period.  Check logs.")
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
                StartServiceManager()
        End Select
    End Sub
    Private Sub StartServiceMonitor()
        App.TraceLog(TraceLevel.Info, "Initializing Service Monitor connection...")
        Dim primaryMonitor As String = App.Config.GetString(ConfigSection.configuration, "serviceMonitor/primaryServer")
        Dim secondaryMonitor As String = App.Config.GetString(ConfigSection.configuration, "serviceMonitor/secondaryServer")
        Dim serverPort As Integer = App.Config.GetInteger(ConfigSection.configuration, "serviceMonitor/port")
        If primaryMonitor.Length = 0 Then
            App.TraceLog(TraceLevel.Info, "Service Monitor connection disabled, no primaryServer found in configuration.")
            Exit Sub
        End If
        If serverPort = 0 Then
            App.TraceLog(TraceLevel.Info, "Service Monitor connection disabled, no primaryServer port number found in configuration.")
            Exit Sub
        End If
        mServiceMonitorClient = New Connector("SDCSolutions.IntelliSpeech.Service", primaryMonitor, serverPort)

        mServiceMonitorClient.AddProperty("Command1", "Enable Outdialer")
        mServiceMonitorClient.AddExtendedProperty("Command1", "Description", "Starts the ENS Outdialer subsystem.   Database will be watched for outgoing ENS transactions.")
        mServiceMonitorClient.AddProperty("Command2", "Disable Outdialer")
        mServiceMonitorClient.AddExtendedProperty("Command2", "Description", "Stops the ENS Outdialer subsystem.   Existing calls in queue will complete.")
        mServiceMonitorClient.AddProperty("Command3", "Enable Grammar Compiler")
        mServiceMonitorClient.AddExtendedProperty("Command3", "Description", "Starts the Speech Grammar Compiler subsystem.")
        mServiceMonitorClient.AddProperty("Command4", "Disable Grammar Compiler")
        mServiceMonitorClient.AddExtendedProperty("Command4", "Description", "Stops the Speech Grammar Compiler subsystem.")
        mServiceMonitorClient.AddProperty("Command5", "Enable Report Generator")
        mServiceMonitorClient.AddExtendedProperty("Command5", "Description", "Starts the Speech Report Generator subsystem.")
        mServiceMonitorClient.AddProperty("Command6", "Disable Report Generator")
        mServiceMonitorClient.AddExtendedProperty("Command6", "Description", "Stops the Speech Report Generator subsystem.")

        mServiceMonitorClient.AddProperty("Service Uptime", App.UpTime())
        '   mServiceMonitorClient.AddProperty("Grammar Compiler Configuration Path", mServiceManager.GetConfigPath)
        mServiceMonitorClient.AddExtendedProperty("Service Uptime", "IsDynamic", "True")
        AddHandler mServiceMonitorClient.CommandReceived, AddressOf OnSMCommand
        AddHandler mServiceMonitorClient.PropertyChanged, AddressOf OnPropertyChanged
        AddHandler mServiceMonitorClient.PropertyValueRequest, AddressOf OnPropertyValueRequest
        mServiceMonitorClient.Start()
    End Sub

    Private Sub OnPropertyValueRequest(ByVal sender As Object, ByVal e As ServiceMonitorClientPropertyRequestArgs)
        Select Case e.PropertyID.ToLower
            Case "service uptime"
                mServiceMonitorClient.UpdateProperty(e.PropertyID, App.UpTime)

        End Select
    End Sub
    Private Sub OnSMCommand(ByVal sender As Object, ByVal e As ServiceMonitorClientCommandArgs)
        Select Case e.CommandName
            Case "Enable OutDialer"
                If mENSOutdialerEngine.Running Then Exit Sub
                mENSOutdialerEngine.Start()
            Case "Disable OutDialer"
                mENSOutdialerEngine.Stop()
            Case "Enable Grammar Compiler"
                If mServiceManager.Running Then Exit Sub
                mServiceManager.Start(False)
            Case "Disable Grammar Compiler"
                mServiceManager.Stop()
            Case "Enable Report Generator"
                If mReportRunning Then Exit Sub
                'StartReportController()
            Case "Disable Report Generator"
                StopReportThread()
        End Select
    End Sub
    Private Sub OnPropertyChanged(ByVal sender As Object, ByVal e As ServiceMonitorClientPropertyArgs)
        Select Case e.PropertyID
            Case "Grammar Compiler Configuration Path"
                mServiceManager.SetConfigPathAndFile(System.IO.Path.GetDirectoryName(e.Value), System.IO.Path.GetFileName(e.Value))

        End Select
    End Sub
    Private Sub StopReportThread()
        mReportRunning = False
        If mReportThread IsNot Nothing Then
            mReportThread.Abort()
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
                Exit Sub
            End Try
            If Not mReportRunning Then
                mReportController.stop()
                Exit Sub
            End If
        Loop While mReportRunning
        mReportController.stop()
    End Sub
    Private Sub OutDialerThread()
        mENSOutdialerEngine = New ENSOutdialerEngine
        AddHandler mENSOutdialerEngine.AbortService, AddressOf OnAbortService
        mENSOutdialerEngine.Start()
    End Sub


    Private Sub ShowConsoleBanner()
        System.Console.WriteLine("SDC IntelliSpeech Service Console")
        System.Console.WriteLine("=================================")
        System.Console.WriteLine("Version " & App.Version)
        System.Console.WriteLine(vbCrLf)
    End Sub
#End Region
End Module
