using UnityEngine;

public class GrandmaAirControl : MonoBehaviour
{
    public ParticleSystem boostParticles;

    private float lastDiveHeight;
    private bool isDiving = false;

    private Rigidbody rb;
    private Animator animator;

    private bool isFlying = false;
    private bool hasLanded = false;

    [Header("Visual Tilt Settings")]
    public Transform grandmaModel; // assign in Inspector
    public float tiltAngle = 20f;  // max tilt angle in degrees
    public float tiltSpeed = 5f;   // how fast to tilt and return

    [Header("Air Control Settings")]
    public float strafeForce = 2f;
    public float forwardGlideForce = 4f;
    public float maxHorizontalSpeed = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;

    [Header("New Mechanics")]
    public float boostForce = 60f;
    public float diveForce = 25f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!isFlying || hasLanded) return;

        float horizontalInput = Input.GetAxis("Horizontal");

        // Determine target tilt angle
        float targetZRotation = -horizontalInput * tiltAngle;

        // Apply tilt to grandma model only (smoothly)
        if (grandmaModel != null)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetZRotation);
            grandmaModel.localRotation = Quaternion.Lerp(grandmaModel.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
    }

    void FixedUpdate()
    {
        if (!isFlying || hasLanded) return;

        float input = Input.GetAxis("Horizontal");

        Vector3 strafe = transform.right * input * strafeForce;
        Vector3 forward = transform.forward * forwardGlideForce;
        Vector3 airControl = strafe + forward;

        // Boost (W key)
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 boost = transform.forward * boostForce;
            rb.AddForce(boost, ForceMode.Acceleration);

            if (boostParticles != null && !boostParticles.isPlaying)
                boostParticles.Play();
        }
        else
        {
            if (boostParticles != null && boostParticles.isPlaying)
                boostParticles.Stop();
        }

        // Dive (Ctrl key)
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            Vector3 diveDirection = (transform.forward + Vector3.down).normalized;
            rb.AddForce(diveDirection * diveForce, ForceMode.Acceleration);

            lastDiveHeight = transform.position.y;
            isDiving = true;
        }
        else if (isDiving)
        {
            // Maintain glide at dive level
            Vector3 currentVelocity = rb.linearVelocity;
            if (transform.position.y < lastDiveHeight)
            {
                rb.linearVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            }
            isDiving = false;
        }

        rb.AddForce(airControl * Time.fixedDeltaTime * 60f, ForceMode.Force);

        // Clamp horizontal speed
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxHorizontalSpeed)
        {
            Vector3 limited = horizontalVelocity.normalized * maxHorizontalSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }

        // Ground check
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer))
        {
            hasLanded = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            if (boostParticles != null)
                boostParticles.Stop();

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

        if (animator != null)
        {
            animator.SetTrigger("Fly");
        }
    }
}
