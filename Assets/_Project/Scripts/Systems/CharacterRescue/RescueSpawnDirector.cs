using UnityEngine;
using System.Collections.Generic;

public class RescueSpawnDirector : MonoBehaviour
{
    [Header("Rescue Spawning")]
    [SerializeField] private RescueSpawnTable rescueSpawnTable;
    [SerializeField] private Transform spawnRoot;
    [SerializeField] private Transform cameraTransform;
    
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float minSpawnDistance = 100f;
    
    [Header("References")]
    [SerializeField] private RunStats runStats;
    [SerializeField] private CharacterUnlockManager unlockManager;
    
    private float nextSpawnTime;
    private Dictionary<CharacterType, int> activeRescues = new Dictionary<CharacterType, int>();
    
    public static event System.Action<GameObject> OnRescueSpawned;
    
    void Start()
    {
        if (!unlockManager)
            unlockManager = CharacterUnlockManager.Instance;
            
        if (!runStats)
            runStats = FindObjectOfType<RunStats>();
            
        if (!cameraTransform)
            cameraTransform = Camera.main?.transform;
            
        if (!spawnRoot)
            spawnRoot = transform;
            
        nextSpawnTime = Time.time + spawnInterval;
    }
    
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnRescue();
            UpdateNextSpawnTime();
        }
    }
    
    private void TrySpawnRescue()
    {
        if (!rescueSpawnTable || !runStats) return;
        
        float currentDistance = runStats.Distance;
        if (currentDistance < minSpawnDistance) return;
        
        var rescueEntry = rescueSpawnTable.GetRandomRescueEntry(currentDistance);
        if (rescueEntry == null) return;
        
        // Check if character is already unlocked
        if (unlockManager && unlockManager.IsCharacterUnlocked(rescueEntry.characterType))
        {
            return; // Don't spawn already unlocked characters
        }
        
        // Check max instances
        if (activeRescues.ContainsKey(rescueEntry.characterType) && 
            activeRescues[rescueEntry.characterType] >= rescueEntry.maxInstances)
        {
            return;
        }
        
        SpawnRescue(rescueEntry);
    }
    
    private void SpawnRescue(RescueSpawnTable.RescueSpawnEntry entry)
    {
        if (!entry.rescuePrefab) return;
        
        Vector3 spawnPos = GetSpawnPosition();
        GameObject rescueInstance = Instantiate(entry.rescuePrefab, spawnPos, Quaternion.identity, spawnRoot);
        
        // Set up the rescue pickup
        var pickup = rescueInstance.GetComponent<Pickup>();
        if (pickup)
        {
            pickup.pickupType = Pickup.PickupType.CharacterRescue;
            pickup.characterToRescue = entry.characterType;
        }
        
        // Track instance
        if (!activeRescues.ContainsKey(entry.characterType))
            activeRescues[entry.characterType] = 0;
        activeRescues[entry.characterType]++;
        
        // Clean up when destroyed
        var destroyTracker = rescueInstance.AddComponent<RescueInstanceTracker>();
        destroyTracker.Initialize(this, entry.characterType);
        
        OnRescueSpawned?.Invoke(rescueInstance);
    }
    
    private Vector3 GetSpawnPosition()
    {
        Vector3 cameraPos = cameraTransform ? cameraTransform.position : Vector3.zero;
        float spawnX = cameraPos.x + rescueSpawnTable.spawnDistanceAhead;
        float spawnY = rescueSpawnTable.spawnYPosition;
        
        return new Vector3(spawnX, spawnY, 0f);
    }
    
    private void UpdateNextSpawnTime()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }
    
    public void OnRescueDestroyed(CharacterType characterType)
    {
        if (activeRescues.ContainsKey(characterType))
        {
            activeRescues[characterType]--;
            if (activeRescues[characterType] <= 0)
            {
                activeRescues.Remove(characterType);
            }
        }
    }
    
    public void SetSpawnInterval(float interval)
    {
        spawnInterval = interval;
    }
    
    public void SetMinSpawnDistance(float distance)
    {
        minSpawnDistance = distance;
    }
}

public class RescueInstanceTracker : MonoBehaviour
{
    private RescueSpawnDirector director;
    private CharacterType characterType;
    
    public void Initialize(RescueSpawnDirector spawnDirector, CharacterType charType)
    {
        director = spawnDirector;
        characterType = charType;
    }
    
    void OnDestroy()
    {
        if (director)
        {
            director.OnRescueDestroyed(characterType);
        }
    }
}
