using UnityEngine;
using UnityEngine.EventSystems;

public class GeyserPuffSpawner : MonoBehaviour
{
    public ObjectPool puffPool;
    public float spawnInterval = 0.1f;
    public Transform spawnPoint;
    public float spawnRadius = 0.5f;

    [Header("Puff Settings")]
    public float puffRiseSpeed = 1f;
    public float puffLifetime = 1.5f;
    public float puffYRotationSpeed = 180f;
    public Color puffColor = Color.white;
    public Vector2 puffXScaleRange = new Vector2(0.6f, 1.2f); // Range of random X scale
    public Vector2 puffYScaleRange = new Vector2(0.6f, 1.2f); // same for Y scale, if needed
    public Vector3 moveDirection = Vector3.up; // Can set to Vector3.down in Inspecto



    private float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            GameObject puff = puffPool.GetPooledObject();

            Vector2 offset2D = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = spawnPoint.position + new Vector3(offset2D.x, 0f, offset2D.y);


            puff.transform.position = spawnPosition;
            puff.transform.rotation = Quaternion.identity;

            GeyserPuff puffScript = puff.GetComponent<GeyserPuff>();
            if (puffScript != null)
            {
                puffScript.Setup(puffRiseSpeed, puffLifetime, puffYRotationSpeed, puffXScaleRange,puffYScaleRange, moveDirection, puffColor);
            }
            puff.SetActive(true);
            //Debug.Log("Spawned puff at: " + spawnPosition);
            timer = 0f;
        }
    }
}

