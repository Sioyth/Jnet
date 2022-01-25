using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    private int _id = 0;
    private Server _server = new Server();
    private Client _client = new Client();
    private static NetworkManager _instance;

    [SerializeField] private GameObject _networkingUI;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private InputField _inputField;

    private Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    public static NetworkManager Instance { get => _instance; set => _instance = value; }

    private NetworkManager()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("A NetworkManager is already instanced.");
    }

    public void Host()
    {
        //_networkingUI.SetActive(false);
        _server.Start();
        _server.NewConnection += () =>
        {
            ExecuteOnMainThread.Instance.Execute(() => _players.Add(_id++, Instantiate(_playerPrefab)));
        };

    }

    public void Connect()
    {
        _networkingUI.SetActive(false);
        _client.Connect();
        _players.Add(0, Instantiate(_playerPrefab));
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

    public IEnumerator SpawnPlayer()
    {
        GameObject player;
        player = Instantiate(_playerPrefab);
        Debug.Log("Working");
        yield return null;
    }
}
