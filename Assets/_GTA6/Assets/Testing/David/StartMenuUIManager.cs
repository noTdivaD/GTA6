using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUIManager : MonoBehaviour
{
    // Reference to the UI elements
    public GameObject pressAnyButtonPanel;
    public GameObject startMenuPanel;
    private bool hasPressedButton = false;

    void Start()
    {
        // Show the "Press Any Button" panel and hide the Start Menu
        pressAnyButtonPanel.SetActive(true);
        startMenuPanel.SetActive(false);
    }

    void Update()
    {
        // Check for any button pressed to show the Start Menu
        if (!hasPressedButton && Input.anyKeyDown)
        {
            hasPressedButton = true;
            StartCoroutine(ShowStartMenu());
        }
    }

    private System.Collections.IEnumerator ShowStartMenu()
    {
        // Press Any Button     Fade out
        CanvasGroup pressGroup = pressAnyButtonPanel.GetComponent<CanvasGroup>();
        if (pressGroup != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                pressGroup.alpha = Mathf.Lerp(1f, 0f, t);
                yield return null;
            }
        }

        // Hide the "Press Any Button" panel and show the Start Menu
        pressAnyButtonPanel.SetActive(false);
        startMenuPanel.SetActive(true);

        // Start Menu Fade In
        CanvasGroup menuGroup = startMenuPanel.GetComponent<CanvasGroup>();
        if (menuGroup != null)
        {
            menuGroup.alpha = 0f;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                menuGroup.alpha = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }
        }
    }

    // Play Button
    public void PlayGame()
    {
        // TODO: Load the game scene
        //SceneManager.LoadScene("_______");
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
