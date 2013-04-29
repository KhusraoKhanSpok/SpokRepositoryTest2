Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports Amcom.SDC.BaseServices
Public Class ENSTransactions
    Inherits Collection(Of ENSTransaction)
    Private mD As Dictionary(Of String, Integer)
    Public Sub New()
        MyBase.new()
        mD = New Dictionary(Of String, Integer)
    End Sub
    Public Shadows Sub Add(ByVal tran As ENSTransaction)
        MyBase.Add(tran)
        mD.Add(tran.TransactionType, tran.Priority)
    End Sub
    Public Shadows Sub remove(ByVal tran As ENSTransaction)
        MyBase.Remove(tran)
        mD.Remove(tran.TransactionType)
    End Sub
    Public ReadOnly Property SQLWhere() As String
        Get

            Dim sb As New Text.StringBuilder
            For Each t As ENSTransaction In Me
                If sb.Length = 0 Then
                    sb.Append("TRNTYP = ")
                    sb.Append("'")
                    sb.Append(SqlEscape(t.TransactionType))
                    sb.Append("'")
                Else
                    sb.Append(" OR TRNTYP = ")
                    sb.Append("'")
                    sb.Append(SqlEscape(t.TransactionType))
                    sb.Append("'")
                End If
            Next
            Return sb.ToString
        End Get
    End Property
    Public Function ProirityFromTransactionType(ByVal t As String) As Integer
        Return mD.Item(t)
    End Function
End Class
