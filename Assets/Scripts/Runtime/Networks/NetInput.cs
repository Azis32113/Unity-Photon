using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

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