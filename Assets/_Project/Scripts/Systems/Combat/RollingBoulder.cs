using UnityEngine;

public class RollingBoulder : MonoBehaviour
{
    [Header("Rolling")]
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private float rollDirection = -1f; // -1 for left, 1 for right
    
    [Header("Damage")]
    [SerializeField] private int damageOnTouch = 1;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Set initial velocity
        if (rb)
        {
            rb.linearVelocity = new Vector3(rollDirection * rollSpeed, 0f, 0f);
        }
    }
    
    void Update()
    {
        // Keep rolling
        if (rb)
        {
            rb.linearVelocity = new Vector3(rollDirection * rollSpeed, rb.linearVelocity.y, 0f);
        }
        else
        {
            transform.Translate(Vector3.right * rollDirection * rollSpeed * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Deal damage to player
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<Health>();
            if (health)
            {
                health.Damage(damageOnTouch);
            }
        }
    }
}
