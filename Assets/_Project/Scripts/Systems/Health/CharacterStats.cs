using UnityEngine;

[CreateAssetMenu(menuName = "Endless/Stats/CharacterStats", fileName = "CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [Header("Health")]
    public int maxHealth = 3;
    
    [Header("Cooldowns")]
    public float attackCooldown = 0.5f;
    public float abilityCooldown = 2.0f;
    
    [Header("Character Type")]
    public CharacterType characterType = CharacterType.Knight;
}

public enum CharacterType
{
    Knight,
    Mage
}
