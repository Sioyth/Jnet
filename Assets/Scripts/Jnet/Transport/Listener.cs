using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public abstract class Listener
{

    private ushort _port;
    protected Socket _socket;
    private SocketAsyncEventArgs _event;

    protected Action _newConnection;
    protected Action _connectionTimeOut;
    protected List<Connection> _connections = new List<Connection>();

    #region Listen
    public void Listen(ushort port, bool skipConnect = false)
    {
        _port = port;
        try
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Blocking = false;
            _socket.Bind(new IPEndPoint(IPAddress.Any, _port));

            _event = new SocketAsyncEventArgs();

            if(skipConnect)
                _event.Completed += OnPacketReceived;
            else
                _event.Completed += OnConnect;

            StartListening(_event);

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void StartListening(SocketAsyncEventArgs e)
    {
        // Clear the accepted socket
        e.AcceptSocket = null;

        // Setup / clear buffer
        e.SetBuffer(new byte[1024], 0, 1024);

        if (!_socket.ReceiveAsync(e))
            _socket.ReceiveAsync(e);
    }
    #endregion

    #region New Connections
    private void OnConnect(object sender, SocketAsyncEventArgs e)
    {
        if (e.BytesTransferred > 0)
        {
            if (e.SocketError != SocketError.Success)
                Debug.Log("ERROR"); // TODO: Close Socket

            try
            {
                OnNewConnection(e);
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

    private void OnNewConnection(SocketAsyncEventArgs e)
    {
        Debug.Log("A client has connected");
        _newConnection?.Invoke();
        _connections.Add(new Connection(e.ConnectSocket));

        SocketAsyncEventArgs s = e;
        s.AcceptSocket = _connections[_connections.Count - 1].Socket;
        s.Completed += OnPacketReceived;
        s.UserToken = _connections[_connections.Count - 1];

        _connections[_connections.Count - 1].TimeOutTimer.OnElapsedTime += OnClientTimeOut;
        OnPacketReceived(null, s);
    }

    //protected void OnNewConnection(object sender, SocketAsyncEventArgs e)
    //{
    //    OnNewConnection(e);
    //}
    #endregion

    private void OnPacketReceived(object sender, SocketAsyncEventArgs e)
    {
        if (e.BytesTransferred > 0)
        {
            if (e.SocketError != SocketError.Success)
                Debug.Log("ERROR"); // TODO: Close Socket

            Packet p = e.Buffer.FromJsonBinary<Packet>();
            Debug.Log("R - " + e.Buffer.FromJsonBinary<Packet>()._msg);

            // clear buffer
            e.SetBuffer(new byte[1024], 0, 1024);
            //_onPacketReceived.Invoke();

            //((Connection)e.UserToken).TimeOutTimer.Start();
            ((Connection)e.UserToken).Socket.ReceiveAsync(e);
        }
    }

    private void OnClientTimeOut()
    {
        _connectionTimeOut?.Invoke();
        Debug.Log("A client has disconnected");

        //_connections[index].Shutdown(SocketShutdown.Both);
        //_connections[index].Dispose();
        // _connections.Remove(index);
    }

    private void Send(byte[] data, int id = 0)
    {
        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        e.SetBuffer(data, 0, data.Length);
        _connections[id]?.Socket.SendAsync(e);
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
}
