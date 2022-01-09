using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client
{
    private ushort _port = 11000;
    private string _ip = "127.0.0.1";
    private Socket _socket;

    public bool Connect()
    {

        // Create UDP Socket
        _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        if (_socket == null)
            return false;

        // Set the socket into non-blocking mode 
        _socket.Blocking = false;
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(_ip), _port);

        try 
        {
            _socket.Connect(ip);
        } 
        catch(Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Couldn't connect Socket");
            return false;
        }

        Debug.Log($"Connection was to {_ip} was sucessfull");

        SendMessage("Hello Server");

        return true;
    }

    public void SendMessage(string s)
    {
        try
        {
            if (_socket == null)
                return;

            // Encode the data string into a byte array.  
            byte[] msg = Encoding.ASCII.GetBytes(s);

            // Send the data through the socket.  
            int bytesSent = _socket.Send(msg);
        }
        catch (SocketException e)
        {
            Debug.LogException(e);
            Debug.Log("Couldn't Send Message");
        }
    }

    ~Client()
    {
        _socket.Close();
    }

}
