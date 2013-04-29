Public Class NewOutboundCallEventArgs
    Inherits EventArgs
    Private mCall As DialoutCall
    Public Sub New(ByVal [call] As DialoutCall)
        mCall = [call]
    End Sub
    Public ReadOnly Property [Call]() As DialoutCall
        Get
            Return mCall
        End Get
    End Property
End Class
