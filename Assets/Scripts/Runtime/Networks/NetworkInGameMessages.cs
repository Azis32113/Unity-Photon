using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkInGameMessages : NetworkBehaviour
{
    public static NetworkInGameMessages Instance;

    [SerializeField] private InGameMessageUIHandler inGameMessageUIHandler;

    private void Awake() 
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SendInGameRPCMessage(string userNickname, string message)
    {
        RPC_InGameMessage($"<b>{userNickname}</b> {message}");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_InGameMessage(string message, RpcInfo info = default)
    {
        Debug.Log($"[RPC] InGameMessage {message}");

        inGameMessageUIHandler.OnGameMessageReceived(message);
    }
}



