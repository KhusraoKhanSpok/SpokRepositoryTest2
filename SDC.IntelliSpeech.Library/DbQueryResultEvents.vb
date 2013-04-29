Public Class DbQueryResultEvents
    Inherits EventArgs
    Private mFileName As String
    Private mResult As AlternateNameCollection
    Public Sub New(ByVal fileName As String, ByVal result As AlternateNameCollection)
        mFileName = fileName
        mResult = result
    End Sub
    Public ReadOnly Property FileName() As String
        Get
            Return mFileName
        End Get
    End Property
    Public ReadOnly Property ResultCollection() As AlternateNameCollection
        Get
            Return mResult
        End Get
    End Property
End Class
