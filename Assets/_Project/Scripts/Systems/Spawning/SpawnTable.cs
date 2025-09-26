using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Endless/Spawning/SpawnTable", fileName = "SpawnTable")]
public class SpawnTable : ScriptableObject
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject prefab;
        public float weight = 1f;
        public float minDistance = 0f;
        public int maxInstancesAlive = 3;
    }
    
    public List<SpawnEntry> entries = new List<SpawnEntry>();
}
