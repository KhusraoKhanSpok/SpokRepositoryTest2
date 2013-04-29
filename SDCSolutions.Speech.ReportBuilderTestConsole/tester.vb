Imports Amcom.SDC.Speech.ReportBuilder
Imports Amcom.SDC.BaseServices

Module tester

    Dim quit As Boolean = False
    Dim abortTime As Long = 0 'Quit time. The amount of time before we kill the thread. 
    Private _TesterThread As Threading.Thread
    Private _ReportControllerThread As Threading.Thread

    Sub Main()

        Console.WriteLine("Speech Reportbuilder Test harness is starting up...")
        App.Initialize(ApplicationType.Console, My.Settings)
        AddHandler App.TraceMessage, AddressOf handleTraceMessage

        Dim command As String = ""
        While Not quit
            Console.WriteLine("Enter command: ")
            command = Console.ReadLine()
            interperetCommand(command)

        End While

    End Sub

    Sub handleTraceMessage(ByVal sender As Object, ByVal e As BaseServices.TraceMessageEventArgs)
        Console.WriteLine(e.Level & "|" & e.Message)
    End Sub

    Sub interperetCommand(ByVal command As String)
        command = command.ToLower 'lowercase everything so we don't have to deal with typos. 

        Dim commandStack As String() = Split(command, " ") 'Split the string by spaces. 
        Dim verb As String = commandStack(0)

        Select Case verb
            Case "quit", "q"
                Console.WriteLine("Quitting...")
                quit = True
            Case "set"
                Try
                    Dim noun As String = commandStack(1)
                    Dim value As String = commandStack(2)
                    If noun = "aborttime" Then
                        abortTime = Long.Parse(value)
                        Console.WriteLine("Abort time set to: " & abortTime)
                    Else
                        Console.WriteLine("I don't know what that is. I only know to set the 'abortTime' right now.")
                    End If
                Catch ex As Exception
                    Console.WriteLine("Mistyped command. Please enter: 'set <item> <value>'.")
                End Try
            Case "run", "go"
                'Run the test.
                Console.WriteLine("Running test, aborting in " & abortTime & " seconds.")
                _TesterThread = New Threading.Thread(AddressOf testThread)
                _TesterThread.Name = "Tester Thread"
                _TesterThread.IsBackground = True
                _TesterThread.Start()
            Case "clear", "c"
                Console.Clear()
            Case "kill", "k"
                abortTime = 0
                Console.WriteLine("Manual Kill was set.")

            Case "help"
                Console.Clear()
                Console.WriteLine("Available commands:" & vbCrLf)
                Console.WriteLine("quit,q|Quits the tester" & vbCrLf)
                Console.WriteLine("set aborttime <#>|Sets the number of seconds before the thread is aborted." & vbCrLf)
                Console.WriteLine("go,run|Runs the test." & vbCrLf)
                Console.WriteLine("clear|Clears the console." & vbCrLf)

            Case Else
                Console.WriteLine("Unrecognized command.")


        End Select

    End Sub

    Sub testThread()
        'This will wait for X seconds, then kill the report thread it creates, and then it's self. 
        Dim waitCount As Integer = 0
        Dim reportMaker As New Speech.ReportBuilder.SpeechReportController
        _ReportControllerThread = New Threading.Thread(AddressOf reportMaker.MakeWeeklyReports)
        _ReportControllerThread.Name = "Report Controller Thread"
        _ReportControllerThread.IsBackground = True
        _ReportControllerThread.Start()


        Do
            Threading.Thread.Sleep(1000)
            waitCount += 1
            Console.WriteLine(waitCount & " SECONDS ELAPSED...")

            If waitCount >= abortTime Then
                Console.WriteLine("KILLING THREAD NOW")
                _ReportControllerThread.Abort()
                Exit Sub
            End If

        Loop

    End Sub


End Module
