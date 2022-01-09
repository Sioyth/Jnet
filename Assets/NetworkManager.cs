using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    private Server _server = new Server();
    private Client _client = new Client();

    // Start is called before the first frame update
    void Start()
    {
        //_server = new Server();
        //_client = new Client();
    }

   public void Host()
   {
        _server.Start();
   }

    public void Connect()
    {
        _client.Connect();
    }

    public void SendMessage()
    {
        _client.SendMessage("Hello server");
    }
}
