Public Class badCallLogEx
    Inherits Exception
    Dim msg As String

    Public Overrides ReadOnly Property Message() As String
        Get
            Return msg
        End Get
    End Property
    Public Sub New(ByVal message As String)
        msg = message
    End Sub

End Class
