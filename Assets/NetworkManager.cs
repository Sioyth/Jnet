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
        //_client.SendMessage(_inputField.text);
        JTimer timer = new JTimer(10);
        timer.OnElapsedTime += () =>
        {
            Debug.Log("Time has passed");
        };
        timer.Start();
    }

    public void SendPacket()
    {
        _client.SendPacket(_inputField.text);
    }
}
