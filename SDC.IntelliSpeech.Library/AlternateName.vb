Public Class AlternateName
    Private mAltName As String
    Private mDirRefNum As Integer
    Public Sub New(ByVal altName As String, ByVal dirRefNum As Integer)
        mAltName = altName
        mDirRefNum = dirrefNum
    End Sub
    Public ReadOnly Property AltName() As String
        Get
            Return mAltName
        End Get
    End Property
    Public ReadOnly Property DirRefNum() As Integer
        Get
            Return mDirRefNum
        End Get
    End Property
End Class
