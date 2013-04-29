Imports Newtonsoft.Json
Imports Amcom.SDC.BaseServices

Public Class SpeechCalls
    Private mRows As New Dictionary(Of String, Dictionary(Of String, Parameter))
    Private mCols As New Dictionary(Of String, Parameter)
    Private state As States = States.starting
    Private Enum States
        starting
        alldata
        row

    End Enum
    Public Sub New(ByVal jsonString As String)

        Dim mcols As Dictionary(Of String, Parameter) = Nothing
        If jsonString = "[]" Then Exit Sub
        Dim p As Parameter = Nothing
        For Each callinfo As JavaScriptObject In JavaScriptConvert.DeserializeObject(jsonString)
            mcols = New Dictionary(Of String, Parameter)
            For Each k As String In callinfo.Keys
                mcols.Add(k, New Parameter(k, callinfo(k).ToString))
            Next
            If Not mRows.ContainsKey(callinfo("foreignkey").ToString) Then
                mRows.Add(callinfo("foreignkey").ToString, mcols)
            End If

        Next
    End Sub
    Public ReadOnly Property AllCallCount() As Integer
        Get
            Return mRows.Count
        End Get
    End Property
    Public ReadOnly Property InProcessCallCount() As Integer
        Get
            Dim count As Integer = 0
            For Each l As Dictionary(Of String, Parameter) In mRows.Values
                If l("status").Value.ToLower = "inprocess" Then count += 1
            Next
            Return count
        End Get
    End Property
    Public ReadOnly Property PendingCallCount() As Integer
        Get
            Dim count As Integer = 0
            For Each l As Dictionary(Of String, Parameter) In mRows.Values
                If l("status").Value.ToLower = "pending" Then count += 1
            Next
            Return count
        End Get
    End Property
    Public ReadOnly Property PendingCalls() As Dictionary(Of String, Dictionary(Of String, Parameter))
        Get
            Dim d As New Dictionary(Of String, Dictionary(Of String, Parameter))
            For Each l As Dictionary(Of String, Parameter) In mRows.Values
                If l("status").Value.ToLower.Contains("pending") Then
                    d.Add(l("foreignkey").Value, l)
                End If
            Next
            Return d
        End Get
    End Property

    Public ReadOnly Property RetryCalls() As Dictionary(Of String, Dictionary(Of String, Parameter))
        Get
            Dim d As New Dictionary(Of String, Dictionary(Of String, Parameter))
            Dim count As Integer = 0
            For Each l As Dictionary(Of String, Parameter) In mRows.Values
                count += 1
                Debug.Print(count & " " & l("status").Value)
                If l("status").Value.ToLower.Contains("retry") Then
                    d.Add(l("foreignkey").Value, l)
                End If
            Next
            Return d
        End Get
    End Property
    Public Function DoesCallExist(ByVal key As String) As Boolean
        Return mRows.ContainsKey(key)
    End Function
    Public Function CallInfo(ByVal key As String) As Dictionary(Of String, Parameter)
        Return mRows(key)
    End Function
End Class
