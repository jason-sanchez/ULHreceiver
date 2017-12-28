Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic
Imports System.Diagnostics '20140920 added to handle process calls

'20140305 - increased buffer size from 16384
'20140325 - new procedure to check for chr(28) and chr(13) together as the last two characters of the file.
'20140331 - check for Wrapper and create file if wrapped correctly.
'20140331 - cjear the control characters
'20140411 - added use of Instr check to loop to remove exceptions on checking Mid$ function when string is null.
'20140413 - added connection check after loop
'20140414 - removed thread restart if no exception.
'20140414 - moved ACK datetime to update right before ACK sent.
'20140416 - added another clearleft to handle double tabs
'20140501 - added additional byte array code to receive messages
'20140501 - reconfigured the receive messages loop. Compiled as LoopReceiver27.
'20140813 - Mods to port 2203 for W3 Production Version.

'20140906 - TestRcvr version
'20140910 - save problem files. Renamed TestRcvr_1. 1846 - aded 10 ms delay. 1957 - changed buffer to 1024
'20140920 - cscitwlisten version from current version running in production. Includes Timer for bouncing feed. EMTRcvr.exe.
'20140921 - Add error logging to error file.
'20140923 - process problem along with copying it.
'20140924 - log record count on feed bounce. Increased outfile records to 900000. Log receiver start time.
'20140925 - Per Kelly. Change timer to 4 minutes.
'20140928 - put location variable in Recieve Messages to find where error is actually occuring. Process the problem file in addition to saving in problems directory.
'20140929 - added more location variables.
'20140930 - code to handle out of bounds error at location 12. Trim the string.
'20141001 - Receive Messages - if first character is CR, strip it off.

'20151006 - added text showing program start;removed problem file creation;chanded ack to wave3_prod vice test.
'20151221 - change ACK to KY1 Production.

Public Class Form1
    Public intStartCount As Integer = 0 '20140920
    Public intEndCount As Integer = 0 '20140920

    'Public objIniFile As New INIFile("d:\W3Production\HL7Receiver.ini") 'Prod
    Public objIniFile As New INIFile("C:\ULHTest\ULHHL7Receiver.ini") 'Test
    Public strInputDirectory As String = ""
    Public strProblemDirectory As String = "" '20140910
    Public counter As Long = 0
    Public port As Integer = objIniFile.GetInteger("Settings", "port", "(0)") ' 2203
    Private _server As Sockets.TcpListener
    Private _remoteIPEndPoint As New System.Net.IPEndPoint(System.Net.IPAddress.Any, 0)
    Private _threadReceive As Threading.Thread

    Dim CommServerListener As New TcpListener(IPAddress.Any, port) '20140813
    Dim Timer2 As New System.Timers.Timer(1000)

    '20140214 - add ackamsg string variable to send an updated string in the acknowledgement message.
    Dim strAckMessage As String = ""
    Dim strAckDTString As String = ""

    Dim boolFSdetected As Boolean = False
    Dim strLogDirectory As String = "" '20140921



    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        '20100223 - added code to stop the thread if someone closes application.
        stopThread()
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _server = Nothing
        strInputDirectory = objIniFile.GetString("Settings", "root", "(none)") ' d:\W3feeds\HL7
        strProblemDirectory = objIniFile.GetString("Settings", "problemfiles", "(none)") '20140910 - d:\problems

        port = objIniFile.GetInteger("Settings", "port", "(0)") ' 2203
        strLogDirectory = objIniFile.GetString("Settings", "logs", "(none)") '20140921

        txtPort.Text = port
        txtInputDirectory.Text = strInputDirectory

        ''20140214
        strAckDTString = calcDTString(DateTime.Now)
        'txtACK.Text = "MSH|^~\&|Wave3_Test|Systemax|Systemax|Systemax|" & strAckDTString & "||ACK^O01|" & strAckDTString & "10101|P|2.3|||||||||" & Chr(13)
        'txtACK.Text = txtACK.Text & "MSA|AA|0101010101010101|||||" & vbCrLf
        ''txtACK.Text = "MSH|^~\&|Wave3_Test|F80|Systemax|F80|20080125130256||ACK^O01|2008012513025688101|P|2.3|||||||||" & Chr(13)
        ''txtACK.Text = txtACK.Text & "MSA|AA|0801031513280001|||||" & vbCrLf

        '20100105 start connection when program starts
        txtPort.Enabled = False
        'txtACK.Enabled = False
        btnStart.Enabled = False
        btnStop.Enabled = True


        '20140920 add a timer to run the OnTimedEvent subroutine at a scheduled interval
        '1000 ms = 1 second
        '60000 ms = 1 minute
        '600000 ms = 10 minutes
        '1800000 ms = 30 minutes
        '3600000 ms = 1 hour
        intStartCount = 0

        Dim aTimer As System.Timers.Timer = New System.Timers.Timer(240000) '240000 = 4 minutes
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent
        aTimer.Enabled = True




        My.Application.DoEvents()
        startThread()
        writeToError("Receiver Started: " & DateTime.Now & vbCrLf) '20140924
        'txtStartStatus.Text = "Program Start: " & DateTime.Now
    End Sub

    Private Sub OnTimedEvent()
        intEndCount = CInt(txtCounter.Text) ' Get the count right now

        If intStartCount = intEndCount Then 'No Records returned over the interval so bounce the feed

            WriteToLog("Bounce the feed:" & vbCrLf)
            writeToError("Bounce the feed: " & txtCounter.Text & vbCrLf) '20140924
            My.Application.DoEvents()
            WriteToLog("[" & Now.ToString() & "] Receiver Stopped" & vbCrLf)
            writeToError("[" & Now.ToString() & "] Receiver Stopped" & vbCrLf) '20140921
            stopThread()

            My.Application.DoEvents()
            WriteToLog("[" & Now.ToString() & "] Receiver Started" & vbCrLf)
            writeToError("[" & Now.ToString() & "] Receiver Started" & vbCrLf) '20140921
            txtCounter.Text = "0"
            intStartCount = 0
            startThread()


        Else ' numbers are different so reset the strt count to what it is now for the next comparison.
            intStartCount = intEndCount


        End If

    End Sub
    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        txtPort.Enabled = False
        'txtACK.Enabled = False
        btnStart.Enabled = False
        btnStop.Enabled = True
        My.Application.DoEvents()
        txtCounter.Text = "0"
        intStartCount = 0
        WriteToLog("[" & Now.ToString() & "] Receiver Started" & vbCrLf)
        startThread()


    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        txtPort.Enabled = True
        'txtACK.Enabled = True
        btnStart.Enabled = True
        btnStop.Enabled = False
        My.Application.DoEvents()
        WriteToLog("[" & Now.ToString() & "] Receiver Stopped" & vbCrLf)
        stopThread()
    End Sub

    Private Sub ReceiveMessages(ByVal state As Object)
        '20140501 - reconfigured the receive messages loop. Compiled as LoopReceiver27.
        Dim stream As NetworkStream
        '20080417 - changed byte array to 9086 from 1024
        '20100105 - changed to 16384
        '20140305 - changed to 32768
        Dim bytes(1024) As Byte '20140501 '20140910 - changed buffer to 1024
        Dim data As String = Nothing
        Dim client As TcpClient = Nothing
        Dim i As Int32
        Dim strFileText As String = ""
        Dim location As Integer = 0 '20140928

        Dim connectionEstablished As Boolean = False
        Try
            While True


                'If Thread.CurrentThread.IsAlive Then
                'If _server.Pending() Then


                'WriteToLog("Connection Established = " & connectionEstablished & vbCrLf & vbCrLf)
                If Not connectionEstablished Then
                    client = _server.AcceptTcpClient()
                    stream = client.GetStream()
                    connectionEstablished = True


                End If

                stream = client.GetStream()
                WriteToLog(vbCrLf & "[" & Now.ToString() & "] Connection established with " & client.Client.RemoteEndPoint.ToString() & vbCrLf)
                WriteToLog("MESSAGE:" & vbCrLf)

                My.Application.DoEvents() '20140409 - added
                Threading.Thread.Sleep(10) '20140409 - added '201409101846 readded
                '=================================================================================================================================
                Do
                    'Dim bytes(32768) As Byte '20140501
                    location = 1 '20140928
                    i = stream.Read(bytes, 0, bytes.Length)


                    While (i <> 0)
                        location = 2 '20140928
                        If i <> 0 Then
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i)
                            WriteToLog(data)
                            strFileText = strFileText & data
                        End If

                        

                        If stream.DataAvailable Then
                            location = 3 '20140928
                            i = stream.Read(bytes, 0, bytes.Length)
                        Else
                            i = 0
                        End If



                    End While
                    location = 4 '20140928


                Loop Until InStr(strFileText, Chr(28)) > 0 'Mid$(strFileText, Len(strFileText) - 1, 1) <> Chr(28)
                '====================================================================================================================================
                strFileText = Trim(strFileText) ' 201409301046

                '20141001 - if the first character is a CR, chr(13) the strip it off
                If Mid$(strFileText, 1, 1) = Chr(13) Then
                    strFileText = Mid$(strFileText, 2)
                Else
                    strFileText = strFileText
                End If

                boolFSdetected = True

                '20140414 - moved strAckDTString to here. 
                strAckDTString = calcDTString(DateTime.Now)

                My.Application.DoEvents()
                location = 10 '20140929
                Dim fileArray() As String = Split(strFileText, Chr(13))
                location = 11 '20140929
                Dim tempLineArray() As String = Split(fileArray(0), "|")
                location = 12 '20140929

                '20140930 ======================================================
                'code to handle out of bounds error at location 12.
                Dim tempLineCount As Integer = tempLineArray.Length
                Dim MessageControlID As String = ""

                If tempLineCount > 10 Then
                    MessageControlID = tempLineArray(9)
                Else
                    MessageControlID = "0000000"
                End If
                '===============================================================

                location = 13 '20140929
                Dim strMSHValue As String = tempLineArray(0)
                location = 50 '20140929
                '20151006 - changed ack message to wave3_prod vice wave3_test
                '20151221 - changed ACK to KY1 Production
                strAckMessage = "MSH|^~\&|ULH|Systemax|Systemax|Systemax|" & strAckDTString & "||ACK^O01|" & MessageControlID & "|P|2.3|||||||||" & Chr(13)

                If Mid$(strFileText, Len(strFileText) - 1, 1) = Chr(28) And Mid$(strFileText, 1, 1) = Chr(11) Then
                    strAckMessage = strAckMessage & "MSA|AA|" & MessageControlID & "|||||" & vbCrLf
                    strAckMessage = strAckMessage & Mid$(strFileText, Len(strFileText) - 2, 1) & vbCrLf


                    WriteToLog(vbCrLf & "Length: " & Len(strFileText) & vbCrLf)

                    strFileText = clearLeft(strFileText)
                    strFileText = clearRight(strFileText)
                    strFileText = clearLeft(strFileText) '20140416 added another clearleft to handle double tabs

                    counter += 1
                    CreateOutputFile(strFileText)

                    location = 60 '20140929
                    data = ""
                    strFileText = ""
                    Dim msg As Byte() = System.Text.Encoding.ASCII.GetBytes(Chr(11) & strAckMessage & Chr(28) & Chr(13))
                    stream.Write(msg, 0, msg.Length)

                    location = 70 '20140929

                    WriteToLog(vbCrLf & vbCrLf & strAckMessage & vbCrLf) ' 20140328 added
                    WriteToLog(vbCrLf & "--End of message" & vbCrLf & vbCrLf)

                Else
                    location = 80 '20140929

                    ''20140401 - bad message dont ack
                    'WriteToLog(vbCrLf & "--Message Incomplete - Resend..." & vbCrLf & vbCrLf)
                    'data = ""
                    'strFileText = ""

                    '20140906 - add code below
                    strAckMessage = strAckMessage & "MSA|AA|" & MessageControlID & "|||||" & vbCrLf
                    strAckMessage = strAckMessage & Mid$(strFileText, Len(strFileText) - 2, 1) & vbCrLf

                    WriteToLog(vbCrLf & "Length: " & Len(strFileText) & vbCrLf)

                    '20151006 - removed create problem file code
                    'CreateProblemFile(strFileText) '20140910. 20140923 moved to above character clearing

                    strFileText = clearLeft(strFileText) '20140923 added
                    strFileText = clearRight(strFileText) '20140923 added
                    strFileText = clearLeft(strFileText) '20140416 added another clearleft to handle double tabs. '20140923 added

                    counter += 1 '20140928 added
                    CreateOutputFile(strFileText) '20140928 added

                    'CreateProblemFile(strFileText) '20140910.


                    data = ""
                    strFileText = ""

                    location = 90 '20140929

                    Dim msg As Byte() = System.Text.Encoding.ASCII.GetBytes(Chr(11) & strAckMessage & Chr(28) & Chr(13))
                    stream.Write(msg, 0, msg.Length)

                    WriteToLog(vbCrLf & vbCrLf & strAckMessage & vbCrLf) ' 20140328 added
                    WriteToLog(vbCrLf & "--End of message" & vbCrLf & vbCrLf)
                End If

                '201404013 - added connection check
                If client.Connected Then
                    connectionEstablished = True
                    WriteToLog(vbCrLf & "Connection Open." & vbCrLf & vbCrLf)
                Else
                    connectionEstablished = False
                    WriteToLog(vbCrLf & "Connection Closed." & vbCrLf & vbCrLf)
                End If

                'stream.Close()
                'client.Close()
                'connectionEstablished = True
                'Else
                ' suspend the thread so the application doesn't
                ' consume the cpu by constantly looping...

                '20140418 - added thread sleep to prevent CPU lockup.


                'End If
                'End If
            End While


        Catch tEx As ThreadAbortException
            ' Ignore this type of error since we manually stopped the thread
            writeToException("[" & Now.ToString() & "] Thread Abort Exception:" & vbCrLf & tEx.Message & vbCrLf) '20140921
        Catch ex As Exception
            WriteToLog(vbCrLf & vbCrLf & "***ERROR (ReceiveMessages): " & ex.ToString() & vbCrLf)
            writeToException(vbCrLf & vbCrLf & "***ERROR (ReceiveMessages): " & ex.ToString() & vbCrLf)
            writeToException("I = " & i.ToString & vbCrLf)
            writeToException("bytes.length = " & CStr(bytes.Length) & vbCrLf)
            writeToException("location = " & location.ToString & vbCrLf) '20140928 added
            '20100105 added code below to restart on error

            'stopThread() '20140921
            My.Application.DoEvents()
            'startThread() '20140921
            My.Application.DoEvents()

        Finally

            '20140413 - removed thread restart if no error.
            'stopThread()
            'My.Application.DoEvents()
            'startThread()
            'My.Application.DoEvents()

        End Try
    End Sub

    ' Thread-safe way of writing to the log
    Delegate Sub WriteToLogCallback(ByVal LogText As String)

    Private Sub WriteToLog(ByVal LogText As String)

        If Me.txtLog.InvokeRequired Then
            Dim del As New WriteToLogCallback(AddressOf WriteToLog)
            Me.Invoke(del, New Object() {LogText})
        Else
            If Len(Me.txtLog.Text) > 64000 Then Me.txtLog.Text = ""

            Me.txtLog.Text &= LogText
            '20120806 - put latest message at the top
            'Me.txtLog.Text = LogText & Me.txtLog.Text & vbCrLf
            Me.txtCounter.Text = CStr(counter)
        End If


    End Sub
    Public Sub startThread()
        Try
            'Dim localAddr As IPAddress = IPAddress.Parse(txtboxIPAddress.Text)

            '_server = New TcpListener(localAddr, txtPort.Text)
            '_server.Start()

            '_threadReceive = New Threading.Thread(AddressOf ReceiveMessages)
            '_threadReceive.Start()
            _server = New Sockets.TcpListener(IPAddress.Any, txtPort.Text)
            _server.Start()

            _threadReceive = New Threading.Thread(AddressOf ReceiveMessages)
            _threadReceive.Start()
            txtStartStatus.Text = DateTime.Now

        Catch ex As Exception
            'WriteToLog(vbCrLf & "***ERROR (starting service): " & ex.ToString())
        End Try
    End Sub
    Public Sub stopThread()
        Try
            _server.Stop()
            _threadReceive.Abort()
        Catch ex As Exception
            'WriteToLog(vbCrLf & "***ERROR (stopping service): " & ex.ToString())
        End Try
    End Sub
    Public Sub CreateProblemFile(ByVal strLTWOutput As String)
        '20140910 save files that don't process properly. Uses code from createoutputfile subroutine.
        Dim line As String = ""
        Dim objTStreamCounter As Object
        Dim intCounter As Integer = 0

        Dim filename As String
        Dim objTStreamOutput As Object

        'If the file does not exist, create it.
        ' strInputDirectory = c:\feeds
        If Not File.Exists(strProblemDirectory & "\problemCounter.txt") Then
            objTStreamCounter = File.CreateText(strProblemDirectory & "\problemCounter.txt")
            objTStreamCounter.WriteLine("000")
            objTStreamCounter.Close()
        End If


        objTStreamCounter = New StreamReader(strProblemDirectory & "\problemCounter.txt")
        line = objTStreamCounter.readline
        intCounter = CInt(line)
        intCounter = intCounter + 1
        If intCounter >= 100000 Then intCounter = 0
        objTStreamCounter.Close()

        'write the  file to "d:\problems" a new HL7 file is created
        filename = strProblemDirectory & "\HL7." & padleft(Str(intCounter), 3)
        objTStreamOutput = File.AppendText(filename)
        objTStreamOutput.Writeline(strLTWOutput)
        objTStreamOutput.Close()

        'update the counter file
        objTStreamCounter = New StreamWriter(strProblemDirectory & "\problemCounter.txt")
        objTStreamCounter.WriteLine(padleft(Str(intCounter), 3))
        objTStreamCounter.Close()
    End Sub

    Public Sub CreateOutputFile(ByVal strLTWOutput As String)
        'Function to create an HL7 output file

        Dim line As String = ""
        Dim objTStreamCounter As Object
        Dim intCounter As Integer = 0

        Dim filename As String
        Dim objTStreamOutput As Object

        'If the file does not exist, create it.
        ' strInputDirectory = c:\feeds
        If Not File.Exists(strInputDirectory & "\RcvCounter.txt") Then
            objTStreamCounter = File.CreateText(strInputDirectory & "\RcvCounter.txt")
            objTStreamCounter.WriteLine("000")
            objTStreamCounter.Close()
        End If

        'read the present file number for counter.Txt. convert it to an integer and increment it.
        'reset counter after 1000 records
        'objTStreamCounter = New StreamReader("c:\feeds\Rcvcounter.txt")
        objTStreamCounter = New StreamReader(strInputDirectory & "\RcvCounter.txt")
        line = objTStreamCounter.readline
        intCounter = CInt(line)
        intCounter = intCounter + 1
        If intCounter >= 900000 Then intCounter = 0
        objTStreamCounter.Close()

        'write the  file to "c:\feeds" a new HL7 file is created
        filename = strInputDirectory & "\HL7." & padleft(Str(intCounter), 3)
        objTStreamOutput = File.AppendText(filename)
        objTStreamOutput.Writeline(strLTWOutput)
        objTStreamOutput.Close()

        'update the counter file
        objTStreamCounter = New StreamWriter(strInputDirectory & "\RcvCounter.txt")
        objTStreamCounter.WriteLine(padleft(Str(intCounter), 3))
        objTStreamCounter.Close()
    End Sub

    Public Function padleft(ByRef inputStr As String, ByRef strLength As Short) As String
        'pad an input string with zeros based on desired strLength
        Dim varLength As Short
        Dim strOutput As String
        Dim i As Short


        strOutput = ""
        varLength = Len(Trim(inputStr))
        For i = 1 To ((strLength - varLength))
            strOutput = strOutput & "0"
        Next
        strOutput = strOutput & Trim(inputStr)
        padleft = strOutput


    End Function
    Public Function clearLeft(ByRef inputStr As String) As String
        Dim strOutput As String
        If Mid$(inputStr, 1, 1) = Chr(11) Then 'if the first character is a VT, chr(11) the strip it off
            strOutput = Mid$(inputStr, 2)
        Else
            strOutput = inputStr
        End If
        clearLeft = strOutput
    End Function
    Public Function clearRight(ByRef inputStr As String) As String
        Dim strOutput As String
        Dim reverseStr As String

        reverseStr = StrReverse(inputStr)

        If Mid$(reverseStr, 1, 1) = Chr(13) Then
            strOutput = Mid$(reverseStr, 2)
        Else
            strOutput = reverseStr
        End If
        reverseStr = strOutput

        If Mid$(reverseStr, 1, 1) = Chr(28) Then
            strOutput = Mid$(reverseStr, 2)
        Else
            strOutput = reverseStr
        End If
        clearRight = StrReverse(strOutput)
    End Function
    Public Function clearRight2(ByRef inputStr As String) As String
        '20140325 - new procedure to check for chr(28) and chr(13) together as the last two characters of the file.
        Dim strOutput As String = ""
        Dim reverseStr As String

        reverseStr = StrReverse(inputStr)
        If Mid$(reverseStr, 1, 1) = Chr(13) And Mid$(reverseStr, 2, 1) = Chr(28) Then
            strOutput = Mid$(reverseStr, 3)
        Else
            strOutput = reverseStr
        End If
        clearRight2 = StrReverse(strOutput)
    End Function


    Private Sub btnClearText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearText.Click
        Me.txtLog.Text = ""
        '20121213 - clear button now resets program counter to zero and sets the count display to "0"
        Me.txtCounter.Text = "0"
        counter = 0
    End Sub

    Public Function calcDTString(ByVal dTime As DateTime) As String
        calcDTString = ""
        Dim strYear As String = Year(dTime)
        Dim strMonth As String = Month(dTime)
        Dim strDay As String = Day(dTime)
        Dim strHour As String = Hour(dTime)
        Dim strMinute As String = Minute(dTime)
        Dim strSecond As String = Second(dTime)

        If Len(strMonth) = 1 Then
            strMonth = "0" & strMonth
        End If

        If Len(strDay) = 1 Then
            strDay = "0" & strDay
        End If

        If Len(strHour) = 1 Then
            strHour = "0" & strHour
        End If

        If Len(strMinute) = 1 Then
            strMinute = "0" & strMinute
        End If

        If Len(strSecond) = 1 Then
            strSecond = "0" & strSecond
        End If

        If IsDate(dTime) Then
            calcDTString = strYear & strMonth & strDay & strHour & strMinute & strSecond
        End If
    End Function


    Public Sub writeToError(ByVal strMsg As String)
        '20140205 - use a text file to log errors instead of the event log
        Dim file As System.IO.StreamWriter
        Dim tempLogFileName As String = strLogDirectory & "ULHReceiver_log.txt"
        file = My.Computer.FileSystem.OpenTextFileWriter(tempLogFileName, True)
        file.WriteLine(DateTime.Now & " : " & strMsg)
        file.Close()
    End Sub

    Public Sub writeToException(ByVal strMsg As String)
        '20140205 - use a text file to log errors instead of the event log
        Dim file As System.IO.StreamWriter
        Dim tempLogFileName As String = strLogDirectory & "ULHReceiver_Exception_log.txt"
        file = My.Computer.FileSystem.OpenTextFileWriter(tempLogFileName, True)
        file.WriteLine(DateTime.Now & " : " & strMsg)
        file.Close()
    End Sub
End Class
