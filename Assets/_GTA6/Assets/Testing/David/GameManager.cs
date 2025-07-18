using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Game over states
    private bool isGameOver = false;
    public GameOverUIManager gameOverUIManager;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Check if the game is over
        if (isGameOver)
            return;

        // TODO: Score tracking, etc.
    }

    public void GameOver()
    {
        // Prevent multiple game over calls
        if (isGameOver)
            return;

        // Set the game over state
        isGameOver = true;

        // Show the Game Over UI
        if (gameOverUIManager != null)
            gameOverUIManager.ShowGameOver();
        else
            Debug.LogWarning("GameOverUIManager is not assigned!");
    }

    public void ResetGameState()
    {
        // Reset the game state
        isGameOver = false;

        //TODO: Score reset, player position reset, etc.
    }
}
