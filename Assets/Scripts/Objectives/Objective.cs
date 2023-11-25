using System;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public event Action<Objective> ActivatedEvent;
    public event Action<Objective> CompletedEvent;
    public event Action<Objective> ResetEvent;

    public State CurrentState => _state;

    [SerializeField] private BaseInteractable[] interactableSequence;

    private State _state;
    private int _index;

    private void Awake()
    {
        foreach(BaseInteractable interactable in interactableSequence)
        {
            interactable.InteractEvent += OnInteracted;
        }
    }

    private void OnInteracted(BaseInteractable interactable)
    {
        if(_state != State.Active)
        {
            return;
        }

        if(interactable == interactableSequence[_index])
        {
            _index++;

            if(_index >= interactableSequence.Length)
            {
                CompleteObjective();
            }
        }
        else
        {
            ResetObjective();
        }
    }

    public void ActivateObjective()
    {
        _state = State.Active;
    }

    public void ResetObjective()
    {
        _index = 0;

        ResetEvent?.Invoke(this);
    }

    public void CompleteObjective()
    {
        _state = State.Completed;

        CompletedEvent?.Invoke(this);
    }

    public enum State
    {
        None,
        Active,
        Completed
    }
}
