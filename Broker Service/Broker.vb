Imports Broker_Service

Module Broker
    Private WithEvents server As New ServerListener
    Private connectedClients As New SortedSet(Of ServerClient)
    Enum logTypes As Short
        xNormal = 0
        xInfo = 1
        xWarning = 2
        xError = 3
        xCritical = 4
        xSecurity = 5
    End Enum

    Enum messageTypes As Byte
        normal = 0
        image = 1
    End Enum

    Const brokerPort As Integer = 6112
    Const brokerIP As String = "127.0.0.1"

    Sub Main()
        server.Listen(brokerPort)
    End Sub

    Private Sub sendToServer(ByRef type As Byte, ByRef message As String)
        Select Case DirectCast(type, messageTypes)
            Case messageTypes.normal
                'Send a normal message here
            Case messageTypes.image
                'Send an image 
        End Select
    End Sub

    Private Sub logIt(ByRef type As Short, ByRef message As String)
        Select Case DirectCast(type, logTypes)
            Case logTypes.xNormal
                'Logging Stuff Here
            Case logTypes.xInfo
                'Logging Stuff Here
            Case logTypes.xWarning
                'Logging Stuff Here
            Case logTypes.xError
                'Logging Stuff Here
            Case logTypes.xCritical
                'Logging Stuff Here
            Case logTypes.xSecurity
                'Logging Stuff Here
        End Select
    End Sub

    Private Sub server_ClientStateChanged(sender As ServerListener, client As ServerClient, connected As Boolean) Handles server.ClientStateChanged
        Try
            If connected Then
                connectedClients.Add(client)
                logIt(logTypes.xInfo, "A Scraper has connected to the server.")
            Else
                If connectedClients.Contains(client) Then
                    connectedClients.Remove(client)
                    logIt(logTypes.xInfo, "A Scraper has disconnected from the server.")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub server_ClientReadPacket(sender As ServerListener, client As ServerClient, data() As Byte) Handles server.ClientReadPacket
        Dim byteList As List(Of Byte) = data.ToList
        Dim messageIdentifier As Byte = byteList(0)
        byteList.RemoveAt(0)
        data = byteList.ToArray
        Dim message As String = Text.Encoding.Unicode.GetString(data)
        Select Case DirectCast(messageIdentifier, messageTypes)
            Case messageTypes.normal
                'Handle packet we've received here!
            Case messageTypes.image
                'Handle base64 image here!
        End Select
    End Sub

End Module
