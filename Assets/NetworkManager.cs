using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    private Server _server = new Server();
    private Client _client = new Client();

    [SerializeField] private InputField _inputField;

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
        _client.SendMessage(_inputField.text);
    }

    public void SendPacket()
    {
        //_client.SendPacket<Packet>(new Packet("hash", _inputField.text));
        _client.SendPacket(_inputField.text);
    }
}
