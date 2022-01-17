using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client
{
    private static string _protocolID = "hash";
    private ushort _port = 11000;
    private string _ip = "127.0.0.1";
    private Socket _socket;
    private SocketAsyncEventArgs _event;

    public bool Connect()
    {

        // Create UDP Socket
        _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);

        // Set the socket into non-blocking mode 
        _socket.Blocking = false;
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(_ip), _port);

        _event = new SocketAsyncEventArgs();
        //_event.Completed += Callback;
       
        try 
        {
            _socket.Connect(ip);
            Debug.Log($"Connection was to {_ip} was sucessfull");
        } 
        catch(Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Couldn't connect Socket");
            return false;
        }


        //SendMessage(_protocolID + "Hello Server");
        SendPacket("Heyo");

        return true;
    }

    public void SendMessage(string msg)
    {
        try
        {
            if (_socket == null)
                return;

            // Encode the data string into a byte array.  
            byte[] buffer = Encoding.ASCII.GetBytes(msg);
            _event.SetBuffer(buffer, 0, buffer.Length);

            // Send the data through the socket.  
            _socket.SendAsync(_event);
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
            Debug.Log("Couldn't Send Message");
        }
    }

    public void SendPacket<T>(T t)
    {
        try
        {
            if (_socket == null)
                Debug.Log("Null socket");

            // Encode the data string into a byte array.  
            byte[] buffer = t.ToJsonBinary();
            _event.SetBuffer(buffer, 0, buffer.Length);

            // Send the data through the socket.  
            _socket.SendAsync(_event);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Couldn't Send Packet");
        }
    }

    public void SendPacket(string msg)
    {
        Packet packet = new Packet(_protocolID, msg);
        SendPacket<Packet>(packet);
    }

    ~Client()
    {
        _socket.Close();
    }

}
