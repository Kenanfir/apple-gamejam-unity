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
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth == null) return;
        
        // Check if we're on different teams
        DamageDealer targetDealer = other.GetComponent<DamageDealer>();
        if (targetDealer != null && targetDealer.team == team) return;
        
        targetHealth.Damage(damage);
    }
}
