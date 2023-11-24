using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorInteractionListener : BaseInteractionListener
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool startingState;

    private bool currentState;

    private void Awake()
    {
        currentState = !startingState;
        Animate(startingState);
    }

    public override void Animate(bool _state)
    {
        if (currentState == _state)
        {
            return;
        }
        if (_state)
        {
            Debug.Log($"opening door");
            animator.SetTrigger("Open");
        }
        else
        {
            Debug.Log($"closing door");
            animator.SetTrigger("Close");
        }
        currentState = _state;
    }
}
