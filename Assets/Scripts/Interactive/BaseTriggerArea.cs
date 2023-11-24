using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class BaseTriggerArea : NetworkBehaviour
{
    NetworkVariable<bool> IsActive = new NetworkVariable<bool>();
    [SerializeField] private LayerMask triggerMask;
    [SerializeField] private Collider trigger;
    [SerializeField] private UnityEvent OnTriggerEntered, OnTriggerExited;

    private bool isActive;

    public override void OnNetworkSpawn()
    {
        IsActive.OnValueChanged += OnStateChange;
    }

    public override void OnNetworkDespawn()
    {
        IsActive.OnValueChanged -= OnStateChange;
    }

    private void OnStateChange(bool _previous, bool _current)
    {
        if (_previous == _current)
        {
            return;
        }

        Activate(_current);
    }

    private void Activate(bool _current)
    {
        if (isActive == _current)
        {
            return;
        }
        isActive = _current;
        if (isActive)
        {
            OnTriggerEntered?.Invoke();
        }
        else
        {
            OnTriggerExited?.Invoke();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ActivateServerRpc(bool _state)
    {
        IsActive.Value = _state;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (triggerMask.Contains(_other.gameObject.layer) && !isActive)
        {
            ActivateServerRpc(true);
            Activate(true);
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (triggerMask.Contains(_other.gameObject.layer) && isActive)
        {
            ActivateServerRpc(false);
            Activate(false);
        }
    }
}
