using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class BaseSequenceInteractionListener : NetworkBehaviour
{
    NetworkVariable<int> networkState = new NetworkVariable<int>(-1);

    [SerializeField] protected bool startingState;
    [SerializeField] protected int sequenceCount;
    [SerializeField] private UnityEvent OnSequenceComplete, OnSequenceFail;

    protected bool currentState;
    private int currentSequenceIndex;

    private void Awake()
    {
        currentSequenceIndex = -1;
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

    public void Activate(int _state)
    {
        ActivateServerRpc(_state);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ActivateServerRpc(int _state)
    {
        networkState.Value = _state;
    }

    private void OnStateChanged(int _previous, int _current)
    {
        if (currentState == !startingState)
        {
            return;
        }

        if (_previous == _current)
        {
            return;
        }
        if (currentSequenceIndex + 1 == _current)
        {
            currentSequenceIndex = _current;
            if (currentSequenceIndex == sequenceCount - 1)
            {
                Animate(!startingState);
                OnSequenceComplete?.Invoke();
            }
        }
        else
        {
            currentSequenceIndex = -1;
            OnSequenceFail?.Invoke();
        }
    }

    public virtual void Animate(bool _state)
    {
    }
}
