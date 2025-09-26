using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Endless/CharacterRescue/RescueSpawnTable", fileName = "RescueSpawnTable")]
public class RescueSpawnTable : ScriptableObject
{
    [System.Serializable]
    public class RescueSpawnEntry
    {
        [Header("Character Info")]
        public CharacterType characterType;
        public GameObject rescuePrefab;
        
        [Header("Spawn Requirements")]
        public float requiredDistance = 500f;
        public float spawnWeight = 1f;
        public int maxInstances = 1;
        
        [Header("Visual")]
        public string characterName;
        public Sprite characterIcon;
        public Color characterColor = Color.white;
    }
    
    [Header("Rescue Characters")]
    public List<RescueSpawnEntry> rescueEntries = new List<RescueSpawnEntry>();
    
    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 30f;
    public float spawnYPosition = 0f;
    
    public RescueSpawnEntry GetRescueEntry(CharacterType characterType)
    {
        foreach (var entry in rescueEntries)
        {
            if (entry.characterType == characterType)
            {
                return entry;
            }
        }
        return null;
    }
    
    public List<RescueSpawnEntry> GetAvailableRescues(float currentDistance)
    {
        List<RescueSpawnEntry> available = new List<RescueSpawnEntry>();
        
        foreach (var entry in rescueEntries)
        {
            if (currentDistance >= entry.requiredDistance)
            {
                available.Add(entry);
            }
        }
        
        return available;
    }
    
    public RescueSpawnEntry GetRandomRescueEntry(float currentDistance)
    {
        var available = GetAvailableRescues(currentDistance);
        if (available.Count == 0) return null;
        
        float totalWeight = 0f;
        foreach (var entry in available)
        {
            totalWeight += entry.spawnWeight;
        }
        
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        
        foreach (var entry in available)
        {
            currentWeight += entry.spawnWeight;
            if (randomValue <= currentWeight)
            {
                return entry;
            }
        }
        
        return available[available.Count - 1];
    }
}
