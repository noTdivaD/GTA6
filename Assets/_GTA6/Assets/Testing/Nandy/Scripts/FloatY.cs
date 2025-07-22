using UnityEngine;

public class FloatY : MonoBehaviour
{
    public float amplitude = 0.25f;     // How high it moves
    public float frequency = 1f;        // How fast it oscillates

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0f, offsetY, 0f);
    }
}

