using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using System.Net;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }

    [SerializeField] TextMeshProUGUI playerNameTMP; 
    [SerializeField] Transform camTarget;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    [HideInInspector] public NetworkString<_16> nickName { get; set; }

    [Networked] public int token {get; set; }

    private bool isPublicJoinMessageSent = false;

    public override void Spawned()
    {
        if (HasInputAuthority) 
        {
            Local = this;

            CameraFollow.Instance.SetTarget(camTarget);

            RPC_SetNickName(PlayerPrefs.GetString(Constants.LocalData.PLAYER_NICK_NAME));

            Debug.Log($"Spawned Local Player");
        }
        else 
        {
            // Set the player as a player object
            Runner.SetPlayerObject(Object.InputAuthority, Object);

            Debug.Log($"Spawned Remote Player");
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            if (Runner.TryGetPlayerObject(player, out var playerLeftMetworkObject)) 
            {
                if (playerLeftMetworkObject == Object) 
                {
                    Debug.Log("Client logout");
                    NetworkInGameMessages.Instance.SendInGameRPCMessage(playerLeftMetworkObject.GetComponent<NetworkPlayer>().nickName.ToString(), "left");
                    FindObjectOfType<Spawner>().RemoveConnectionTokenMapping(token);
                }
            }
        }

        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    private static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnNicknameChanged value {changed.Behaviour.nickName}");
        changed.Behaviour.OnNickNameChanged();
    }

    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");
        playerNameTMP.text = nickName.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickName {nickName}");
        this.nickName = nickName;

        if (!isPublicJoinMessageSent)
        {
            isPublicJoinMessageSent = true;
            NetworkInGameMessages.Instance.SendInGameRPCMessage(nickName, "joined");
        }
    }
}
