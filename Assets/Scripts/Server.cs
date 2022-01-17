using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

// rename
public class ConnectedClient
{
    private Socket _socket;
    private JTimer _timeOutTimer;

    public ConnectedClient(Socket socket)
    {
        Socket = socket;
        _timeOutTimer = new JTimer(1);
    }

    public JTimer TimeOutTimer { get => _timeOutTimer; set => _timeOutTimer = value; }
    public Socket Socket { get => _socket; set => _socket = value; }
    public void Close()
    {
        _socket?.Shutdown(SocketShutdown.Both);
        _socket?.Close();
    }

    
}

public class Server
{
    private static string _protocolID = "hash";
    private static ushort _port = 11000;
    private Socket _listener;

    private SocketAsyncEventArgs _event;
    private List<ConnectedClient> _connections = new List<ConnectedClient>();

    public void Start()
    {
        Listen();
    }

    private bool Listen()
    {
        // Create Listener Socket
        _listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Set the socket into non-blocking mode 
        _listener.Blocking = false;

        try
        {
            _listener.Bind(new IPEndPoint(IPAddress.Any, _port));
            _event = new SocketAsyncEventArgs();
            _event.Completed += OnConnect;

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
        e.AcceptSocket = null;

        // clear buffer
        e.SetBuffer(new byte[1024], 0, 1024);

        if(!_listener.ReceiveAsync(e))
        {
            _listener.ReceiveAsync(e);
        }

    }
    private void OnConnect(object sender, SocketAsyncEventArgs e)
    {
        if (e.BytesTransferred > 0)
        {
            if (e.SocketError != SocketError.Success)
                Debug.Log("ERROR"); // TODO: Close Socket

            try
            {
                Packet p = e.Buffer.FromJsonBinary<Packet>();

                if (p._protocolID != _protocolID)
                    Debug.Log("Protocol Error");

                Debug.Log(p._msg);

                NewConnection(e.ConnectSocket);
                //StartListening(e);
                
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
            if (e.SocketError != SocketError.Success)
                Debug.Log("ERROR"); // TODO: Close Socket

            Packet p = e.Buffer.FromJsonBinary<Packet>();
            if (p._protocolID != _protocolID)
                Debug.Log("Wrong protocol!");

            Debug.Log("R - " + e.Buffer.FromJsonBinary<Packet>()._msg);

            // clear buffer
            e.SetBuffer(new byte[1024], 0, 1024);

            ((ConnectedClient)e.UserToken).TimeOutTimer.Start();
            ((ConnectedClient)e.UserToken).Socket.ReceiveAsync(e);
            
            
        }
    }

    private async void Timer(float lastTime)
    {
        float t = Time.time - lastTime;

        if (t > 10)
        {
            //TimeOut();
        }
    }

    //TODO do a BeginReceive
    private void OnReceive(SocketAsyncEventArgs e)
    {
        OnReceive(null, e);
    }

    private void BeginReceive(SocketAsyncEventArgs e)
    {
        
    }


    // Rename
    private void NewConnection(Socket socket)
    {
        Debug.Log("A client has connected");

        _connections.Add(new ConnectedClient(socket));
        SocketAsyncEventArgs s = new SocketAsyncEventArgs();

        s.AcceptSocket = _connections[_connections.Count - 1].Socket;
        s.SetBuffer(new byte[1024], 0, 1024);
        s.Completed += OnReceive;
        s.UserToken = _connections[_connections.Count - 1];

        _connections[_connections.Count - 1].TimeOutTimer.OnElapsedTime += ClientTimeOut;
        _connections[_connections.Count - 1].Socket.ReceiveAsync(s);

    }

    private void ClientTimeOut()
    {
        //Debug.Log(source.GetType().Name);
        //((Timer)source).Stop();
        Debug.Log("A client has disconnected");
        
        //_connections[index].Shutdown(SocketShutdown.Both);
        //_connections[index].Dispose();
        // _connections.Remove(index);
    }

    // Rename
    private void Exit()
    {
        _listener.Close();
    }

    ~Server()
    {
        Exit();
    }
}
