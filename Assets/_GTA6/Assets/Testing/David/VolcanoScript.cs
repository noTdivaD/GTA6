using UnityEngine;

public class VolcanoScript : MonoBehaviour
{
    public float pullForce = 1f;
    public float spiralSpeed = 2f;
    public float flushSpeed = 3f;
    public float shakeAmount = 0.01f;
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
                // Disable player controls (fixing okay but optional the flush is strong enough)
                //other.GetComponent<PlayerController>().enabled = false;

                // Game over
                GameManager.Instance.GameOver();
            }

            // Disable gravity and start pulling
            otherRigidBody.useGravity = false;
            StartCoroutine(VolcanoFlush(otherRigidBody));
        }
    }

    // skull emote free me from my suffering
    System.Collections.IEnumerator VolcanoFlush(Rigidbody otherRigidBody)
    {
        // The point towards which the object will be pulled
        Vector3 volcanoCenter = transform.position;

        // Set X and Z limits based on the sphere collider (so the object doesn't go outside and clip through the volcano)
        SphereCollider sphere = GetComponent<SphereCollider>();
        float worldRadius = sphere.radius * Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        while (Vector3.Distance(otherRigidBody.position, volcanoCenter) > 0.1f)
        {
            // Direction towards the volcano center
            Vector3 direction = (volcanoCenter - otherRigidBody.position).normalized;

            // Spiral by spinning the vector around the direction (kinda like a corkscrew)
            Vector3 spiralOffset = Vector3.Cross(direction, Vector3.up).normalized * 0.5f;
            float spiralAngle = spiralSpeed * Time.deltaTime;
            spiralOffset = Quaternion.AngleAxis(spiralAngle, direction) * spiralOffset;

            // Shake so that it does move in spiral instead of just being sucked in
            Vector3 shake = Random.insideUnitSphere * shakeAmount;

            // Calculate new target position
            Vector3 targetPosition = otherRigidBody.position
                + direction * pullForce * Time.deltaTime
                + spiralOffset
                + shake;

            // Clamp X and Z to be within the limits
            Vector3 offsetFromCenter = targetPosition - volcanoCenter;
            float horizontalDistance = new Vector2(offsetFromCenter.x, offsetFromCenter.z).magnitude;

            if (horizontalDistance > worldRadius)
            {
                // Get it back inside the limits
                float scale = worldRadius / horizontalDistance;
                offsetFromCenter.x *= scale;
                offsetFromCenter.z *= scale;
                targetPosition = volcanoCenter + offsetFromCenter;
            }

            // Finally, move the object
            otherRigidBody.MovePosition(targetPosition);

            // Shrink it too
            otherRigidBody.transform.localScale = Vector3.Lerp(
                otherRigidBody.transform.localScale,
                flushScale,
                flushSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Heat death of the object
        if (otherRigidBody != null)
            Destroy(otherRigidBody.gameObject);
    }
}
