using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game over variables
    [Header("Game Over settings")]
    [SerializeField] private bool isGameOver = false;
    [SerializeField] public GameOverUIManager gameOverUIManager;

    // Singleton instance
    public static GameManager Instance
    {
        get;
        private set;
    }

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
        // End of service if the game is over :(
        if (isGameOver)
            return;

        // TODO: Score tracking, etc.
    }

    public void GameOver()
    {
        // If already game over why game over again
        if (isGameOver)
            return;

        isGameOver = true;

        if (gameOverUIManager != null)
            gameOverUIManager.ShowGameOver();
        else
            Debug.LogWarning("singleton linking bug (GameManager.cs)");
    }

    public void ResetGameState()
    {
        // Reset every state here
        isGameOver = false;
    }
}
