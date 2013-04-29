
Imports Amcom.SDC.BaseServices
Imports Amcom.SDC.Speech.ReportBuilder

Public Class ChangeDBDialog
    Dim fieldToChange As String = ""
    Dim customer As String = ""

    Public Property changeField() As String
        Get
            Return fieldToChange
        End Get
        Set(ByVal value As String)
            fieldToChange = value
        End Set
    End Property

    Public Property changeCustomer() As String
        Get
            Return customer
        End Get
        Set(ByVal value As String)
            customer = value
        End Set
    End Property


    Private Sub ChangeDBDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.lblChanging.Text = fieldToChange
        Me.tbNewValue.Clear()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'Updates the database with a new value. 
        Dim configs As New ReportConfigReader

        'Create the new connection to the database. 
        Dim dbConn As New SqlClient.SqlConnection(App.ConnectionString("desk"))
        dbConn.Open()

        'Create the new database command
        Dim command As New SqlClient.SqlCommand()
        command.Connection = dbConn
        command.CommandText = "UPDATE [SpeechConfig] SET " & _
                                "[" & fieldToChange & " ] = '" & Me.tbNewValue.Text & "' " & _
                                "WHERE [CASLOC] = '" & customer & "'"

        'Execute the command
        Dim rowsAffected As Integer = 0
        Try
            rowsAffected = command.ExecuteNonQuery()
        Catch ex As OleDb.OleDbException
            MsgBox("There was an error trying to commit your changes: " & ex.Message, MsgBoxStyle.Exclamation, "Error commiting changes to database.")
        End Try

        'Check the rows returned and make sure our command worked. 
        If rowsAffected = 0 Then
            MsgBox("There was an issue attempting to update the database.", MsgBoxStyle.Exclamation, "Error updating database!")
        Else
            Me.Close()
        End If

    End Sub
End Class