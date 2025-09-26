# Endless - 3D Side-Scroller Game Jam Project

## Game Overview
**Endless** is a 3D side-scroller endless runner game featuring a **character rescue system**. Players start as a Knight and must rescue other characters (Mage, Archer, Warrior) during their run. Rescued characters become available for future runs.

## Game Loop
- **Endless Runner**: Continuous side-scrolling gameplay
- **Character Rescue**: Rescue characters by bumping into them during runs
- **Single Character System**: Play as one character per run (Knight by default)
- **Character Unlocking**: Rescued characters saved permanently for future runs
- **Progressive Difficulty**: Obstacles and enemies increase over time

## Controls
- **Jump**: Space
- **Attack**: Left Mouse Button
- **Ability**: Right Mouse Button
- **Pause**: Escape
- **Rescue Characters**: Bump into them (automatic)

## Game Design

### Characters
- **Knight**: Starting character, always unlocked
- **Mage**: Rescue at 500m distance
- **Archer**: Rescue at 1000m distance  
- **Warrior**: Rescue at 1500m distance
- **Rogue**: Rescue at 2000m distance

### Enemies
- **Slime**: Magic-weak, basic ground enemy
- **Lich**: Projectile-based magic enemy
- **Rock Golem**: Physical tank enemy
- **Flying Enemy**: Air threat, requires different tactics

### Obstacles
- **High Barriers**: Must be jumped over
- **Pits**: Must be jumped over
- **Rolling Boulders**: Must be avoided or destroyed

### Visual Style
- Low-poly 3D aesthetic
- Forest backdrop
- Particle effects for dust and magic
- Clean, readable UI

## Project Structure

### Scenes
- **Boot**: Initialization and splash screen
- **MainMenu**: Title screen with Play/Quit options
- **Game**: Main gameplay scene with side-scroll setup
- **Sandbox**: Testing environment

### Architecture
- **Event-Driven**: Modular systems with clear separation
- **ScriptableObject Configs**: Balance data in Data/Balance/
- **Prefab-Based**: All game objects as prefabs for easy iteration
- **URP Pipeline**: Modern rendering with mobile optimization

## Development Notes

### Time-to-First-Play (TTFF)
- Target: ≤ 5 minutes after opening project
- Menu → Game scene loads quickly
- No complex initialization required

### Jam Scope
- Prioritize core gameplay loop in Game.unity
- Placeholder art until assets imported
- Focus on mechanics over polish
- Modular design for easy iteration

### Asset Links
Third-party asset links stored in `Assets/ThirdParty/Links/`:
- Knight: RPG Tiny Hero Duo PBR Polyart
- Mage: Battle Wizard Poly Art
- Slime: RPG Monster Duo PBR Polyart
- Lich: Mini Legion Lich PBR HP Polyart
- Rock Golem: Mini Legion Rock Golem PBR HP Polyart
- Flying Enemy: RPG Monster Partners PBR Polyart

## Build Settings
- **Platform**: macOS (Intel/Apple Silicon)
- **Scene Order**: Boot → MainMenu → Game → Sandbox
- **Target**: Unity 6000.0.58f1

## Acceptance Criteria
- [x] Menu loads and displays correctly
- [x] Game scene loads with ground and background
- [x] HUD placeholder visible
- [x] No scripts required for basic structure
- [x] All scenes in build settings
- [x] URP pipeline configured
- [x] Input System ready
- [x] Cinemachine placeholder in Game scene

## Next Steps
1. Import character assets from Unity Asset Store
2. Set up Game scene with rescue system (see GAME_SCENE_SETUP.md)
3. Create rescue character prefabs
4. Configure spawn tables and data assets
5. Implement main menu character selection
6. Add audio and visual effects
7. Polish UI and game feel
