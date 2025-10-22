using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Player Mechanics")]
    [SerializeField] private CharacterController playerCharacterController;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float rayDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    public Transform playerPickedUpObjectSlot;

    [Header("Camera Settings")]
    [SerializeField] private float topClamp = 90f;
    [SerializeField] private float bottomClamp = -90f;

    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 moveDir;
    private float xRotation = 0f;

    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerMove();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void PlayerMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f;
        }

        Vector2 moveInput = inputHandler.GetMovementVectorNormalized();
        moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;

        playerCharacterController.Move(moveDir * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        playerCharacterController.Move(velocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        Vector2 lookInput = inputHandler.GetMouseVectorNormalized();

        // Don't multiply mouse input by deltaTime (for desktop)
        float deltaTimeMultiplier = 1.0f;

        // Add rotation based on input
        xRotation += lookInput.y * mouseSensitivity * deltaTimeMultiplier;
        float yRotation = lookInput.x * mouseSensitivity * deltaTimeMultiplier;

        // Clamp vertical look
        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp);

        // Apply rotation to camera (pitch)
        playerCamera.localRotation = Quaternion.Euler(-xRotation, 0f, 0f);

        // Rotate player body (yaw)
        transform.Rotate(Vector3.up * yRotation);
    }

    public void TriggerInteract(InputAction.CallbackContext context)
    {
        int interactableMask = LayerMask.GetMask("Interactable");

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, rayDistance, interactableMask))
        {
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }

    public void TriggerPickUp(InputAction.CallbackContext context)
    {
        int pickableMask = LayerMask.GetMask("Pickable");

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, rayDistance, pickableMask))
        {
            if (hit.collider.gameObject.TryGetComponent<Pickable>(out Pickable pickable))
            {
                pickable.HandlePickUp();
            }
        }
    }

    public void TriggerThrow(InputAction.CallbackContext context)
    {
        if (playerPickedUpObjectSlot.childCount > 0 &&
            playerPickedUpObjectSlot.GetChild(0).TryGetComponent<Pickable>(out Pickable pickable))
        {
            pickable.ExecuteThrow(pickable.transform);
        }
    }

    public void TriggerInteractionWithPickedUpObject(InputAction.CallbackContext context)
    {
        if (playerPickedUpObjectSlot.childCount > 0 &&
            playerPickedUpObjectSlot.GetChild(0).TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.Interact();
        }
    }
}
