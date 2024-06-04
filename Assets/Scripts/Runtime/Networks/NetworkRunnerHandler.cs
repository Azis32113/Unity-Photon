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

        var clientTask = InitializeNetworkRunner(_runner, gameMode, GameManager.Instance.GetConnectionToken(), NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, sessionName, null);

        Debug.Log($"Server NetworkRunner Started");
    }

    public void StartHostMigtration(HostMigrationToken hostMigrationToken)
    {
        _runner = Instantiate(_networkRunnerPrefab);
        _runner.name = "Network Runner - Migrated";

        var clientTask = InitializeNetworkRunnerHostMigration(_runner, hostMigrationToken);
        
        Debug.Log($"Host Migration Started");
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, byte[] connectionToken, NetAddress address, SceneRef scene, string sessionName, Action<NetworkRunner> initialized)
    {
        INetworkSceneManager sceneManager = GetSceneManager(runner);

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            Initialized = initialized,
            SceneManager = sceneManager,
            ConnectionToken = connectionToken
        });
    }

    protected virtual Task InitializeNetworkRunnerHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        INetworkSceneManager sceneManager = GetSceneManager(runner);

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            // GameMode = gameMode,
            // Address = address,
            // Scene = scene,
            // SessionName = sessionName,
            // Initialized = initialized,
            SceneManager = sceneManager,
            HostMigrationToken = hostMigrationToken, // contains all necessary info to restart the Runner
            HostMigrationResume = HostMigrationResume, // this will be invoked to resume the simulation
            ConnectionToken = GameManager.Instance.GetConnectionToken()
        });
    }

    private void HostMigrationResume(NetworkRunner runner) 
    {
        Debug.Log("HostMigrationResume started");

        // get a reference for each Network Object from the old Host
        foreach (var resumeNetworkObject in runner.GetResumeSnapshotNetworkObjects())
        {
            // grab all player objects, movement component
            if (resumeNetworkObject.TryGetBehaviour<PlayerMovementHandler>(out var movementHandler))
            {
                runner.Spawn(resumeNetworkObject, position: movementHandler.transform.position, rotation: movementHandler.transform.rotation, onBeforeSpawned: (runner, newNetworkObject) => 
                {
                    newNetworkObject.CopyStateFrom(resumeNetworkObject);

                    // Map the connection token with the new Network player
                    if (resumeNetworkObject.TryGetBehaviour<NetworkPlayer>(out var oldNetworkPlayer))
                    {
                        // Store player token for reconnection
                        FindObjectOfType<Spawner>().SetConnectionTokenMapping(oldNetworkPlayer.token, newNetworkObject.GetComponent<NetworkPlayer>());
                    }
                });
            }
        }

        Debug.Log("HostMigrationResume Completed");
    }

    private INetworkSceneManager GetSceneManager(NetworkRunner runner)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            // handle networked objects that already exists in the scene
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        return sceneManager;
    }



    
}