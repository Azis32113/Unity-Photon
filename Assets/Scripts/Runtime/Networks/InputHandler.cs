using Fusion;
using UnityEngine;

public enum InputButton 
{
    Fire = 0,
    Forward = 1,
    Backward = 2,
}

public struct NetInput : INetworkInput
{
    public float Horizontal;
    public NetworkButtons Buttons;
}

public class InputHandler
{
    public static NetInput GetInput() 
    {
        NetInput accumulatedInput = default;
        NetworkButtons buttons = default;

        float direction = 0;
        if (Input.GetKey(KeyCode.A)) {
            direction -= 1; 
        } 
        if (Input.GetKey(KeyCode.D)) direction += 1; 

        accumulatedInput.Horizontal = direction;
        buttons.Set(InputButton.Forward, Input.GetKey(KeyCode.W));
        buttons.Set(InputButton.Backward, Input.GetKey(KeyCode.S));

        accumulatedInput.Buttons = new NetworkButtons(accumulatedInput.Buttons.Bits | buttons.Bits);

        return accumulatedInput;
    }
}