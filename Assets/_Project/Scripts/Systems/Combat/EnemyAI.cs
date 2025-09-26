using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool canFly = false;
    [SerializeField] private bool moveTowardsPlayer = true;
    
    [Header("Attack")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    
    private Transform player;
    private float lastAttackTime;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Find player
        var partyController = FindObjectOfType<PartyController>();
        if (partyController && partyController.ActiveMember)
        {
            player = partyController.ActiveMember.transform;
        }
    }
    
    void Update()
    {
        if (!player) return;
        
        // Move towards player
        if (moveTowardsPlayer)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = canFly ? direction.y : 0f; // Ground enemies don't fly
            
            if (rb)
            {
                rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, 0f);
            }
            else
            {
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
        }
        
        // Attack if in range
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                Attack();
            }
        }
    }
    
    private void Attack()
    {
        lastAttackTime = Time.time;
        
        if (projectilePrefab)
        {
            // Fire projectile towards player
            Vector3 direction = (player.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(direction));
            
            // Add velocity to projectile
            var projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb)
            {
                projectileRb.linearVelocity = direction * 5f;
            }
        }
    }
}
