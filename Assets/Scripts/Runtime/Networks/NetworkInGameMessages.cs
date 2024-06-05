using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class NetworkInGameMessages : NetworkBehaviour
{
    public static NetworkInGameMessages Instance;

    [SerializeField] private InGameMessageUIHandler inGameMessageUIHandler;

    public override void Spawned()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SendInGameRPCMessage(string userNickname, string message)
    {
        RPC_InGameMessage($"[{DateTime.Now:HH:mm:ss}] <b>{userNickname}</b> {message}");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_InGameMessage(string message, RpcInfo info = default)
    {
        Debug.Log($"[RPC] InGameMessage {message}");

        inGameMessageUIHandler.OnGameMessageReceived(message);
    }
}



