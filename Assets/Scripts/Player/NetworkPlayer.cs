using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public enum PlayerType
{
    Explorer,
    Navigator
}

public class NetworkPlayer : NetworkBehaviour
{
    public PlayerType PlayerType => playerType;

    [SerializeField] private PlayerType playerType;
    [SerializeField] private Transform interactPos;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private float interactionRange;
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Phone phone;
    [SerializeField] private CharacterController controller;

    [Header("Player")]
    [SerializeField] private float MoveSpeed = 4.0f;
    [SerializeField] private float RotationSpeed = 1.0f;

    [Header("Cinemachine")]
    [SerializeField] private Transform upDownTransform;
    [SerializeField] private float TopClamp = 90.0f;
    [SerializeField] private float BottomClamp = -90.0f;

    private float targetPitch;
    private float rotationVelocity;

    private BaseInteractable currentInteractable;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Cursor.lockState = CursorLockMode.Locked;
            audioListener.enabled = true;
            virtualCam.Priority = 1;
        }
        else
        {
            audioListener.enabled = false;
            virtualCam.Priority = 0;
        }

        Debug.Log($" trying to init phone {phone != null} owner:{IsOwner} isServer:{IsServer}");
        phone?.Init();
    }


    virtual public void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleInteractions();
        HandleMovement();
    }

    private void HandleInteractions()
    {
        TryGetCurrentInteractableAndHighlight();
        if (currentInteractable != null && Input.GetMouseButtonDown(0))
        {
            currentInteractable.Interact();
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        CameraRotation();
    }

    private void CameraRotation()
    {
        Vector2 _input = new Vector2(Input.GetAxis("Mouse X") * Time.deltaTime, -Input.GetAxis("Mouse Y") * Time.deltaTime);

        targetPitch += _input.y * RotationSpeed;
        rotationVelocity = _input.x * RotationSpeed;

        targetPitch = ClampAngle(targetPitch, BottomClamp, TopClamp);
        upDownTransform.localRotation = Quaternion.Euler(targetPitch, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * rotationVelocity);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
            lfAngle += 360f;
        if (lfAngle > 360f)
            lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void HandleMovement()
    {      
        Vector2 _movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 inputDirection = new Vector3(_movementVector.x, 0.0f, _movementVector.y).normalized;

        if (_movementVector != Vector2.zero)
        {
            inputDirection = transform.right * _movementVector.x + transform.forward * _movementVector.y;
        }
        controller.Move(inputDirection * MoveSpeed * Time.deltaTime);
    }

    private void TryGetCurrentInteractableAndHighlight()
    {
        if (Physics.Raycast(interactPos.position, interactPos.forward, out RaycastHit _hitInfo, interactionRange, interactionMask))
        {
            BaseInteractable _newInteractable = _hitInfo.transform.GetComponent<BaseInteractable>();
            if (currentInteractable != _newInteractable)
            {
                currentInteractable?.StopHighlight();
                _newInteractable?.StartHighlight();
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
