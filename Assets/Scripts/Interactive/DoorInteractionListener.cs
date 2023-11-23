using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorInteractionListener : BaseInteractionListener
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool startingState;

    private bool currentState;

    NetworkVariable<bool> networkState = new NetworkVariable<bool>();

    public override void OnNetworkSpawn()
    {
        networkState.OnValueChanged += OnStateChanged;
    }

    public override void OnNetworkDespawn()
    {
        networkState.OnValueChanged -= OnStateChanged;
    }

    private void OnStateChanged(bool _previous, bool _current)
    {
        Animate(_current);
    }

    public override void Activate(bool _state)
    {
        ActivateServerRpc(_state);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ActivateServerRpc(bool _state)
    {
        networkState.Value = _state;
    }

    private void Animate(bool _state)
    {
        if (currentState == _state)
        {
            return;
        }
        if (_state)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            animator.SetTrigger("Close");
        }
        currentState = _state;
    }
}
