using UnityEngine;

public class GrandmaAirControl : MonoBehaviour
{
    private bool hasPlayerGivenInput = false;

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
    public float strafeForce = 8f;
    public float forwardGlideForce = 4f;
    public float maxHorizontalSpeed = 7f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;

    [Header("New Mechanics")]
    public float boostForce = 60f;
    public float diveForce = 25f;
    private bool justLaunched = true;
    private float launchTimer = 0f;
    public float airControlDelay = 0.2f; // Time in seconds before air control starts

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!isFlying || hasLanded) return;

        float horizontalInput = Input.GetAxis("Horizontal");

        // --- Tilt the model (Z-axis lean) ---
        float targetZRotation = -horizontalInput * tiltAngle;

        if (grandmaModel != null)
        {
            Quaternion targetTilt = Quaternion.Euler(0f, 0f, targetZRotation);
            grandmaModel.localRotation = Quaternion.Lerp(grandmaModel.localRotation, targetTilt, Time.deltaTime * tiltSpeed);
        }

        // --- Rotate model to face input direction (Yaw) ---
        Vector3 moveInput = new Vector3(horizontalInput, 0f, 1f); // forward + left/right
        if (moveInput.magnitude > 0.1f)
        {
            Quaternion targetYaw = Quaternion.LookRotation(moveInput.normalized, Vector3.up);
            grandmaModel.rotation = Quaternion.Lerp(grandmaModel.rotation, targetYaw, Time.deltaTime * tiltSpeed);
        }
    }

    void FixedUpdate()
    {
        if (!isFlying || hasLanded) return;

        if (justLaunched)
        {
            launchTimer += Time.fixedDeltaTime;
            if (launchTimer < airControlDelay)
                return; // Wait before enabling air control

            // Wait for actual input before enabling control
            float hInput = Input.GetAxisRaw("Horizontal");
            bool boostPressed = Input.GetKey(KeyCode.W);
            bool divePressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

            if (Mathf.Abs(hInput) > 0.01f || boostPressed || divePressed)
            {
                hasPlayerGivenInput = true;
                justLaunched = false;
            }
            else
            {
                return; // Still coasting without input
            }
        }



        float input = Input.GetAxis("Horizontal");

        // Strafe (additive, not override)
        if (Mathf.Abs(input) > 0.01f)
        {
            Vector3 strafe = transform.right * input * strafeForce;
            rb.AddForce(strafe, ForceMode.Acceleration);
        }

        // Constant forward glide
        Vector3 glide = transform.forward * forwardGlideForce;
        rb.AddForce(glide * Time.fixedDeltaTime * 60f, ForceMode.Force);

        // Boost forward (W)
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

        // Dive
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            Vector3 diveDirection = (transform.forward + Vector3.down).normalized;
            rb.AddForce(diveDirection * diveForce, ForceMode.Acceleration);

            lastDiveHeight = transform.position.y;
            isDiving = true;
        }
        else if (isDiving)
        {
            if (transform.position.y < lastDiveHeight)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            }
            isDiving = false;
        }

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
        hasPlayerGivenInput = false;
        isFlying = true;
        hasLanded = false;
        justLaunched = true;
        launchTimer = 0f;

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
