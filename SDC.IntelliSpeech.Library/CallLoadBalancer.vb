Imports Amcom.SDC.BaseServices

Public Class CallLoadBalancer
    Private mCalls As Dictionary(Of Integer, Queue(Of DialoutCall)) = Nothing
    Private mLastCampaign As Integer
    Private mLastIndex As Integer = 0
    Private mDB As SODSData
    Private mMaxretries As Integer = 0
    Public Sub New(ByVal db As SODSData, ByVal retryCount As Integer)
        mLastCampaign = -1
        mDB = db
        mCalls = New Dictionary(Of Integer, Queue(Of DialoutCall))
        mMaxretries = retryCount
    End Sub
    Public Property MaxRetries() As Integer
        Get
            Return mMaxretries
        End Get
        Set(ByVal value As Integer)
            mMaxretries = value
        End Set
    End Property
    Public Sub Add(ByVal c As DialoutCall)
        Select Case c.Status
            Case DialoutCall.CallStatus.inprocessRetry, DialoutCall.CallStatus.preprocess, DialoutCall.CallStatus.newcall
            Case Else
                App.TraceLog(TraceLevel.Info, "Call Not added, status is: " & c.Status.ToString)
                Exit Sub
        End Select
        App.TraceLog(TraceLevel.Info, "New call added to LoadBalancer: " & c.DisplayMessage)
        If Not mCalls.ContainsKey(c.CampaignID) Then
            Dim s As New Queue(Of DialoutCall)
            s.Enqueue(c)
            mCalls.Add(c.CampaignID, s)
        Else
            mCalls(c.CampaignID).Enqueue(c)
        End If
    End Sub
    Private Sub ReAdd(ByVal c As DialoutCall)
        If Not mCalls.ContainsKey(c.CampaignID) Then
            Dim s As New Queue(Of DialoutCall)
            s.Enqueue(c)
            mCalls.Add(c.CampaignID, s)
        Else
            mCalls(c.CampaignID).Enqueue(c)
        End If

    End Sub
    Public ReadOnly Property NextCall() As DialoutCall
        Get
            Dim campaign As Integer = -1
            'if nothing added, get out
            Dim found As Boolean = False
            Do
                If mCalls.Count = 0 Then Return Nothing

                If mLastCampaign = -1 Then
                    For Each campaign In mCalls.Keys
                        Exit For
                    Next
                Else
                    found = False
                    For Each campaign In mCalls.Keys
                        If found Then Exit Do
                        If campaign = mLastCampaign Then
                            found = True
                            Continue For
                        End If
                    Next
                End If
                'if we did not find a campaign, reset
                If campaign = -1 Then
                    mLastCampaign = -1
                End If
                If mCalls(campaign).Count = 0 Then
                    mCalls.Remove(campaign)
                Else
                    Exit Do
                End If
            Loop

            mLastCampaign = campaign
            Dim thecall As DialoutCall = mCalls(campaign).Dequeue

            If thecall.RetryCount > mMaxretries Then
                Return Nothing
            End If
            If thecall.RetryCount > 0 Then
                If Not thecall.ReadyToRetryDial Then
                    Me.ReAdd(thecall)
                    Return Nothing
                Else
                    App.TraceLog(TraceLevel.Info, "LoadBalancer: running call " & thecall.DBRecordID)
                    Return thecall
                End If
            Else
                If mDB.IsCallStillOpen(thecall) Then
                    App.TraceLog(TraceLevel.Info, "LoadBalancer: running call " & thecall.DBRecordID)
                    Return thecall
                End If
                App.TraceLog(TraceLevel.Info, "Removing call " & thecall.DisplayMessage)
                Return Nothing
            End If
            Return Nothing
        End Get
    End Property
End Class
