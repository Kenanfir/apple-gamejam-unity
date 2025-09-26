# ScriptableObject Connection Guide

## 🔗 Current Status: Scripts ↔ Assets Connections

### ✅ **Properly Connected Scripts:**

1. **CharacterStats.cs** → **KnightStats.asset & MageStats.asset**
   - ✅ GUID: `adeb0624324bb4f3b8c82370831a7b87`
   - ✅ ScriptableObject assets properly reference the script
   - ✅ Used by: `PartyMember.cs` → `stats` field

2. **GameTuning.cs** → **GameTuning_Default.asset**
   - ✅ GUID: `8ae48c104ae4c4341b888f061f95a234`
   - ✅ ScriptableObject asset properly references the script
   - ✅ Used by: `SpawnDirector.cs`, `RunStats.cs`, `GameController.cs`

3. **SpawnTable.cs** → **EnemySpawnTable.asset & ObstacleSpawnTable.asset**
   - ✅ GUID: `006a1987540fb469fb750c2a53367e0d`
   - ✅ ScriptableObject assets properly reference the script
   - ✅ Used by: `SpawnDirector.cs`

### 🔧 **Script Dependencies & References:**

#### **PartyMember.cs** Dependencies:
```csharp
[SerializeField] private CharacterStats stats;  // ← Links to KnightStats/MageStats assets
```
- **Health.cs** (auto-found via GetComponent)
- **PlayerMotor.cs** (auto-found via GetComponent)
- **Animator** (auto-found via GetComponent)

#### **AttackDriver.cs** Dependencies:
```csharp
[SerializeField] private PartyMember partyMember;  // ← Links to PartyMember component
```
- **InputActionReference** for attack/ability inputs
- **GameObject prefabs** for slash/blast effects

#### **SpawnDirector.cs** Dependencies:
```csharp
[SerializeField] private SpawnTable enemySpawnTable;     // ← Links to EnemySpawnTable.asset
[SerializeField] private SpawnTable obstacleSpawnTable; // ← Links to ObstacleSpawnTable.asset
[SerializeField] private GameTuning gameTuning;        // ← Links to GameTuning_Default.asset
```

#### **GameController.cs** Dependencies:
```csharp
[SerializeField] private PartyController partyController;
[SerializeField] private SpawnDirector spawnDirector;
[SerializeField] private RunStats runStats;
[SerializeField] private GameTuning gameTuning;
[SerializeField] private PlayerConfig playerConfig;
```

### 🎯 **Unity Inspector Setup Required:**

#### **1. PartyMember Components (on Knight/Mage prefabs):**
- ✅ **Health** component (auto-assigned)
- ✅ **PlayerMotor** component (auto-assigned)
- ✅ **CharacterStats** → Assign `KnightStats.asset` or `MageStats.asset`
- ✅ **Animator** component (auto-assigned)
- ✅ **AttackDriver** component with proper input references

#### **2. SpawnDirector Setup:**
- ✅ **Enemy Spawn Table** → Assign `EnemySpawnTable.asset`
- ✅ **Obstacle Spawn Table** → Assign `ObstacleSpawnTable.asset`
- ✅ **Game Tuning** → Assign `GameTuning_Default.asset`
- ✅ **Spawn Root** → Assign empty GameObject for spawned objects
- ✅ **Camera Transform** → Assign main camera

#### **3. GameController Setup:**
- ✅ **Party Controller** → Assign PartyController component
- ✅ **Spawn Director** → Assign SpawnDirector component
- ✅ **Run Stats** → Assign RunStats component
- ✅ **Game Tuning** → Assign `GameTuning_Default.asset`
- ✅ **Player Config** → Assign existing PlayerConfig asset

### 🚨 **Potential Issues & Solutions:**

#### **Issue 1: Missing Input Action References**
**Problem:** AttackDriver.cs needs InputActionReference for attack/ability
**Solution:** 
1. Create InputActionReference assets in `Assets/_Project/Data/Settings/Input/`
2. Assign them to AttackDriver components

#### **Issue 2: Missing Prefab References**
**Problem:** AttackDriver.cs needs slashHitboxPrefab and blastProjectilePrefab
**Solution:**
1. Create SlashHitbox.prefab with DamageDealer component
2. Create BlastProjectile.prefab with Projectile script
3. Assign to AttackDriver components

#### **Issue 3: Missing Component References**
**Problem:** Scripts can't find required components
**Solution:**
1. Ensure all required components are on the same GameObject
2. Use `[RequireComponent(typeof(ComponentName))]` attributes
3. Add null checks and auto-assignment in Awake()

### 🔍 **Verification Checklist:**

#### **ScriptableObject Assets:**
- [ ] KnightStats.asset has correct GUID reference
- [ ] MageStats.asset has correct GUID reference  
- [ ] GameTuning_Default.asset has correct GUID reference
- [ ] EnemySpawnTable.asset has correct GUID reference
- [ ] ObstacleSpawnTable.asset has correct GUID reference

#### **Component References:**
- [ ] PartyMember.stats assigned to appropriate CharacterStats asset
- [ ] SpawnDirector tables assigned to SpawnTable assets
- [ ] SpawnDirector.gameTuning assigned to GameTuning asset
- [ ] GameController references assigned to appropriate components

#### **Input System:**
- [ ] AttackDriver has InputActionReference for attack
- [ ] AttackDriver has InputActionReference for ability
- [ ] PartyController has InputActionReference for character switching

#### **Prefab References:**
- [ ] AttackDriver has slashHitboxPrefab assigned
- [ ] AttackDriver has blastProjectilePrefab assigned
- [ ] SpawnDirector has spawn tables with prefab entries

### 🛠️ **Quick Fix Commands:**

If you need to regenerate GUIDs or fix references:

```bash
# Regenerate all meta files (Unity will reassign GUIDs)
find Assets/_Project -name "*.meta" -delete

# Or manually fix specific assets by updating the GUID in the asset file
```

### 📋 **Next Steps:**

1. **Open Unity Editor**
2. **Check Console for Missing Reference warnings**
3. **Assign ScriptableObject assets to component fields**
4. **Create missing prefabs (SlashHitbox, BlastProjectile)**
5. **Set up Input Action References**
6. **Test the connections in Play Mode**

The scripts are properly structured and the GUIDs are now correctly linked. The main work remaining is the Unity Inspector setup and prefab creation.
