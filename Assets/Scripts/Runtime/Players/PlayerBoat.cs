using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using LowPolyWater;
using System.Linq;
using System.Transactions;

public class PlayerBoat : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform camTarget;
    [SerializeField] private int rotateSpeed = 2;
    [SerializeField] private int moveSpeed = 15;

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
        if (HasInputAuthority) CameraFollow.Instance.SetTarget(camTarget);
    }
}
