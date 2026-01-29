# Project Structure Documentation

## Overview

The HexMinesweeper project uses a **layered architecture** with files organized into folders by responsibility. This structure makes the codebase easier to navigate, maintain, and understand.

```
HexMinesweeperWPF/
├── UI/                          # User Interface Layer
│   ├── MainWindow.xaml          # Main game window layout (XAML)
│   ├── MainWindow.xaml.cs       # Main game window logic
│   ├── HighScoreWindow.xaml     # High scores display dialog
│   ├── HighScoreWindow.xaml.cs  # High scores dialog logic
│   ├── InputHighScoreName.xaml  # Name input dialog
│   └── InputHighScoreName.xaml.cs # Name input dialog logic
│
├── Models/                       # Data Models
│   ├── HexCoord.cs              # Hexagonal grid coordinate system
│   ├── HexTile.cs               # Individual tile state and visuals
│   ├── HighScore.cs             # High score entry data model
│   └── GameDifficulty.cs        # Difficulty level enumeration
│
├── Logic/                        # Game Logic & Algorithms
│   └── GridManager.cs           # Game board management and rules
│
├── Configuration/               # Configuration & Settings
│   ├── DifficultySettings.cs    # Difficulty configuration class
│   └── GameDifficulties.cs      # Difficulty registry (static)
│
├── Interfaces/                  # Contracts & Interfaces
│   └── IUpdateValues.cs         # UI update interface (Observer pattern)
│
├── docs/                        # Documentation
│   ├── README.md                # Project overview & guide
│   └── TECHNICAL.md             # Technical deep-dive & development journey
│
├── Properties/                  # Project Properties
│   └── AssemblyInfo.cs
│
├── bin/                         # Build Output
├── obj/                         # Intermediate Build Files
│
├── App.xaml                     # Application entry point (XAML)
├── App.xaml.cs                  # Application entry point (C#)
├── HexMinesweeper.csproj        # Project file
├── HexMinesweeper.sln           # Solution file
└── README.md                    # Quick start guide (root)
```

---

## Folder Structure Explanation

### 1. UI/ - User Interface Layer

**Purpose**: Contains all WPF UI components (XAML markup and code-behind)

**Files**:
- `MainWindow.xaml` / `MainWindow.xaml.cs`: Main game window
  - Displays the hexagonal grid
  - Handles user clicks (left and right mouse buttons)
  - Manages game timer
  - Shows difficulty buttons, pause button, bomb count, timer
  
- `HighScoreWindow.xaml` / `HighScoreWindow.xaml.cs`: High scores dialog
  - Displays top 3 scores per difficulty
  - Uses DataGrids to show score information
  - Modal dialog (blocks main window while open)
  
- `InputHighScoreName.xaml` / `InputHighScoreName.xaml.cs`: Name input dialog
  - Appears when player wins
  - Prompts for player name
  - Shows game time and difficulty for context
  - Uses DialogResult pattern

**Design Principle**: All visual elements and UI event handlers go here. Business logic is kept in the Logic/ folder.

### 2. Models/ - Data Models

**Purpose**: Represent data structures and game entities

**Files**:
- `HexCoord.cs`: Coordinate system for hexagonal grid
  - Immutable X, Y properties
  - Represents position of a tile
  - Uses double-width coordinate system
  
- `HexTile.cs`: Individual game tile
  - State: IsBomb, IsRevealed, IsFlagged, BombNeighbours
  - Visual properties: Polygon (WPF shape), TextBlock (number display)
  - Methods: ShowNumber(), ToggleFlag()
  
- `HighScore.cs`: High score entry
  - PlayerName, TimeTaken, DateAchieved, Difficulty
  - JSON serializable (used for persistence)
  
- `GameDifficulty.cs`: Enumeration
  - Defines difficulty levels: Easy, Medium, Hard
  - Type-safe alternative to strings or magic numbers

**Design Principle**: Models hold data and visual elements, but no business logic. Logic layer manipulates models.

### 3. Logic/ - Game Logic & Algorithms

**Purpose**: Contains core game logic, algorithms, and game rules

**Files**:
- `GridManager.cs`: Game board manager
  - Creates and stores all HexTile objects
  - Implements algorithms:
    - AllocateBombs(): Random bomb placement
    - GetNeighbours(): Hexagonal neighbour calculation
    - FloodFillReveal(): Cascading tile reveal
    - CalculateBombNeighbours(): Count adjacent bombs
    - CheckWin(): Detect win condition
  - Manages high score persistence (load/save JSON)
  - Implements IUpdateValues pattern for UI communication

**Design Principle**: This layer is independent of UI. It can be tested without any WPF components. The interface IUpdateValues allows UI callbacks without direct dependency.

### 4. Configuration/ - Configuration & Settings

**Purpose**: Centralize difficulty settings and configuration

**Files**:
- `DifficultySettings.cs`: Data class
  - Properties: Name, HexesWide, HexesHigh, Bombs, HexRadius
  - Immutable (private setters)
  - Represents one difficulty's configuration
  
- `GameDifficulties.cs`: Static registry class
  - Predefined instances: Easy, Medium, Hard
  - Method: GetSettings(GameDifficulty enum)
  - Centralizes difficulty definitions

**Design Principle**: Configuration is global and immutable. Using static class prevents instantiation and ensures single source of truth.

### 5. Interfaces/ - Contracts & Design Patterns

**Purpose**: Define contracts for loose coupling between layers

**Files**:
- `IUpdateValues.cs`: Observer pattern interface
  - Methods: UpdateBombCount(int), StopTimer()
  - Implemented by: MainWindow
  - Used by: GridManager
  - Allows GridManager to request UI updates without referencing MainWindow directly

**Design Principle**: Depend on interfaces, not concrete classes. Enables unit testing with mock implementations.

### 6. docs/ - Documentation

**Purpose**: Project documentation for learning and reference

**Files**:
- `README.md`: Overview, rules, gameplay, architecture
- `TECHNICAL.md`: Deep technical guide, WPF principles, development journey
- `PROJECT_STRUCTURE.md`: This file - explanation of folder organization

---

## Design Patterns Used

### 1. Separation of Concerns

Layers separate different concerns:
```
UI Layer (MainWindow, Dialogs)
        ↓ uses
Logic Layer (GridManager)
        ↓ manipulates
Data Layer (Models: HexTile, HexCoord, HighScore)
```

- **Benefit**: Test logic without UI
- **Benefit**: Reuse logic with different UI (console, web, etc.)
- **Benefit**: Change UI without touching game rules

### 2. Observer Pattern (IUpdateValues)

```
GridManager (Logic)
        ↓ uses
IUpdateValues (Interface)
        ↓ implemented by
MainWindow (UI)
```

- GridManager calls updateValues.UpdateBombCount(-1)
- MainWindow implements IUpdateValues, handles the call
- Loose coupling: GridManager doesn't know about MainWindow

### 3. Static Registry (GameDifficulties)

```
GameDifficulty (Enum)
        ↓
GameDifficulties.GetSettings()
        ↓
DifficultySettings (Config object)
```

- Centralized configuration
- Type-safe enum-based lookup
- Immutable settings

### 4. Model-View Pattern

- **Models** (HexTile, HexCoord): Hold state
- **View** (MainWindow): Display state and handle input
- **Controller** (MainWindow event handlers): Translate input to actions

---

## File Dependencies

### MainWindow depends on:
- Models: HexTile, HexCoord
- Logic: GridManager
- Configuration: DifficultySettings, GameDifficulties
- Interfaces: IUpdateValues
- Other UI: HighScoreWindow, InputHighScoreName

### GridManager depends on:
- Models: HexTile, HexCoord, HighScore
- Configuration: DifficultySettings
- Interfaces: IUpdateValues (but doesn't know about MainWindow)

### HexTile depends on:
- Models: HexCoord
- WPF: Polygon, TextBlock

### No files depend on:
- App.xaml (except entry point)
- HighScoreWindow, InputHighScoreName (optional features)

---

## Adding New Features

### Example: Add a "Beginner" Difficulty Level

**Steps**:

1. **Modify Models/**
   - (No changes needed - GameDifficulty enum will be updated)

2. **Modify Configuration/**
   - Add case to GameDifficulty enum:
     ```csharp
     public enum GameDifficulty
     {
         Easy,
         Medium,
         Hard,
         Beginner  // New!
     }
     ```
   - Add to GameDifficulties class:
     ```csharp
     public static readonly DifficultySettings Beginner =
         new DifficultySettings("Beginner", 3, 3, 1, 60);
     ```
   - Add case to GetSettings():
     ```csharp
     case GameDifficulty.Beginner:
         return Beginner;
     ```

3. **Modify UI/**
   - Add button to MainWindow.xaml
   - Add event handler to MainWindow.xaml.cs:
     ```csharp
     private void BeginnerButton_Click(object sender, RoutedEventArgs e)
     {
         StartGame(GameDifficulty.Beginner);
     }
     ```

4. **No other files need changes!**
   - GridManager works with any DifficultySettings
   - Logic layer is unchanged
   - Models are unchanged

---

## Namespace Structure

All classes are in the **HexMinesweeper** namespace:

```csharp
namespace HexMinesweeper
{
    // UI
    public partial class MainWindow : Window { }
    public partial class HighScoreWindow : Window { }
    public partial class InputHighScoreName : Window { }
    public partial class App : Application { }
    
    // Models
    public class HexCoord { }
    public class HexTile { }
    public class HighScore { }
    public enum GameDifficulty { }
    
    // Logic
    internal class GridManager { }
    
    // Configuration
    public class DifficultySettings { }
    public static class GameDifficulties { }
    
    // Interfaces
    internal interface IUpdateValues { }
}
```

**Note**: Public classes can be accessed from outside the assembly. Internal classes are only accessible within this assembly. This is good encapsulation.

---

## Build & File Organization

### Project File (HexMinesweeper.csproj)

The `.csproj` file automatically discovers all `.cs` files in the project, regardless of folder structure. No need to manually register files.

XAML files are also automatically discovered and compiled.

### Compiled Output

When built:
```
bin/Debug/net8.0-windows/
├── HexMinesweeper.exe           # Executable
├── HexMinesweeper.dll           # Assembly (compiled code)
└── HexMinesweeper.runtimeconfig.json
```

All classes from all folders are compiled into a single assembly. Folder structure is for **development organization**, not runtime structure.

---

## Best Practices Demonstrated

1. ✓ **Separation of Concerns**: UI, Logic, Models in separate folders
2. ✓ **Single Responsibility**: Each class has one job
3. ✓ **Loose Coupling**: Use interfaces (IUpdateValues)
4. ✓ **DRY (Don't Repeat Yourself)**: Shared configuration in Configuration/ folder
5. ✓ **Clear Naming**: Folder names describe content
6. ✓ **Discoverability**: Easy to find what you're looking for
7. ✓ **Extensibility**: Adding features requires minimal changes
8. ✓ **Testability**: Logic layer can be tested independently

---

## Summary

This project structure exemplifies professional software engineering:

- **UI/**  - Event-driven presentation
- **Logic/** - Pure game rules and algorithms
- **Models/** - Data representation
- **Configuration/** - Global settings
- **Interfaces/** - Design patterns and contracts
- **docs/** - Documentation for learning

The layered architecture makes the codebase understandable, maintainable, and extensible - perfect for an AQA A-Level exemplar project.
