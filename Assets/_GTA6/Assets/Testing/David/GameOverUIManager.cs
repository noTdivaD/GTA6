using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    // Game Over UI (Panel)
    public GameObject gameOverUI;

    // Fade In variables
    private CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float delayValue = 1f;

    void Start()
    {
        // Hidden on start
        gameOverUI.SetActive(false);

        // Faded out on start
        canvasGroup = gameOverUI.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        // Link to singleton GameManager (if there are any restart issues this is probably the cause)
        if (GameManager.Instance != null)
            GameManager.Instance.gameOverUIManager = this;
    }

    public void ShowGameOver()
    {
        // Show Game Over after slight delay
        gameOverUI.SetActive(true);
        StartCoroutine(GameOverDelay());
    }

    private System.Collections.IEnumerator GameOverDelay()
    {
        // Wait for the delay
        yield return new WaitForSecondsRealtime(delayValue);

        // Game over activation
        yield return StartCoroutine(FadeInGameOverUI());
    }

    // Fade In
    private System.Collections.IEnumerator FadeInGameOverUI()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Increment time
            float alphaValue = elapsedTime / fadeDuration; // As time passes, visibility increases
            canvasGroup.alpha = Mathf.SmoothStep(0, 1, alphaValue); // SmoothStep works like Lerp
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