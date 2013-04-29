Public Class ThreadExceptionDetail
    Private _count As Integer = 0
    Private _lastExceptionTime As Object = Nothing
    Private _lastMessage As String
    Public Sub New()
        _lastExceptionTime = Now
    End Sub
    Public ReadOnly Property Count() As Integer
        Get
            Return _count
        End Get
    End Property
    Public ReadOnly Property TimeDifference(ByVal d As Date) As Integer
        Get
            Return Math.Abs(DateDiff(DateInterval.Second, d, _lastExceptionTime))
        End Get
    End Property
    Public ReadOnly Property Uptime() As String
        Get
            Dim seconds As Integer = Math.Abs(DateDiff(DateInterval.Second, Now, _lastExceptionTime))
            Dim hours = seconds \ 3600
            seconds = seconds Mod 60
            Dim mins As Integer = seconds \ 60
            seconds = seconds Mod 60
            Return Format(hours, "00") & ":" & Format(mins, "00") & ":" & Format(seconds, "00")
        End Get
    End Property
    Public Sub Add(ByVal ex As System.Exception)
        _lastMessage = ex.Message
        _lastExceptionTime = Now
        _count += 1
    End Sub
End Class
