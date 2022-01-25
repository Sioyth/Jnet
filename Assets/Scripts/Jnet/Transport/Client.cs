using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : Listener
{
    private ushort _port = 11001;
    private string _ip = "127.0.0.1";

    public bool Connect()
    {
        if (_socket == null)
        {
            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _socket.Blocking = false;
            _connections.Add(new Connection(_socket));
        }

        SocketAsyncEventArgs asyncEvent = new SocketAsyncEventArgs();
        asyncEvent.UserToken = _connections[0];
        asyncEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(_ip), 11000);
        asyncEvent.Completed += (s, e) =>
        {
            if (e.SocketError == SocketError.Success)
            {
                SendPacket("Heyo");
                Listen(_port, true);
                Debug.Log($"Client Connected to {_ip}");
            }
            else
                Debug.Log($"Client Couldn't connect to {_ip}");
        };

        try
        {
            _socket.ConnectAsync(asyncEvent);
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }

        return true;
    }

    ~Client()
    {
        _socket.Close();
    }

}
