using UnityEngine;

public class GeyserPuff : MonoBehaviour
{
    [Header("Visual")]
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    [Header("Animation Settings")]
    public float lifetime = 1.5f;
    public float riseSpeed = 1f;
    public float yRotationSpeed = 180f; // Degrees per second
    public Vector2 xScaleRange = new Vector2(0.6f, 1.2f); // Range of random X scale
    public Vector2 yScaleRange = new Vector2(0.6f, 1.2f); // same for Y scale, if needed

    [HideInInspector]
    public Vector3 moveDirection = Vector3.up; // Default to up

    private float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        timer = 0f;

        // Randomize sprite
        if (sprites != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }

        // Random X scale
        float randomXScale = Random.Range(xScaleRange.x, xScaleRange.y);
        transform.localScale = new Vector3(randomXScale, 1f, 1f);

        // Reset Y rotation (we rotate over time)
        transform.rotation = Quaternion.identity;
    }

    void LateUpdate()
    {
        Debug.Log("Puff active: " + name);
        timer += Time.deltaTime;

        // Move upward
        transform.Translate(moveDirection * riseSpeed * Time.deltaTime, Space.World);

        // Rotate around Y axis
        transform.Rotate(0f, yRotationSpeed * Time.deltaTime, 0f, Space.Self);

        // Disable after lifetime
        if (timer >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
    public void Setup(float riseSpeed, float lifetime, float yRotationSpeed, Vector2 xScaleRange, Vector2 yScaleRange, Vector3 moveDir, Color color)
    {
        this.riseSpeed = riseSpeed;
        this.lifetime = lifetime;
        this.yRotationSpeed = yRotationSpeed;
        float randomXScale = Random.Range(xScaleRange.x, xScaleRange.y);
        float randomYScale = Random.Range(yScaleRange.x, yScaleRange.y);
        transform.localScale = new Vector3(randomXScale, randomYScale, 1f);
        this.moveDirection = moveDir;
        spriteRenderer.color = color;
    }
}
