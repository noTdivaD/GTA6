using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TimedForceApplier : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 forceToApply;
    private float duration;
    private float elapsed;
    private bool isActive = false;

    public void ApplyForce(Vector3 force, float applyDuration)
    {
        forceToApply = force;
        duration = applyDuration;
        elapsed = 0f;
        isActive = true;

        // Ensure we have the rigidbody at this point
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isActive || rb == null)
            return;

        if (elapsed < duration)
        {
            float dt = Time.fixedDeltaTime;
            Vector3 forceStep = (forceToApply / duration) * dt;
            rb.AddForce(forceStep, ForceMode.VelocityChange); // Stronger and smoother than Acceleration
            elapsed += dt;
        }
        else
        {
            isActive = false;
            Destroy(this); // Clean up
        }
    }
}
