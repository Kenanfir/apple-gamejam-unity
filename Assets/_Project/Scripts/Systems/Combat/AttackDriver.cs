using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class AttackDriver : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference abilityAction;
    
    [Header("Combat")]
    [SerializeField] private PartyMember partyMember;
    [SerializeField] private Transform attackOrigin;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject slashHitboxPrefab;
    [SerializeField] private GameObject blastProjectilePrefab;
    
    [Header("Attack Settings")]
    [SerializeField] private float slashRange = 0.8f;
    [SerializeField] private float slashDuration = 0.2f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 3f;
    
    private float attackCooldownTimer;
    private float abilityCooldownTimer;
    
    public float AttackCooldownPercent => attackCooldownTimer / (partyMember?.Stats?.attackCooldown ?? 1f);
    public float AbilityCooldownPercent => abilityCooldownTimer / (partyMember?.Stats?.abilityCooldown ?? 1f);
    
    public static event Action<AttackDriver> OnAttack;
    public static event Action<AttackDriver> OnAbility;
    public static event Action<AttackDriver, float, float> OnCooldownChanged;
    
    void Awake()
    {
        if (!partyMember) partyMember = GetComponent<PartyMember>();
        if (!attackOrigin) attackOrigin = transform;
    }
    
    void OnEnable()
    {
        if (attackAction) attackAction.action.performed += OnAttackPerformed;
        if (abilityAction) abilityAction.action.performed += OnAbilityPerformed;
        
        if (attackAction) attackAction.action.Enable();
        if (abilityAction) abilityAction.action.Enable();
    }
    
    void OnDisable()
    {
        if (attackAction) attackAction.action.Disable();
        if (abilityAction) abilityAction.action.Disable();
    }
    
    void Update()
    {
        // Update cooldowns
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        
        if (abilityCooldownTimer > 0)
        {
            abilityCooldownTimer -= Time.deltaTime;
        }
        
        // Notify cooldown changes
        OnCooldownChanged?.Invoke(this, AttackCooldownPercent, AbilityCooldownPercent);
    }
    
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!partyMember || !partyMember.IsAlive) 
        {
            Debug.Log("Attack failed: No party member or not alive");
            return;
        }
        if (attackCooldownTimer > 0) 
        {
            Debug.Log("Attack failed: On cooldown");
            return;
        }
        
        Debug.Log("Attack performed!");
        PerformAttack();
    }
    
    private void OnAbilityPerformed(InputAction.CallbackContext context)
    {
        if (!partyMember || !partyMember.IsAlive) return;
        if (abilityCooldownTimer > 0) return;
        
        PerformAbility();
    }
    
    private void PerformAttack()
    {
        if (!partyMember.Stats) return;
        
        attackCooldownTimer = partyMember.Stats.attackCooldown;
        OnAttack?.Invoke(this);
        
        switch (partyMember.CharacterType)
        {
            case CharacterType.Knight:
                PerformKnightSlash();
                break;
            case CharacterType.Mage:
                PerformMageBlast();
                break;
        }
    }
    
    private void PerformAbility()
    {
        if (!partyMember.Stats) return;
        
        abilityCooldownTimer = partyMember.Stats.abilityCooldown;
        OnAbility?.Invoke(this);
        
        switch (partyMember.CharacterType)
        {
            case CharacterType.Knight:
                PerformKnightAbility();
                break;
            case CharacterType.Mage:
                PerformMageAbility();
                break;
        }
    }
    
    private void PerformKnightSlash()
    {
        if (!slashHitboxPrefab) 
        {
            Debug.LogError("SlashHitbox prefab is null! Check AttackDriver configuration.");
            return;
        }
        
        Debug.Log("Creating slash hitbox!");
        GameObject hitbox = Instantiate(slashHitboxPrefab, attackOrigin.position, attackOrigin.rotation);
        hitbox.transform.SetParent(attackOrigin);
        
        // Configure hitbox
        var damageDealer = hitbox.GetComponent<DamageDealer>();
        if (damageDealer)
        {
            damageDealer.GetComponent<DamageDealer>().enabled = true;
        }
        
        // Destroy after duration
        Destroy(hitbox, slashDuration);
    }
    
    private void PerformMageBlast()
    {
        if (!blastProjectilePrefab) return;
        
        GameObject projectile = Instantiate(blastProjectilePrefab, attackOrigin.position, attackOrigin.rotation);
        
        // Add velocity
        var rb = projectile.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = attackOrigin.forward * projectileSpeed;
        }
        
        // Destroy after lifetime
        Destroy(projectile, projectileLifetime);
    }
    
    private void PerformKnightAbility()
    {
        // Stronger slash for now
        PerformKnightSlash();
    }
    
    private void PerformMageAbility()
    {
        // Charged blast for now
        PerformMageBlast();
    }
}
