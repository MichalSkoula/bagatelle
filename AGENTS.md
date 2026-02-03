# Bagatelle

A cross-platform bagatelle (pinball-style marble game) built with MonoGame. Two-player turn-based game where players launch balls into scoring holes while obstacles and physics create unpredictable outcomes.

## Build Commands

```bash
# Build Windows DirectX version
dotnet build Bagatelle.DX/Bagatelle.DX.csproj

# Build Android version
dotnet build Bagatelle.Android/Bagatelle.Android.csproj

# Run Windows version
dotnet run --project Bagatelle.DX/Bagatelle.DX.csproj
```

## Architecture

### Project Structure
- **Bagatelle.Shared** - Core game logic (shared project, imported by platform projects)
- **Bagatelle.DX** - Windows DirectX platform entry point
- **Bagatelle.Android** - Android platform entry point

### Core Systems

**Screen Management** (`Screens/`)
- `ScreenController` manages screen lifecycle and transitions
- All screens inherit from `BaseScreen` with `LoadContent()`, `Update()`, `Draw()` pattern
- Navigate via `Game1.Screens.SetScreen(new ScreenName(this))`

**Game Logic** (`Logic/`)
- `GameManager` orchestrates game state (`WaitingToLaunch` → `BallInPlay` → `GameOver`)
- `Physics` is a static utility class handling all collision detection and response
- Turn-based: each player has 5 balls, scores are recalculated based on final ball positions each turn

**Game Objects** (`GameObjects/`)
- `Board` defines the playing field geometry (semicircular top, rectangular body, launch channel)
- `Ball`, `Peg`, `Hole` are the interactive elements with collision radii defined in `GameConstants`

**Input & Platform** (`Controls/`)
- `InputManager` abstracts mouse/touch input with scaling for resolution independence
- Platform detection via `Platform.Current` and `#if ANDROID`/`#if WINDOWS` directives

### Coordinate System
- Virtual resolution: 480×800 (portrait)
- All game logic uses virtual coordinates; `Game1` applies scale matrix for actual screen size
- Origin (0,0) is top-left

## Key Conventions

### Physics
- All physics constants centralized in `GameConstants`
- Collision handling in `Physics.cs` - board walls, pegs, holes, ball-to-ball
- Holes apply "trap" force that pulls slow-moving balls to center

### Adding New Screens
1. Create class inheriting `BaseScreen`
2. Implement `Update(GameTime)` and `Draw(GameTime, SpriteBatch)`
3. Use `Game1.Screens.SetScreen()` for navigation

### Drawing
- Use `DrawHelper` static methods for primitives (circles, rectangles, lines)
- SpriteBatch is passed to `Draw()` - batch is already begun with scale transform
