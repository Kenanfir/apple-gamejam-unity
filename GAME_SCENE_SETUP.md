# 🎮 Game Scene Setup - Complete Guide

## 📁 Scene Hierarchy

```
Game (Scene)
├── GameController (Empty GameObject)
├── Player (Empty GameObject)
│   └── Knight (Knight.prefab)
├── Environment (Empty GameObject)
│   ├── Ground (Plane)
│   ├── WallGenerator (Empty GameObject)
│   └── EnvironmentMover (Empty GameObject)
├── Spawning (Empty GameObject)
│   ├── RescueSpawnDirector (Empty GameObject)
│   ├── EnemySpawnDirector (Empty GameObject)
│   └── SpawnRoot (Empty GameObject)
├── UI (Empty GameObject)
│   └── HUD_Canvas (Canvas)
├── Main Camera
└── Directional Light
```

---

## 🎯 Step-by-Step Setup

### 1. **GameController (Empty GameObject)**
**Components:**
- `GameController` script
- `RunStats` script

**Settings:**
- `usePartySystem = false`
- `playerHealth = [Knight's Health component]`
- `rescueSpawnDirector = [RescueSpawnDirector]`
- `spawnDirector = [EnemySpawnDirector]`
- `runStats = [RunStats component]`
- `gameTuning = [GameTuning asset]`
- `playerConfig = [PlayerConfig asset]`

**RunStats Component Settings:**
- `gameTuning = [GameTuning asset]`
- `playerConfig = [PlayerConfig asset]`

---

### 2. **Player (Empty GameObject)**
**Child:**
- `Knight (Knight.prefab)`
  - **Position:** (0, 0, 0)
  - **Components:** PlayerMotor, GroundCheck, Health, AttackDriver, Rigidbody, BoxCollider
  - **Tag:** "Player"

**Setup:**
- Drag Knight.prefab into scene as child of Player

---

### 3. **Environment (Empty GameObject)**

#### **3a. Ground (Plane)**
**Settings:**
- **Tag:** "Ground"
- **Position:** (0, -1, 0)
- **Scale:** (50, 1, 10)

#### **3b. WallGenerator (Empty GameObject)**
**Components:**
- `WallGenerator` script

**References to assign:**
- `playerTransform = [Knight's Transform]`
- `castleWallPrefab = [wall.prefab]`
- `environmentParent = [this GameObject]`

**Settings:**
- `castleWallSpacing = 5f`
- `spawnDistanceAhead = 50f`
- `destroyDistanceBehind = -30f`
- `castleWallY = -3f`

#### **3c. EnvironmentMover (Empty GameObject)**
**Components:**
- `EnvironmentMover` script

**References to assign:**
- `playerMotor = [Knight's PlayerMotor component]`

---

### 4. **Spawning (Empty GameObject)**

#### **4a. RescueSpawnDirector (Empty GameObject)**
**Components:**
- `RescueSpawnDirector` script

**References to assign:**
- `rescueSpawnTable = [RescueSpawnTable asset]`
- `spawnRoot = [SpawnRoot GameObject]`
- `cameraTransform = [Main Camera]`
- `runStats = [RunStats component]`
- `unlockManager = [CharacterUnlockManager]`

**Settings:**
- `spawnInterval = 10f`
- `minSpawnDistance = 100f`

#### **4b. EnemySpawnDirector (Empty GameObject)**
**Components:**
- `SpawnDirector` script

**References to assign:**
- `enemySpawnTable = [EnemySpawnTable asset]`
- `obstacleSpawnTable = [ObstacleSpawnTable asset]`
- `spawnRoot = [SpawnRoot GameObject]`
- `cameraTransform = [Main Camera]`
- `gameTuning = [GameTuning asset]`

#### **4c. SpawnRoot (Empty GameObject)**
**Purpose:** Parent for all spawned objects (enemies, obstacles, rescues)
**Position:** (0, 0, 0)
**Children:** All spawned enemies and obstacles will be instantiated here

---

### 5. **UI (Empty GameObject)**

#### **5a. HUD_Canvas (Canvas)**
**Settings:**
- **Render Mode:** Screen Space - Overlay

**Components:**
- `HUDController` script

**Settings:**
- `usePartySystem = false`
- `playerHealth = [Knight's Health component]`
- `singlePlayerHealthSlider = [Health Bar Slider]`
- `singlePlayerHealthText = [Health Text]`

**UI Elements to create:**
- **Health Bar (Slider)**
  - Position: Top-left
  - Min: 0, Max: 1
- **Health Text (TextMeshPro)**
  - Position: Next to health bar
  - Text: "3/3"
- **Distance Text (TextMeshPro)**
  - Position: Top-center
  - Text: "Distance: 0m"
- **Score Text (TextMeshPro)**
  - Position: Top-right
  - Text: "Score: 0"
- **Rescue Notification Panel**
  - Position: Center
  - Initially hidden
- **Pause Overlay**
  - Position: Full screen
  - Initially hidden
- **Game Over Overlay**
  - Position: Full screen
  - Initially hidden

---

### 6. **Main Camera**
**Settings:**
- **Position:** (0, 5, -10)
- **Rotation:** (15, 0, 0)
- **Projection:** Perspective

---

### 7. **Directional Light**
**Settings:**
- **Intensity:** 1.0
- **Color:** White
- **Rotation:** (45, 45, 0)

---

## 📦 Required Data Assets

### **✅ Already Created Assets:**

#### **CharacterUnlockData.asset**
**Path:** Assets/_Project/Data/CharacterRescue/CharacterUnlockData.asset
**Status:** ✅ Created
**Contains:** Rescue character configuration with distance requirements

#### **RescueSpawnTable.asset**
**Path:** Assets/_Project/Data/CharacterRescue/RescueSpawnTable.asset
**Status:** ✅ Created
**Contains:** Rescue spawn settings and character entries

#### **GameTuning_Default.asset**
**Path:** Assets/_Project/Data/Balance/Game/GameTuning_Default.asset
**Status:** ✅ Exists
**Contains:** Game balance settings (spawn intervals, speeds, etc.)

#### **PlayerConfig_Default.asset**
**Path:** Assets/_Project/Data/Balance/Player/PlayerConfig_Default.asset
**Status:** ✅ Exists
**Contains:** Player movement settings (speed, jump, gravity, etc.)

#### **EnemySpawnTable.asset**
**Path:** Assets/_Project/Data/Balance/Spawning/EnemySpawnTable.asset
**Status:** ✅ Exists
**Contains:** Enemy spawn configuration

#### **ObstacleSpawnTable.asset**
**Path:** Assets/_Project/Data/Balance/Spawning/ObstacleSpawnTable.asset
**Status:** ✅ Exists
**Contains:** Obstacle spawn configuration

---

## 👾 Enemy Prefabs

### **Existing Enemy Prefabs:**
- **Slime.prefab** - Ground enemy
- **Flying.prefab** - Flying enemy  
- **Lich.prefab** - Magic enemy
- **RockGolem.prefab** - Heavy enemy

### **Enemy Prefab Components:**
Each enemy prefab should have:
- **Transform** - Position, rotation, scale
- **Rigidbody** - Physics (for movement)
- **Collider** - Collision detection
- **EnemyAI** - AI behavior (movement, attack)
- **Health** - Health system
- **DamageDealer** - Damage dealing
- **EnemyStats** - Stats configuration
- **Visual Model** - 3D character model
- **Tag:** "Enemy"

### **Enemy Spawning:**
- **Enemies are spawned automatically** by EnemySpawnDirector
- **Spawned into SpawnRoot** as children
- **Positioned ahead of player** based on camera position
- **Destroyed when behind player** to save memory

---

## 🎭 Required Rescue Prefabs

### **Create these rescue character prefabs:**

#### **Mage_Rescue.prefab**
**Components:**
- `Pickup` script
  - `pickupType = CharacterRescue`
  - `characterToRescue = Mage`
- `Collider` (set as trigger)
- Visual model/icon
- Tag: "Player"

#### **Archer_Rescue.prefab**
**Components:**
- `Pickup` script
  - `pickupType = CharacterRescue`
  - `characterToRescue = Archer`
- `Collider` (set as trigger)
- Visual model/icon
- Tag: "Player"

#### **Warrior_Rescue.prefab**
**Components:**
- `Pickup` script
  - `pickupType = CharacterRescue`
  - `characterToRescue = Warrior`
- `Collider` (set as trigger)
- Visual model/icon
- Tag: "Player"

---

## 🎮 Input System Setup

### **Configure Input Actions:**
- **Gameplay Action Map:**
  - `Jump` - Space/Up Arrow
  - `Attack` - Left Mouse/Click
  - `Ability` - Right Mouse/Right Click
  - `Pause` - Escape

### **Assign References:**
- **Knight's PlayerMotor:**
  - `jumpAction = [Jump InputActionReference]`
- **GameController:**
  - Handle pause input

---

## 🚀 Testing

1. **Start the game**
2. **Run forward** - walls should generate
3. **Reach 500m** - Mage rescue should spawn
4. **Bump into Mage** - "Rescued Mage!" notification
5. **Check CharacterUnlockManager** - Mage should be unlocked
6. **Test all gameplay** - jump, attack, health, UI updates

This setup creates a complete endless runner with character rescue mechanics!
