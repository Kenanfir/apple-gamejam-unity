using UnityEngine;
using System.Collections.Generic;

public class WallGenerator : MonoBehaviour
{
    [Header("Castle Wall Prefabs")]
    [SerializeField] private GameObject castleWallPrefab; // Castle wall segments that player runs on (with battlements)
    
    [Header("Generation Settings")]
    [SerializeField] private float castleWallSpacing = 5f; // Spacing between castle wall segments
    [SerializeField] private float spawnDistanceAhead = 50f;
    [SerializeField] private float destroyDistanceBehind = -30f;
    
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform environmentParent;
    
    [Header("Castle Wall Position")]
    [SerializeField] private float castleWallY = -3f; // Y position for castle wall segments (ground level)
    
    [Header("Rotation Settings")]
    [SerializeField] private float yRotation = 90f; // Fixed Y-axis rotation in degrees
    
    private List<GameObject> activeCastleWalls = new List<GameObject>();
    private float lastWallSpawnX;
    
    void Start()
    {
        if (!playerTransform)
            playerTransform = FindObjectOfType<PlayerMotor>()?.transform;
            
        if (!environmentParent)
            environmentParent = transform;
            
        // Spawn initial castle walls
        SpawnInitialCastleWalls();
    }
    
    void Update()
    {
        if (!playerTransform) return;
        
        float playerX = playerTransform.position.x;
        
        // Spawn new castle walls ahead
        if (playerX + spawnDistanceAhead > lastWallSpawnX)
        {
            SpawnCastleWallAhead();
        }
        
        // Clean up castle walls behind player
        CleanupCastleWallsBehind(playerX);
    }
    
    private void SpawnInitialCastleWalls()
    {
        float startX = playerTransform.position.x - 20f;
        for (int i = 0; i < 10; i++)
        {
            SpawnCastleWallAtX(startX + i * castleWallSpacing);
        }
    }
    
    private void SpawnCastleWallAhead()
    {
        SpawnCastleWallAtX(lastWallSpawnX + castleWallSpacing);
    }
    
    private void SpawnCastleWallAtX(float x)
    {
        if (!castleWallPrefab) return;
        
        Vector3 spawnPos = new Vector3(x, castleWallY, 0f);
        
        // Fixed Y-axis rotation
        Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);
        
        GameObject castleWall = Instantiate(castleWallPrefab, spawnPos, rotation, environmentParent);
        activeCastleWalls.Add(castleWall);
        
        lastWallSpawnX = x;
    }
    
    
    private void CleanupCastleWallsBehind(float playerX)
    {
        for (int i = activeCastleWalls.Count - 1; i >= 0; i--)
        {
            if (activeCastleWalls[i] == null)
            {
                activeCastleWalls.RemoveAt(i);
                continue;
            }
            
            if (activeCastleWalls[i].transform.position.x < playerX + destroyDistanceBehind)
            {
                Destroy(activeCastleWalls[i]);
                activeCastleWalls.RemoveAt(i);
            }
        }
    }
    
    
    public void SetCastleWallSpacing(float spacing)
    {
        castleWallSpacing = spacing;
    }
    
    public void SetSpawnDistance(float distance)
    {
        spawnDistanceAhead = distance;
    }
    
    public void SetDestroyDistance(float distance)
    {
        destroyDistanceBehind = distance;
    }
    
    public void SetYRotation(float rotation)
    {
        yRotation = rotation;
    }
}
