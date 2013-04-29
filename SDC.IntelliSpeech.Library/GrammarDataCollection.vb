Imports System.Collections.ObjectModel
Public Class GrammarDataCollection
    Inherits Dictionary(Of String, GrammarData)
    Private mInterval As Integer
    Private mConnectionString As String
    Private mCompiler As String
    Private mCompilerOptions As String
    Private mOutputPath As String
    Public Sub New(ByVal connectionString As String, ByVal updateInterval As String, ByVal compiler As String, ByVal compilerOptions As String, ByVal outputPath As String)

        MyBase.new()
        mOutputPath = outputPath
        mCompiler = compiler
        mCompilerOptions = compilerOptions
        mConnectionString = connectionString
        mInterval = updateInterval
    End Sub
    Public ReadOnly Property CompilerPath() As String
        Get
            Return mCompiler
        End Get
    End Property
    Public ReadOnly Property CompilerOptions() As String
        Get
            Return mCompilerOptions
        End Get
    End Property
    Public ReadOnly Property ConnectionString() As String
        Get
            Return mConnectionString
        End Get
    End Property
    Public ReadOnly Property Interval() As Integer
        Get
            Return mInterval
        End Get
    End Property
    Public ReadOnly Property OutputPath() As String
        Get
            Return mOutputPath
        End Get
    End Property
End Class
