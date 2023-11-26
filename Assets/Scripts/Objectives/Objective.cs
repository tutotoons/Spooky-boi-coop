using System;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public event Action<Objective> UpdatedGoalEvent;
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
        Debug.Log("On interact in state: " + _state.ToString());

        if(_state != State.Active)
        {
            return;
        }

        if(interactable == interactableSequence[_index])
        {
            _index++;

            if (_index >= interactableSequence.Length)
            {
                CompleteObjective();
            }
            else
            {
                UpdatedGoalEvent?.Invoke(this);
            }
        }
        else
        {
            ResetObjective();
        }
    }

    public Transform GetTarget()
    {
        if(interactableSequence.Length == 0)
        {
            return transform;
        }

        return interactableSequence[_index].transform;
    }

    public void ResetObjective()
    {
        _index = 0;

        ResetEvent?.Invoke(this);
    }

    public void CompleteObjective()
    {
        _state = State.Completed;

        Debug.Log("Completed objective with name: " + gameObject.name);

        CompletedEvent?.Invoke(this);
    }

    public enum State
    {
        Active,
        Completed
    }
}
