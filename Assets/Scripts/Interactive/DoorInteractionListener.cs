using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractionListener : BaseInteractionListener
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool startingState;

    private bool currentState;

    private void Awake()
    {
        Activate(startingState);
    }

    public override void Activate(bool _state)
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
