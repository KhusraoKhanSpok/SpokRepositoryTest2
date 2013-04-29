Public Class Responder
    Private _Campaign As String
    Private _MaxResponders As Integer
    Private _Completed As Boolean
    Private _Finished As Boolean
    Private _CampaignKilled As Boolean
    Public Sub New(ByVal campaign As String, ByVal maxResponers As Integer)
        _Campaign = campaign
        _MaxResponders = maxResponers
        _Completed = False
        _Finished = False
    End Sub
    Public Property CampaignKilled() As Boolean
        Get
            Return _CampaignKilled
        End Get
        Set(ByVal value As Boolean)
            _CampaignKilled = value
        End Set
    End Property
    Public ReadOnly Property Campaign() As String
        Get
            Return _Campaign
        End Get
    End Property
    Public Property Finished() As Boolean
        Get
            Return _Finished
        End Get
        Set(ByVal value As Boolean)
            _Finished = value
        End Set
    End Property
    Public ReadOnly Property MaxResponders() As Integer
        Get
            Return _MaxResponders
        End Get
    End Property
    Public Property Completed() As Boolean
        Get
            Return _Completed
        End Get
        Set(ByVal value As Boolean)
            _Completed = value
        End Set
    End Property
End Class
