using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



[RequireComponent(typeof(Collider))]
public class BaseInteractable : NetworkBehaviour
{
    private enum InteractableType
    {
        Enable,
        Disable,
        Toggle
    }

    [SerializeField] private Outline outline;
    [SerializeField] private InteractableType interactableType;
    [SerializeField] private Animator animator;
    [SerializeField] private BaseInteractionListener[] interactives;
    [SerializeField] protected float interactionCooldown;
    private bool previousState;
    protected float timer;

    public virtual void Interact()
    {
        if (timer > 0f)
        {
            return;
        }
        timer = interactionCooldown;
        AnimateInteraction();

        bool _state = GetState();
        foreach (BaseInteractionListener _interactive in interactives)
        {
            _interactive.Activate(_state);
        }
    }

    public virtual void AnimateInteraction()
    {
        animator.SetTrigger("Interact");
    }

    private void Update()
    {
        timer -= Time.deltaTime;
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

    public void StopHighlight()
    {
        outline.enabled = false;
    }

    public void StartHighlight()
    {
        outline.enabled = true;
    }
}
