using UnityEngine;

[CreateAssetMenu(menuName = "Endless/Tuning/GameTuning", fileName = "GameTuning")]
public class GameTuning : ScriptableObject
{
    [Header("Speed Ramp")]
    public float speedRampPerSecond = 0.5f;
    
    [Header("Spawning")]
    public float baseSpawnInterval = 1.2f;
    public float spawnIntervalMin = 0.3f;
    
    [Header("Scoring")]
    public float distancePerPoint = 1f;
    
    [Header("Spawn Positions")]
    public float spawnDistanceAhead = 20f;
    public float groundLaneY = 0f;
    public float airLaneY = 2f;
}
