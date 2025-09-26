using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 1;
    [SerializeField] private DamageDealer.Team team = DamageDealer.Team.Player;
    
    [Header("Visual")]
    [SerializeField] private GameObject slashEffect;
    
    void Start()
    {
        // Add DamageDealer component
        var damageDealer = gameObject.AddComponent<DamageDealer>();
        damageDealer.enabled = true;
        
        // Play slash effect
        if (slashEffect)
        {
            Instantiate(slashEffect, transform.position, transform.rotation);
        }
    }
}
