using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over Panel Element")]
    [SerializeField] private GameObject gameOverPanel;

    void Awake()
    {
        // Link to GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.gameOverManager = this;

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
            Debug.Log("[GameOverManager] Canvas has been found.");

            GameObject found = GameManager.Instance.FindChildRecursive(canvas.transform, "GameOverPanel");
            if (found != null)
            {
                gameOverPanel = found;
                gameOverPanel.SetActive(false);
                Debug.Log("[GameOverManager] GameOverPanel successfully assigned.");
            }
            else
                Debug.LogWarning("[GameOverManager] GameOverPanel not found in scene.");
        }
        else
        {
            Debug.LogError("[GameOverManager] Canvas is missing.");
            return;
        }

        // Start off as inactive and hiddens
        gameOverPanel.SetActive(false);
        gameOverPanel.GetComponent<CanvasGroup>().alpha = 0f;

        // Assign buttons
        AssignButtons();
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        StartCoroutine(FadeInGameOverUI());
    }

    private System.Collections.IEnumerator FadeInGameOverUI()
    {
        // Small delay before showing the Game Over
        yield return new WaitForSecondsRealtime(1f);

        // Fade in the Game Over UI
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alphaValue = elapsedTime / 1f;
            gameOverPanel.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, alphaValue);
            yield return null;
        }
    }

    public void RestartGame()
    {
        Debug.Log("[GameOverManager] Restarting the game...");

        // Reset game state
        if (GameManager.Instance != null)
        {
            Debug.Log("[GameOverManager] Game state has reloaded.");
            GameManager.Instance.ResetGameState();
        }
        else
            Debug.LogWarning("[GameOverManager] Game state has failed to reload.");

        // Reload the scene
        Debug.Log("[GameOverManager] Scene reloaded: " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Debug.Log("[GameOverManager] Loading main menu...");

        // Reset game state
        if (GameManager.Instance != null)
        {
            Debug.Log("[GameOverManager] Game state has been reset.");
            GameManager.Instance.ResetGameState();
        }
        else
            Debug.LogWarning("[GameOverManager] Game state has not been reset.");

        // Go back to the main menu
        SceneManager.LoadScene("StartMenu");
    }

    void AssignButtons()
    {
        Debug.Log("[GameOverManager] Assigning buttons...");

        Transform buttonContainer = gameOverPanel.transform.Find("ButtonContainer");
        if (buttonContainer != null)
        {
            Button retryButton = buttonContainer.Find("RetryButton").GetComponent<Button>();
            Button mainMenuButton = buttonContainer.Find("MainMenuButton").GetComponent<Button>();

            if (retryButton != null)
                retryButton.onClick.AddListener(RestartGame);

            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(LoadMainMenu);

            Debug.Log("[GameOverManager] Buttons successfully assigned.");
        }
        else
        {
            Debug.LogWarning("[GameOverManager] ButtonContainer not found under GameOverPanel.");
        }
    }
}
