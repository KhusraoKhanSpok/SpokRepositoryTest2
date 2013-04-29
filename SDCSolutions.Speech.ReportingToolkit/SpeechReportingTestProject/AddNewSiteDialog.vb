Imports Amcom.SDC.Speech.ReportBuilder
Public Class AddNewSiteDialog

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'User clicked ok. Make the new site.
        makeNewSiteEntry(Me.tbNewSite.Text)
        Me.Close()
    End Sub

    Private Sub makeNewSiteEntry(ByVal name As String)
        'Makes a new site entry. 
        Dim configs As New ReportConfigReader
        Dim is4Conn As New OleDb.OleDbConnection("Provider=SQLOLEDB;Server=" & configs.databaseSpeechServer & ";Database=" & configs.speechDataBase & ";USER ID=" & configs.dbSpeechUserName & ";Password=" & configs.dbSpeechPassword & ";")
        Dim sqlStatement As String

        sqlStatement = "SELECT * FROM [SDC_Config] WHERE directoryGroup = 'default'"

        is4Conn.Open()

        'Get the directory group for default. 
        Dim da As New OleDb.OleDbDataAdapter(sqlStatement, is4Conn)
        Dim ds As New DataSet
        da.Fill(ds)

        Dim row As DataRow 'The datarow. 

        With ds.Tables(0)
            If .Rows.Count = 0 Then
                'There was no default site. Something is wrong, but lets get the schema anyways. 
                da.FillSchema(ds, SchemaType.Source)


                row = .NewRow

                'Assign the new row from scratch
                row.Item("directoryGroup") = name
                row.Item("repDayOfWeek") = 5
                row.Item("repTimeOfDay") = 1
                row.Item("repMakeChannelReport") = False
                row.Item("repSmtpTo") = "mail@sdcsolutions.com"
                row.Item("repSmtpFrom") = "mail@sdcsolutions.com"
                row.Item("repSmtpHost") = "mailhost.speechCustomer.com"
                row.Item("repSmtpPort") = 25
                row.Item("repSmtpAddress") = "mail@sdcsolutions.com"
                row.Item("repSmtpSubject") = "IntelliSPEECH Reports"
                row.Item("repSmtpMessage") = "Please see attached reports."
                row.Item("repSmtpTimeout") = 20000
                row.Item("repSmtpUsername") = "Username"
                row.Item("repSmtpPassword") = "Password"
                row.Item("repReportPath") = "Path"
                row.Item("repReportPath") = "Path"

            Else

                'The default directory group exists. We can just copy over old settings. 
                row = .NewRow

                'Assign the new row from scratch
                row.Item("directoryGroup") = name
                row.Item("repDayOfWeek") = .Rows(0).Item("repDayOfWeek")
                row.Item("repTimeOfDay") = .Rows(0).Item("repTimeOfDay")
                row.Item("repMakeChannelReport") = .Rows(0).Item("repMakeChannelReport")
                row.Item("repSmtpTo") = .Rows(0).Item("repSmtpTo")
                row.Item("repSmtpFrom") = .Rows(0).Item("repSmtpFrom")
                row.Item("repSmtpHost") = .Rows(0).Item("repSmtpHost")
                row.Item("repSmtpPort") = .Rows(0).Item("repSmtpPort")
                row.Item("repSmtpAddress") = .Rows(0).Item("repSmtpAddress")
                row.Item("repSmtpSubject") = .Rows(0).Item("repSmtpSubject")
                row.Item("repSmtpMessage") = .Rows(0).Item("repSmtpMessage")
                row.Item("repSmtpTimeout") = .Rows(0).Item("repSmtpTimeout")
                row.Item("repSmtpUsername") = .Rows(0).Item("repSmtpUsername")
                row.Item("repSmtpPassword") = .Rows(0).Item("repSmtpPassword")
                row.Item("repReportPath") = .Rows(0).Item("repReportPath")
                row.Item("repRenderedPath") = .Rows(0).Item("repRenderedPath")



            End If

            .Rows.Add(row)

        End With

        Dim comm As New OleDb.OleDbCommandBuilder(da)

        da.Update(ds) 'Update the database with the new site. 

    End Sub
End Class