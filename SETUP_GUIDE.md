# Endless Runner Game Setup Guide

This guide will help you set up the complete endless runner gameplay system in Unity.

## 🎯 Overview

The game features:
- **Knight & Mage characters** with different abilities
- **Character switching** (1/2 keys)
- **Auto-run with jumping** (Space)
- **Combat system** (LMB attack, RMB ability)
- **Endless spawning** with difficulty ramp
- **Health system** with events
- **Score & distance tracking**
- **Pause functionality** (Esc)

## 📁 Project Structure

```
Assets/_Project/
├── Scripts/
│   ├── Common/
│   │   └── DamageDealer.cs
│   └── Systems/
│       ├── Health/
│       │   ├── Health.cs
│       │   └── CharacterStats.cs
│       ├── Movement/ (existing)
│       ├── Combat/
│       │   ├── AttackDriver.cs
│       │   ├── EnemyAI.cs
│       │   ├── SlashHitbox.cs
│       │   ├── Projectile.cs
│       │   └── RollingBoulder.cs
│       ├── Party/
│       │   ├── PartyMember.cs
│       │   └── PartyController.cs
│       ├── Spawning/
│       │   └── SpawnDirector.cs
│       ├── GameLoop/
│       │   ├── GameController.cs
│       │   └── RunStats.cs
│       └── UI/
│           └── HUDController.cs
├── Data/
│   └── Balance/
│       ├── Player/
│       │   ├── KnightStats.asset
│       │   └── MageStats.asset
│       ├── Game/
│       │   └── GameTuning_Default.asset
│       └── Spawning/
│           ├── EnemySpawnTable.asset
│           └── ObstacleSpawnTable.asset
└── Prefabs/
    ├── Characters/
    ├── Enemies/
    ├── Obstacles/
    └── UI/
```

## 🛠️ Setup Steps

### 1. Configure Layers and Tags

**Tags to create:**
- `Player`
- `Enemy` 
- `Obstacle`
- `Hazard`
- `Projectile`
- `Pickup`

**Layers to create:**
- `Player`
- `Enemy`
- `Ground`
- `Cameras`
- `UI`

### 2. Create Combat Prefabs

#### SlashHitbox Prefab
1. Create empty GameObject → "SlashHitbox"
2. Add Capsule Collider (trigger, size: 0.8, 0.8, 0.8)
3. Add SlashHitbox script
4. Add DamageDealer component (team: Player, damage: 1)
5. Save as `Prefabs/Combat/SlashHitbox.prefab`

#### BlastProjectile Prefab
1. Create Sphere → "BlastProjectile"
2. Add Rigidbody
3. Add Projectile script
4. Add DamageDealer component (team: Player, damage: 1)
5. Save as `Prefabs/Combat/BlastProjectile.prefab`

### 3. Update Character Prefabs

#### Knight.prefab
Add these components:
- `Health` (maxHealth: 3)
- `PartyMember` (assign KnightStats.asset)
- `AttackDriver` (assign SlashHitbox prefab)
- `DamageDealer` (team: Player, damage: 1)

#### Mage.prefab  
Add these components:
- `Health` (maxHealth: 2)
- `PartyMember` (assign MageStats.asset)
- `AttackDriver` (assign BlastProjectile prefab)
- `DamageDealer` (team: Player, damage: 1)

### 4. Configure Enemy Prefabs

For each enemy (Slime, Lich, RockGolem, Flying):
- Add `Health` component
- Add `EnemyAI` script
- Add `DamageDealer` (team: Enemy)
- Set appropriate tags and layers

### 5. Configure Obstacle Prefabs

#### HighBarrier.prefab
- Tag: `Obstacle`
- Add `Health` component
- Add `DamageDealer` (team: Enemy)

#### PitMarker.prefab
- Tag: `Hazard`
- Add trigger collider
- Add `DamageDealer` (team: Enemy, high damage)

#### RollingBoulder.prefab
- Tag: `Obstacle`
- Add `RollingBoulder` script
- Add `Rigidbody`
- Add `DamageDealer` (team: Enemy)

### 6. Create Game Scene Setup

#### Game.unity Scene
1. **Ground Setup:**
   - Create ground plane
   - Set layer to `Ground`
   - Add collider

2. **Camera Setup:**
   - Position camera to follow player
   - Set layer to `Cameras`

3. **Spawn Root:**
   - Create empty GameObject → "SpawnRoot"
   - Position ahead of camera

4. **Game Controller Setup:**
   - Create empty GameObject → "GameController"
   - Add `GameController` script
   - Add `PartyController` script
   - Add `SpawnDirector` script
   - Add `RunStats` script
   - Wire all references

5. **Party Setup:**
   - Place Knight and Mage prefabs in scene
   - Assign to PartyController members list
   - Set Knight as active (index 0)

6. **HUD Setup:**
   - Create Canvas with `HUDController` script
   - Add health bars, cooldown indicators, score text
   - Add pause overlay and game over overlay

### 7. Input Actions Configuration

The input actions are already configured in `Endless.inputactions`:
- **Jump:** Space
- **Attack:** LMB
- **Ability:** RMB  
- **Switch Character 1:** 1
- **Switch Character 2:** 2
- **Switch Character 3:** 3
- **Pause:** Esc

### 8. ScriptableObject Assets

Create these assets in the Data/Balance folders:
- `KnightStats.asset` (already created)
- `MageStats.asset` (already created)
- `GameTuning_Default.asset` (already created)
- `EnemySpawnTable.asset` (already created)
- `ObstacleSpawnTable.asset` (already created)

## 🎮 Gameplay Features

### Character Switching
- Press **1** for Knight (tank, physical attacks)
- Press **2** for Mage (glass cannon, magic attacks)
- Only one character active at a time
- Dead characters stay disabled

### Combat System
- **Knight:** Slash attack destroys physical obstacles
- **Mage:** Blast projectile destroys magical obstacles
- Different cooldowns per character
- Team-based damage system

### Spawning System
- Weighted random spawning
- Difficulty ramps over time
- Max instances per enemy type
- Spawns ahead of camera

### Health System
- Event-driven health updates
- HUD subscribes to health events
- Party wipe detection
- Game over when all characters die

## 🔧 Troubleshooting

### Common Issues:
1. **Input not working:** Check Input Action references in components
2. **Spawning not working:** Verify SpawnDirector references and spawn tables
3. **Health not updating:** Check Health component and event subscriptions
4. **Character switching not working:** Verify PartyController member assignments

### Testing Checklist:
- [ ] Characters auto-run forward
- [ ] Space bar makes characters jump
- [ ] 1/2 keys switch between Knight/Mage
- [ ] LMB/RMB perform attacks/abilities
- [ ] Health bars update when taking damage
- [ ] Enemies spawn and move toward player
- [ ] Obstacles spawn and can be destroyed
- [ ] Score increases over time
- [ ] Esc pauses the game
- [ ] Game over when all characters die

## 🚀 Next Steps

Once basic gameplay is working:
1. Add visual effects (particles, animations)
2. Implement pickups (health, power-ups)
3. Add sound effects and music
4. Polish UI and add more HUD elements
5. Balance difficulty and spawn rates

The system is designed to be modular and event-driven, making it easy to add new features without breaking existing functionality.
