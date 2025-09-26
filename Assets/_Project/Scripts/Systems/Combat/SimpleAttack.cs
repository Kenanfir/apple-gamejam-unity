using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleAttack : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference attackAction;
    
    [Header("Attack Settings")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDuration = 0.3f;
    [SerializeField] private float attackCooldown = 0.5f;
    
    private float lastAttackTime;
    
    void Start()
    {
        if (!attackOrigin) attackOrigin = transform;
    }
    
    void OnEnable()
    {
        if (attackAction) 
        {
            attackAction.action.performed += OnAttackPerformed;
            attackAction.action.Enable();
        }
    }
    
    void OnDisable()
    {
        if (attackAction) 
        {
            attackAction.action.performed -= OnAttackPerformed;
            attackAction.action.Disable();
        }
    }
    
    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (Time.time - lastAttackTime < attackCooldown) 
        {
            Debug.Log("Attack on cooldown");
            return;
        }
        
        Debug.Log("Simple attack performed!");
        PerformAttack();
        lastAttackTime = Time.time;
    }
    
    private void PerformAttack()
    {
        if (!attackPrefab) 
        {
            Debug.LogError("Attack prefab is null! Assign SimpleAttack.prefab to the attackPrefab field.");
            return;
        }
        
        // Create attack visual
        Vector3 attackPos = attackOrigin.position + attackOrigin.forward * attackRange;
        GameObject attackObj = Instantiate(attackPrefab, attackPos, attackOrigin.rotation);
        
        // Add damage component
        var damageDealer = attackObj.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            damageDealer = attackObj.AddComponent<DamageDealer>();
        }
        damageDealer.enabled = true;
        
        // Destroy after duration
        Destroy(attackObj, attackDuration);
        
        Debug.Log($"Attack created at {attackPos}");
    }
}
