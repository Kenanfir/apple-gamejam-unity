using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 1;
    
    [Header("Team")]
    [SerializeField] private Team team = Team.Player;
    
    [Header("Detection")]
    [SerializeField] private bool useOnTrigger = true;
    [SerializeField] private bool useOnCollision = false;
    
    public enum Team
    {
        Player,
        Enemy,
        Neutral
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!useOnTrigger) return;
        DealDamage(other);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (!useOnCollision) return;
        DealDamage(collision.collider);
    }
    
    private void DealDamage(Collider other)
    {
        Debug.Log($"DamageDealer: Collision detected with {other.name} (Team: {team})");
        
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth == null) 
        {
            Debug.Log($"No Health component found on {other.name}");
            return;
        }
        
        // Check if we're on different teams
        DamageDealer targetDealer = other.GetComponent<DamageDealer>();
        if (targetDealer != null && targetDealer.team == team) 
        {
            Debug.Log($"Same team ({team}), no damage to {other.name}");
            return;
        }
        
        Debug.Log($"Dealing {damage} damage to {other.name}! (Target health: {targetHealth.CurrentHealth}/{targetHealth.MaxHealth})");
        targetHealth.Damage(damage);
        Debug.Log($"After damage: {targetHealth.CurrentHealth}/{targetHealth.MaxHealth}");
    }
}
