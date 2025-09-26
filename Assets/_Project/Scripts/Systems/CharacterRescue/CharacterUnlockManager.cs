using UnityEngine;
using System.Collections.Generic;
using System;

public class CharacterUnlockManager : MonoBehaviour
{
    [Header("Character Unlock Data")]
    [SerializeField] private CharacterUnlockData unlockData;
    
    [Header("Events")]
    public static event Action<CharacterType> OnCharacterRescued;
    public static event Action<List<CharacterType>> OnUnlockedCharactersChanged;
    
    private static CharacterUnlockManager _instance;
    public static CharacterUnlockManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CharacterUnlockManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("CharacterUnlockManager");
                    _instance = go.AddComponent<CharacterUnlockManager>();
                }
            }
            return _instance;
        }
    }
    
    private List<CharacterType> unlockedCharacters = new List<CharacterType>();
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUnlockedCharacters();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Knight is always unlocked by default
        if (!IsCharacterUnlocked(CharacterType.Knight))
        {
            UnlockCharacter(CharacterType.Knight);
        }
    }
    
    public bool IsCharacterUnlocked(CharacterType characterType)
    {
        return unlockedCharacters.Contains(characterType);
    }
    
    public void RescueCharacter(CharacterType characterType)
    {
        if (!IsCharacterUnlocked(characterType))
        {
            UnlockCharacter(characterType);
            OnCharacterRescued?.Invoke(characterType);
        }
    }
    
    private void UnlockCharacter(CharacterType characterType)
    {
        if (!unlockedCharacters.Contains(characterType))
        {
            unlockedCharacters.Add(characterType);
            SaveUnlockedCharacters();
            OnUnlockedCharactersChanged?.Invoke(new List<CharacterType>(unlockedCharacters));
        }
    }
    
    public List<CharacterType> GetUnlockedCharacters()
    {
        return new List<CharacterType>(unlockedCharacters);
    }
    
    public int GetUnlockedCharacterCount()
    {
        return unlockedCharacters.Count;
    }
    
    private void LoadUnlockedCharacters()
    {
        unlockedCharacters.Clear();
        
        // Load from PlayerPrefs
        string savedCharacters = PlayerPrefs.GetString("UnlockedCharacters", "");
        if (!string.IsNullOrEmpty(savedCharacters))
        {
            string[] characterStrings = savedCharacters.Split(',');
            foreach (string characterString in characterStrings)
            {
                if (Enum.TryParse(characterString, out CharacterType characterType))
                {
                    unlockedCharacters.Add(characterType);
                }
            }
        }
    }
    
    private void SaveUnlockedCharacters()
    {
        string characterString = string.Join(",", unlockedCharacters);
        PlayerPrefs.SetString("UnlockedCharacters", characterString);
        PlayerPrefs.Save();
    }
    
    public void ResetUnlockedCharacters()
    {
        unlockedCharacters.Clear();
        PlayerPrefs.DeleteKey("UnlockedCharacters");
        PlayerPrefs.Save();
        
        // Knight is always unlocked
        UnlockCharacter(CharacterType.Knight);
    }
}
