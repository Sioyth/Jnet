public class Server : Listener
{
    private ushort _port = 11000;

    public Server()
    {
    }

    public void Start()
    {
        Listen(_port);
    }

    ~Server()
    {
        //Exit();
    }
}
