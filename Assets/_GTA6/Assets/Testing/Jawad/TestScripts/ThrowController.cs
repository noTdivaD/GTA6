using UnityEngine;
using UnityEngine.UI;

public class ThrowController : MonoBehaviour
{

    private Animator animator;
    private bool hasPlayedUppercut = false;

    [Header("References")]
    public LineRenderer lineRenderer;
    public GameObject grandmaObject;
    public Transform throwPoint;
    public Slider throwTimerBar;

    [Header("Throw Settings")]
    public float throwForce = 20f;
    public float maxAngle = 45f;
    public int trajectoryPoints = 30;
    public float timeStep = 0.1f;
    public float directionChangeSpeed = 40f;
    public float pauseBeforeThrow = 0.5f;

    // Internal State
    private float directionAngle = 0f;
    private bool hasThrown = false;
    private bool isDecidingDirection = false;
    private bool hasLockedDirection = false;
    private float waitTime = 0f;
    private float timer = 0f;
    private int directionSign = 1;

    void Start()
    {

        animator = GetComponentInChildren<Animator>();


        // Show static straight line initially
        UpdateTrajectoryLine(0f);

        // Reset progress bar
        if (throwTimerBar != null)
            throwTimerBar.value = 0f;
    }

    void Update()
    {
        if (!isDecidingDirection && Input.GetKeyDown(KeyCode.Space))
        {
            isDecidingDirection = true;
            timer = 0f;
            waitTime = Random.Range(2f, 5f);
            hasPlayedUppercut = false; // reset animation trigger flag
        }

        if (hasThrown) return;

        if (!isDecidingDirection)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isDecidingDirection = true;
                waitTime = Random.Range(2f, 5f);
                timer = 0f;
                directionAngle = 0f;
                directionSign = Random.Range(0, 2) == 0 ? -1 : 1;
                hasLockedDirection = false;
                hasPlayedUppercut = false;

                if (throwTimerBar != null)
                    throwTimerBar.value = 0f;

                Debug.Log("Demon is deciding... Throwing in " + waitTime.ToString("F2") + " seconds.");
            }
        }
        else
        {
            timer += Time.deltaTime;
            float timeLeft = waitTime - timer;

            // Update progress bar
            if (throwTimerBar != null)
                throwTimerBar.value = Mathf.Clamp01(timer / waitTime);

            // Rotate direction while deciding
            if (timeLeft > pauseBeforeThrow)
            {
                directionAngle += directionSign * directionChangeSpeed * Time.deltaTime;

                if (directionAngle > maxAngle)
                {
                    directionAngle = maxAngle;
                    directionSign = -1;
                }
                else if (directionAngle < -maxAngle)
                {
                    directionAngle = -maxAngle;
                    directionSign = 1;
                }

                UpdateTrajectoryLine(directionAngle);
            }
            else if (!hasLockedDirection)
            {
                hasLockedDirection = true;
                UpdateTrajectoryLine(directionAngle); // Freeze line
                Debug.Log("Direction locked at " + directionAngle.ToString("F2") + "°");
            }

            // 🔥 Trigger animation just before throw
            if (!hasPlayedUppercut && timer >= waitTime - 0.5f)
            {
                hasPlayedUppercut = true;
                animator.SetTrigger("PlayUppercut");
            }

            // Time to throw
            if (timer >= waitTime)
            {
                isDecidingDirection = false;
                ThrowGrandma();
                hasThrown = true;
                HideTrajectory();

                if (throwTimerBar != null)
                    throwTimerBar.value = 1f;
            }
        }
    }


    void ThrowGrandma()
    {
        Rigidbody rb = grandmaObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 aimDirection = Quaternion.Euler(0f, directionAngle, 0f) * Vector3.forward;
            Vector3 launchDirection = (aimDirection + Vector3.up).normalized;

            rb.isKinematic = false;
            rb.linearDamping = 0.2f;
            rb.angularDamping = 2f;
            Physics.gravity = new Vector3(0, -1.5f, 0); // light gravity
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            rb.AddForce(launchDirection * throwForce, ForceMode.Impulse);

            // Prevent bouncing
            Collider col = grandmaObject.GetComponent<Collider>();
            if (col != null)
            {
                PhysicsMaterial noBounce = new PhysicsMaterial();
                noBounce.bounciness = 0f;
                noBounce.bounceCombine = PhysicsMaterialCombine.Minimum;
                noBounce.frictionCombine = PhysicsMaterialCombine.Average;
                col.material = noBounce;
            }

            // Activate grandma flight control
            GrandmaAirControl airControl = grandmaObject.GetComponent<GrandmaAirControl>();
            if (airControl != null)
            {
                airControl.StartFlying();
            }

            // Camera follow
            GrandmaCameraFollow camFollow = Camera.main.GetComponent<GrandmaCameraFollow>();
            if (camFollow != null)
            {
                camFollow.target = grandmaObject.transform;
            }

            Debug.Log("THROW! Grandma launched at angle: " + directionAngle.ToString("F2") + "°");
        }
    }

    void UpdateTrajectoryLine(float angle)
    {
        Vector3 aimDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
        Vector3 launchDirection = (aimDirection + Vector3.up).normalized;
        Vector3 velocity = launchDirection * throwForce;

        lineRenderer.positionCount = trajectoryPoints;
        Vector3[] points = new Vector3[trajectoryPoints];

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * timeStep;
            Vector3 point = throwPoint.position + velocity * t + 0.5f * Physics.gravity * t * t;
            points[i] = point;
        }

        lineRenderer.SetPositions(points);
    }

    void HideTrajectory()
    {
        lineRenderer.positionCount = 0;
    }
}
