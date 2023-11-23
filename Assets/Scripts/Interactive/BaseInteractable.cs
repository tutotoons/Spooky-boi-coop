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
        Switch
    }

    [SerializeField] private InteractableType interactableType;
    [SerializeField] private BaseInteractionListener[] interactives;
    
    private bool previousState;

    public virtual void Interact()
    {
        bool _state = GetState();
        foreach (BaseInteractionListener _interactive in interactives)
        {
            _interactive.Activate(_state);
        }
    }

    public virtual bool GetState()
    {
        switch (interactableType)
        {
            case InteractableType.Enable:
                return true;
            case InteractableType.Disable:
                return false;
            case InteractableType.Switch:
                previousState = !previousState;
                return previousState;
            default:
                return true;
        }
    }

    internal void StopHighlight()
    {
    }

    internal void StartHighlight()
    {
    }
}
