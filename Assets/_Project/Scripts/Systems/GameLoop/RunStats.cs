using UnityEngine;
using System;

public class RunStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float distance = 0f;
    [SerializeField] private float timeAlive = 0f;
    [SerializeField] private int score = 0;
    
    [Header("Game Tuning")]
    [SerializeField] private GameTuning gameTuning;
    
    [Header("Speed Reference")]
    [SerializeField] private PlayerConfig playerConfig;
    
    public float Distance => distance;
    public float TimeAlive => timeAlive;
    public int Score => score;
    
    public static event Action<float> OnDistanceChanged;
    public static event Action<int> OnScoreChanged;
    
    void Update()
    {
        // Update time alive
        timeAlive += Time.deltaTime;
        
        // Update distance based on current speed
        float currentSpeed = playerConfig ? playerConfig.maxSpeed : 6f;
        distance += currentSpeed * Time.deltaTime;
        
        // Update score
        int newScore = Mathf.RoundToInt(distance * gameTuning.distancePerPoint);
        if (newScore != score)
        {
            score = newScore;
            OnScoreChanged?.Invoke(score);
        }
        
        // Notify distance change
        OnDistanceChanged?.Invoke(distance);
    }
    
    public void ResetStats()
    {
        distance = 0f;
        timeAlive = 0f;
        score = 0;
        OnDistanceChanged?.Invoke(distance);
        OnScoreChanged?.Invoke(score);
    }
}
