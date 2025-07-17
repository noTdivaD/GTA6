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
        float normalizedLateral = Mathf.Abs(lateralVelocity) >= lateralDeadzone
            ? Mathf.Clamp(lateralVelocity / maxSpeedForZoom, -1f, 1f)
            : 0f;

        float targetXOffset = -normalizedLateral * maxCameraSideOffset;

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
        float targetTilt = Mathf.Clamp(-acceleration.z * maxTiltAngle, -maxTiltAngle, maxTiltAngle);
        float currentTilt = Mathf.SmoothDampAngle(
            virtualCamera.transform.eulerAngles.x,
            targetTilt,
            ref tiltVelocity,
            tiltSmoothTime
        );

        Quaternion targetRotation = Quaternion.Euler(currentTilt, virtualCamera.transform.eulerAngles.y, 0);
        virtualCamera.transform.rotation = targetRotation;
    }
}
