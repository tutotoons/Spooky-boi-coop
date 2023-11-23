using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider), typeof(EventTrigger))]
public class BaseInteractable : MonoBehaviour
{
    [SerializeField] private BaseInteractionListener[] interactives;

    public virtual void Interact()
    {
        foreach (BaseInteractionListener _interactive in interactives)
        {
            _interactive.Activate(true);
        }
    }

    internal void StopHighlight()
    {
        Debug.Log($"{name} stop highlight");
    }

    internal void StartHighlight()
    {
        Debug.Log($"{name} start highlight");
    }
}
