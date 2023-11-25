using System;
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
    public event Action DiedEvent;

    public PlayerType PlayerType => playerType;

    [SerializeField] private PlayerType playerType;
    [SerializeField] private Transform interactPos;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private float interactionRange;
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Phone phone;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private RenderTexture povRenderTexture;

    [Header("Player")]
    [SerializeField] private float MoveSpeed = 4.0f;
    [SerializeField] private float RotationSpeed = 1.0f;
    [SerializeField] private int lives = 3;

    [Header("Jumping and gravity")]
    [SerializeField] private float JumpHeight = 1.2f;
    [SerializeField] private float Gravity = -15.0f;
    [SerializeField] private bool Grounded = true;
    [SerializeField] private float GroundedOffset = -0.14f;
    [SerializeField] private float GroundedRadius = 0.5f;
    [SerializeField] private LayerMask GroundLayers;

    [Header("looking around")]
    [SerializeField] private Transform upDownTransform;
    [SerializeField] private float TopClamp = 90.0f;
    [SerializeField] private float BottomClamp = -90.0f;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepDistanceThreshold = 0.5f;

    private Vector3 previousFootstepPosition = new Vector3(-99999, -99999);
    private int footstepIndex;
    private float targetPitch;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

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

            if (playerType == PlayerType.Explorer)
            {
                cinemachineBrain.enabled = false;
                virtualCam.enabled = false;
                playerCamera.targetTexture = povRenderTexture;
                playerCamera.forceIntoRenderTexture = true;
            }
        }

        if (playerType == PlayerType.Explorer)
        {
            LightManager.Instance?.SetActivePlayer(transform);
        }
        RadarManager.Instance.SetPlayer(transform);
        phone?.Init();
    }

    virtual public void Update()
    {
        HandleFootsteps();

        if (!IsOwner)
        {
            return;
        }

        HandleInteractions();
        JumpAndGravity();
        GroundedCheck();
        HandleMovement();
    }

    public void TakeLive()
    {
        lives--;

        if(lives <= 0)
        {
            enabled = false;

            if(IsOwner)
            {
                DiedEvent?.Invoke();
            }
        }
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

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void HandleMovement()
    {      
        Vector2 _movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 inputDirection = new Vector3(_movementVector.x, 0.0f, _movementVector.y).normalized;

        if (_movementVector != Vector2.zero)
        {
            inputDirection = transform.right * _movementVector.x + transform.forward * _movementVector.y;
        }
        controller.Move(inputDirection * MoveSpeed * Time.deltaTime + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void HandleFootsteps()
    {
        if(Vector3.Distance(transform.position, previousFootstepPosition) > footstepDistanceThreshold)
        {
            previousFootstepPosition = transform.position;

            if(footstepSource != null && footstepClips.Length > 0)
            {
                footstepSource.PlayOneShot(footstepClips[footstepIndex % footstepClips.Length]);
                footstepIndex++;
            }
        }
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
