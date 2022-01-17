using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
public class Server
{
    private static string _protocolID = "hash";
    private static ushort _port = 11000;
    private Socket _socket;

    private SocketAsyncEventArgs _event;
    private List<Socket> _connections = new List<Socket>();
    
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
            _event.Completed += OnConnect;

            byte[] buffer = new byte[1024];
            _event.SetBuffer(buffer, 0, 1024);
            
            //_socket.ReceiveAsync(_event);
            StartListening(_event);
            

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }

        return true;
    }

    private void StartListening(SocketAsyncEventArgs e)
    {
        //e.AcceptSocket = null;
        _socket.ReceiveAsync(e);
        
    }
    private void OnConnect(object sender, SocketAsyncEventArgs e)
    {
        if (e.BytesTransferred > 0)
        {
            if (e.SocketError != SocketError.Success)
                Debug.Log("ERROR"); // TODO: Close Socket

            //string data = Encoding.ASCII.GetString(e.Buffer, 0, e.BytesTransferred);
            //Debug.Log(data);

            try
            {
                Packet p = e.Buffer.FromJsonBinary<Packet>();

                // clear buffer
                byte[] buffer = new byte[1024];
                _event.SetBuffer(buffer, 0, 1024);

                if (p._protocolID != _protocolID)
                    Debug.Log("Protocol Error");

                //_connections.Add(e.ConnectSocket);
                //SocketAsyncEventArgs s = new SocketAsyncEventArgs();
                //s.AcceptSocket = e.AcceptSocket;
                //s.Completed += OnReceive;
                //_connections[_connections.Count - 1].ReceiveAsync(e);

                Debug.Log("Connect:" + p._msg);

           
                if (!_socket.ReceiveAsync(e))
                {
                    Debug.Log("sync");
                    OnConnect(null, e);
                }

            }
            catch (Exception e2)
            {
                Debug.LogException(e2);
            }

        }
        else
        {
            Debug.Log("No data");
        }
    }

    private void OnReceive(object sender, SocketAsyncEventArgs e)
    {
        if (e.BytesTransferred > 0)
        {
            if(e.SocketError != SocketError.Success)
                Debug.Log("ERROR"); // TODO: Close Socket

            //string data = Encoding.ASCII.GetString(e.Buffer, 0, e.BytesTransferred);
            //Debug.Log(data);

            Packet p = e.Buffer.FromJsonBinary<Packet>();
            if (p._protocolID != _protocolID)
                Debug.Log("Wrong protocol!");
            
            Debug.Log("Receive" + e.Buffer.FromJsonBinary<Packet>()._msg);
            if (!_socket.ReceiveAsync(e))
            {
                // Call completed synchonously
                OnReceive(e);
            }
        }
    }

    private void OnReceive(SocketAsyncEventArgs e)
    {
        OnReceive(null, e);
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
