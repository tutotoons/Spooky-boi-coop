using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BaseToggleInteractable : BaseInteractable
{
    private enum InteractableType
    {
        Enable,
        Disable,
        Toggle
    }

    [SerializeField] private InteractableType interactableType;
    [SerializeField] private Animator animator;
    [SerializeField] private BaseToggleInteractionListener[] interactives;
    private bool previousState;

    public override void Interact()
    {
        if (!CanInteract())
        {
            return;
        }

        base.Interact();

        timer = interactionCooldown;
        AnimateInteraction();

        bool _state = GetState();
        foreach (BaseToggleInteractionListener _interactive in interactives)
        {
            _interactive.Activate(_state);
        }
    }

    public virtual void AnimateInteraction()
    {
        animator.SetTrigger("Interact");
    }

    public virtual bool GetState()
    {
        switch (interactableType)
        {
            case InteractableType.Enable:
                return true;
            case InteractableType.Disable:
                return false;
            case InteractableType.Toggle:
                previousState = !previousState;
                return previousState;
            default:
                return true;
        }
    }
}
