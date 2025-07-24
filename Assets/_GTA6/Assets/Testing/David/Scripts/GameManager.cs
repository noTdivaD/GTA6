using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game State settings")]
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private bool isPaused = false;

    [Header("UI Managers")]
    [HideInInspector] public GameOverManager gameOverManager;
    [HideInInspector] public PauseManager pauseManager;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Debug.Log("[GameManager] Awake called. Instance: " + Instance);

        // Singleton
        if (Instance == null)
        {
            Debug.Log("[GameManager] Setting instance.");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Debug.LogWarning("[GameManager] Duplicate detected, destroying.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load StartMenu immediately after initializing GameManager
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Additive);
    }

    void Update()
    {
        // Check for Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                isPaused = false;
                if (pauseManager != null)
                    pauseManager.Resume();
                else
                    Debug.LogWarning("PauseManager is not linked.");
            }
            else
            {
                isPaused = true;
                if (pauseManager != null)
                    pauseManager.Pause();
                else
                    Debug.LogWarning("PauseManager is not linked.");
            }
        }
    }

    void OnDisable()
    {
        Debug.LogWarning("[GameManager] was disabled!");
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Debug.Log("[GameManager] Destroying instance.");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[GameManager] Scene loaded: " + scene.name);

        if (!gameObject.activeSelf)
        {
            Debug.LogWarning("[GameManager] was deactivated. Reactivating...");
            gameObject.SetActive(true);
        }

        StartCoroutine(ReconnectManagersNextFrame());
    }

    private System.Collections.IEnumerator ReconnectManagersNextFrame()
    {
        // Wait one frame to ensure all objects are fully loaded
        yield return null;

        gameOverManager = FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null)
            Debug.Log("[GameManager] GameOverManager has been re-linked.");
        else
            Debug.LogWarning("[GameManager] GameOverManager not found in scene.");

        pauseManager = FindFirstObjectByType<PauseManager>();
        if (pauseManager != null)
            Debug.Log("[GameManager] PauseManager has been re-linked.");
        else
            Debug.LogWarning("[GameManager] PauseManager not found in scene.");
    }

    public void GameOver()
    {
        // Avoid multiple calls
        if (isGameOver)
            return;

        isGameOver = true;
        if (gameOverManager != null)
            gameOverManager.ShowGameOver();
        else
            Debug.LogWarning("GameOverManager is not linked.");
    }

    public void ResetGameState()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;

        if (gameOverManager != null && gameOverManager.gameObject != this.gameObject)
            gameOverManager.gameObject.SetActive(false);

        if (pauseManager != null && pauseManager.pausePanel != this.gameObject)
            pauseManager.pausePanel.SetActive(false);
    }

    public GameObject FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child.gameObject;

            GameObject result = FindChildRecursive(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
}
