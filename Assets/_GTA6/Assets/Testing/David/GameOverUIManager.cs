using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    // Reference to the Game Over UI panel
    public GameObject gameOverUI;

    // Reference to the CanvasGroup for fading effects
    private CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    void Start()
    {
        // The game over UI is inactive at the start
        gameOverUI.SetActive(false);

        // Get CanvasGroup component
        canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameOverUI.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        // Get GameManager singleton instance
        if (GameManager.Instance != null)
            GameManager.Instance.gameOverUIManager = this;
    }

    // Show the Game Over UI with delay and fade
    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        StartCoroutine(GameOverDelay());
    }

    private System.Collections.IEnumerator GameOverDelay()
    {
        // Wait for the delay
        yield return new WaitForSecondsRealtime(1f);

        // Game over activation
        yield return StartCoroutine(FadeInGameOverUI());
    }

    // Fade In
    private System.Collections.IEnumerator FadeInGameOverUI()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) // Incremental fade in
        {
            elapsedTime += Time.unscaledDeltaTime; // Increment time
            float t = elapsedTime / fadeDuration; // Calculate normalized time
            canvasGroup.alpha = Mathf.SmoothStep(0, 1, t); // SmoothStep works like Lerp
            yield return null;
        }
    }

    // Restart button
    public void RestartGame()
    {
        // Reset the game state and reload the current scene
        GameManager.Instance.ResetGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quit button
    public void QuitGame()
    {
        // Quit the game application
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}