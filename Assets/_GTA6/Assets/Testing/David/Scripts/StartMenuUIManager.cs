using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUIManager : MonoBehaviour
{
    // UI components and variables
    [Header ("UI Elements")] 
    [SerializeField] private GameObject pressAnyButtonPanel;
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private bool hasPressedButton = false;

    void Start()
    {
        pressAnyButtonPanel.SetActive(true);
        startMenuPanel.SetActive(false);
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
        SceneManager.LoadScene("Jawad");
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
