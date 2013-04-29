Public Class PoliceLog
    Dim configs As New ReportConfigReader

    Sub makeReport(ByVal site As String, ByVal report As String)

        configs.LoadSiteConfig(site) 'Select the customer we are making this report for. 

        If Not FileIO.FileSystem.FileExists(configs.policeFileLocation) Then
            'Starting a new file. 
            FileIO.FileSystem.WriteAllText(configs.policeFileLocation, Now & ": " & report & vbCrLf, False)
        Else
            FileIO.FileSystem.WriteAllText(configs.policeFileLocation, Now & ": " & report & vbCrLf, True)
        End If

    End Sub
End Class
