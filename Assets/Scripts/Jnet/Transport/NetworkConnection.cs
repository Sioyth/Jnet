using System.Net.Sockets;

// rename
public class NetworkConnection
{
    private Socket _socket;
    private object _userHandle;
    private JTimer _timeOutTimer;

    public NetworkConnection(Socket socket, object userHandle = null)
    {
        _socket = socket;
        _userHandle = userHandle;
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
