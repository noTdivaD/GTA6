using UnityEngine;

public class DownCloudTrigger : MonoBehaviour
{
    [Header("Geyser Launch Settings")]
    public float downwardForce = 12f;
  
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                Vector3 launchDirection = Vector3.down * downwardForce;
                rb.AddForce(launchDirection, ForceMode.VelocityChange);

                Debug.Log("Grandma boosted by geyser!");
            }
        }
    }
}
