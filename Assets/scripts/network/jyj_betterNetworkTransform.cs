using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;

public class jyj_betterNetworkTransform : NetworkTransform
{
    [SerializeField] private AuthMode authMode = AuthMode.CLIENT;
    protected override bool OnIsServerAuthoritative()
    {
        return (authMode == AuthMode.SERVER);
    }
}

public enum AuthMode
{
    SERVER,
    CLIENT
}
