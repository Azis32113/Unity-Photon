using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour
{
    public static NetworkRunnerHandler Instance { get; private set;}
    [SerializeField] private NetworkRunner _networkRunnerPrefab;
    private NetworkRunner _runner;

    private void Awake() {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start() {
        _runner = Instantiate(_networkRunnerPrefab);
        _runner.name = "Network Runner";
        
        
        
        Enum.TryParse(PlayerPrefs.GetString(Constants.ServerData.GAME_MODE), out GameMode gameMode);
        string sessionName = PlayerPrefs.GetString(Constants.ServerData.SESSION_NAME);

        var clientTask = InitializeNetworkRunner(_runner, gameMode, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, sessionName, null);
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, string sessionName, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            // handle networked objects that already exists in the scene
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs{
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }
}