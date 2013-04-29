Public Class CampaignCompletedEventArgs
    Inherits EventArgs
    Private _Campaign As String
    Public Sub New(ByVal campaign As String)
        _Campaign = campaign
    End Sub
    Public ReadOnly Property Campaign() As String
        Get
            Return _Campaign
        End Get
    End Property
End Class
