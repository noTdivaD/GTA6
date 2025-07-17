using Unity.Cinemachine;
using UnityEngine;

public class CameraOffsetController : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public Transform target;
    public float maxCameraSideOffset = 1f;
    public float offsetSmoothTime = 0.2f;

    private CinemachineThirdPersonFollow followComponent;
    private float currentCameraSideVelocity = 0f;

    void Start()
    {
        followComponent = virtualCamera.GetComponent<CinemachineThirdPersonFollow>();
    }

    void LateUpdate()
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        Vector3 localVelocity = target.InverseTransformDirection(rb.linearVelocity);

        // Move the camera opposite to horizontal movement
        float targetCameraSide = -Mathf.Clamp(localVelocity.x, -1f, 1f) * maxCameraSideOffset;

        followComponent.CameraSide = Mathf.SmoothDamp(
            followComponent.CameraSide,
            targetCameraSide,
            ref currentCameraSideVelocity,
            offsetSmoothTime
        );
    }
}

