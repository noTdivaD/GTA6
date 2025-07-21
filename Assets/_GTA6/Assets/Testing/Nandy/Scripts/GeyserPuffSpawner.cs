using UnityEngine;
using UnityEngine.EventSystems;

public class GeyserPuffSpawner : MonoBehaviour
{
    public ObjectPool puffPool;
    public float spawnInterval = 0.1f;
    public Transform spawnPoint;
    public Vector3 moveDirection = Vector3.up; // Can set to Vector3.down in Inspecto
    public float spawnRadius = 0.5f;

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
                puffScript.moveDirection = moveDirection;
            }
            puff.SetActive(true);
            Debug.Log("Spawned puff at: " + spawnPosition);
            timer = 0f;
        }
    }
}

