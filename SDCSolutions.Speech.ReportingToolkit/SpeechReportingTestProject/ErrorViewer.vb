Public Class ErrorViewer
    Dim errorList As New Collection
    Private Sub ErrorViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        errorList = SpeechReportma.reportErrors
        errorDisplay.Text = ""
        For Each problem As String In errorList
            errorDisplay.Text = errorDisplay.Text & problem & vbCrLf & vbCrLf 'Put a line between each error. 
        Next
    End Sub
End Class