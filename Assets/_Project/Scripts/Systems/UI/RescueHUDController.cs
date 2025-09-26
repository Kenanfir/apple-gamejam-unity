using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RescueHUDController : MonoBehaviour
{
    [Header("Health Display")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Header("Rescue Notifications")]
    [SerializeField] private GameObject rescueNotificationPrefab;
    [SerializeField] private Transform notificationParent;
    [SerializeField] private float notificationDuration = 3f;
    
    [Header("Pause Overlay")]
    [SerializeField] private GameObject pauseOverlay;
    
    [Header("Game Over Overlay")]
    [SerializeField] private GameObject gameOverOverlay;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    
    [Header("Rescue Progress")]
    [SerializeField] private TextMeshProUGUI rescuedCountText;
    [SerializeField] private TextMeshProUGUI nextRescueText;
    
    private Health playerHealth;
    private RunStats runStats;
    private CharacterUnlockManager unlockManager;
    private RescueSpawnTable rescueSpawnTable;
    
    void Start()
    {
        // Find references
        playerHealth = FindObjectOfType<Health>();
        runStats = FindObjectOfType<RunStats>();
        unlockManager = CharacterUnlockManager.Instance;
        
        // Subscribe to events
        if (unlockManager)
        {
            CharacterUnlockManager.OnCharacterRescued += OnCharacterRescued;
        }
        
        if (runStats)
        {
            RunStats.OnDistanceChanged += OnDistanceChanged;
            RunStats.OnScoreChanged += OnScoreChanged;
        }
        
        GameController.OnGameOver += OnGameOver;
        GameController.OnPauseChanged += OnPauseChanged;
        
        // Initialize UI
        UpdateHealthDisplay();
        UpdateRescueProgress();
    }
    
    void OnDestroy()
    {
        if (unlockManager)
        {
            CharacterUnlockManager.OnCharacterRescued -= OnCharacterRescued;
        }
        
        if (runStats)
        {
            RunStats.OnDistanceChanged -= OnDistanceChanged;
            RunStats.OnScoreChanged -= OnScoreChanged;
        }
        
        GameController.OnGameOver -= OnGameOver;
        GameController.OnPauseChanged -= OnPauseChanged;
    }
    
    void Update()
    {
        UpdateHealthDisplay();
    }
    
    private void OnCharacterRescued(CharacterType characterType)
    {
        ShowRescueNotification(characterType);
        UpdateRescueProgress();
    }
    
    private void ShowRescueNotification(CharacterType characterType)
    {
        if (!rescueNotificationPrefab || !notificationParent) return;
        
        GameObject notification = Instantiate(rescueNotificationPrefab, notificationParent);
        
        // Set up notification text
        var textComponent = notification.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent)
        {
            textComponent.text = $"Rescued {characterType}!";
        }
        
        // Auto-destroy after duration
        StartCoroutine(DestroyNotificationAfterDelay(notification, notificationDuration));
    }
    
    private IEnumerator DestroyNotificationAfterDelay(GameObject notification, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (notification)
        {
            Destroy(notification);
        }
    }
    
    private void UpdateHealthDisplay()
    {
        if (!playerHealth) return;
        
        if (healthSlider)
        {
            healthSlider.value = playerHealth.HealthPercentage;
        }
        
        if (healthText)
        {
            healthText.text = $"{playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
        }
    }
    
    private void UpdateRescueProgress()
    {
        if (!unlockManager) return;
        
        if (rescuedCountText)
        {
            int rescuedCount = unlockManager.GetUnlockedCharacterCount();
            rescuedCountText.text = $"Rescued: {rescuedCount}";
        }
        
        if (nextRescueText && rescueSpawnTable && runStats)
        {
            var availableRescues = rescueSpawnTable.GetAvailableRescues(runStats.Distance);
            if (availableRescues.Count > 0)
            {
                nextRescueText.text = $"Next rescue at {availableRescues[0].requiredDistance}m";
            }
            else
            {
                nextRescueText.text = "All characters rescued!";
            }
        }
    }
    
    private void OnDistanceChanged(float distance)
    {
        if (distanceText)
        {
            distanceText.text = $"Distance: {distance:F1}m";
        }
        
        UpdateRescueProgress();
    }
    
    private void OnScoreChanged(int score)
    {
        if (scoreText)
        {
            scoreText.text = $"Score: {score}";
        }
    }
    
    private void OnGameOver()
    {
        if (gameOverOverlay)
        {
            gameOverOverlay.SetActive(true);
        }
        
        if (finalScoreText && runStats)
        {
            finalScoreText.text = $"Final Score: {runStats.Score}\nCharacters Rescued: {unlockManager?.GetUnlockedCharacterCount() ?? 0}";
        }
    }
    
    private void OnPauseChanged(bool isPaused)
    {
        if (pauseOverlay)
        {
            pauseOverlay.SetActive(isPaused);
        }
    }
}
