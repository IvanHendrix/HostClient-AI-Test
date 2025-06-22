using Logic.Player;
using UI;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private ConnectUI _connectUI;
    [SerializeField] private GameObject _mainUICamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        _connectUI.OnHostClick += OnStartHost;
        _connectUI.OnClientClick += OnStartClient;
    }

    public void OnShutDown()
    {
        NetworkManager.Singleton.Shutdown();
        
        PlayerRegistry.AllPlayers.Clear();
        
        _connectUI.gameObject.SetActive(true);
        _mainUICamera.SetActive(true);
        
        foreach (var netObj in FindObjectsOfType<NetworkObject>())
        {
            if (netObj.IsSpawned)
                netObj.Despawn(true);
        }
    }
    
    private void OnStartHost()
    {
        NetworkManager.Singleton.StartHost();
        _connectUI.gameObject.SetActive(false);
        _mainUICamera.SetActive(false);
    }

    private void OnStartClient()
    {
        NetworkManager.Singleton.StartClient();
        _connectUI.gameObject.SetActive(false);
        _mainUICamera.SetActive(false);
    }
}