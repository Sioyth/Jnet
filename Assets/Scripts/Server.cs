using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
public class Server
{
    private static string _protocolID = "hash";
    private static ushort _port = 11000;
    private Socket _socket;

    private SocketAsyncEventArgs _event;
    public void Start()
    {
        Listen();
    }

    private bool Listen()
    {
        // Create UDP Socket
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Set the socket into non-blocking mode 
        _socket.Blocking = false;

        try
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, _port));
            _event = new SocketAsyncEventArgs();
            _event.Completed += ReceiveCallback;

            byte[] buffer = new byte[1024];
            _event.SetBuffer(buffer, 0, 1024);

            //while (_isUp && _isListening)
            {
                _socket.ReceiveAsync(_event);
            }

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }

        return true;
    }

    private void ReceiveCallback(object sender, SocketAsyncEventArgs e)
    {
        if (e.BytesTransferred > 0)
        {
            if(e.SocketError != SocketError.Success)
                return; // TODO: Close Socket

            //string data = Encoding.ASCII.GetString(e.Buffer, 0, e.BytesTransferred);
            //Debug.Log(data);

            Debug.Log(e.Buffer.FromJsonBinary<Packet>().msg);

            // You need to issue another ReceiveAsync, you can't just call ProcessReceive again
            if (!_socket.ReceiveAsync(e))
            {
                // Call completed synchonously
                ReceiveCallback(e);
            }
        }
    }

    private void ReceiveCallback(SocketAsyncEventArgs e)
    {
        ReceiveCallback(null, e);
    }

    // Rename
    private void Exit()
    {
        _socket.Close();
    }

    ~Server()
    {
        Exit();
    }
}
