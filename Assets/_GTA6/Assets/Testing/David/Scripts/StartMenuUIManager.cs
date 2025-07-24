using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUIManager : MonoBehaviour
{
    // UI components and variables
    [Header ("UI Elements")] 
    [SerializeField] private GameObject pressAnyButtonPanel;
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private bool hasPressedButton = false;
    [SerializeField] private Camera cameraComponent;
    [SerializeField] private Vector3 targetPosition;
    public string karenName = "MyNameKaren";

    void Start()
    {
        pressAnyButtonPanel.SetActive(true);
        startMenuPanel.SetActive(false);

        targetPosition = new Vector3(0f, 0f, 45f);
    }

    void Update()
    {
        // Check button press once
        if (!hasPressedButton && Input.anyKeyDown)
        {
            hasPressedButton = true;
            StartCoroutine(ShowStartMenu());
        }
    }

    private System.Collections.IEnumerator ShowStartMenu()
    {
        // Fade out
        CanvasGroup pressGroup = pressAnyButtonPanel.GetComponent<CanvasGroup>();
        if (pressGroup != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                pressGroup.alpha = Mathf.SmoothStep(1f, 0f, t);
                yield return null;
            }
        }

        // Switch scene

        pressAnyButtonPanel.SetActive(false);
        startMenuPanel.SetActive(true);
        if (karenName != null && karenName != "")
        {
            AudioClip karenClip = AudioManager.Instance.GetClipByName(karenName, AudioManager.Instance.characterSfxClips);
            if (karenClip != null)
            {
                AudioManager.Instance.PlayCharacterSFX(karenClip);
            }


        }

        // Camera moves forward with Lerp
        cameraComponent.transform.position = Vector3.Lerp(cameraComponent.transform.position, targetPosition, 0.1f);

        // Fade in
        CanvasGroup menuGroup = startMenuPanel.GetComponent<CanvasGroup>();
        if (menuGroup != null)
        {
            menuGroup.alpha = 0f;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                menuGroup.alpha = Mathf.SmoothStep(0f, 1f, t);
                yield return null;
            }
        }
    }

    // Play Button
    public void PlayGame()
    {
        // remember to add the scene to build settings
        SceneManager.LoadScene("MainScene");
    }

    // Quit Button
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
