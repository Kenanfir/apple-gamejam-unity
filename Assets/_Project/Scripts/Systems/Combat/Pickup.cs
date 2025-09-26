using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("Pickup Type")]
    public PickupType pickupType = PickupType.Health;
    [SerializeField] private int value = 1;
    
    [Header("Character Rescue")]
    public CharacterType characterToRescue = CharacterType.Knight;
    
    [Header("Effects")]
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private AudioClip pickupSound;
    
    public enum PickupType
    {
        Health,
        Invulnerability,
        SpeedBoost,
        CharacterRescue
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // Handle character rescue separately
        if (pickupType == PickupType.CharacterRescue)
        {
            HandleCharacterRescue();
        }
        else
        {
            // Handle other pickups (simplified for single character)
            var player = other.GetComponent<Health>();
            if (player)
            {
                ApplyPickup(player);
            }
        }
        
        // Play effects
        if (pickupEffect)
        {
            Instantiate(pickupEffect, transform.position, transform.rotation);
        }
        
        if (pickupSound)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
        
        Destroy(gameObject);
    }
    
    private void HandleCharacterRescue()
    {
        var unlockManager = CharacterUnlockManager.Instance;
        if (unlockManager)
        {
            unlockManager.RescueCharacter(characterToRescue);
        }
    }
    
    private void ApplyPickup(Health player)
    {
        switch (pickupType)
        {
            case PickupType.Health:
                player.Heal(value);
                break;
                
            case PickupType.Invulnerability:
                // TODO: Implement invulnerability system
                break;
                
            case PickupType.SpeedBoost:
                // TODO: Implement speed boost system
                break;
        }
    }
}
