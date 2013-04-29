Imports System.Diagnostics
Imports System.io
Imports System.Threading
Imports Amcom.SDC.BaseServices

Public Class CompileFinishedEventArgs
    Inherits EventArgs
    Private mExitCode As Integer
    Private mRuntime As String
    Private mErrorMessage As String
    Private mKey As String
    Public Sub New(ByVal key As String, ByVal exitCode As Integer, ByVal runtime As String, ByVal errMessage As String)
        mKey = key
        mExitCode = exitCode
        mRuntime = runtime
        mErrorMessage = errMessage
    End Sub
    Public ReadOnly Property Key() As String
        Get
            Return mKey
        End Get
    End Property
    Public ReadOnly Property ExitCode() As Integer
        Get
            Return mExitCode
        End Get
    End Property
    Public ReadOnly Property RunningTime() As String
        Get
            Return mRuntime
        End Get
    End Property
    Public ReadOnly Property ErrorMessage() As String
        Get
            Return mErrorMessage
        End Get
    End Property
End Class

Public Class ProcessGrammar
    Private mPath As String
    Private mOpts As String
    Private mFile As String
    Private mSourcefile As String
    Private mOutPath As String
    Private mKey As String
    Public Event Finished As EventHandler(Of CompileFinishedEventArgs)
    Public Delegate Sub FinishedDelegate(ByVal sender As Object, ByVal e As CompileFinishedEventArgs)
    Public Sub New(ByVal key As String, ByVal compilerPath As String, ByVal compilerOptions As String, ByVal sourceFile As String, ByVal outputPath As String)
        mKey = key
        mPath = compilerPath
        mOpts = compilerOptions
        mSourcefile = sourceFile
        mOutPath = outputPath
    End Sub
   
    Public Sub DoCompile()
        App.TraceLog(TraceLevel.Verbose, "Starting Compile...")
        ' create a new process
        Dim file As String = mSourcefile.Replace("/", "\")
        If Not My.Computer.FileSystem.FileExists(mPath) Then
            App.TraceLog(TraceLevel.Error, "Complier not found at path " & mPath)
            RaiseEvent Finished(Me, New CompileFinishedEventArgs(mKey, -1, "", "Complier not found at path " & mPath))
            Exit Sub
        End If
        App.TraceLog(TraceLevel.Verbose, "Compile Command line: " & IO.Path.GetFileName(mPath) & " " & mOpts & " " & file)

        'Create Process Start information
        Dim processStartInfo As New ProcessStartInfo(mPath, mOpts & " " & file)
        processStartInfo.WorkingDirectory = IO.Path.GetDirectoryName(mPath)
        processStartInfo.ErrorDialog = False
        processStartInfo.UseShellExecute = False
        processStartInfo.RedirectStandardError = True
        processStartInfo.RedirectStandardInput = True
        processStartInfo.RedirectStandardOutput = True

        'Execute the process
        Dim myProcess As New Process
        Dim outputReader As StreamReader = Nothing
        Dim errorReader As StreamReader = Nothing
        Try
            myProcess.StartInfo = processStartInfo
            myProcess.StartInfo.UseShellExecute = False
            Dim processStarted As Boolean = myProcess.Start
            If processStarted Then
                'Get the output stream
                outputReader = myProcess.StandardOutput
                errorReader = myProcess.StandardError
                Dim result As String = outputReader.ReadToEnd
                If result.Contains(vbLf) Then
                    result = result.Replace(vbCr, "")
                    For Each item As String In result.Split(vbLf)
                        App.TraceLog(TraceLevel.Verbose, "Complier: " & item)
                    Next
                Else
                    App.TraceLog(TraceLevel.Verbose, "Compiler: " & result)
                End If
            Else
                App.TraceLog(TraceLevel.Error, "Compiler process failed to start")
            End If
        Catch ex As Exception
            RaiseEvent Finished(Me, New CompileFinishedEventArgs(mKey, -1, "", "Complier Process Exception: " & ex.Message))
            Exit Sub
        End Try
        Dim duration As Integer = Math.Abs(DateDiff(DateInterval.Second, myProcess.StartTime, myProcess.ExitTime))
        Dim exitCode As Integer = myProcess.ExitCode
        myProcess.Close()

        App.TraceLog(TraceLevel.Info, "Compile finished.")
        Dim outFile As String = IO.Path.GetFileNameWithoutExtension(mSourcefile) & ".gram"
        Dim gramFile As String = IO.Path.Combine(IO.Path.GetDirectoryName(mPath), outFile)
        If Not My.Computer.FileSystem.FileExists(gramFile) Then
            RaiseEvent Finished(Me, New CompileFinishedEventArgs(mKey, -2, "", "Complier did not produce output file " & gramFile & "!"))
            Exit Sub
        End If
        Try
            mOutPath = mOutPath.Replace("/", "\")
            My.Computer.FileSystem.CopyFile(gramFile, IO.Path.Combine(mOutPath, IO.Path.GetFileNameWithoutExtension(mSourcefile) & "-new.gram"), True)
            App.TraceLog(TraceLevel.Verbose, "Copied grammar file to path " & IO.Path.Combine(mOutPath, "directory-new.gram"))
        Catch ex As Exception
            RaiseEvent Finished(Me, New CompileFinishedEventArgs(mKey, -3, "", "Failed to copy Grammar file to directory " & mOutPath & ":  " & ex.Message))
            Exit Sub
        End Try
        RaiseEvent Finished(Me, New CompileFinishedEventArgs(mKey, 0, "Completed in " & CStr(duration) & " seconds", ""))

    End Sub
End Class
