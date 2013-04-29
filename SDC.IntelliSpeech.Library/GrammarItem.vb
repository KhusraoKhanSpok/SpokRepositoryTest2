Public Class GrammarItem
    Private mName As String
    Private mIDs As List(Of Integer)
    Private _baseGrammar As String = ""
#Region "Properties"
    Public Property Name() As String
        Get
            Return mName.Replace("_", " ").Trim.ToLower
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property
    Public ReadOnly Property AltName() As String
        Get
            Return mName.ToLower
        End Get
    End Property
    Public ReadOnly Property IDs() As List(Of Integer)
        Get
            Return mIDs
        End Get
    End Property
#End Region
#Region "Methods"
    Public Sub AddID(ByVal ID As Integer)
        If mIDs.Contains(ID) Then Exit Sub
        mIDs.Add(ID)
    End Sub
    Public Sub New(ByVal altname As String, ByVal id As Integer, ByVal baseGrammar As String)
        _baseGrammar = baseGrammar
        mName = altname
        mIDs = New List(Of Integer)
        mIDs.Add(id)
    End Sub
    Public Sub New(ByVal altname As String, ByVal baseGrammar As String)
        _baseGrammar = baseGrammar
        mName = Name
        mIDs = New List(Of Integer)
    End Sub
    Private Sub New(ByVal baseGrammar As String)
        _baseGrammar = baseGrammar
        mName = ""
        mIDs = New List(Of Integer)
    End Sub
    Public Overrides Function ToString() As String
        '      <item tag="id=&apos;121627&apos;; altname=&apos;margaret_adams&apos;; grammar_name=&apos;directory&apos;;">margaret adams</item>
        '      <item tag="id=&apos;121831,121832,121833,121834,121835,121836,121837,121838,121839,121840,121841&apos;; altname=&apos;administration&apos;; grammar_name=&apos;directory&apos;;">administration</item>
        Dim sb As New Text.StringBuilder
        sb.Append("<item tag=")
        sb.Append(Chr(34))
        sb.Append("id=&apos;")
        If mIDs.Count = 1 Then
            sb.Append(mIDs(0))
        Else
            Dim last As Integer = mIDs(mIDs.Count - 1)
            For Each id As Integer In mIDs
                sb.Append(id)
                If id = last Then Continue For
                sb.Append(",")
            Next
        End If
        sb.Append("&apos;;")

        'AltName
        sb.Append("altname=&apos;")
        sb.Append(Me.AltName)


        If _baseGrammar.Length Then
            sb.Append("&apos;;grammar_name=&apos;directory&apos;;")
        Else
            sb.Append("&apos;;grammar_name=&apos;" & _baseGrammar & "&apos;;")
        End If
        sb.Append(Chr(34))
        sb.Append(">")
        sb.Append(Me.Name)
        sb.Append("</item>")
        Return sb.ToString
    End Function
#End Region

End Class
