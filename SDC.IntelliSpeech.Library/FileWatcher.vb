Imports System.IO
Imports Amcom.SDC.BaseServices
Public Class FileWatcher
    Private WithEvents mFW As IO.FileSystemWatcher


    Public Event FileCreated(ByVal FullPath As String)
    Public Event FileDeleted(ByVal FilePath As String)
    Public Event FileChanged(ByVal FullPath As String)
    Public Event FileRenamed(ByVal OldFileName As String, ByVal newFileName As String)

    Public Sub New()
        Initialize()
    End Sub
    Public Sub New(ByVal watchFolder As String, ByVal startWatching As Boolean)
        Initialize()

        Me.FolderToMonitor = watchFolder
        Me.StartWatch()
    End Sub
    Public Sub New(ByVal watchFolder As String)
        Initialize()
        Me.FolderToMonitor = watchFolder
    End Sub
    Public Sub Initialize()
        mFW = New IO.FileSystemWatcher()
        'Watch all changes
        mFW.NotifyFilter = IO.NotifyFilters.Attributes Or _
                                           IO.NotifyFilters.CreationTime Or _
                                           IO.NotifyFilters.DirectoryName Or _
                                           IO.NotifyFilters.FileName Or _
                                           IO.NotifyFilters.LastWrite Or _
                                           IO.NotifyFilters.Security Or _
                                           IO.NotifyFilters.Size
        'Can also use io.notifyfilter.LastAccess but
        'that raises event every time file is accessed.
        'just looking for changes in this example
    End Sub

    Public Property FolderToMonitor() As String 'Folder to monitor
        'Simple Demo so I am only allowing
        'one folder to be monitored at a time


        Get
            FolderToMonitor = mFW.Path
        End Get
        Set(ByVal Value As String)
            If Right(Value, 1) <> "\" Then Value = Value & "\"
            If IO.Directory.Exists(Value) Then
                mFW.Path = Value
            End If
        End Set
    End Property
    Public Property IncludeSubfolders() As Boolean
        Get
            IncludeSubfolders = mFW.IncludeSubdirectories
        End Get
        Set(ByVal Value As Boolean)
            mFW.IncludeSubdirectories = Value
        End Set
    End Property


    Public Function StartWatch() As Boolean
        App.TraceLog(TraceLevel.Info, "Starting File Watcher")
        Dim bAns As Boolean = False
        Try
            mFW.EnableRaisingEvents = True
            bAns = True
        Catch ex As Exception
        End Try
        Return bAns
    End Function

    Public Function StopWatch() As Boolean
        App.TraceLog(TraceLevel.Info, "Stopping File Watcher")
        Dim bAns As Boolean = False
        Try
            mFW.EnableRaisingEvents = False
            bAns = True
        Catch ex As Exception
        End Try
        Return bAns
    End Function

    'WithEvents Keyword automatically gives you the
    'signature for these subs


    Private Sub OnCreated(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles mFW.Created
        RaiseEvent FileCreated(e.FullPath)
    End Sub

    Private Sub OnChanged(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles mFW.Changed
        RaiseEvent FileChanged(e.FullPath)

    End Sub

    Private Sub OnRenamed(ByVal sender As Object, ByVal e As System.IO.RenamedEventArgs) Handles mFW.Renamed
        RaiseEvent FileRenamed(e.OldFullPath, e.FullPath)
    End Sub

    Private Sub OnDeleted(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles mFW.Deleted
        RaiseEvent FileDeleted(e.FullPath)
    End Sub
End Class
