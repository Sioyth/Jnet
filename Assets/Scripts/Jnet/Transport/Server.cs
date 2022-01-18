using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

// rename
public class Connection
{
    private Socket _socket;
    private JTimer _timeOutTimer;

    public Connection(Socket socket)
    {
        Socket = socket;
        _timeOutTimer = new JTimer(10);
    }

    public JTimer TimeOutTimer { get => _timeOutTimer; set => _timeOutTimer = value; }
    public Socket Socket { get => _socket; set => _socket = value; }
    public void Close()
    {
        _socket?.Shutdown(SocketShutdown.Both);
        _socket?.Close();
    }

    
}

public class Server : Listener
{
    private ushort _port = 11000;

    public void Start()
    {
        Listen(_port);
    }

    ~Server()
    {
        //Exit();
    }
}
