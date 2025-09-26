using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class RescueGameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RescueSpawnDirector rescueSpawnDirector;
    [SerializeField] private SpawnDirector enemySpawnDirector;
    [SerializeField] private RunStats runStats;
    [SerializeField] private GameTuning gameTuning;
    [SerializeField] private PlayerConfig playerConfig;
    
    [Header("Game State")]
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private bool isPaused = false;
    
    [Header("Player")]
    [SerializeField] private Health playerHealth;
    
    public bool IsGameOver => isGameOver;
    public bool IsPaused => isPaused;
    
    public static event Action OnGameOver;
    public static event Action<bool> OnPauseChanged;
    
    void Start()
    {
        // Find player health if not assigned
        if (!playerHealth)
            playerHealth = FindObjectOfType<Health>();
            
        // Subscribe to player death
        if (playerHealth)
        {
            Health.OnHealthDepleted += OnPlayerDied;
        }
        
        // Start the game
        StartGame();
    }
    
    void OnDestroy()
    {
        if (playerHealth)
        {
            Health.OnHealthDepleted -= OnPlayerDied;
        }
    }
    
    void Update()
    {
        // Handle pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        // Handle restart input when game over
        if (isGameOver && Input.anyKeyDown)
        {
            RestartGame();
        }
    }
    
    private void StartGame()
    {
        isGameOver = false;
        isPaused = false;
        Time.timeScale = 1f;
        
        // Reset stats
        if (runStats) runStats.ResetStats();
        
        // Enable spawning
        if (rescueSpawnDirector) rescueSpawnDirector.enabled = true;
        if (enemySpawnDirector) enemySpawnDirector.enabled = true;
    }
    
    private void OnPlayerDied(Health health)
    {
        if (isGameOver || health != playerHealth) return;
        
        isGameOver = true;
        Time.timeScale = 0f;
        
        // Disable spawning
        if (rescueSpawnDirector) rescueSpawnDirector.enabled = false;
        if (enemySpawnDirector) enemySpawnDirector.enabled = false;
        
        OnGameOver?.Invoke();
    }
    
    public void TogglePause()
    {
        if (isGameOver) return;
        
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        
        OnPauseChanged?.Invoke(isPaused);
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
