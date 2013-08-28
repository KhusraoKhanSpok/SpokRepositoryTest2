Imports Amcom.SDC.BaseServices
Public Class ReportConfigReader


    'Database variables
    Dim dbSpeechServer As String 'The SQL server the intellispeech 4 DB will reside on. 
    Dim dbDeskServer As String 'The SQL server the intelliDESK DB resides on. 
    Dim speechDb As String 'Intellispeech database we are to look at. 
    Dim deskDb As String 'The intellidesk database we are to look at. 
    Dim dbDeskPort As String 'The intellidesk port used to access the database. 
    Dim dbSpeechUser As String 'The username we use to gain access to the speech database. 
    Dim dbSpeechPass As String 'The password used to gain access to the speech database. 
    Dim dbDeskUser As String 'The Username for the desk database. 
    Dim dbDeskPass As String 'The Password for the desk database. 
    Dim dbSpeechPort As String 'The port used to access the speech port. 

    'Site variables
    Dim customerSites As New Collection 'Holds all of the sites that are in the reportconfig table of the database. 
    Dim currentSite As String 'Holds the current selected site, for access from other classes. 
    Dim maxNamesForSite As Integer 'Holds the maximum amount of licenced names for a customer. 

    'Customer name
    Dim customerName As String = "" 'Holds the name of the customer from the licence table. 
    Dim speechPorts As Integer = 0 'Holds the number of speech ports this customer has available. 
    Dim ensPorts As Integer = 0 'Holds the number of ENS ports this customer has available.
    Dim iSpeechMode As String = "SDC" ' Holds the mode that Ispeech is running in i.e SDC or XTEND

    'Report Variables
    Dim reportDirectory As String 'The location of the report directory holding our files. 
    Dim renderedDirectory As String 'The location to place reports that have been generated. 
    Dim channelReport As String 'Flag to trigger if we make a calls per channel report. 
    Dim dayToReport As Integer 'The day of the week to generate the reports. 
    Dim hourToReport As Integer 'The hour of the day to generate the reports.
    'Dim policeFile As String 'The location of the "police file" for a customer site. 

    'Email Variables
    Dim smtpServer As String 'The smtp server used to send e-mail. 
    Dim smtpPort As String 'The port we will use for SMTP access. 
    Dim toAddress As String 'The address we will be sending our reports to. 
    Dim fromAddress As String 'The address we will be sending reports from. 
    Dim mailMessage As String 'The text that will appear in the body of the message we will send. 
    Dim smtpSubject As String 'The subject that our email message will have.
    Dim smtpUser As String ' the username used to gain access to the SMTP server. 
    Dim smtpPass As String 'The password used to gain access to the SMTP server. 

    'Internal control variables for this class only
    Dim isLoaded As Boolean = False 'Will flip to true if we load the config.
    Dim errorMessage As String 'Is filled with an error message if we run into an error. 


    'Will need read only properties for all variables, so that we may access them properly. 

    'Database properties:
    Public ReadOnly Property databaseSpeechServer() As String
        Get
            Load() 'Loads the config if not already. 
            Return dbSpeechServer
        End Get
    End Property
    'Public ReadOnly Property policeFileLocation() As String
    '    Get
    '        Load() 'Loads the config if not already. 
    '        Return policeFile
    '    End Get
    'End Property
    Public ReadOnly Property databaseSpeechPort() As String
        Get
            Load() 'Loads the config if not already. 
            Return dbSpeechPort
        End Get
    End Property
    Public ReadOnly Property databaseDeskServer() As String
        Get
            Load() 'Loads the config if not already. 
            Return dbDeskServer
        End Get
    End Property
    Public ReadOnly Property databaseDeskPort() As String
        Get
            Load() 'Loads the config if not already. 
            Return dbDeskPort
        End Get
    End Property
    Public ReadOnly Property speechDataBase() As String
        Get
            Load() 'Loads the config if not already. 
            Return speechDb
        End Get
    End Property
    Public ReadOnly Property deskDataBase() As String
        Get
            Load() 'Loads the config if not already. 
            Return deskDb
        End Get
    End Property
    Public ReadOnly Property dbSpeechUserName() As String
        Get
            Load() 'Loads the config if not already. 
            Return dbSpeechUser
        End Get
    End Property
    Public ReadOnly Property dbSpeechPassword() As String
        Get
            Load() 'Loads the config if not already. 
            Return dbSpeechPass
        End Get
    End Property
    Public ReadOnly Property dbDeskUserName() As String
        Get
            Load() 'Loads the config if not already. 
            Return dbDeskUser
        End Get
    End Property
    Public ReadOnly Property dbDeskPassword() As String
        Get
            Load() 'Loads the config if not already. 
            Return dbDeskPass
        End Get
    End Property

    'Site property
    Public ReadOnly Property sites() As Collection
        Get
            Load() 'Loads the config if not already. 
            Return customerSites
        End Get
    End Property
    Public ReadOnly Property selectedSite() As String
        Get
            Load() 'Loads the config if not already. 
            Return currentSite
        End Get
    End Property

    'Properties for reporting and timing. 
    Public ReadOnly Property dayToRun() As String
        Get
            Load() 'Loads the config if not already. 
            Return dayToReport
        End Get
    End Property
    Public ReadOnly Property hourToRun() As String
        Get
            Load() 'Loads the config if not already. 
            Return hourToReport
        End Get
    End Property
    Public ReadOnly Property reportLocation() As String
        Get
            Load() 'Loads the config if not already. 
            Return reportDirectory
        End Get
    End Property
    Public ReadOnly Property renderedLocation() As String
        Get
            Load() 'Loads the config if not already. 
            Return renderedDirectory
        End Get
    End Property
    Public ReadOnly Property reportChannels() As Boolean
        Get
            Load() 'Loads the config if not already. 
            If LCase(channelReport) = "true" Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property Customer_Name() As String
        Get
            Return customerName
        End Get
    End Property

    'SMTP properties
    Public ReadOnly Property mailServer() As String
        Get
            Load() 'Loads the config if not already. 
            Return smtpServer
        End Get
    End Property
    Public ReadOnly Property mailPort() As String
        Get
            Load() 'Loads the config if not already. 
            Return smtpPort
        End Get
    End Property
    Public ReadOnly Property mailToAddress() As String
        Get
            Load() 'Loads the config if not already. 
            Return toAddress
        End Get
    End Property
    Public ReadOnly Property mailFromAddress() As String
        Get
            Load() 'Loads the config if not already. 
            Return fromAddress
        End Get
    End Property
    Public ReadOnly Property mailBody() As String
        Get
            Load() 'Loads the config if not already. 
            Return mailMessage
        End Get
    End Property
    Public ReadOnly Property mailSubject() As String
        Get
            Load() 'Loads the config if not already. 
            Return smtpSubject
        End Get
    End Property
    Public ReadOnly Property mailUser() As String
        Get
            Load() 'Loads the config if not already. 
            Return smtpUser
        End Get
    End Property
    Public ReadOnly Property mailPass() As String
        Get
            Load() 'Loads the config if not already. 
            Return smtpPass
        End Get
    End Property
    Public ReadOnly Property maxLicensedNames() As Integer
        Get
            Load()
            Return maxNamesForSite

        End Get
    End Property
    Public ReadOnly Property speechPortsAvail() As Integer
        Get
            Load()
            Return speechPorts
        End Get
    End Property
    Public ReadOnly Property ensPortsAvail() As Integer
        Get
            Load()
            Return ensPorts
        End Get
    End Property
    Public ReadOnly Property getIspeechMode() As String
        Get
            Load()
            Return iSpeechMode.Trim(" ")
        End Get
    End Property


    'Subroutines:
    Sub LoadConfig()
        'Parses the SDCconfig XML file and from that reads all applicable configuration settings from it. 
        App.TraceLog(TraceLevel.Verbose, "VERBOSE-LoadConfig(): Loading configuration settings...")
        Try
            'Dim configFilePath As String 'Contains the registry value with the path to the config file. 
            'Dim configFile As String 'Contains the reg value of the filename of the config file. 

            ''Get registry values
            'configFilePath = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\SDC Solutions\IntelliSPEECH\", "Config_Path", "FAIL")
            'configFile = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\SDC Solutions\IntelliSPEECH\", "Config_Report", "FAIL")
            'If configFilePath = "FAIL" Or configFile = "FAIL" Then
            '    Dim registryEx As New Exception("Could not find SDC Registry values pointing to config file.")
            '    Throw registryEx
            'End If

            ''Open XML file
            'Dim configXml As New Xml.XmlDocument()
            'Dim node As Xml.XmlNode

            'FileIO.FileSystem.CurrentDirectory = configFilePath

            'configXml.Load(configFile)

            'For each value, Xpath the data needed, and assign it to the appropriate variable. 

            'First find the server values and database connection string parts:

            'Find the IntelliDesk SQL server. 
            'node = configXml.SelectSingleNode("/CONFIG/DATA/SDCINTELLIDESK6_Server/Server")
            'dbDeskServer = node.InnerText

            ''Find the IntelliDesk SQL port #.
            'node = configXml.SelectSingleNode("/CONFIG/DATA/SDCINTELLIDESK6_Server/Port")
            'dbDeskPort = node.InnerText

            ''Find the desk database name
            'node = configXml.SelectSingleNode("/CONFIG/DATA/SDCINTELLIDESK6_Server/DataBaseName")
            'deskDb = node.InnerText

            ''Find the desk username.
            'node = configXml.SelectSingleNode("/CONFIG/DATA/SDCINTELLIDESK6_Server/Username")
            'dbDeskUser = node.InnerText

            ''Find the desk password
            'node = configXml.SelectSingleNode("/CONFIG/DATA/SDCINTELLIDESK6_Server/Password")
            'dbDeskPass = node.InnerText

            ''Find the intellispeech4 Server
            'node = configXml.SelectSingleNode("/CONFIG/DATA/INTELLISPEECH_Server/Server")
            'dbSpeechServer = node.InnerText

            ''Find the IntelliSpeechSQL port #.
            'node = configXml.SelectSingleNode("/CONFIG/DATA/INTELLISPEECH_Server/Port")
            'dbSpeechPort = node.InnerText

            ''Find the speech database name
            'node = configXml.SelectSingleNode("/CONFIG/DATA/INTELLISPEECH_Server/DataBaseName")
            'speechDb = node.InnerText

            ''Find the speech username
            'node = configXml.SelectSingleNode("/CONFIG/DATA/INTELLISPEECH_Server/Username")
            'dbSpeechUser = node.InnerText

            ''Find the speech password
            'node = configXml.SelectSingleNode("/CONFIG/DATA/INTELLISPEECH_Server/Password")
            'dbSpeechPass = node.InnerText

            isLoaded = True


            'Now grab the sites and put them in our sites collection
            Try
                Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("desk"))
                Dim sqlStatement As String

                sqlStatement = "SELECT [CASLOC] FROM [SpeechConfig]"

                is4Conn.Open()

                'Fill the config dataset with only the sites available.   
                Dim da As New SqlClient.SqlDataAdapter(sqlStatement, is4Conn)
                Dim configDs As New DataSet
                da.Fill(configDs, "configs")

                For Each site As DataRow In configDs.Tables(0).Rows 'for each site, make an entry. This property will be exposed to the outside world. 
                    customerSites.Add(site.Item(0))
                Next
            Catch ex As OleDb.OleDbException
                App.TraceLog(TraceLevel.Warning, "WARNING-LoadConfig(): Could not read from DB Config table: " & ex.Message)
            End Try


            'Close config file.


            App.TraceLog(TraceLevel.Verbose, "VERBOSE-LoadConfig(): ...Done")
        Catch ex As NullReferenceException
            errorMessage = ex.Message
            throwBadConfigEx(errorMessage)
        Catch ex As IO.FileNotFoundException
            errorMessage = ex.Message
            throwBadConfigEx(errorMessage)

            errorMessage = ex.Message
            throwBadConfigEx(errorMessage)
        Catch ex As Exception
            errorMessage = ex.Message
            throwBadConfigEx(errorMessage)


        Finally

        End Try
    End Sub

    Sub Load()
        'Checks if the config is loaded, and if not, loads the config. 
        If Not isLoaded Then
            LoadConfig()
        End If
    End Sub

    Sub LoadSiteConfig(ByVal customer As String)
        'Make a database connection based on what we have already from the original config. 
        'If there is no config table value, we do nothing and use the SDC_Config.xml as a default, which should already be loaded. 
        Try

            Load() 'If we haven't loaded our standard config yet, then do so. 

            'Make a connection string based on what we have. 

            Dim is4Conn As New SqlClient.SqlConnection(App.ConnectionString("desk"))
            Dim sqlStatement As String

            sqlStatement = "SELECT [CASLOC],[dirNamesLic],[prtSpeech],[prtENS],[companyName],[RepdayOfWeek],[ReptimeOfDay],[RepmakeChannelReport],[RepsmtpTo],[RepsmtpFrom]" & _
                            ",[RepsmtpHost],[RepsmtpPort],[RepsmtpAddress],[RepsmtpSubject],[RepsmtpMessage],[RepsmtpTimeOut]" & _
                            ",[RepsmtpUserName],[RepsmtpPassword],[RepreportPath],[ReprenderedPath],[appApplicationMode] " & _
                            "FROM [SpeechConfig] " & _
                            "WHERE [CASLOC] = '" & customer & "'"

            is4Conn.Open()

            'Fill the config dataset with stuff only for our customer.  
            Dim da As New SqlClient.SqlDataAdapter(sqlStatement, is4Conn)
            Dim configDs As New DataSet
            da.Fill(configDs, "configs")

            'Error check to make sure we actualy had a row returned. 
            If configDs.Tables(0).Rows.Count < 1 Then
                throwBadConfigEx("Site " & customer & " is not in the report config table, or is not found.")
            End If

            'Now that we have the dataset, assign our variables to what we need. 

            'Get the speech ports available
            speechPorts = configDs.Tables(0).Rows(0).Item("prtSpeech")

            'Get the ENS prts available
            ensPorts = configDs.Tables(0).Rows(0).Item("prtENS")

            'Find day to process
            dayToReport = configDs.Tables(0).Rows(0).Item("RepdayOfWeek")

            'Find hour to process
            hourToReport = configDs.Tables(0).Rows(0).Item("ReptimeOfDay")

            'Directory stuff - where can we find rdlc files, and where do we put the finished pdfs?
            'Find our report directory
            reportDirectory = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, "Report Files\")

            'Find where to place rendered reports
            renderedDirectory = configDs.Tables(0).Rows(0).Item("ReprenderedPath")

            'Report decision stuff: what reports shall we process?
            channelReport = configDs.Tables(0).Rows(0).Item("RepmakeChannelReport")

            'SMTP variables are filled here!
            'Find the smtp server
            smtpServer = configDs.Tables(0).Rows(0).Item("RepsmtpHost")

            'Find the smtp port
            smtpPort = configDs.Tables(0).Rows(0).Item("RepsmtpPort")

            'Find the address to send reports to.
            toAddress = configDs.Tables(0).Rows(0).Item("RepsmtpTo")

            'Find the address we send reports from.
            fromAddress = configDs.Tables(0).Rows(0).Item("RepsmtpFrom")

            'Find the message to place in the body of the e-mail message. 
            mailMessage = configDs.Tables(0).Rows(0).Item("RepsmtpMessage")

            'Find the subject for the e-mail message. 
            smtpSubject = configDs.Tables(0).Rows(0).Item("RepsmtpSubject")

            'Find the username to access SMTP
            smtpUser = configDs.Tables(0).Rows(0).Item("RepsmtpUserName")

            'Find the password to access SMTP
            smtpPass = configDs.Tables(0).Rows(0).Item("RepsmtpPassword")

            'Find the max number of directory names allowed for this customer. 
            maxNamesForSite = configDs.Tables(0).Rows(0).Item("dirNamesLic")

            'Get the business wide customer name - 
            customerName = configDs.Tables(0).Rows(0).Item("companyName")

            'Get the IspeechMode we're running
            iSpeechMode = configDs.Tables(0).Rows(0).Item("appApplicationMode")

            'Find the police file from the configs.
            ' -MS 3/10/2008 - hiding this in a static location with a static name. 
            'policeFile = My.Computer.FileSystem.SpecialDirectories.Temp & "\ReportDebugLog.txt"

            currentSite = customer 'Assign the currently selected site to the indicated customer.

            ''Run new query on license table to get customer name.
            'Dim speechCon As New SqlClient.SqlConnection(App.ConnectionString("speech"))
            'speechCon.Open()
            'sqlStatement = "SELECT [Customer_Name] FROM ASR_License"

            'configDs = New DataSet

            'da = New SqlClient.SqlDataAdapter(sqlStatement, speechCon)
            'da.Fill(configDs)

            'customerName = configDs.Tables(0).Rows(0).Item("Customer_Name")

            'speechCon.Close()

        Catch ex As Exception
            throwBadConfigEx("Loading config from DB Failed: " & ex.Message)
        End Try


    End Sub

    Sub throwBadConfigEx(ByVal message As String)
        'Throws a bad config exception back to the calling class. 
        Dim badConfig As New Amcom.SDC.BaseServices.AmcomException("Loading config failed: " & message)
        App.ExceptionLog(badConfig)
        App.TraceLog(TraceLevel.Error, "ERROR-LoadConfig(): Error loading config: " & badConfig.Message)
        Throw badConfig
    End Sub

End Class
