using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using System;

public class PlayerBoat : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameTMP; 
    Rigidbody rb;
    [SerializeField] Transform camTarget;
    [SerializeField] private int rotateSpeed = 2;
    [SerializeField] private int moveSpeed = 15;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName { get; set; }

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        // get input
        // getting the input from the user, putting it in a 'input' var
        // also, if this is not the current player (its a remote playe object) - leaving this script
        if (!GetInput<NetInput>(out var input)) return;

        float movement = 0;

        // if (input.Buttons.IsSet(MyButtons.Fire)){ /*do fire*/ }
        if (input.Buttons.IsSet(InputButton.Forward)){ movement += 1; }
        if (input.Buttons.IsSet(InputButton.Backward)){ movement -= 0.2f; }

        rb.velocity = Vector3.Lerp(rb.velocity, moveSpeed * movement * transform.forward, Time.fixedDeltaTime);

        Vector3 rotationSpeed = new Vector3(0, rotateSpeed * Mathf.Clamp(rb.velocity.magnitude * input.Horizontal, -1, 1), 0);
        transform.Rotate(rotationSpeed, Space.Self);
    }

    public override void Spawned()
    {
        if (HasInputAuthority) 
        {
            CameraFollow.Instance.SetTarget(camTarget);

            RPC_SetNickName(PlayerPrefs.GetString(Constants.Data.PLAYER_NICK_NAME));
        }
    }

    private static void OnNickNameChanged(Changed<PlayerBoat> changed)
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
    }
}
