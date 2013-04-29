Public Class ConfigException
    Inherits Amcom.SDC.BaseServices.AmcomException

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub 'New
End Class
