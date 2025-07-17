using UnityEngine;

public class VolcanoScript : MonoBehaviour
{
    public float pullForce = 5f;
    public float spiralSpeed = 50f;
    public float flushSpeed = 1f;
    public float shakeAmount = 0.1f;
    public Vector3 flushScale = Vector3.zero;

    void OnTriggerEnter(Collider other)
    {
        // Only affect objects with Rigidbody
        Rigidbody otherRigidBody = other.GetComponent<Rigidbody>();
        if (otherRigidBody != null)
        {
            // Check if it's a player object
            if (other.CompareTag("Player"))
            {
                // Disable player controls
                //other.GetComponent<PlayerController>().enabled = false;

                // Game over
                GameManager.Instance.GameOver();
            }

            // Disable gravity and start pulling
            otherRigidBody.useGravity = false;
            StartCoroutine(VolcanoFlush(otherRigidBody));

            // Testing
            GameManager.Instance.GameOver();
        }
    }

    System.Collections.IEnumerator VolcanoFlush(Rigidbody otherRigidBody)
    {
        // The volcano's center is where the other object will be pulled towards
        Vector3 volcanoCenter = transform.position;

        while (Vector3.Distance(otherRigidBody.position, volcanoCenter) > 0.1f)
        {
            Vector3 direction = (volcanoCenter - otherRigidBody.position).normalized;

            // Spiral offset
            Vector3 spiralOffset = Vector3.Cross(direction, Vector3.up).normalized * 0.5f;
            float spiralAngle = spiralSpeed * Time.deltaTime;
            spiralOffset = Quaternion.AngleAxis(spiralAngle, direction) * spiralOffset;

            // Small random shake
            Vector3 shake = Random.insideUnitSphere * shakeAmount;

            // Calculate new target position
            Vector3 targetPosition = otherRigidBody.position
                + direction * pullForce * Time.deltaTime
                + spiralOffset
                + shake;

            // Move the Rigidbody
            otherRigidBody.MovePosition(targetPosition);

            // Shrink the object
            otherRigidBody.transform.localScale = Vector3.Lerp(otherRigidBody.transform.localScale, flushScale, flushSpeed * Time.deltaTime);

            yield return null;
        }

        // Heat death of the object
        Destroy(otherRigidBody.gameObject);
    }
}
