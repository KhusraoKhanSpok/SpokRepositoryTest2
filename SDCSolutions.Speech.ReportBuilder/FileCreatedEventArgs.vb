Public Class FileCreatedEventArgs
    'This is a class for holding fileCreated even args.
    Inherits EventArgs
    Dim fileMade As String = ""

    Public ReadOnly Property fileCreated() As String
        Get
            Return fileMade
        End Get
    End Property
    Public Sub New(ByVal fileName As String)
        fileMade = fileName
    End Sub
End Class
