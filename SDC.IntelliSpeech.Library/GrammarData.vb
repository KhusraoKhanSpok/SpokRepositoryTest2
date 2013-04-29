Imports Amcom.SDC.BaseServices

Public Class GrammarData
    Private mQuerySql As String
    Private mUpdateSql As String
    Private mIsUpdatedSql As String
    Private mGrammarFile As String
    Private mPreList As List(Of String)
    Private mPostList As List(Of String)
    Private mLastProcessedDate As Object
    Private mUpdateType As updateTypes = updateTypes.scheduled
    Private mTimeOfDay As String
    Private mUpdateInterval As Integer
    Private mNextProcessingDate As Date = Nothing

    Private mProcessing As Boolean
    Public Enum updateTypes As Integer
        scheduled
        continuous
    End Enum
    Public Sub New(ByVal grammarFile As String, ByVal updateType As String, ByVal timeOfDay As String, ByVal updateInterval As Integer, ByVal querySql As String, ByVal updateSql As String, ByVal isUpdatedSql As String, ByVal prelist As List(Of String), ByVal postList As List(Of String))
        mProcessing = False
        mUpdateInterval = updateInterval
        mGrammarFile = grammarFile
        mQuerySql = querySql
        mUpdateSql = updateSql
        mIsUpdatedSql = isUpdatedSql
        mPreList = prelist
        mPostList = postList
        mLastProcessedDate = Nothing
        Select Case updateType.ToUpper
            Case "SCHEDULED"
                mUpdateType = updateTypes.scheduled
            Case "CONTINUOUS"
                mUpdateType = updateTypes.continuous
        End Select
        mTimeOfDay = timeOfDay
    End Sub
    Public ReadOnly Property UpdateIntervalMins() As Integer
        Get
            Return mUpdateInterval
        End Get
    End Property
    Public Property Processing() As Boolean

        Get
            App.TraceLog(TraceLevel.Verbose, "is mProcessing=" & mProcessing & " " & mGrammarFile)
            Return mProcessing
        End Get
        Set(ByVal value As Boolean)
            mProcessing = value
            App.TraceLog(TraceLevel.Verbose, "Setting processing to complete for grammar:" & GrammarFile)
            App.TraceLog(TraceLevel.Verbose, "Value of mProcessing" & mProcessing)

        End Set
    End Property
    Public ReadOnly Property ProcessNow() As Boolean
        Get
            Select Case mUpdateType
                Case updateTypes.continuous

                    If mLastProcessedDate Is Nothing Then
                        mNextProcessingDate = DateAdd(DateInterval.Minute, mUpdateInterval, Now)
                        mLastProcessedDate = Now
                        mProcessing = True
                        Return True
                    End If
                    If DateDiff(DateInterval.Second, mNextProcessingDate, Now) > 0 Then
                        mNextProcessingDate = DateAdd(DateInterval.Minute, mUpdateInterval, mNextProcessingDate)
                        mLastProcessedDate = Now
                        mProcessing = True
                        Return True
                    End If
                    Return False

                Case updateTypes.scheduled
                    If Processing() Then
                        App.TraceLog(TraceLevel.Verbose, "grammar build already in process for" & GrammarFile)
                        App.TraceLog(TraceLevel.Verbose, "Next Scheduled processing for grammar:" & GrammarFile & " is:" & mNextProcessingDate)
                        Return False
                    End If


                    If mLastProcessedDate Is Nothing Then
                        If (DateDiff(DateInterval.Second, Now, CDate(DateValue(Now).ToShortDateString & " " & mTimeOfDay)) > 0) Then
                            mNextProcessingDate = CDate(DateValue(Now) & " " & mTimeOfDay)
                            mProcessing = True
                            mLastProcessedDate = Now
                            App.TraceLog(TraceLevel.Verbose, "Next Scheduled processing for grammar:" & GrammarFile & " is:" & mNextProcessingDate)
                        Else
                            mNextProcessingDate = CDate(DateValue(Now).AddDays(1).ToShortDateString & " " & mTimeOfDay)
                            mProcessing = True
                            mLastProcessedDate = Now
                            App.TraceLog(TraceLevel.Verbose, "Next Scheduled processing for grammar:" & GrammarFile & " is:" & mNextProcessingDate)
                        End If

                        Return True
                    End If

                    If DateDiff(DateInterval.Second, Now, mNextProcessingDate) < 0 Then
                        mLastProcessedDate = Now
                        mNextProcessingDate = CDate(DateValue(Now).AddDays(1).ToShortDateString & " " & mTimeOfDay)
                        App.TraceLog(TraceLevel.Verbose, "Next Scheduled processing for grammar:" & GrammarFile & " is:" & mNextProcessingDate)
                        mProcessing = True
                        Return True
                    End If
                    Return False
            End Select
            Return False
        End Get

    End Property
    Public ReadOnly Property UpdateType() As updateTypes
        Get
            Return mUpdateType
        End Get
    End Property
    Public ReadOnly Property TimeOfDay() As String
        Get
            Return mTimeOfDay
        End Get
    End Property
    Public Property LastProcessDate() As Object
        Get
            Return mLastProcessedDate
        End Get
        Set(ByVal value As Object)
            mLastProcessedDate = value
        End Set
    End Property
    Public ReadOnly Property GrammarFile() As String
        Get
            Return mGrammarFile
        End Get
    End Property
    Public ReadOnly Property PreList() As List(Of String)
        Get
            Return mPreList
        End Get
    End Property
    Public ReadOnly Property PostList() As List(Of String)
        Get
            Return mPostList
        End Get
    End Property
    Public ReadOnly Property QuerySql() As String
        Get
            Return mQuerySql
        End Get
    End Property
    Public ReadOnly Property UpdateSql() As String
        Get
            Return mUpdateSql
        End Get
    End Property
    Public ReadOnly Property IsUpdatedSql() As String
        Get
            Return mIsUpdatedSql
        End Get
    End Property

End Class
