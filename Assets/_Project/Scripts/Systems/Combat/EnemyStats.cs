using UnityEngine;

[CreateAssetMenu(menuName = "Endless/Stats/EnemyStats", fileName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Health")]
    public int maxHealth = 1;
    
    [Header("Damage")]
    public int touchDamage = 1;
    
    [Header("Weakness")]
    public DamageType weakAgainst = DamageType.Any;
    
    [Header("Movement")]
    public float moveSpeed = 2f;
    public bool canFly = false;
}

public enum DamageType
{
    Physical,
    Magic,
    Any
}
