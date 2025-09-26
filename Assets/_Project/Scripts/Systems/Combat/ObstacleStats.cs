using UnityEngine;

[CreateAssetMenu(menuName = "Endless/Stats/ObstacleStats", fileName = "ObstacleStats")]
public class ObstacleStats : ScriptableObject
{
    [Header("Destruction")]
    public CharacterType destroyableBy = CharacterType.Knight;
    
    [Header("Damage")]
    public int damageOnTouch = 1;
    
    [Header("Movement")]
    public bool isRolling = false;
    public float rollSpeed = 5f;
}
