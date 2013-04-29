Imports System.Threading
Imports System.Data.SqlClient
Imports Amcom.SDC.BaseServices
Public Class DbQuery
    Private mGrammarDataCollecton As GrammarDataCollection
    Private mAbort As Boolean
    Private mStartTime As Object
    Private mBusy As Boolean
    Private mStopped As Boolean
    Public Event NewQueryResults As EventHandler(Of DbQueryResultEvents)
    Public Delegate Sub NewQueryResultsHandler(ByVal e As EventArgs)

    Private Sub DoDBQuery()
        Dim baseName As String = ""
        mStopped = False
        App.TraceLog(TraceLevel.Verbose, "Checking grammarConfig for grammar processing rules...")
        For Each fileName As String In mGrammarDataCollecton.Keys
            baseName = IO.Path.GetFileNameWithoutExtension(fileName)
            If Not mGrammarDataCollecton(fileName).ProcessNow Then Continue For
            App.TraceLog(TraceLevel.Verbose, "Querying database for grammar file " & IO.Path.GetFileName(fileName) & "...")
            Dim c As New AlternateNameCollection
            Using conn As New SqlConnection(mGrammarDataCollecton.ConnectionString)
                Try
                    conn.Open()
                    Dim cmd As New SqlCommand(mGrammarDataCollecton(fileName).QuerySql, conn)
                    Dim an As AlternateName
                    Using reader As SqlDataReader = cmd.ExecuteReader
                        If reader.HasRows Then
                            App.TraceLog(TraceLevel.Info, "Found new records to process...")
                            While reader.Read
                                If IsDBNull(reader!ASR_AlternateName) OrElse IsDBNull(reader!DirRefNum) Then
                                    App.TraceLog(TraceLevel.Warning, "NULL records found in Grammar ASR_Alternatename or DirRefNum")
                                    Continue While
                                End If
                                an = New AlternateName(reader!ASR_AlternateName, reader!DirRefNum)
                                c.Add(an)
                            End While
                        Else
                            mGrammarDataCollecton(fileName).Processing = False

                        End If
                    End Using
                Catch ex As SqlException
                    Throw New AmcomException("Failed to query database... error was: " & ex.Message)
                End Try
            End Using
            If c.Count = 0 Then
                App.TraceLog(TraceLevel.Verbose, "Nothing to do for grammar file " & IO.Path.GetFileName(fileName) & "...")
                Continue For 'nothing to do
            End If
            App.TraceLog(TraceLevel.Verbose, "Completed processing for grammar file " & IO.Path.GetFileName(fileName) & "...")
            RaiseEvent NewQueryResults(Me, New DbQueryResultEvents(fileName, c))
        Next
    End Sub
    Public Sub UpdateStatus(ByVal fileNameKey As String)
        Using conn As New SqlConnection(mGrammarDataCollecton.ConnectionString)
            Try
                conn.Open()
                Dim cmd As New SqlCommand(mGrammarDataCollecton(fileNameKey).UpdateSql, conn)
                Dim result As Integer = cmd.ExecuteNonQuery()
                Debug.Print(result)
            Catch ex As SqlException
                App.TraceLog(TraceLevel.Error, "Failed to update database: error was" & ex.Message)
                Exit Sub
            End Try
        End Using
        App.TraceLog(TraceLevel.Info, "Database status updated successfully")
    End Sub

    Public Sub Initialize()
        mAbort = False
        mStartTime = Nothing
        mBusy = False
    End Sub
    Public Sub New(ByVal gdc As GrammarDataCollection)
        mGrammarDataCollecton = gdc
    End Sub

    Public Sub UpdateConfigParams(ByVal gdc As GrammarDataCollection)
        If mBusy Then Throw New AmcomException("Cannont update while processing")
        mGrammarDataCollecton = gdc
    End Sub

    Public Sub Start()
        Do
            Try
                If mAbort Then Exit Do
                Threading.Thread.Sleep(1000)
                If mGrammarDataCollecton Is Nothing OrElse mBusy Then Continue Do
                If mGrammarDataCollecton.Count = 0 Then Continue Do
                'If mStartTime Is Nothing Then
                '    Select Case mGrammarDataCollecton.Type.ToLower
                '        Case "continuous"
                '            mStartTime = Now
                '        Case "daily"
                '            mStartTime = GetDailyStartTime()
                '        Case "scheduled"
                '            App.TraceLog(TraceLevel.Info, "Schedule type value of 'schedule' has been depreciated; value will be changed to 'daily'")
                '            mStartTime = GetDailyStartTime()
                '        Case Else
                '            App.TraceLog(TraceLevel.Error, "INVALID scheduler type in config.xml!  Cannot process grammar")
                '            Exit Do
                '    End Select
                'End If
            Catch ex As ThreadAbortException
                mBusy = False
                mAbort = True
                Exit Do
            End Try
            'If DateDiff(DateInterval.Second, mStartTime, Now) >= 0 Then
            mBusy = True
            Try
                DoDBQuery()
            Catch ex As ThreadAbortException
                mBusy = False
                mAbort = True
            Catch ex As AmcomException
                mBusy = False
                App.TraceLog(TraceLevel.Error, "Database failure: " & ex.Message)
            End Try
            'Select Case mGrammarDataCollecton.Type.ToLower
            '    Case "continuous"
            '        mStartTime = DateAdd(DateInterval.Second, CDbl(mGrammarDataCollecton.Interval), Now)
            '    Case "daily", "scheduled"
            '        mStartTime = GetDailyStartTime()
            'End Select
            App.TraceLog(TraceLevel.Info, "Database Processing finished.")
            ' App.TraceLog(TraceLevel.Info, "Next grammar processing at: " & mStartTime)
            mBusy = False
            'End If
        Loop While mAbort = False
        If mAbort Then App.TraceLog(TraceLevel.Info, "Stopping Grammar Processing Thread")
        mStopped = True
    End Sub
    Public Sub [Stop]()
        mAbort = True
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Public ReadOnly Property Stopped() As String
        Get
            Return mStopped
        End Get
    End Property

End Class
