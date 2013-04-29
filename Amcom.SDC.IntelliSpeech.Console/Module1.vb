Imports System.Threading
Imports Amcom.SDC.Speech.ReportBuilder
Imports Amcom.SDC.BaseServices.SDCThreading
Imports Amcom.SDC.IntelliSpeech.Library
Imports Amcom.SDC.BaseServices


Module CSModule
    Private mServiceManager As ServiceManager = Nothing
    Private mENSOutdialerEngine As ENSOutdialerEngine = Nothing
    Private mReportController As SpeechReportController = Nothing
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


        StartReportThread()
        'StartOutDialerThread()
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
