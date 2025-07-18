using UnityEngine;

public class GeyserTrigger : MonoBehaviour
{
    [Header("Geyser Launch Settings")]
    public float upwardForce = 12f;
    public float forwardForce = 6f;
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                Vector3 launchDirection = Vector3.up * upwardForce + other.transform.forward * forwardForce;
                rb.AddForce(launchDirection, ForceMode.VelocityChange);

                Debug.Log("Grandma boosted by geyser!");
            }
        }
    }
}
