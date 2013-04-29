Imports Amcom.SDC.BaseServices
Public Class CallLogPolicer
    'Class that contains functions that polices a call log before report data is acquired. 
    'Finds, Logs, and changes the call log entry in order to conform with reporting standards.
    Dim policer As New PoliceLog

    Function PoliceCalls(ByVal reportdata As DataSet) As DataSet
        'Master filter. Will filter calls for all filters this class is coded to look for, and report on them. 
        Dim ds As DataSet = reportdata

        ds = PoliceBlankCalls(ds)
        'ds = PoliceOperatorCalls(ds) 'Not the double pegger. 
        'ds = policeGhostTransfers(ds)
        'ds = policeBadOpTransfers(ds)
        'ds = policeGoodbye(ds)

        Return ds
    End Function

    Function PoliceBlankCalls(ByVal reportData As DataSet) As DataSet
        'Finds blank calls that did not peg anything, but have a call ID. 
        'CALL THIS FIRST.
        Dim ds As New DataSet
        ds = reportData

        Dim i As Integer = ds.Tables(0).Rows.Count - 1 'Set a counter.

        'Part of the SQL query we are emulating in order to take these calls out.      
        'AND (NOT call_duration = 0) AND (NOT(Status_Code = 0 AND AA_Refnum = 0 
        'AND Hangup = 0 AND Paged = 0 AND Transfered = 0)) 

        'For each of the rows in this dataset. 
        While i > -1
            'Look at the row, and see if it matches our criteria. 

            With ds.Tables(0).Rows(i)
                If .Item("Status_Code") = 0 And .Item("AA_Refnum") = 0 And .Item("Hangup") = 0 And .Item("Paged") = 0 And .Item("Transfered") = 0 And .Item("PA") = False Then
                    'Its a blank call entry. Log it and change it to a hangup. 
                    .Item("Hangup") = 1 'Set the hangup flag...
                    .Item("Status_Code") = 3000 'Set status code to bad call. 
                    'policer.makeReport(.Item("CASLOC"), "Found blank call with call ID: " & .Item("Call_ID") & ". Omitting call from report.")
                    'ds.Tables(0).Rows.RemoveAt(i) 'Remove the row from the dataset. 
                    App.TraceLog(TraceLevel.Warning, "CallLogPolicer modified call:" & .Item("Call_ID") & ". The call was handled as a hangup")

                End If

                i -= 1 'increment counter. 

            End With
        End While


        Return ds
    End Function

    Function PoliceOperatorCalls(ByVal reportdata As DataSet) As DataSet
        'Finds calls that transfered to operator for no apparent reason, and sets them to "Requested operator" 
        'CALL THIS SECOND
        Dim ds As New DataSet
        ds = reportdata

        Dim i As Integer = 0 'Set a counter.
        'The part of the SQL query we are emulating with an IF statement. This will find unknown operator calls. 

        'AND (NOT ((Status_code < 3000 OR status_code > 5000) 
        'AND (status_code <> 1002 OR status_code <> 1001) 
        'AND Transfered = 1 AND Operator = 1 AND Paged = 0 
        'AND AA_Refnum = -999))


        'For each of the rows in this dataset. 
        For i = 0 To ds.Tables(0).Rows.Count - 1
            'Look at the row, and see if it matches our criteria. 

            With ds.Tables(0).Rows(i)
                If .Item("Hangup") = False And .Item("Status_Code") < 3000 Then 'Completed Call - Stage 1 filter.
                    If .Item("Operator") = True And .Item("Transfered") = True And .Item("PA") = 0 Then 'Stage 2 filter - the call went to the operator.
                        If .Item("status_code") <> 1001 And .Item("Status_Code") <> 1002 Then 'Stage 3 filter - did the call go to the operator for a known reason?

                            'It's an unknown operator transfer. Set the pegs accordingly!
                            .Item("status_code") = 1001

                            policer.makeReport(.Item("directoryGroup"), "Found unknown operator transfer with call ID: " & .Item("Call_ID") & ". Setting pegs to 'Requested Operator'.")
                        End If

                    End If

                End If
            End With
        Next


        Return ds

    End Function

    Function policeGhostTransfers(ByVal reportdata As DataSet)
        'Finds calls that look like they transfered, but they did not peg correctly. 
        'CALL THIS THIRD. 
        Dim ds As New DataSet
        ds = reportdata

        Dim i As Integer = 0 'Set a counter.


        'For each of the rows in this dataset. 
        For i = 0 To ds.Tables(0).Rows.Count - 1
            'Look at the row, and see if it matches our criteria. 

            With ds.Tables(0).Rows(i)
                If .Item("Status_Code") = 0 And .Item("AA_Refnum") <> 0 And .Item("Hangup") = 0 And .Item("Paged") = 0 And .Item("Transfered") = 0 Then
                    'Its a blank call entry. Log it and change it to a hangup. 
                    .Item("hangup") = True 'Set the hangup flag...
                    .Item("operator") = False 'Set the hangup flag...
                    policer.makeReport(.Item("directoryGroup"), "Found 'Ghost Transfer' with ID: " & .Item("Call_ID") & ". Transfer was not pegged, so assuming unhandled hangup.")
                End If
            End With
        Next


        Return ds
    End Function

    Function policeBadOpTransfers(ByVal reportdata As DataSet)
        'Finds calls that pegged the operator status code, but hung up instead. Set them to a HANGUP only. 
        'CALL THIS FOURTH 
        Dim ds As New DataSet
        ds = reportdata

        Dim i As Integer = 0 'Set a counter.


        'For each of the rows in this dataset. 
        For i = 0 To ds.Tables(0).Rows.Count - 1
            'Look at the row, and see if it matches our criteria. 

            With ds.Tables(0).Rows(i)
                If .Item("Status_Code") = 1001 Or .Item("Status_Code") = 1002 And .Item("Hangup") = True And .Item("Transfered") = False Then                    'Its a blank call entry. Log it and change it to a hangup. 
                    .Item("Status_Code") = 4501 'Set the status code to something else.
                    policer.makeReport(.Item("directoryGroup"), "Found 'Hangup on Operator Transfer' with ID: " & .Item("Call_ID") & ". Caller hungup while transferring to operator. Setting pegs to hangup.")
                End If
            End With
        Next


        Return ds
    End Function

    Function policeGoodbye(ByVal reportdata As DataSet)
        'Finds calls that said 'goodbye' after doing nothing that we peg. We'll peg these as successfull transfers.
        'CALL THIS FIFTH 
        Dim ds As New DataSet
        ds = reportdata

        Dim i As Integer = 0 'Set a counter.


        'For each of the rows in this dataset. 
        For i = 0 To ds.Tables(0).Rows.Count - 1
            'Look at the row, and see if it matches our criteria. 

            With ds.Tables(0).Rows(i)
                If .Item("Operator") = False And .Item("Hangup") = False And .Item("Transfered") = False And .Item("Paged") = False Then                    'Its a blank call entry. Log it and change it to a hangup. 
                    .Item("Transfered") = True 'Set the status code to something else.
                    policer.makeReport(.Item("directoryGroup"), "Found 'User said Goodbye' with ID: " & .Item("Call_ID") & ". Callers said 'Goodbye' at the first prompt. Setting pegs to transfer!")
                End If
            End With
        Next


        Return ds
    End Function

End Class
