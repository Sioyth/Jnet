using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    private uint _id = 0;
    //TODO: change this to a better client handling id
    private uint _clientsCount = 0;
    private Server _server = new Server();
    private Client _client = new Client();
    private static NetworkManager _instance;

    [Header("Debug")]
    [SerializeField] private GameObject _networkingUI;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private InputField _inputField;

    private Dictionary<uint, GameObject> _connectedClients = new Dictionary<uint, GameObject>();
    public static NetworkManager Instance { get => _instance; set => _instance = value; }
    public uint Id { get => _id; }

    private NetworkManager()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("A NetworkManager is already instanced.");
    }

    public void Host()
    {
        _networkingUI.SetActive(false);
        _server.Start();
        _connectedClients.Add(0, Instantiate(_playerPrefab));

        _server.NewConnection += () =>
        {
            _clientsCount++;

            ExecuteOnMainThread.Instance.Execute( () => 
            { 
                GameObject player = Instantiate(_playerPrefab);
                player.GetComponent<NetworkObject>().SetOwner(_clientsCount);
                _connectedClients.Add(_clientsCount, player);
            });

            //send packet to assign clientID;
        };

    }

    public void Connect()
    {
        _networkingUI.SetActive(false);
        _client.Connect();
        _connectedClients.Add(0, Instantiate(_playerPrefab));
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
