using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseInteractionListener : NetworkBehaviour
{
    NetworkVariable<bool> networkState = new NetworkVariable<bool>();

    public override void OnNetworkSpawn()
    {
        networkState.OnValueChanged += OnStateChanged;
    }

    public override void OnNetworkDespawn()
    {
        networkState.OnValueChanged -= OnStateChanged;
    }

    public void Activate(bool _state)
    {
        ActivateServerRpc(_state);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ActivateServerRpc(bool _state)
    {
        networkState.Value = _state;
    }

    private void OnStateChanged(bool _previous, bool _current)
    {
        Animate(_current);
    }

    public virtual void Animate(bool _state)
    {
    }
}
