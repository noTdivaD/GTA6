using UnityEngine;

public class GrandmaAirControl : MonoBehaviour
{
    private Rigidbody rb;
    private bool isFlying = false;
    private bool hasLanded = false;

    [Header("Air Control Settings")]
    public float strafeForce = 2f;
    public float forwardGlideForce = 4f; // increase glide speed
    public float maxHorizontalSpeed = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isFlying || hasLanded) return;

        float input = Input.GetAxis("Horizontal");

        Vector3 strafe = transform.right * input * strafeForce;
        Vector3 forward = transform.forward * forwardGlideForce;
        Vector3 airControl = strafe + forward;

        rb.AddForce(airControl * Time.fixedDeltaTime * 60f, ForceMode.Force);

        // Clamp horizontal speed
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxHorizontalSpeed)
        {
            Vector3 limited = horizontalVelocity.normalized * maxHorizontalSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }

        // Check if she hit the ground
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer))
        {
            hasLanded = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            Debug.Log("Grandma has landed!");
        }
    }

    public void StartFlying()
    {
        isFlying = true;
        hasLanded = false;

        if (rb != null)
        {
            rb.linearDamping = 0.5f;
            rb.angularDamping = 2f;
        }
    }
}
