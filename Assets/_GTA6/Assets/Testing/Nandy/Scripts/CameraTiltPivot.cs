using UnityEngine;

public class CameraTiltPivot : MonoBehaviour
{
    [Header("Settings")]
    public float tiltSensitivity = 50f;
    public float minPitch = -30f;
    public float maxPitch = 30f;
    public float returnDelay = 0.5f;
    public float returnSpeed = 4f;

    private float pitch = 0f;
    private float returnTimer = 0f;
    private bool isReturning = false;

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
                pitch = Mathf.Lerp(pitch, 0f, Time.deltaTime * returnSpeed);
                ApplyRotation();

                if (Mathf.Abs(pitch) < 0.1f)
                {
                    pitch = 0f;
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
