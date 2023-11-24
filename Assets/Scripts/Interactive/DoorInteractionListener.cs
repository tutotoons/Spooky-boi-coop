using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorInteractionListener : BaseInteractionListener
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool startingState;

    private bool currentState;

    public override void Animate(bool _state)
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
