using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GeyserTrigger : MonoBehaviour
{
    [Header("Geyser Launch Settings")]
    public float upwardForce = 12f;
    public float forwardForce = 6f;
    public string playerTag = "Player";
    public float forceApplyDuration = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && !other.GetComponent<TimedForceApplier>())
            {
                Vector3 launchDirection = Vector3.up * upwardForce + other.transform.forward * forwardForce;

                // Attach the TimedForceApplier to apply force over time
                var applier = other.gameObject.AddComponent<TimedForceApplier>();
                applier.forceToApply = launchDirection;
                applier.duration = forceApplyDuration;

                Debug.Log("Grandma boosted smoothly by geyser!");
            }
        }
    }
}
