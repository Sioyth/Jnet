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

        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        e.UserToken = _connections[0];
        e.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
        e.Completed += (s, e2) =>
        {
            if (e.SocketError == SocketError.Success)
            {
                SendPacket("Heyo");
                Listen(_port, true);
                //_socket.Bind(new IPEndPoint(IPAddress.Parse("192.168.2.168"), _port));
                //Debug.Log($"Client Connected to {_ip}");
            }
            else
                Debug.Log($"Client Couldn't connect to {_ip}");
        };

        _connections[0].Socket.Connect(new IPEndPoint(IPAddress.Parse(_ip), 11000));
        Listen(_port, true);


        return true;
    }

    public void SendPacket<T>(T t)
    {
        try
        {
            if (_socket == null)
                Debug.Log("Null socket");

            //Encode the data string into a byte array.  
            byte[] buffer = t.ToJsonBinary();
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(buffer, 0, buffer.Length);

            //Send the data through the socket.  
            _connections[0].Socket.SendAsync(e);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Couldn't Send Packet");
        }
    }

    public void SendPacket(string msg)
    {
        Packet packet = new Packet("", msg);
        SendPacket<Packet>(packet);
    }

    ~Client()
    {
        _socket.Close();
    }

}
