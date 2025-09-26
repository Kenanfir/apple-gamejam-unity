using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SystemValidator : MonoBehaviour
{
    [Header("Validation Results")]
    [SerializeField] private List<string> validationResults = new List<string>();
    
    [Header("System Checks")]
    [SerializeField] private bool checkScriptableObjects = true;
    [SerializeField] private bool checkComponentReferences = true;
    [SerializeField] private bool checkInputActions = true;
    
    [ContextMenu("Validate All Systems")]
    public void ValidateAllSystems()
    {
        validationResults.Clear();
        
        if (checkScriptableObjects)
        {
            ValidateScriptableObjects();
        }
        
        if (checkComponentReferences)
        {
            ValidateComponentReferences();
        }
        
        if (checkInputActions)
        {
            ValidateInputActions();
        }
        
        LogValidationResults();
    }
    
    private void ValidateScriptableObjects()
    {
        // Check if CharacterStats assets exist
        var characterStatsAssets = Resources.FindObjectsOfTypeAll<CharacterStats>();
        if (characterStatsAssets.Length == 0)
        {
            validationResults.Add("❌ No CharacterStats assets found");
        }
        else
        {
            validationResults.Add($"✅ Found {characterStatsAssets.Length} CharacterStats assets");
        }
        
        // Check if GameTuning assets exist
        var gameTuningAssets = Resources.FindObjectsOfTypeAll<GameTuning>();
        if (gameTuningAssets.Length == 0)
        {
            validationResults.Add("❌ No GameTuning assets found");
        }
        else
        {
            validationResults.Add($"✅ Found {gameTuningAssets.Length} GameTuning assets");
        }
        
        // Check if SpawnTable assets exist
        var spawnTableAssets = Resources.FindObjectsOfTypeAll<SpawnTable>();
        if (spawnTableAssets.Length == 0)
        {
            validationResults.Add("❌ No SpawnTable assets found");
        }
        else
        {
            validationResults.Add($"✅ Found {spawnTableAssets.Length} SpawnTable assets");
        }
    }
    
    private void ValidateComponentReferences()
    {
        // Check PartyController
        var partyController = FindObjectOfType<PartyController>();
        if (partyController == null)
        {
            validationResults.Add("❌ PartyController not found in scene");
        }
        else
        {
            validationResults.Add("✅ PartyController found");
            
            // Check if it has members
            if (partyController.AllMembers.Count == 0)
            {
                validationResults.Add("⚠️ PartyController has no members assigned");
            }
            else
            {
                validationResults.Add($"✅ PartyController has {partyController.AllMembers.Count} members");
            }
        }
        
        // Check SpawnDirector
        var spawnDirector = FindObjectOfType<SpawnDirector>();
        if (spawnDirector == null)
        {
            validationResults.Add("❌ SpawnDirector not found in scene");
        }
        else
        {
            validationResults.Add("✅ SpawnDirector found");
        }
        
        // Check GameController
        var gameController = FindObjectOfType<GameController>();
        if (gameController == null)
        {
            validationResults.Add("❌ GameController not found in scene");
        }
        else
        {
            validationResults.Add("✅ GameController found");
        }
        
        // Check HUDController
        var hudController = FindObjectOfType<HUDController>();
        if (hudController == null)
        {
            validationResults.Add("❌ HUDController not found in scene");
        }
        else
        {
            validationResults.Add("✅ HUDController found");
        }
    }
    
    private void ValidateInputActions()
    {
        // Check if Input System is properly set up
        var inputActions = FindObjectsOfType<AttackDriver>();
        int validInputCount = 0;
        
        foreach (var attackDriver in inputActions)
        {
            // Use reflection to check private fields
            var attackActionField = typeof(AttackDriver).GetField("attackAction", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var abilityActionField = typeof(AttackDriver).GetField("abilityAction", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (attackActionField != null && abilityActionField != null)
            {
                var attackAction = attackActionField.GetValue(attackDriver) as UnityEngine.InputSystem.InputActionReference;
                var abilityAction = abilityActionField.GetValue(attackDriver) as UnityEngine.InputSystem.InputActionReference;
                
                if (attackAction != null && abilityAction != null)
                {
                    validInputCount++;
                }
            }
        }
        
        if (validInputCount == 0)
        {
            validationResults.Add("❌ No AttackDriver components with valid input actions found");
        }
        else
        {
            validationResults.Add($"✅ Found {validInputCount} AttackDriver components with input actions");
        }
    }
    
    private void LogValidationResults()
    {
        Debug.Log("=== SYSTEM VALIDATION RESULTS ===");
        foreach (var result in validationResults)
        {
            Debug.Log(result);
        }
        Debug.Log("=== END VALIDATION ===");
    }
}
