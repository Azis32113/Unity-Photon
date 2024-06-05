using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPlayer playerPrefab;
    [SerializeField] private WaveManager wavePrefab;
    [SerializeField] private WaterManager waterPrefab;
    [SerializeField] private NetworkInGameMessages networkInGameMessagesPrefab;

    // Mapping between token id and re-created players
    private Dictionary<int, NetworkPlayer> mapTokenIDWithNetworkPlayer;
    
    private WaveManager spawnedWaveManager = default;
    private WaterManager spawnedWaterManager = default;
    private NetworkInGameMessages spawnedNetworkInGameMessages = default;

    private void Awake() {
        mapTokenIDWithNetworkPlayer = new Dictionary<int, NetworkPlayer>();
    }

    private int GetPlayerToken(NetworkRunner runner, PlayerRef player)
    {
        if (runner.LocalPlayer == player)
        {
            // Just use the local player connection token
            return ConnectionTokenUtils.HashToken(GameManager.Instance.GetConnectionToken());
        }
        else
        {
            // Get the connection token stored when the client connects to this Host
            var token = runner.GetPlayerConnectionToken(player);
            if (token != null) return ConnectionTokenUtils.HashToken(token);
            else Debug.LogError("GetPlayerToken return invalid token");
            return 0;
        }
    }

    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("OnConnectedToServer"); }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Debug.Log("OnConnectFailed"); }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Debug.Log("OnConnectRequest"); }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { Debug.Log("OnCustomAuthenticationResponse"); }

    public void OnDisconnectedFromServer(NetworkRunner runner) { Debug.Log("OnDisconnectFromServer"); }

    public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) 
    {
        Debug.Log("OnHostMigration");

        // shut down the current runner
        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        // find the network runner handler and start the host migration
        NetworkRunnerHandler.Instance.StartHostMigtration(hostMigrationToken);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) { 
        if (NetworkPlayer.Local != null)
        {
            input.Set(CharacterInputHandler.GetNetworkInput());
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { Debug.Log("OnInputMissing"); }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            if (spawnedWaveManager == null)
            {
                spawnedWaveManager = runner.Spawn(wavePrefab);
            }

            if (spawnedWaterManager == null)
            {
                spawnedWaterManager = runner.Spawn(waterPrefab);
            }

            if (spawnedNetworkInGameMessages == null)
            {
                spawnedNetworkInGameMessages = runner.Spawn(networkInGameMessagesPrefab);
            }
            
            // Get the token for the player
            int playerToken = GetPlayerToken(runner, player);           

            Debug.Log($"OnPlayerJoined, we are server. Connection token {playerToken}");

            if (mapTokenIDWithNetworkPlayer.TryGetValue(playerToken, out var networkPlayer))
            {
                Debug.Log($"Found old connection token, token: {playerToken}. Assigning controls to that player");

                networkPlayer.GetComponent<NetworkObject>().AssignInputAuthority(player);
            }
            else 
            {
                Debug.Log($"Spawning new player connection token, token: {playerToken}");
                NetworkPlayer spawnedNetworkPlayer = runner.Spawn(playerPrefab, Utils.GetRandomPosition(), Quaternion.identity, player);

                // Store the token for the player
                spawnedNetworkPlayer.token = playerToken;

                // Store the mapping between playerToken and the spawned network player
                mapTokenIDWithNetworkPlayer.Add(playerToken, spawnedNetworkPlayer);
            }            
        }
        
        else Debug.Log("OnPlayerJoined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { Debug.Log("OnPlayerLeft"); }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { Debug.Log("OnReliableDataReceived"); }

    public void OnSceneLoadDone(NetworkRunner runner) { Debug.Log("OnSceneLoadDone"); }

    public void OnSceneLoadStart(NetworkRunner runner) { Debug.Log("OnSceneLoadStart"); }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { Debug.Log("OnSessionListUpdated"); }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Debug.Log("OnShutdown"); }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { Debug.Log("OnUserSimulationMessage"); }

    public void OnHostMigrationCleanUp()
    {
        Debug.Log("Spawner OnHostMigrationCleanUp Started...");

        // Checking Player Clone
        foreach (var entry in mapTokenIDWithNetworkPlayer)
        {
            NetworkObject networkObjectInDictionary = entry.Value.GetComponent<NetworkObject>();
            
            if (networkObjectInDictionary.InputAuthority.IsNone)
            {
                Debug.Log($"{Time.time} Found player that has not reconnected. Despawning {entry.Value.nickName}");

                networkObjectInDictionary.Runner.Despawn(networkObjectInDictionary);
            }
        }

        Debug.Log("Spawner OnHostMigrationCleanUp Completed");
    }

    public void SetConnectionTokenMapping(int token, NetworkPlayer networkPlayer)
    {
        mapTokenIDWithNetworkPlayer.Add(token, networkPlayer);
    }

    public void RemoveConnectionTokenMapping(int token)
    {
        mapTokenIDWithNetworkPlayer.Remove(token);
    }

    public void SetConnectionWaveManager(WaveManager waveManager)
    {
        spawnedWaveManager = waveManager;
    }

    public void SetConnectionWaterManager(WaterManager waterManager)
    {
        spawnedWaterManager = waterManager;
    }


}
