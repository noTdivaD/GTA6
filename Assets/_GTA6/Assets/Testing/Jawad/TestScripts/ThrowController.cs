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

    private float directionAngle = 0f;
    private bool hasThrown = false;
    private bool isDecidingDirection = false;
    private bool hasLockedDirection = false;
    private float waitTime = 0f;
    private float timer = 0f;
    private int directionSign = 1;
    public float throwElevation = 45f; // <-- Make this public and set a default

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        UpdateTrajectoryLine();
        if (throwTimerBar != null) throwTimerBar.value = 0f;
    }

    void Update()
    {
        if (!isDecidingDirection && Input.GetKeyDown(KeyCode.Space))
        {
            isDecidingDirection = true;
            timer = 0f;
            waitTime = Random.Range(2f, 5f);
            hasPlayedUppercut = false;
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

            if (throwTimerBar != null)
                throwTimerBar.value = Mathf.Clamp01(timer / waitTime);

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

                UpdateTrajectoryLine();
            }
            else if (!hasLockedDirection)
            {
                hasLockedDirection = true;
                UpdateTrajectoryLine();
                Debug.Log("Direction locked at " + directionAngle.ToString("F2") + "°");
            }

            if (!hasPlayedUppercut && timer >= waitTime - 0.5f)
            {
                hasPlayedUppercut = true;
                animator.SetTrigger("PlayUppercut");
            }

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
            rb.isKinematic = true;
            grandmaObject.transform.position = throwPoint.position;
            Vector3 horizontalDirection = Quaternion.Euler(0f, directionAngle, 0f) * Vector3.forward;
            grandmaObject.transform.rotation = Quaternion.LookRotation(horizontalDirection, Vector3.up);

            rb.isKinematic = false;
            rb.linearDamping = 0.2f;
            rb.angularDamping = 2f;
            Physics.gravity = new Vector3(0, -1.5f, 0);
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            Vector3 launchDirection = GetLaunchDirection().normalized;
            rb.AddForce(launchDirection * throwForce, ForceMode.Impulse);

            GrandmaAirControl airControl = grandmaObject.GetComponent<GrandmaAirControl>();
            if (airControl != null)
            {
                airControl.StartFlying();
            }

            Animator grandmaAnim = grandmaObject.GetComponentInChildren<Animator>();
            if (grandmaAnim != null)
            {
                grandmaAnim.SetTrigger("Fly");
            }

            GrandmaCameraFollow camFollow = Camera.main.GetComponent<GrandmaCameraFollow>();
            if (camFollow != null)
            {
                camFollow.target = grandmaObject.transform;
            }
        }
        rb.linearVelocity = GetLaunchDirection().normalized * throwForce;

    }



    void UpdateTrajectoryLine()
    {
        Vector3 velocity = GetLaunchDirection().normalized * throwForce;

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
    // Helper to get launch direction
    private Vector3 GetLaunchDirection()
    {
        Quaternion yaw = Quaternion.Euler(0f, directionAngle, 0f);
        Vector3 forwardDir = yaw * Vector3.forward;
        Vector3 launchDir = (forwardDir + Vector3.up * 0.5f).normalized; // Less vertical
        return launchDir;
    }
}
