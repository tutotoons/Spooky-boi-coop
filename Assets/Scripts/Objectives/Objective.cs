using System;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public event Action<Objective> UpdatedGoalEvent;
    public event Action<Objective> CompletedEvent;
    public event Action<Objective> ResetEvent;

    public bool IsActive;

    [SerializeField] private BaseInteractable[] interactableSequence;

    private int _index;

    private void Awake()
    {
        IsActive = false;
        foreach (BaseInteractable interactable in interactableSequence)
        {
            interactable.InteractEvent += OnInteracted;
        }
    }

    private void OnInteracted(BaseInteractable interactable)
    {
        if(!IsActive)
        {
            return;
        }
        Debug.Log($"{transform.name} On interact {_index} {interactableSequence.Length}");

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
        Debug.Log("Completed objective with name: " + gameObject.name);
        CompletedEvent?.Invoke(this);
    }

    public enum State
    {
        Active,
        Completed
    }
}
