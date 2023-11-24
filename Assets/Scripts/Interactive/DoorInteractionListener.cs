using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorInteractionListener : BaseToggleInteractionListener
{
    [SerializeField] private Animator animator;

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
