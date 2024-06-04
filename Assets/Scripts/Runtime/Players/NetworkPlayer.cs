using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using Unity.VisualScripting;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }

    [SerializeField] TextMeshProUGUI playerNameTMP; 
    [SerializeField] Transform camTarget;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    [HideInInspector] public NetworkString<_16> nickName { get; set; }

    private bool isPublicJoinMessageSent = false;

    public override void Spawned()
    {
        if (HasInputAuthority) 
        {
            Local = this;

            CameraFollow.Instance.SetTarget(camTarget);

            RPC_SetNickName(PlayerPrefs.GetString(Constants.LocalData.PLAYER_NICK_NAME));
            
            Debug.Log("Spawned Local Player");
        }

        else Debug.Log("Spawned Remote Player");
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            NetworkInGameMessages.Instance.SendInGameRPCMessage(nickName.ToString(), "left");
        }
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    private static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} ONHPChanged value {changed.Behaviour.nickName}");
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
