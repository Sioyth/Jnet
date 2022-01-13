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

    public struct Packet
    {
        public string msg;
    }

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
        Packet packet = new Packet()
        {
            msg = "Hello i'm a packete!"
        };

        byte[] bytes = packet.ToJsonBinary();
        Packet packet2 = bytes.FromJsonBinary<Packet>();
        Debug.Log(packet);
        //SendPacket<Packet>(packet);

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
                return;

            // Encode the data string into a byte array.  
            byte[] buffer = t.ToJsonBinary();
            _event.SetBuffer(buffer, 0, buffer.Length);
            
            
            Debug.Log(buffer.FromJsonBinary<T>());

            // Send the data through the socket.  
            _socket.SendAsync(_event);
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
            Debug.Log("Couldn't Send Packet");
        }
    }

    ~Client()
    {
        _socket.Close();
    }

}
