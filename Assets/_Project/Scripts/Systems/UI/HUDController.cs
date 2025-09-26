using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HUDController : MonoBehaviour
{
    [Header("Health Display")]
    [SerializeField] private List<HealthBar> healthBars = new List<HealthBar>();
    
    [Header("Cooldown Display")]
    [SerializeField] private List<CooldownIndicator> cooldownIndicators = new List<CooldownIndicator>();
    
    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Header("Pause Overlay")]
    [SerializeField] private GameObject pauseOverlay;
    
    [Header("Game Over Overlay")]
    [SerializeField] private GameObject gameOverOverlay;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    
    [Header("Single Player Mode")]
    [SerializeField] private bool usePartySystem = false;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Slider singlePlayerHealthSlider;
    [SerializeField] private TextMeshProUGUI singlePlayerHealthText;
    
    private PartyController partyController;
    private RunStats runStats;
    
    [System.Serializable]
    public class HealthBar
    {
        public CharacterType characterType;
        public Slider healthSlider;
        public TextMeshProUGUI healthText;
    }
    
    [System.Serializable]
    public class CooldownIndicator
    {
        public CharacterType characterType;
        public Image attackCooldownImage;
        public Image abilityCooldownImage;
    }
    
    void Start()
    {
        // Find references
        runStats = FindObjectOfType<RunStats>();
        
        if (usePartySystem)
        {
            partyController = FindObjectOfType<PartyController>();
            if (partyController)
            {
                PartyController.OnActiveChanged += OnActiveChanged;
                PartyController.OnPartyWiped += OnPartyWiped;
            }
        }
        else
        {
            // Single player mode
            if (!playerHealth)
                playerHealth = FindObjectOfType<Health>();
        }
        
        if (runStats)
        {
            RunStats.OnDistanceChanged += OnDistanceChanged;
            RunStats.OnScoreChanged += OnScoreChanged;
        }
        
        GameController.OnGameOver += OnGameOver;
        GameController.OnPauseChanged += OnPauseChanged;
        
        // Initialize health display
        if (usePartySystem)
        {
            UpdateAllHealthBars();
        }
        else
        {
            UpdateSinglePlayerHealth();
        }
    }
    
    void OnDestroy()
    {
        if (usePartySystem && partyController)
        {
            PartyController.OnActiveChanged -= OnActiveChanged;
            PartyController.OnPartyWiped -= OnPartyWiped;
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
        if (usePartySystem)
        {
            // Update cooldown indicators
            UpdateCooldownIndicators();
        }
        else
        {
            // Update single player health
            UpdateSinglePlayerHealth();
        }
    }
    
    private void OnActiveChanged(PartyMember activeMember)
    {
        UpdateAllHealthBars();
    }
    
    private void OnPartyWiped()
    {
        // This will be handled by OnGameOver
    }
    
    private void OnGameOver()
    {
        if (gameOverOverlay)
        {
            gameOverOverlay.SetActive(true);
        }
        
        if (finalScoreText && runStats)
        {
            finalScoreText.text = $"Final Score: {runStats.Score}";
        }
    }
    
    private void OnPauseChanged(bool isPaused)
    {
        if (pauseOverlay)
        {
            pauseOverlay.SetActive(isPaused);
        }
    }
    
    private void OnDistanceChanged(float distance)
    {
        if (distanceText)
        {
            distanceText.text = $"Distance: {distance:F1}m";
        }
    }
    
    private void OnScoreChanged(int score)
    {
        if (scoreText)
        {
            scoreText.text = $"Score: {score}";
        }
    }
    
    private void UpdateAllHealthBars()
    {
        if (!partyController) return;
        
        foreach (var member in partyController.AllMembers)
        {
            if (!member) continue;
            
            foreach (var healthBar in healthBars)
            {
                if (healthBar.characterType == member.CharacterType)
                {
                    UpdateHealthBar(healthBar, member.Health);
                    break;
                }
            }
        }
    }
    
    private void UpdateHealthBar(HealthBar healthBar, Health health)
    {
        if (!health) return;
        
        if (healthBar.healthSlider)
        {
            healthBar.healthSlider.value = health.HealthPercentage;
        }
        
        if (healthBar.healthText)
        {
            healthBar.healthText.text = $"{health.CurrentHealth}/{health.MaxHealth}";
        }
    }
    
    private void UpdateCooldownIndicators()
    {
        if (!partyController || !partyController.ActiveMember) return;
        
        var attackDriver = partyController.ActiveMember.GetComponent<AttackDriver>();
        if (!attackDriver) return;
        
        foreach (var indicator in cooldownIndicators)
        {
            if (indicator.characterType == partyController.ActiveMember.CharacterType)
            {
                if (indicator.attackCooldownImage)
                {
                    indicator.attackCooldownImage.fillAmount = 1f - attackDriver.AttackCooldownPercent;
                }
                
                if (indicator.abilityCooldownImage)
                {
                    indicator.abilityCooldownImage.fillAmount = 1f - attackDriver.AbilityCooldownPercent;
                }
                break;
            }
        }
    }
    
    private void UpdateSinglePlayerHealth()
    {
        if (!playerHealth) return;
        
        if (singlePlayerHealthSlider)
        {
            singlePlayerHealthSlider.value = playerHealth.HealthPercentage;
        }
        
        if (singlePlayerHealthText)
        {
            singlePlayerHealthText.text = $"{playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
        }
    }
}
