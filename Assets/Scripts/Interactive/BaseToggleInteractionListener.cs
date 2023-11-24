using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class BaseToggleInteractionListener : NetworkBehaviour
{
    NetworkVariable<bool> networkState = new NetworkVariable<bool>(); 
    [SerializeField] protected bool startingState;
    [SerializeField] UnityEvent OnEnable, OnDisable;

    protected bool currentState;

    private void Awake()
    {
        currentState = !startingState;
        Animate(startingState);
    }

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
        if (_previous == _current)
        {
            return;
        }
        if (_current)
        {
            OnEnable?.Invoke();
        }
        else
        {
            OnDisable?.Invoke();
        }
        Animate(_current);
    }

    public virtual void Animate(bool _state)
    {
    }
}
