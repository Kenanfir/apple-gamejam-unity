using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Endless/CharacterRescue/CharacterUnlockData", fileName = "CharacterUnlockData")]
public class CharacterUnlockData : ScriptableObject
{
    [System.Serializable]
    public class CharacterRescueInfo
    {
        [Header("Character Info")]
        public CharacterType characterType;
        public string characterName;
        public string description;
        
        [Header("Rescue Requirements")]
        public float requiredDistance = 500f;
        public bool isRescued = false;
        
        [Header("Visual")]
        public GameObject rescuePrefab;
        public Sprite characterIcon;
        public Color characterColor = Color.white;
    }
    
    [Header("Rescue Characters")]
    public List<CharacterRescueInfo> rescueCharacters = new List<CharacterRescueInfo>();
    
    [Header("Rescue Settings")]
    public float rescueNotificationDuration = 3f;
    public AudioClip rescueSound;
    public GameObject rescueEffect;
    
    public CharacterRescueInfo GetRescueInfo(CharacterType characterType)
    {
        foreach (var info in rescueCharacters)
        {
            if (info.characterType == characterType)
            {
                return info;
            }
        }
        return null;
    }
    
    public List<CharacterRescueInfo> GetAvailableRescues(float currentDistance)
    {
        List<CharacterRescueInfo> available = new List<CharacterRescueInfo>();
        
        foreach (var info in rescueCharacters)
        {
            if (!info.isRescued && currentDistance >= info.requiredDistance)
            {
                available.Add(info);
            }
        }
        
        return available;
    }
}
