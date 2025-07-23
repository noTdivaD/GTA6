using UnityEngine;

public class TimedForceApplier : MonoBehaviour
{
    public Vector3 forceToApply;
    public float duration = 0.1f;

    private float timer;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = 0f;
    }

    void FixedUpdate()
    {
        if (timer < duration)
        {
            float dt = Time.fixedDeltaTime;
            Vector3 forceStep = forceToApply / duration * dt;
            rb.AddForce(forceStep, ForceMode.Acceleration); // Add smoothly over time
            timer += dt;
        }
        else
        {
            Destroy(this); // Clean up after applying force
        }
    }
}

