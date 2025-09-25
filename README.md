# Endless - 3D Side-Scroller Game Jam Project

## Game Overview
**Endless** is a 3D side-scroller endless runner game featuring two characters: a Knight (tank/physical) and a Mage (magic/phasing). Players can switch between characters using number keys and must survive as long as possible.

## Game Loop
- **Endless Runner**: Continuous side-scrolling gameplay
- **Character Switching**: Knight tanks physical damage, Mage phases through magic
- **Dual Character System**: Switch between Knight (1) and Mage (2) on number keys
- **Game Over**: Lose when both characters die
- **Progressive Difficulty**: Obstacles and enemies increase over time

## Controls
- **Movement**: A/D or Left/Right Arrow Keys
- **Jump**: Space
- **Attack**: Left Mouse Button
- **Ability**: Right Mouse Button
- **Switch Character**: 1 (Knight) / 2 (Mage)
- **Pause**: Escape

## Game Design

### Characters
- **Knight**: Physical tank, can destroy obstacles, weak to magic
- **Mage**: Magic user, can phase through obstacles, weak to physical

### Enemies
- **Slime**: Magic-weak, basic ground enemy
- **Lich**: Projectile-based magic enemy
- **Rock Golem**: Physical tank enemy
- **Flying Enemy**: Air threat, requires different tactics

### Obstacles
- **High Barriers**: Knight can destroy, Mage must phase through
- **Pits**: Must be jumped over
- **Rolling Boulders**: Knight can destroy, Mage must avoid

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
2. Implement movement and character switching scripts
3. Add enemy AI and spawning systems
4. Create obstacle generation system
5. Implement health and damage systems
6. Add audio and visual effects
7. Polish UI and game feel
