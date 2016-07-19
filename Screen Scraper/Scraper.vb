Imports Screen_Scraper

Module Scraper
    Private WithEvents client As New UserClient

    Enum warningTypes As Short
        xNormal = 0
        xInfo = 1
        xWarning = 2
        xError = 3
        xCritical = 4
        xSecurity = 5
    End Enum
    Enum messageTypes As Byte
        normal = 0
        request = 1
    End Enum

    Const brokerPort As Integer = 6112
    Const brokerIP As String = "127.0.0.1"

    Sub Main()
        While client.Connected = False
            Try
                client.Connect(brokerIP, brokerPort)
            Catch ex As Exception

            End Try
        End While
    End Sub



    Private Sub logIt(ByRef type As Short, ByRef message As String)
        Select Case DirectCast(type, warningTypes)
            Case warningTypes.xNormal
                'Logging Stuff Here
            Case warningTypes.xInfo
                'Logging Stuff Here
            Case warningTypes.xWarning
                'Logging Stuff Here
            Case warningTypes.xError
                'Logging Stuff Here
            Case warningTypes.xCritical
                'Logging Stuff Here
            Case warningTypes.xSecurity
                'Logging Stuff Here
        End Select
    End Sub

    Private Sub client_ReadPacket(sender As UserClient, data() As Byte) Handles client.ReadPacket
        Dim byteList As List(Of Byte) = data.ToList
        Dim messageIdentifier As Byte = byteList(0)
        byteList.RemoveAt(0)
        data = byteList.ToArray
        Dim message As String = Text.Encoding.Unicode.GetString(data)
        Select Case DirectCast(messageIdentifier, messageTypes)
            Case messageTypes.normal
                'Handle packet we've received here!
            Case messageTypes.request
                'Handle base64 image here!
        End Select
    End Sub

End Module
