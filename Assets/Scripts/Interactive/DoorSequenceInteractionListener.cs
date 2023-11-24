using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSequenceInteractionListener : BaseSequenceInteractionListener
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
