using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpawnDirector spawnDirector;
    [SerializeField] private RescueSpawnDirector rescueSpawnDirector;
    [SerializeField] private RunStats runStats;
    [SerializeField] private GameTuning gameTuning;
    [SerializeField] private PlayerConfig playerConfig;
    
    [Header("Single Player Mode")]
    [SerializeField] private bool usePartySystem = false;
    [SerializeField] private Health playerHealth;
    
    [Header("Game State")]
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private bool isPaused = false;
    
    public bool IsGameOver => isGameOver;
    public bool IsPaused => isPaused;
    
    public static event Action OnGameOver;
    public static event Action<bool> OnPauseChanged;
    
    void Start()
    {
        // Subscribe to events based on mode
        if (usePartySystem)
        {
            PartyController.OnPartyWiped += OnPartyWiped;
        }
        else
        {
            // Single player mode - subscribe to player health
            if (!playerHealth)
                playerHealth = FindObjectOfType<Health>();
                
            if (playerHealth)
            {
                Health.OnHealthDepleted += OnPlayerDied;
            }
        }
        
        // Start the game
        StartGame();
    }
    
    void OnDestroy()
    {
        if (usePartySystem)
        {
            PartyController.OnPartyWiped -= OnPartyWiped;
        }
        else
        {
            if (playerHealth)
            {
                Health.OnHealthDepleted -= OnPlayerDied;
            }
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
        
        // Enable spawning based on mode
        if (spawnDirector) spawnDirector.enabled = true;
        if (rescueSpawnDirector) rescueSpawnDirector.enabled = true;
    }
    
    private void OnPartyWiped()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Time.timeScale = 0f;
        
        // Disable spawning
        if (spawnDirector) spawnDirector.enabled = false;
        if (rescueSpawnDirector) rescueSpawnDirector.enabled = false;
        
        OnGameOver?.Invoke();
    }
    
    private void OnPlayerDied(Health health)
    {
        if (isGameOver || health != playerHealth) return;
        
        isGameOver = true;
        Time.timeScale = 0f;
        
        // Disable spawning
        if (spawnDirector) spawnDirector.enabled = false;
        if (rescueSpawnDirector) rescueSpawnDirector.enabled = false;
        
        OnGameOver?.Invoke();
        
        // Reload the scene after a short delay
        Debug.Log("Player died! Reloading scene...");
        Invoke(nameof(RestartGame), 1f);
    }
    
    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
