using UnityEngine;
using System.Collections.Generic;
using System;

public class SpawnDirector : MonoBehaviour
{
    [Header("Spawn Tables")]
    [SerializeField] private SpawnTable enemySpawnTable;
    [SerializeField] private SpawnTable obstacleSpawnTable;
    
    [Header("Wall Generation")]
    [SerializeField] private WallGenerator wallGenerator;
    [SerializeField] private bool enableWallGeneration = true;
    
    [Header("Game Tuning")]
    [SerializeField] private GameTuning gameTuning;
    
    [Header("Spawn Root")]
    [SerializeField] private Transform spawnRoot;
    
    [Header("Camera Reference")]
    [SerializeField] private Transform cameraTransform;
    
    private float nextSpawnTime;
    private Dictionary<GameObject, int> activeInstances = new Dictionary<GameObject, int>();
    
    public static event Action<GameObject> OnSpawned;
    
    void Start()
    {
        nextSpawnTime = Time.time + gameTuning.baseSpawnInterval;
        
        // Initialize wall generator if enabled
        if (enableWallGeneration && wallGenerator)
        {
            wallGenerator.enabled = true;
        }
    }
    
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandom();
            UpdateNextSpawnTime();
        }
    }
    
    private void SpawnRandom()
    {
        // Choose between enemy and obstacle (50/50 for now)
        SpawnTable table = UnityEngine.Random.value < 0.5f ? enemySpawnTable : obstacleSpawnTable;
        if (!table) return;
        
        SpawnTable.SpawnEntry entry = GetRandomEntry(table);
        if (entry == null || entry.prefab == null) return;
        
        // Check max instances
        if (activeInstances.ContainsKey(entry.prefab) && 
            activeInstances[entry.prefab] >= entry.maxInstancesAlive)
        {
            return;
        }
        
        // Spawn the object
        Vector3 spawnPos = GetSpawnPosition(entry.prefab);
        GameObject instance = Instantiate(entry.prefab, spawnPos, Quaternion.identity, spawnRoot);
        
        // Track instance
        if (!activeInstances.ContainsKey(entry.prefab))
            activeInstances[entry.prefab] = 0;
        activeInstances[entry.prefab]++;
        
        // Clean up when destroyed
        var destroyTracker = instance.AddComponent<SpawnInstanceTracker>();
        destroyTracker.Initialize(this, entry.prefab);
        
        OnSpawned?.Invoke(instance);
    }
    
    private SpawnTable.SpawnEntry GetRandomEntry(SpawnTable table)
    {
        if (table.entries.Count == 0) return null;
        
        float totalWeight = 0f;
        foreach (var entry in table.entries)
        {
            totalWeight += entry.weight;
        }
        
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        
        foreach (var entry in table.entries)
        {
            currentWeight += entry.weight;
            if (randomValue <= currentWeight)
            {
                return entry;
            }
        }
        
        return table.entries[table.entries.Count - 1];
    }
    
    private Vector3 GetSpawnPosition(GameObject prefab)
    {
        Vector3 cameraPos = cameraTransform ? cameraTransform.position : Vector3.zero;
        float spawnX = cameraPos.x + gameTuning.spawnDistanceAhead;
        
        // Determine Y position based on prefab name or component
        float spawnY = gameTuning.groundLaneY;
        if (prefab.name.ToLower().Contains("flying") || prefab.name.ToLower().Contains("lich"))
        {
            spawnY = gameTuning.airLaneY;
        }
        
        return new Vector3(spawnX, spawnY, 0f);
    }
    
    private void UpdateNextSpawnTime()
    {
        float currentInterval = Mathf.Lerp(gameTuning.baseSpawnInterval, gameTuning.spawnIntervalMin, 
            Time.time * gameTuning.speedRampPerSecond * 0.1f);
        nextSpawnTime = Time.time + currentInterval;
    }
    
    public void OnInstanceDestroyed(GameObject prefab)
    {
        if (activeInstances.ContainsKey(prefab))
        {
            activeInstances[prefab]--;
            if (activeInstances[prefab] <= 0)
            {
                activeInstances.Remove(prefab);
            }
        }
    }
    
    // Public methods for controlling wall generation
    public void SetWallGenerationEnabled(bool enabled)
    {
        enableWallGeneration = enabled;
        if (wallGenerator)
        {
            wallGenerator.enabled = enabled;
        }
    }
    
    public void SetCastleWallSpacing(float spacing)
    {
        if (wallGenerator)
        {
            wallGenerator.SetCastleWallSpacing(spacing);
        }
    }
}

public class SpawnInstanceTracker : MonoBehaviour
{
    private SpawnDirector director;
    private GameObject prefab;
    
    public void Initialize(SpawnDirector spawnDirector, GameObject prefabRef)
    {
        director = spawnDirector;
        prefab = prefabRef;
    }
    
    void OnDestroy()
    {
        if (director && prefab)
        {
            director.OnInstanceDestroyed(prefab);
        }
    }
}
