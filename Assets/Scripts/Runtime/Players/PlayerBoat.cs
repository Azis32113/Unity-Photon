using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerBoat : NetworkBehaviour
{
    Rigidbody rb;

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

        Vector3 rotate = new Vector3(0, input.Horizontal, 0);
        transform.Rotate(rotate * 2f, Space.Self);

        float movement = 0;

        // if (input.Buttons.IsSet(MyButtons.Fire)){ /*do fire*/ }
        if (input.Buttons.IsSet(InputButton.Forward)){ movement += 1; }
        if (input.Buttons.IsSet(InputButton.Backward)){ movement -= 0.2f; }

        rb.velocity = Vector3.Lerp(rb.velocity, 12f * movement * transform.forward, Time.fixedDeltaTime);

        
    } 
}
