using Fusion;
using UnityEngine;

public enum InputButton 
{
    Fire = 0,
}

public struct NetInput : INetworkInput
{
    public float Horizontal;
    public float Vertical;
    public NetworkButtons Buttons;
}

public class CharacterInputHandler
{
    public static NetInput GetNetworkInput() 
    {
        NetInput accumulatedInput = default;
        NetworkButtons buttons = default;

        accumulatedInput.Horizontal = Input.GetAxis("Horizontal");
        accumulatedInput.Vertical = Input.GetAxis("Vertical");

        accumulatedInput.Buttons = new NetworkButtons(accumulatedInput.Buttons.Bits | buttons.Bits);

        return accumulatedInput;
    }
}