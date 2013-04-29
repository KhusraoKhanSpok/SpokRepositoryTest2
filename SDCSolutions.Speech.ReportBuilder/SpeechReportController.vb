

Imports Amcom.SDC.BaseServices

Public Class SpeechReportController

    Private threadFinished As Boolean = False 'Control variable that will stop the main function from exiting until the worker thread finishes. 

    Dim config As ReportConfigReader

    Dim _DebugMode As Boolean = False

    Dim filesCreated As Collection 'This is for clean up if we have to cancel. 

    Public Event threadDone As eventhandler

    Private _ReportMakerThread As Threading.Thread



    Sub MakeWeeklyReports(Optional ByVal debugMode As Boolean = False)

        _DebugMode = debugMode
        App.TraceLog(TraceLevel.Verbose, "VERBOSE - MakeWeeklyReports(): Method called. If it is time to make reports, will do so, otherwise will exit immediatley.")
        If Not My.Computer.FileSystem.FileExists(App.LogPath & "\ReportStopper.Stub") Then
            My.Computer.FileSystem.WriteAllText(App.LogPath & "\ReportStopper.Stub", "Stub file that stops reports. If this is here and reports aren't running, an error occured.", False)
            App.TraceLog(TraceLevel.Verbose, "Stub file placed created: " & App.LogPath & "ReportStopper.Stub")
        Else
            App.TraceLog(TraceLevel.Error, "ERROR - Stub file detected. Not re-starting reports. Exiting now.")
            Exit Sub
        End If
        Try
            filesCreated = New Collection
            config = New ReportConfigReader
            config.Load() 'pre-load all sites.

            AddHandler Me.threadDone, AddressOf _handleThreadDoneEvent



            threadFinished = False

            _ReportMakerThread = New Threading.Thread(AddressOf ReportMakerThread)
            _ReportMakerThread.Name = "ReportMaker"
            _ReportMakerThread.IsBackground = True
            _ReportMakerThread.Start()


            'Wait for the thread to finish. This thread needs to stay alive in order to catch a possible abort command.  
            Do While threadFinished = False
                Threading.Thread.Sleep(100)
            Loop


        Catch ex As AmcomException
            App.TraceLog(TraceLevel.Error, "ERROR - SpeechReporting.MakeReports(1):Error with loading Config: " & ex.Message)
        Catch AbortEx As Threading.ThreadAbortException
            App.TraceLog(TraceLevel.Warning, "WARNING - SpeechReporting.MakeReports(1): Abort command received: " & AbortEx.Message)
            _ReportMakerThread.Abort()
            Exit Sub
        Finally
            If My.Computer.FileSystem.FileExists(App.LogPath & "\ReportStopper.Stub") Then
                My.Computer.FileSystem.DeleteFile(App.LogPath & "\ReportStopper.Stub")
                App.TraceLog(TraceLevel.Verbose, "Stub file removed.")
            End If
        End Try


        'For Each site As String In config.sites
        '    config.LoadSiteConfig(site)
        '    'If it is our time to do our reporting stuff, then GO!
        '    If timeToRun() Then
        '        'Code to tell the report generator to GO!
        '        App.TraceLog(TraceLevel.Info, "INFO - MakeWeeklyReports(): Determined it was time to make reports for " & site & "! Reporting services starting NOW!")
        '        reportMaker.makeReports(config)
        '    End If
        'Next

        RemoveHandler Me.threadDone, AddressOf _handleThreadDoneEvent
        App.TraceLog(TraceLevel.Info, "INFO - MakeWeeklyReports(): Report Services has finished. Exiting main function.")

    End Sub

    Public Sub [stop]()
        _ReportMakerThread.Abort()
    End Sub

    Sub ReportMakerThread()
        Dim reportmaker As ReportGenerator
        Try
            'Prep folders
            For Each site As String In config.sites
                Try
                    config.LoadSiteConfig(site)
                    folderPrep(config)
                Catch ex As AmcomException
                    App.TraceLog(TraceLevel.Error, "ERROR - Config bad for site " & site & ", Skipping.")
                End Try
            Next

            'Now build reports
            For Each site As String In config.sites
                'bit goes here
                Try
                    config.LoadSiteConfig(site)
                    If (timeToRun(config.dayToRun, config.hourToRun) Or _DebugMode) Then
                        'Code to tell the report generator to GO!
                        App.TraceLog(TraceLevel.Info, "INFO - MakeWeeklyReports(): Determined it was time to make reports for " & site & "! Reporting services starting NOW!")

                        reportmaker = New ReportGenerator

                        AddHandler reportmaker.fileMade, AddressOf handleFileMadeEvent
                        reportmaker.makeReports(config)

                        RemoveHandler reportmaker.fileMade, AddressOf handleFileMadeEvent


                        'Now clean up!
                        'GC.Collect()
                        'GC.WaitForPendingFinalizers()

                    End If
                Catch ex As AmcomException
                    App.TraceLog(TraceLevel.Error, "ERROR - Config bad for site " & site & ", Skipping.")

                End Try
                'If it is our time to do our reporting stuff, then GO!

            Next





        Catch AbortEx As Threading.ThreadAbortException
            App.TraceLog(TraceLevel.Info, "ReportMaker: Detected thread abortion. Checking for report files...")

            reportMaker = Nothing
            GC.Collect() 'Collect the garbage!

            If filesCreated.Count > 0 Then
                App.TraceLog(TraceLevel.Info, "Report files detected. Removing them.")
                For Each file As String In filesCreated
                    Try
                        If System.IO.File.Exists(file) Then
                            App.TraceLog(TraceLevel.Verbose, "Deleted: " & file)
                            FileIO.FileSystem.DeleteFile(file)
                        End If
                    Catch ex As Exception
                        App.TraceLog(TraceLevel.Error, "Could not delete files, Reason: " & ex.ToString)
                        Exit Sub
                    End Try
                Next
                App.TraceLog(TraceLevel.Info, "Deleting files finished.")
            Else
                App.TraceLog(TraceLevel.Info, "No files to clean up.")
            End If

            App.TraceLog(TraceLevel.Info, "Clean-up done. Exiting now.")
            Exit Sub
        Catch ex As AmcomException
            App.TraceLog(TraceLevel.Error, ex.ToString)
        Catch ex As Exception
            App.TraceLog(TraceLevel.Error, ex.ToString)
        Finally
            App.TraceLog(TraceLevel.Info, "Worker thread has finished work for reports.")
        End Try

        For Each site As String In config.sites
            Try
                App.TraceLog(TraceLevel.Verbose, "Started Clean-up...")
                config.LoadSiteConfig(site)
                moveFinishedFilesFromTemp(config)
            Catch ex As AmcomException
                App.TraceLog(TraceLevel.Error, "ERROR - Config bad for site " & site & ", Skipping.")
            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "ERROR- Can't move files from temp folder for " & site & ", skipping.")
            Finally

            End Try
        Next

        RaiseEvent threadDone(Me, New EventArgs)
    End Sub

    Sub ForceReports(ByVal startDate As Date, ByVal endDate As Date, ByVal transaction As Boolean, ByVal transfer As Boolean, ByVal channel As Boolean, ByVal email As Boolean, ByVal debug As Boolean, ByVal setConfig As ReportConfigReader)
        App.TraceLog(TraceLevel.Info, "INFO - ForceReports(): Recieved a command to FORCE reports. Making reports NOW!")
        folderPrep(setConfig)
        Dim reportmaker As New ReportGenerator
        reportmaker.makeReports(startDate, endDate, transaction, transfer, channel, email, debug, setConfig, True)
        moveFinishedFilesFromTemp(setConfig)
    End Sub

    Private Function timeToRun(ByVal dayToRun As String, ByVal hourToRun As String) As Boolean
        Dim thisDay As Integer
        Dim thisHour As Integer

        thisDay = Today.DayOfWeek
        thisHour = Now.Hour

        If Integer.Parse(dayToRun) = thisDay And Integer.Parse(hourToRun) = thisHour Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub handleFileMadeEvent(ByVal e As FileCreatedEventArgs)
        'Handles the fileMade event from the report generator. 
        filesCreated.Add(e.fileCreated)
    End Sub

    Private Sub folderPrep(ByVal configs As ReportConfigReader)
        'Deletes and re-creates the TMP folder, just in case we ran into a problem last time. 
        App.TraceLog(TraceLevel.Info, "INFO - Prepping folder for report generation...")
        Try
            If FileIO.FileSystem.DirectoryExists(configs.renderedLocation & "TMP") Then
                FileIO.FileSystem.DeleteDirectory(configs.renderedLocation & "TMP", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            FileIO.FileSystem.CreateDirectory(configs.renderedLocation & "TMP")
        Catch ex As Exception
            App.TraceLog(TraceLevel.Error, "ERROR - There was a problem cleaning folders before running reports: " & ex.ToString)
        End Try
    End Sub

    Private Sub _handleThreadDoneEvent(ByVal sender As Object, ByVal e As EventArgs)
        filesCreated = Nothing
        App.TraceLog(TraceLevel.Info, "All done.")
        threadFinished = True
    End Sub

    Private Sub moveFinishedFilesFromTemp(ByVal configs As ReportConfigReader)
        'Moves all files from the TMP Folder to the proper reporting folder.
        App.TraceLog(TraceLevel.Info, "INFO - Moving finished report folders out of TMP...")

        Dim filesInTmp As New Collection

        For Each file As String In FileIO.FileSystem.GetFiles(configs.renderedLocation & "TMP")
            filesInTmp.Add(file)
        Next

        For Each file As String In filesInTmp
            Try
                FileIO.FileSystem.CopyFile(file, configs.renderedLocation & System.IO.Path.GetFileName(file), True)
                'FileIO.FileSystem.DeleteFile(file)

            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "ERROR - There was an error copying file: " & ex.ToString)
            End Try
        Next

        For Each deleteThisfile As String In filesInTmp
            Try

                FileIO.FileSystem.DeleteFile(deleteThisfile)

            Catch ex As Exception
                App.TraceLog(TraceLevel.Error, "ERROR - There was an error deleting file: " & ex.ToString)
            End Try
        Next
    End Sub
End Class
