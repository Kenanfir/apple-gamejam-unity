using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    
    // Events
    public static event Action<Health, int> OnDamaged;
    public static event Action<Health, int> OnHealed;
    public static event Action<Health> OnDied;
    public static event Action<Health> OnHealthDepleted; // Alias for OnDied for backward compatibility
    
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;
    public float HealthPercentage => (float)currentHealth / maxHealth;
    
    void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void Damage(int amount)
    {
        if (IsDead || amount <= 0) return;
        
        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnDamaged?.Invoke(this, amount);
        
        if (IsDead)
        {
            OnDied?.Invoke(this);
            OnHealthDepleted?.Invoke(this);
        }
    }
    
    public void Heal(int amount)
    {
        if (IsDead || amount <= 0) return;
        
        int oldHealth = currentHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        int actualHealing = currentHealth - oldHealth;
        
        if (actualHealing > 0)
        {
            OnHealed?.Invoke(this, actualHealing);
        }
    }
    
    public void ResetToFull()
    {
        currentHealth = maxHealth;
        OnHealed?.Invoke(this, maxHealth);
    }
    
    public void SetMaxHealth(int newMax)
    {
        maxHealth = newMax;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
}
