Imports Microsoft.Win32
Imports System.xml
Imports System.Data.SqlClient
Imports System.Threading
Imports Amcom.SDC.BaseServices.SDCThreading
Imports Amcom.SDC.BaseServices
Public Class ServiceManager
    Private mConfig As String
    Private mStarted As Boolean
    Private mSDCConfig As String
    Private WithEvents mSDCConfigWatcher As FileWatcher
    Private WithEvents mConfigWatcher As FileWatcher
    Private mConnectionString As String
    Private mQuerySql As String
    Private mUpdateSql As String
    Private mIsUpdatedSql As String
    Private mLastSDCConfigUpdate As Object
    Private mLastConfigUpdate As Object
    Private mDBQuery As DbQuery
    Private mProcessGrammar As ProcessGrammar
    Private mGrammarDataCollection As GrammarDataCollection
    Private mAborted As Boolean
    Private mDBWatcherThread As SafeThread = Nothing
    Private WithEvents mRestartOneShot As System.Timers.Timer
    Public Event AbortService As EventHandler
#Region "Constructor"
    Public Sub New()
        Initialize()
    End Sub
#End Region
#Region "Properties"
    Public ReadOnly Property Running() As Boolean
        Get
            Return Not mAborted
        End Get
    End Property
#End Region

#Region "Methods"
    Private Sub CompileGrammer(ByVal key As String)
        mProcessGrammar = New ProcessGrammar(key, mGrammarDataCollection.CompilerPath, mGrammarDataCollection.CompilerOptions, key, mGrammarDataCollection.OutputPath)
        AddHandler mProcessGrammar.Finished, AddressOf CompileFinishedEventHandler
        Dim t As New Thread(AddressOf StartGrammarCompiler)
        t.IsBackground = True
        t.Name = "Grammar Compiler"
        t.Start()
    End Sub
    Private Sub StartGrammarCompiler()
        mProcessGrammar.DoCompile()
        RemoveHandler mProcessGrammar.Finished, AddressOf CompileFinishedEventHandler
    End Sub
    Private Sub CompileFinishedEventHandler(ByVal sender As Object, ByVal e As CompileFinishedEventArgs)
        If e.ExitCode = 0 Then
            App.TraceLog(TraceLevel.Info, "COMPILE SUCCEEDED")
            App.TraceLog(TraceLevel.Info, "Compile complete in " & e.RunningTime)
            App.TraceLog(TraceLevel.Info, "Updating database status...")
            mDBQuery.UpdateStatus(e.Key)
            mGrammarDataCollection(e.Key).Processing= False
            App.TraceLog(TraceLevel.Verbose, "Setting processing to false for grammar:" & mGrammarDataCollection(e.Key).GrammarFile)

        Else
            App.TraceLog(TraceLevel.Error, "COMPILE FAILED")
            App.TraceLog(TraceLevel.Error, e.ErrorMessage)
            mGrammarDataCollection(e.Key).Processing = False
        End If
        




    End Sub
    Public Function GetConfigPath() As String
        Dim path = App.RootPath
        'End If
        path = IO.Path.Combine(path, "grammarConfig.xml")
        Return path


    End Function
    Public Function SetConfigPathAndFile(ByVal path As String, ByVal file As String) As Boolean
        Using softKey As RegistryKey = Registry.LocalMachine.OpenSubKey("Software")
            Using SDCSolutionsKey As RegistryKey = softKey.OpenSubKey("SDC Solutions")
                If SDCSolutionsKey Is Nothing Then
                    App.ExceptionLog(New AmcomException("Missing Registry Entry at HKEY_LOCAL_MACHINE\SOFTWARE\SDC Solutions is missing. (Intstallation Error?)"))
                End If
                Using appKey As RegistryKey = SDCSolutionsKey.OpenSubKey(App.Name)
                    If appKey Is Nothing Then
                        App.ExceptionLog(New AmcomException("Missing Registry Entry at HKEY_LOCAL_MACHINE\SOFTWARE\SDC Solutions\" & App.Name & " is missing. (Intstallation Error?)"))
                    End If
                    appKey.SetValue("config_path", path)
                    appKey.SetValue("config_file", file)
                End Using
            End Using
        End Using
    End Function
    Private Sub ProcessQueryResults(ByVal sender As Object, ByVal e As DbQueryResultEvents)
        If e.ResultCollection.Count = 0 Then
            App.TraceLog(TraceLevel.Info, "No new grammar items for " & e.FileName)
            Exit Sub
        End If
        Dim gx As New GrammerXml(e.FileName, mGrammarDataCollection(e.FileName).PreList, mGrammarDataCollection(e.FileName).PostList)
        Dim gi As GrammarItem = Nothing
        Dim name As String = ""
        For Each an As AlternateName In e.ResultCollection
            If name.ToLower = an.AltName.ToLower Then
                gi.AddID(an.DirRefNum)
            Else
                gi = New GrammarItem(an.AltName, an.DirRefNum, IO.Path.GetFileNameWithoutExtension(e.FileName))
                name = an.AltName
                gx.AddItem(gi)
            End If
        Next
        App.TraceLog(TraceLevel.Info, "Writing Grammar File " & e.FileName)
        gx.WriteGrammarFile()
        CompileGrammer(e.FileName)
    End Sub
    Private Sub Initialize()
        mConfig = ""
        mStarted = False
        mSDCConfigWatcher = Nothing
        mConfigWatcher = Nothing
        mSDCConfig = ""
        mConnectionString = ""
        mQuerySql = ""
        mUpdateSql = ""
        mIsUpdatedSql = ""
        mLastSDCConfigUpdate = Nothing
        mLastConfigUpdate = Nothing
        mGrammarDataCollection = Nothing
        mAborted = False
        App.TraceLog(TraceLevel.Info, My.Application.Info.ProductName & " " & My.Application.Info.Version.ToString)

    End Sub


    Private Sub ReadSDCConfiguration()
        '<intelliSPEECH_Service>
        '  <server>
        '    <address>127.0.0.1</address>
        '    <database>
        '      <server>127.0.0.1</server>
        '      <database>SDCINTW</database>
        '      <uid>sdc</uid>
        '      <password>thinksdc</password>
        '    </database>
        '    <config>
        '      <fileLocation>%install_path%</fileLocation>
        '    </config>
        '  </server>
        '</intelliSPEECH_Service>
        Dim doc As New XmlDocument
        'doc.Load(mSDCConfig)
        'doc.Load(mConfig)
        'Try
        '    mConfig = doc.SelectSingleNode("CONFIG/intelliSPEECH_Service/config/fileLocation").InnerText
        'Catch ex As NullReferenceException
        '    App.TraceLog(TraceLevel.Error, "'CONFIG/intelliSPEECH_Service' section not found in SDC_Config.xml!")
        '    App.TraceLog(TraceLevel.Error, "'CONFIG/intelliSPEECH_Service' section not found in SDC_Config.xml!")
        '    App.TraceLog(TraceLevel.Error, "'CONFIG/intelliSPEECH_Service' section not found in SDC_Config.xml!")
        '    Throw New AmcomException("Stopping service due to configuration Error!")
        'End Try
        'If mConfig.ToLower = "%install_path%" Then
        mConfig = App.RootPath
        'End If
        mConfig = IO.Path.Combine(mConfig, "GrammarConfig.xml")
        If Not My.Computer.FileSystem.FileExists(mConfig) Then
            Throw New AmcomException("No configuration file found at " & mConfig)
        End If
        'Dim server As String = doc.SelectSingleNode("CONFIG/intelliSPEECH_Service/database/server").InnerText
        'Dim database As String = doc.SelectSingleNode("CONFIG/intelliSPEECH_Service/database/database").InnerText
        'Dim uid As String = doc.SelectSingleNode("CONFIG/intelliSPEECH_Service/database/userid").InnerText
        'Dim password As String = doc.SelectSingleNode("CONFIG/intelliSPEECH_Service/database/password").InnerText

        'Dim builder As New SqlConnectionStringBuilder()
        'builder.DataSource = server
        'builder.InitialCatalog = database
        'builder.UserID = uid
        'builder.Password = password
        'builder.IntegratedSecurity = False
        mConnectionString = App.ConnectionString("desk")
    End Sub
    Private Sub ReadDBConfiguration()
        Dim doc As New XmlDocument
        doc.Load(mConfig)
        Dim preList As New List(Of String)
        Dim postList As New List(Of String)
        Try                                         '<grammarConfig><naturalLanguage><pre>
            For Each item As XmlNode In doc.SelectNodes("/grammarConfig/naturalLanguage/pre/item")
                preList.Add(item.InnerText)
            Next
            For Each item As XmlNode In doc.SelectNodes("/grammarConfig/naturalLanguage/post/item")
                postList.Add(item.InnerText)
            Next
            '<grammarCompiler>
            '    <path>D:\Program Files\SpeechWorks\OpenSpeech Recognizer\bin\sgc.exe</path>
            '    <commandLineOptions>-optimize 5</commandLineOptions>
            '</grammarCompiler>
            Dim compiler As String = doc.SelectSingleNode("grammarConfig/grammarCompiler/path").InnerText
            Dim compilerOptions As String = doc.SelectSingleNode("grammarConfig/grammarCompiler/commandLineOptions").InnerText
            Dim outputPath As String = doc.SelectSingleNode("grammarConfig/grammarPath").InnerText
            Dim updateInterval As Integer = doc.SelectSingleNode("grammarConfig/updateInterval").InnerText

            mGrammarDataCollection = New GrammarDataCollection(mConnectionString, updateInterval, compiler, compilerOptions, outputPath)

            For Each gram As XmlNode In doc.SelectNodes("grammarConfig/grammar")
                Dim updateType As String = gram.SelectSingleNode("updateType").InnerText
                Dim timeOfDay As String = gram.SelectSingleNode("timeOfDay").InnerText
                Dim filename As String = gram.Attributes.GetNamedItem("filename").Value
                Dim grammarPath As String = doc.SelectSingleNode("grammarConfig/grammarPath").InnerText
                Dim QuerySql = gram.SelectSingleNode("sqlQuery").InnerText
                Dim UpdateSql = doc.SelectSingleNode("grammarConfig/sql/sqlUpdate").InnerText
                Dim IsUpdatedSql = doc.SelectSingleNode("grammarConfig/sql/sqlIsUpdatedQuery").InnerText
                filename = IO.Path.Combine(grammarPath, filename)
                Dim watchData As GrammarData = New GrammarData(filename, updateType, timeOfDay, updateInterval, QuerySql, UpdateSql, IsUpdatedSql, preList, postList)
                mGrammarDataCollection.Add(filename, watchData)
            Next
        Catch ex As Exception
            App.ExceptionLog(New AmcomException("Unexpected error loading config.xml -- Check configuration document", ex))
            App.TraceLog(TraceLevel.Error, "Failed to read config.xml -- check values")
        End Try
    End Sub

    Public Sub Start(ByVal asService As Boolean)
        Try
            Thread.CurrentThread.Name = "Service Manager"
        Catch ex As InvalidOperationException
        End Try
        'Added... See if configured...
        Dim isEnabled As Boolean = App.Config.GetBoolean(ConfigSection.modules, "sdcGrammarCompiler/enabled", False)

        If Not isEnabled Then

            App.TraceLog(TraceLevel.Info, "Grammar Compiler is disabled by configuration.")
            Do
                Try
                    Thread.Sleep(1000)
                Catch ex As ThreadAbortException
                    mAborted = True
                    Exit Sub
                End Try
            Loop Until mAborted
            Exit Sub
        Else
            App.TraceLog(TraceLevel.Info, "Grammar Compiler is enabled by configuration.")
        End If


        mSDCConfig = ""
        App.TraceLog(TraceLevel.Info, "Starting Servce Manager")
        Try
            mSDCConfig = GetConfigPath()
        Catch ex As AmcomException
            mStarted = False
            Throw New AmcomException(ex.Message)
        End Try
        If Not My.Computer.FileSystem.FileExists(mSDCConfig) Then
            Throw New AmcomException("Failed to find configuration file at " & mSDCConfig & ".  (Installation issue?)")
        End If
        ReadSDCConfiguration()
        If mConfig.Length = 0 Then
            App.TraceLog(TraceLevel.Warning, "Grammar Complier can not start due to to configuration error")
            Exit Sub
        End If
        ReadDBConfiguration()
        ' mSDCConfigWatcher = New FileWatcher(IO.Path.GetDirectoryName(mSDCConfig), True)
        ' App.TraceLog(TraceLevel.Info, "Watching for changes in " & IO.Path.GetFileName(mSDCConfig) & " at " & IO.Path.GetDirectoryName(mSDCConfig))
        'mConfigWatcher = New FileWatcher(IO.Path.GetDirectoryName(mConfig), True)
        ' App.TraceLog(TraceLevel.Info, "Watching for changes in " & IO.Path.GetFileName(mConfig) & " at " & IO.Path.GetDirectoryName(mConfig))
        StartDatabaseScannerThread()
        '       If asService Then
        Do
            Try
                Thread.Sleep(100)
            Catch ex As ThreadAbortException
                App.TraceLog(TraceLevel.Info, "Service Manager Thread closing")
                Exit Do
            End Try
        Loop Until mAborted
        '      End If
    End Sub
    Private Sub StartDatabaseScannerThread()
        mDBQuery = New DbQuery(mGrammarDataCollection)
        App.TraceLog(TraceLevel.Info, "Starting database thread....")
        mDBWatcherThread = New SafeThread(New ParameterizedThreadStart(AddressOf StartDBWatcher))
        AddHandler mDBWatcherThread.ThreadException, AddressOf OnThreadException
        mDBWatcherThread.IsBackground = True
        mDBWatcherThread.Name = "Database Scanner"
        mDBWatcherThread.Tag = Now
        mDBWatcherThread.Start(mDBQuery)
    End Sub
    Private Sub OnThreadException(ByVal t As SafeThread, ByVal e As System.Exception)
        App.TraceLog(TraceLevel.Error, "Database watcher thread has Exceptioned with the following Error: " & e.Message)
        If Not TypeOf e Is SqlException Then
            App.ExceptionLog(New AmcomException("Exception in DBWatcher", e))
        Else
            App.ExceptionLog(New AmcomException("SQL Exception in DBWatcher: " & e.Message))
        End If
        If IsDate(t.Tag) Then
            If Math.Abs(DateDiff(DateInterval.Second, CDate(t.Tag), Now)) < 30 Then
                App.TraceLog(TraceLevel.Error, "Aborting Service -- Thread " & t.Name & " EXCEPTIONing more than once in a 30-second period.  Check logs.")
                RaiseEvent AbortService(Me, New System.EventArgs)
                Exit Sub
            End If
        End If
        App.TraceLog(TraceLevel.Error, "An unexpected exception occurred in the " & t.Name & " thread.  Will re-start.")
        App.TraceLog(TraceLevel.Error, "The thread will restart in 5 seconds.")
        Thread.Sleep(5000)
        StartDatabaseScannerThread()
    End Sub
   
    Private Sub StartDBWatcher(ByVal o As Object)

        AddHandler mDBQuery.NewQueryResults, AddressOf ProcessQueryResults
        mDBQuery.Start()
        RemoveHandler mDBQuery.NewQueryResults, AddressOf ProcessQueryResults
    End Sub
    Public Sub [Stop]()
        App.TraceLog(TraceLevel.Info, "Stopping Servce Manager")
        If mDBQuery IsNot Nothing Then mDBQuery.Stop()
        mAborted = True
        If mDBWatcherThread IsNot Nothing Then mDBWatcherThread.Abort()
        App.TraceLog(TraceLevel.Info, "Servce Manager Stop Complete")
    End Sub

#End Region

    Private Sub mSDCConfigWatcher_FileChanged(ByVal FullPath As String) Handles mSDCConfigWatcher.FileChanged
        Dim changed As Boolean = False
        If Not My.Computer.FileSystem.FileExists(FullPath) Then
            App.TraceLog(TraceLevel.Warning, "===>>>>>SDC_Config.xml has been removed from system. Operating with loaded values")
            Exit Sub
        End If
        If mLastSDCConfigUpdate Is Nothing Then
            changed = True
            mLastSDCConfigUpdate = My.Computer.FileSystem.GetFileInfo(FullPath).LastWriteTime
        Else
            Dim newdate As Date = My.Computer.FileSystem.GetFileInfo(FullPath).LastWriteTime
            If DateDiff(DateInterval.Second, mLastSDCConfigUpdate, newdate) > 2 Then
                changed = True
                mLastSDCConfigUpdate = newdate
            End If
        End If
        If changed Then
            If My.Computer.FileSystem.FileExists(FullPath) Then
                If mRestartOneShot Is Nothing Then
                    mRestartOneShot = New System.Timers.Timer
                End If
                mRestartOneShot.Interval = 100
                mRestartOneShot.Enabled = True
                App.TraceLog(TraceLevel.Info, "====>>>>grammarConfig.xml file changed.<<<<====  Re-initializing service.")
            Else
                App.TraceLog(TraceLevel.Warning, "===>>>>>grammarConfig.xml has been removed from system. Operating with loaded values")
            End If
        End If
    End Sub
    Private Sub mConfigWatcher_FileChanged(ByVal FullPath As String) Handles mConfigWatcher.FileChanged, mConfigWatcher.FileDeleted
        Dim changed As Boolean = False
        If Not My.Computer.FileSystem.FileExists(FullPath) Then
            App.TraceLog(TraceLevel.Warning, "===>>>>>grammarConfig.xml has been removed from system. Operating with loaded values")
            Exit Sub
        End If
        If mLastConfigUpdate Is Nothing Then
            changed = True
            mLastConfigUpdate = My.Computer.FileSystem.GetFileInfo(FullPath).LastWriteTime
        Else

            Dim newdate As Date = My.Computer.FileSystem.GetFileInfo(FullPath).LastWriteTime
            If DateDiff(DateInterval.Second, mLastSDCConfigUpdate, newdate) > 3 Then
                changed = True
                mLastConfigUpdate = newdate
            End If
        End If
        If changed Then
            If My.Computer.FileSystem.FileExists(FullPath) Then
                If mRestartOneShot Is Nothing Then
                    mRestartOneShot = New System.Timers.Timer
                End If
                mRestartOneShot.Interval = 100
                mRestartOneShot.Enabled = True
                App.TraceLog(TraceLevel.Info, "====>>>>Config.xml file changed.<<<<====  Re-initializing service.")
            Else
                App.TraceLog(TraceLevel.Warning, "===>>>>>Config.xml has been removed from system. Operating with loaded values")
            End If
        End If
    End Sub


    Private Sub mRestartOneShot_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles mRestartOneShot.Elapsed
        mRestartOneShot.Enabled = False
        mDBQuery.Stop()
        mSDCConfigWatcher.StopWatch()
        mConfigWatcher.StopWatch()
        Threading.Thread.Sleep(1000)
        Me.Start(True)
    End Sub
End Class
