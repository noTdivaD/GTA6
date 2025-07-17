using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivityX = 150f;
    [SerializeField] private float mouseSensitivityY = 100f;
    [SerializeField] private float minY = -30f;
    [SerializeField] private float maxY = 60f;

    private PlayerInputActions inputActions;
    private Vector2 lookInput;
    private float xRotation = 0f; // pitch
    private bool cursorLocked = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();


    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            cursorLocked = !cursorLocked;

            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !cursorLocked;
        }
    }
    private void LateUpdate()
    {
        float mouseX = lookInput.x * mouseSensitivityX * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        // Rotate player around Y axis
        playerBody.Rotate(Vector3.up * mouseX);

        // Apply vertical tilt to camera
        virtualCamera.transform.localEulerAngles = new Vector3(xRotation, virtualCamera.transform.localEulerAngles.y, 0f);
    }
}
