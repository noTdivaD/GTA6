using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class DynamicCameraEffects : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public GrandmaAirControl grandmaController;
    private Rigidbody targetRb;
    private CinemachineCamera virtualCamera;
    private CinemachineCameraOffset cameraOffsetExtension;
    public CinemachineThirdPersonFollow thirdPersonFollow;

    [Header("1. Horizontal Offset")]
    public float maxCameraSideOffset = 1f;
    public float offsetSmoothTime = 0.2f;
    public float lateralDeadzone = 0.2f;
    private float currentCameraSideOffset;
    private float currentSideVelocity;

    [Header("2. Zoom by Speed")]
    public float minDistance = 2f;
    public float maxDistance = 6f;
    public float zoomLerpSpeed = 5f;
    public float maxSpeedForZoom = 10f;

    [Header("3. Vertical Tilt by Acceleration")]
    public float maxTiltAngle = 10f;
    public float tiltSmoothTime = 0.2f;
    private float tiltVelocity;
    private Vector3 previousVelocity;

    [Header("4. Shoulder Tilt Settings")]
    public float minShoulderY = -1.5f;
    public float maxShoulderY = 3f;
    public float sensitivity = 0.5f;
    public float returnDelay = 0.5f;
    public float returnSpeed = 2f;
    public float shoulderTiltMultiplier = 0.5f;




    private float currentShoulderY;
    private float defaultShoulderY;
    private float returnTimer = 0f;
    private bool isReturning = false;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineCamera>();

        cameraOffsetExtension = virtualCamera.GetComponent<CinemachineCameraOffset>();
        if (cameraOffsetExtension == null)
        {
            cameraOffsetExtension = virtualCamera.gameObject.AddComponent<CinemachineCameraOffset>();
            cameraOffsetExtension.ApplyAfter = CinemachineCore.Stage.Body;
        }

        if (target == null)
            Debug.LogError("Target not assigned for DynamicCameraEffects!");

        targetRb = target.GetComponent<Rigidbody>();
        previousVelocity = targetRb.linearVelocity;
        if (thirdPersonFollow == null)
        {
            thirdPersonFollow = GetComponent<CinemachineCamera>().GetComponent<CinemachineThirdPersonFollow>();
        }
        defaultShoulderY = thirdPersonFollow.ShoulderOffset.y;
        currentShoulderY = defaultShoulderY;
    }

    void LateUpdate()
    {
        if (target == null || targetRb == null || cameraOffsetExtension == null)
            return;

        Vector3 velocity = targetRb.linearVelocity;
        Vector3 cameraRight = virtualCamera.transform.right;
        float lateralVelocity = Vector3.Dot(velocity, cameraRight);

        Vector3 acceleration = (velocity - previousVelocity) / Time.deltaTime;
        previousVelocity = velocity;

        float speed = velocity.magnitude;

        // 1. Horizontal Camera Offset using Extension
        float normalizedLateral = Mathf.Clamp(lateralVelocity / maxSpeedForZoom, -1f, 1f);

        // Only apply smoothing when outside deadzone
        float targetXOffset = Mathf.Abs(normalizedLateral) > lateralDeadzone
            ? -normalizedLateral * maxCameraSideOffset
            : currentCameraSideOffset;  // don't snap back to center suddenly

        float smoothedXOffset = Mathf.SmoothDamp(
            currentCameraSideOffset,
            targetXOffset,
            ref currentSideVelocity,
            offsetSmoothTime
        );

        currentCameraSideOffset = smoothedXOffset;

        cameraOffsetExtension.Offset = new Vector3(smoothedXOffset, 0f, 0f);

        // 2. Zoom based on speed
        float speedPercent = Mathf.Clamp01(speed / maxSpeedForZoom);
        float targetDistance = Mathf.Lerp(minDistance, maxDistance, speedPercent);

        var thirdPersonFollow = virtualCamera.GetComponent<CinemachineThirdPersonFollow>();
        if (thirdPersonFollow != null)
        {
            thirdPersonFollow.CameraDistance = Mathf.Lerp(
                thirdPersonFollow.CameraDistance,
                targetDistance,
                Time.deltaTime * zoomLerpSpeed
            );
        }

        // 3. Vertical Tilt based on Z acceleration
        //if (thirdPersonFollow != null)
        //{
        //    float accelZ = acceleration.z;

        //    float targetShoulderY = Mathf.Clamp(
        //        thirdPersonFollow.ShoulderOffset.y - accelZ * shoulderTiltMultiplier,
        //        minShoulderY,
        //        maxShoulderY
        //    );

        //    float smoothedShoulderY = Mathf.SmoothDamp(
        //        thirdPersonFollow.ShoulderOffset.y,
        //        targetShoulderY,
        //        ref tiltVelocity,
        //        tiltSmoothTime
        //    );

        //    thirdPersonFollow.ShoulderOffset = new Vector3(
        //        thirdPersonFollow.ShoulderOffset.x,
        //        smoothedShoulderY,
        //        thirdPersonFollow.ShoulderOffset.z
        //    );
        //}

        bool rmbHeld = Input.GetMouseButton(1);
        if (rmbHeld) // Right Mouse Button
        {
            float mouseY = Input.GetAxis("Mouse Y");
            currentShoulderY -= mouseY * sensitivity;
            currentShoulderY = Mathf.Clamp(currentShoulderY, minShoulderY, maxShoulderY);

            ApplyShoulderOffset(currentShoulderY);

            // Reset return timer while RMB is held
            returnTimer = 0f;
            isReturning = false;
        }
        else
        {
            // Increment timer after releasing RMB
            returnTimer += Time.deltaTime;

            if (returnTimer >= returnDelay)
            {
                isReturning = true;
            }
        }
        if (isReturning)
        {
            currentShoulderY = Mathf.Lerp(currentShoulderY, defaultShoulderY, Time.deltaTime * returnSpeed);
            ApplyShoulderOffset(currentShoulderY);

            // Stop returning if very close to default
            if (Mathf.Abs(currentShoulderY - defaultShoulderY) < 0.01f)
            {
                currentShoulderY = defaultShoulderY;
                isReturning = false;
                returnTimer = 0f;
            }
        }
    }
    void ApplyShoulderOffset(float yValue)
    {
        var offset = thirdPersonFollow.ShoulderOffset;
        offset.y = yValue;
        thirdPersonFollow.ShoulderOffset = offset;
    }
}
