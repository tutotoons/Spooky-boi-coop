using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Transform interactPos;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private float interactionRange;
    [SerializeField] private Transform upDownTransform;
    [SerializeField] private Transform leftRightTransform;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float minYRot, maxYRot;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementSpeed;
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private AudioListener audioListener;

    private BaseInteractable currentInteractable;

    private float mouseX;
    private float mouseY;
    private float rotation = 0f;
    private Vector3 movementInput;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            audioListener.enabled = true;
            virtualCam.Priority = 1;
        }
        else
        {
            audioListener.enabled = false;
            virtualCam.Priority = 0;
        }
    }


    virtual public void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        TryGetCurrentInteractableAndHighlight();
        if (currentInteractable != null && Input.GetMouseButtonDown(0))
        {
            currentInteractable.Interact();
        }
    }

    void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, minYRot, maxYRot);
        upDownTransform.localRotation = Quaternion.Euler(rotation, 0, 0);
        leftRightTransform.Rotate(Vector3.up * mouseX);
    }

    public void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 _moveVector = transform.TransformDirection(movementInput) * movementSpeed;
        rb.velocity = new Vector3(_moveVector.x, rb.velocity.y, _moveVector.z);
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
