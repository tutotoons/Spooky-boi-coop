using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractor : MonoBehaviour
{
    [SerializeField] private Transform interactPos;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private float interactionRange;
    [SerializeField] private Transform upDownTransform;
    [SerializeField] private Transform leftRightTransform;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float minYRot, maxYRot;

    private BaseInteractable currentInteractable;

    private float mouseX;
    private float mouseY;
    private float rotation = 0f;

    void LateUpdate()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, minYRot, maxYRot);
        upDownTransform.localRotation = Quaternion.Euler(rotation, 0, 0);
        leftRightTransform.Rotate(Vector3.up * mouseX);
    }

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
