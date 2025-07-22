using UnityEngine;

public class SpiralBooster : MonoBehaviour
{
    [Header("Spiral Settings")]
    public GameObject spiralSprite; // The child GameObject with the sprite renderer
    public float rotationSpeed = 100f; // Degrees per second

    private void Update()
    {
        if (spiralSprite != null)
        {
            spiralSprite.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming Grandma is tagged as "Player"
        {
            if (spiralSprite != null)
            {
                spiralSprite.SetActive(false);
            }
        }
    }
}

