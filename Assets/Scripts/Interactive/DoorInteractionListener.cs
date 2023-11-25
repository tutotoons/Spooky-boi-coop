using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class DoorInteractionListener : BaseToggleInteractionListener
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider coll;
    [SerializeField] private NavMeshObstacle navMeshObstacle;

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
        coll.enabled = !_state;
        navMeshObstacle.enabled = !_state;
        currentState = _state;
    }
}
