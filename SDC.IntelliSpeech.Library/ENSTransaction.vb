Public Class ENSTransaction
    Private mTransType As String
    Private mPriority As Integer
    Public Sub New(ByVal transactionType As String, ByVal priority As Integer)
        mTransType = transactionType
        mPriority = priority
    End Sub
    Public ReadOnly Property TransactionType() As String
        Get
            Return mTransType
        End Get
    End Property
    Public ReadOnly Property Priority() As Integer
        Get
            Return mPriority
        End Get
    End Property
End Class
