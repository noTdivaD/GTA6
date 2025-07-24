using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Panel Element")]
    [SerializeField] public GameObject pausePanel;

    void Awake()
    {
        // Link to GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.pauseManager = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find by name in the Canvas
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            Debug.Log("[PauseManager] Canvas has been found.");

            // Debug to list all children of the canvas
            foreach (Transform child in canvas.transform)
            {
                Debug.Log("[PauseManager] Canvas child: " + child.name);
            }

            GameObject found = GameManager.Instance.FindChildRecursive(canvas.transform, "PausePanel");
            if (found != null)
            {
                pausePanel = found;
                pausePanel.SetActive(false);
                Debug.Log("[PauseManager] PausePanel successfully assigned.");
            }
            else
            {
                Debug.LogWarning("[PauseManager] PausePanel not found in scene.");
            }
        }
        else
        {
            Debug.LogError("[PauseManager] Canvas is missing.");
            return;
        }

        // Start off as inactive
        pausePanel.SetActive(false);

        // Assign buttons
        AssignButtons();
    }

    public void Pause()
    {
        Debug.Log("[PauseManager] Pausing the game...");

        // Show the pause panel and freeze time
        if(pausePanel != null)
        {
            Debug.Log("[PauseManager] Pause success.");
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("[PauseManager] PausePanel is not assigned.");
            return;
        }
    }

    public void Resume()
    {
        Debug.Log("[PauseManager] Resuming the game...");

        // Hide the pause panel
        Time.timeScale = 1f;
        pausePanel.SetActive(false);

        GameObject canvas = GameObject.Find("Canvas");
        SettingsManager settings = GameManager.Instance.FindChildRecursive(canvas.transform, "OptionPanel").GetComponent<SettingsManager>();
        if (settings != null)
        {
            settings.CloseSettings();
        }
    }

    public void LoadMainMenu()
    {
        // Reset game state
        GameManager.Instance.ResetGameState();
        
        // Go back to the main menu
        SceneManager.LoadScene("StartMenu");
    }

    void AssignButtons()
    {
        Debug.Log("[PauseManager] Assigning buttons...");

        Transform buttonContainer = pausePanel.transform.Find("ButtonContainer");
        if (buttonContainer != null)
        {
            Button resumeButton = GameManager.Instance.FindChildRecursive(pausePanel.transform, "ResumeButton").GetComponent<Button>();
            Button optionsButton = GameManager.Instance.FindChildRecursive(pausePanel.transform, "OptionsButton").GetComponent<Button>();
            Button mainMenuButton = GameManager.Instance.FindChildRecursive(pausePanel.transform, "MainMenuButton").GetComponent<Button>();

            if (resumeButton != null)
            {
                resumeButton.onClick.RemoveAllListeners();
                resumeButton.onClick.AddListener(Resume);
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.RemoveAllListeners();
                mainMenuButton.onClick.AddListener(LoadMainMenu);
            }

            GameObject canvas = GameObject.Find("Canvas");
            if (optionsButton != null)
            {
                SettingsManager settings = GameManager.Instance.FindChildRecursive(canvas.transform, "OptionPanel").GetComponent<SettingsManager>();
                if (settings != null)
                {
                    Debug.Log("[PauseManager] OPTION BUTTON OK.");
                    optionsButton.onClick.RemoveAllListeners();
                    optionsButton.onClick.AddListener(settings.OpenSettings);
                }
                else
                {
                    Debug.LogWarning("[PauseManager] SettingsManager (OptionPanel) not found!");
                }
            }
            else {                 
                Debug.LogWarning("[PauseManager] OptionsButton not found in PausePanel.");
            }

            Debug.Log("[PauseManager] Buttons successfully assigned.");
        }
        else
        {
            Debug.LogWarning("[PauseManager] ButtonContainer not found under PausePanel.");
        }
    }
}
