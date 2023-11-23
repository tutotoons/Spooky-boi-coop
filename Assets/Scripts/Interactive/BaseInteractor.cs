using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactPos;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private float interactionRange;

    private BaseInteractable currentInteractable;

    virtual public void Update()
    {
        TryGetCurrentInteractableAndHighlight();
        if (currentInteractable != null && Input.GetMouseButtonDown(0))
        {
            currentInteractable.Interact();
        }
    }

    private void TryGetCurrentInteractableAndHighlight()
    {
        if (Physics.Raycast(interactPos.position, interactPos.forward, out RaycastHit _hitInfo, 5f, interactionMask))
        {
            BaseInteractable _newInteractable = _hitInfo.transform.GetComponent<BaseInteractable>();
            if (currentInteractable != _newInteractable)
            {
                currentInteractable?.StopHighlight();
                _newInteractable.StartHighlight();
                currentInteractable = _newInteractable;
            }
        }
        else
        {
            currentInteractable?.StopHighlight();
            currentInteractable = null;
        }
    }
}
