using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveForce = 50f;              // How much force to apply for movement
    public float maxSpeed = 6f;                // Optional cap on speed
    public float jumpForce = 7f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isOnGround;
    private float detectionRange = 0.5f;
    private bool isSprinting;

    public Vector2 MoveInput => moveInput; // Accessible from other scripts

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent unwanted rotation
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnMove += HandleMove;
        PlayerInputHandler.OnJump += HandleJump;
        PlayerInputHandler.OnDance += HandleDance;
        PlayerInputHandler.OnSprint += HandleSprint;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnMove -= HandleMove;
        PlayerInputHandler.OnJump -= HandleJump;
        PlayerInputHandler.OnDance -= HandleDance;
        PlayerInputHandler.OnSprint -= HandleSprint;
    }

    private void FixedUpdate()
    {
        // Only forward/back controlled movement — test propulsion
        if (Mathf.Abs(moveInput.y) > 0.01f)
        {
            Vector3 forwardForce = transform.forward * moveInput.y * moveForce;
            rb.AddForce(forwardForce, ForceMode.Force);
        }

        // Side push (simulates being pushed left/right like in air)
        if (Mathf.Abs(moveInput.x) > 0.01f)
        {
            Vector3 lateralForce = transform.right * moveInput.x * moveForce * 0.75f; // Slightly weaker
            rb.AddForce(lateralForce, ForceMode.Force);
        }
    }

    private void HandleMove(Vector2 input)
    {
        moveInput = input;
    }

    private void HandleJump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleDance()
    {
        Debug.Log("Dance action triggered!");
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void HandleSprint(bool sprinting)
    {
        isSprinting = sprinting;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isOnGround ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, detectionRange);
        }
    }
}
