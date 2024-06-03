using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SimulationBehaviour, IBeforeUpdate, INetworkRunnerCallbacks
{
    NetInput accumulatedInput; 
    bool resetInput;


    public void BeforeUpdate() 
    {
        if (resetInput)
        {
            resetInput = false;
            accumulatedInput = default;
        }

        NetworkButtons buttons = default;

        float direction = 0;
        if (Input.GetKey(KeyCode.A)) direction -= 1; 
        if (Input.GetKey(KeyCode.D)) direction += 1; 

        accumulatedInput.Horizontal = direction;
        buttons.Set(InputButton.Forward, Input.GetKey(KeyCode.W));
        buttons.Set(InputButton.Backward, Input.GetKey(KeyCode.S));

        accumulatedInput.Buttons = new NetworkButtons(accumulatedInput.Buttons.Bits | buttons.Bits);
    }

    public void OnConnectedToServer(NetworkRunner runner){ }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){ }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){ }

    public void OnDisconnectedFromServer(NetworkRunner runner){ }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){ }
    
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        input.Set(accumulatedInput);
        resetInput = true;
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){ }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    { 
        if (player == runner.LocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){ }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){ }

    public void OnSceneLoadDone(NetworkRunner runner){ }

    public void OnSceneLoadStart(NetworkRunner runner){ }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){ }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    { 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){ }
}