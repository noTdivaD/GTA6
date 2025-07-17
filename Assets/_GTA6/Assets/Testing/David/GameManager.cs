using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Game over state
    bool isGameOver = false;
    public GameOverUIManager gameOverUIManager;

    void Start()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(isGameOver)
        {
            return;
        }
    }

    public void GameOver()
    {
        Debug.Log("die karen die");
        isGameOver = true;

        if (gameOverUIManager != null)
        {
            gameOverUIManager.ShowGameOver();
        }
    }

    public void ResetGameState()
    {
        isGameOver = false;
    }
}
