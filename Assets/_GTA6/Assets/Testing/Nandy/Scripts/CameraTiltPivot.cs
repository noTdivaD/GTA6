using UnityEngine;

public class CameraTiltPivot : MonoBehaviour
{
    [Header("Settings")]
    public float tiltSensitivity = 50f;
    public float minPitch = -30f;
    public float maxPitch = 30f;
    public float returnDelay = 0.5f;
    public float returnSpeed = 4f;

    [Header("Default Rotation")]
    public float defaultPitch = 20f;

    private float pitch = 0f;
    private float returnTimer = 0f;
    private bool isReturning = false;

    void Start()
    {
        // Start from the default rotation
        pitch = defaultPitch;
        ApplyRotation();
    }
    void Update()
    {
        bool rmbHeld = Input.GetMouseButton(1);

        if (rmbHeld)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            pitch -= mouseY * tiltSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            ApplyRotation();

            isReturning = false;
            returnTimer = 0f;
        }
        else
        {
            returnTimer += Time.deltaTime;

            if (returnTimer >= returnDelay)
            {
                isReturning = true;
            }

            if (isReturning)
            {
                pitch = Mathf.Lerp(pitch, defaultPitch, Time.deltaTime * returnSpeed);
                ApplyRotation();

                if (Mathf.Abs(pitch - defaultPitch) < 0.1f)
                {
                    pitch = defaultPitch;
                    isReturning = false;
                }
            }
        }
    }

    void ApplyRotation()
    {
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
