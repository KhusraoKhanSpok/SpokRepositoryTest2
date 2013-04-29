Imports Amcom.SDC.BaseServices
Public Class DialoutCall
    <Flags()> _
    Public Enum CallStatus As Integer
        newcall = 2
        initialized = 4
        preprocess = 8
        inprocess = 16
        inprocessRetry = 32
        inProcessRetryWebServiceDown = 64
        callFail = 128
        callComplete = 256
        callKilled = 512
        callKillComplete = 1024
    End Enum
    Private mVXMLApplicationURL As String
    Private mDialPhoneNumber As String
    Private mStatus As CallStatus
    Private mIniitialDate As Date
    Private mInProcessDate As Date
    Private mCompleteDate As Date
    Private mPriority As Integer = 0
    Private mDBRefNum As Integer = 0
    Private mCampaignID As Integer = 0
    Private mCampaignDesc As String = ""
    Private mRetryCount As Integer = 0
    Private mbQueryTime As Object = Nothing
    Private mCallInterval As Integer = 5


    Public Sub New(ByVal dbRefNum As Integer, ByVal campaignID As Integer, ByVal campaignDesc As String, ByVal retryCount As Integer, ByVal appURL As String, ByVal phoneNumber As String, ByVal Priority As Integer)
        mDBRefNum = dbRefNum
        mStatus = CallStatus.newcall
        If Not appURL.ToLower.Contains("http://") Then
            mVXMLApplicationURL = App.Config.GetString(ConfigSection.modules, "sdcEmergencyNotificationSystem/defaultVXMLApplicationURL") & appURL
        Else
            mVXMLApplicationURL = appURL
        End If
        mDialPhoneNumber = phoneNumber
        mStatus = CallStatus.initialized
        mPriority = Priority
        mCampaignID = campaignID
        mCampaignDesc = campaignDesc
        mRetryCount = retryCount
        mbQueryTime = DateAdd(DateInterval.Second, 30, Now)
        mIniitialDate = Now
    End Sub

    Public Function OKToFail(ByVal retryTime As Integer) As Boolean
        Return (retryTime * mRetryCount) <= Me.DurationMins
    End Function
    Public ReadOnly Property DisplayMessage() As String
        Get
            Dim sb As New Text.StringBuilder
            sb.Append("Refnum: ")
            sb.Append(mDBRefNum)
            sb.Append(" Initiated: ")
            sb.Append(Format(mIniitialDate, "HH:mm:ss"))
            sb.Append(" to ")
            sb.Append(mDialPhoneNumber)
            sb.Append(" Duration: ")
            sb.Append(Me.Duration)
            sb.Append(" retry Count: ")
            sb.Append(mRetryCount)
            sb.Append(" Status: ")
            sb.Append(mStatus.ToString)

            Return sb.ToString
        End Get
    End Property
    Public Property InitialDate() As Date
        Get
            Return mIniitialDate
        End Get
        Set(ByVal value As Date)
            mIniitialDate = value
        End Set
    End Property
    Public ReadOnly Property DurationMins() As Integer
        Get
            Return Math.Abs(DateDiff(DateInterval.Minute, mIniitialDate, Now))
        End Get
    End Property
    Public ReadOnly Property Duration() As String
        Get
            Return GetDuration(mIniitialDate)
        End Get
    End Property
    Public ReadOnly Property ReadyToRetryDial() As Boolean
        Get
            Return DateDiff(DateInterval.Second, mbQueryTime, Now) > 0
        End Get
    End Property
    Public ReadOnly Property ReadyToDial() As Boolean
        Get
            Return Math.Abs(DateDiff(DateInterval.Second, mbQueryTime, Now)) > 5
        End Get
    End Property
    Public ReadOnly Property CampaignID() As Integer
        Get
            Return mCampaignID
        End Get
    End Property
    Public ReadOnly Property CampaignDescription() As String
        Get
            Return mCampaignDesc
        End Get
    End Property
    Public ReadOnly Property DBRecordID() As Integer
        Get
            Return mDBRefNum
        End Get
    End Property
    Public ReadOnly Property ApplicationURL() As String
        Get
            Return mVXMLApplicationURL
        End Get
    End Property
    Public ReadOnly Property PhoneNumber() As String
        Get
            Return mDialPhoneNumber
        End Get
    End Property
    Public ReadOnly Property Priority() As Integer
        Get
            Return mPriority
        End Get
    End Property
    Public Property RetryCount() As Integer
        Get
            Return mRetryCount
        End Get
        Set(ByVal value As Integer)
            mRetryCount = value
            mbQueryTime = Now
        End Set
    End Property
    Public Property Status() As CallStatus
        Get
            Return mStatus
        End Get
        Set(ByVal value As CallStatus)
            mStatus = value
        End Set
    End Property

    Private Function GetDuration(ByVal d As Date) As String
        Dim secs As Integer = Math.Abs(DateDiff(DateInterval.Second, d, Now))
        Dim hours = secs \ 3600
        secs = secs Mod 3600
        Dim mins = secs \ 60
        secs = secs Mod 60
        Return Format(hours, "00") & ":" & Format(mins, "00") & ":" & Format(secs, "00")
    End Function
End Class
