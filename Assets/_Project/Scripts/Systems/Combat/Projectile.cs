using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private bool destroyOnHit = true;
    
    [Header("Effects")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject trailEffect;
    
    private void Start()
    {
        // Destroy after lifetime
        Destroy(gameObject, lifetime);
        
        // Create trail effect
        if (trailEffect)
        {
            Instantiate(trailEffect, transform);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (destroyOnHit)
        {
            // Create hit effect
            if (hitEffect)
            {
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
            
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (destroyOnHit)
        {
            // Create hit effect
            if (hitEffect)
            {
                Instantiate(hitEffect, transform.position, transform.rotation);
            }
            
            Destroy(gameObject);
        }
    }
}
