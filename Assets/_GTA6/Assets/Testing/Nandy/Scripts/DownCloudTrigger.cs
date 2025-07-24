using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class DownCloudTrigger : MonoBehaviour
{
    [Header("Geyser Launch Settings")]
    public float downwardForce = 12f;
    public float slowMotionDuration = 0.2f; // Duration of slow motion effect
    public float slowMotionScale = 0.3f; // Scale of time during slow motion

    public string playerTag = "Player";
    private CinemachineImpulseSource impulseSource;
    public string downCloudSFXName = "DownCloudSFX"; // Name of the down cloud sound effect
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                Vector3 launchDirection = Vector3.down * downwardForce;
                rb.AddForce(launchDirection, ForceMode.VelocityChange);
                CinemachineImpulseSource impulse = GetComponent<CinemachineImpulseSource>();
                if (impulse != null)
                    impulse.GenerateImpulse();
                if (downCloudSFXName != null && downCloudSFXName != "")
                {
                    AudioClip downCloudCLip = AudioManager.Instance.GetClipByName(downCloudSFXName, AudioManager.Instance.sfxClips);
                    if (downCloudCLip != null)
                    {
                        AudioManager.Instance.PlaySFX(downCloudCLip);
                    }
                }
                // Slow time
                StartCoroutine(ShortSlowMotion());//  Trigger camera shake

                Debug.Log("Grandma boosted by geyser!");
            }
        }
    }

    private IEnumerator ShortSlowMotion()
    {
        float originalTimeScale = Time.timeScale;
        float originalFixedDeltaTime = Time.fixedDeltaTime;

        Time.timeScale = slowMotionScale; // Slow down to x%
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust physics timing

        yield return new WaitForSecondsRealtime(slowMotionDuration); // How long the slow-mo lasts in real time

        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }
}
