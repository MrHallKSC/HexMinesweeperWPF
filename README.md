# HexMinesweeper WPF - AQA 7517 NEA Exemplar Project

## Table of Contents
1. [Concept](#concept)
2. [Game Rules](#game-rules)
3. [How to Play](#how-to-play)
4. [Technologies Used](#technologies-used)
5. [AQA Technical Skills Demonstrated](#aqa-technical-skills-demonstrated)
6. [Coding Styles and Standards](#coding-styles-and-standards)
7. [Project Architecture](#project-architecture)
8. [Key Design Decisions](#key-design-decisions)

---

## Concept

**HexMinesweeper** is a WPF (Windows Presentation Foundation) desktop application that implements a variation of the classic Minesweeper game using a hexagonal grid instead of a rectangular grid. The hexagonal grid provides a more complex and interesting gameplay experience compared to traditional Minesweeper, as each hexagonal tile has six neighbours rather than eight.

This project serves as an exemplar for AQA A-Level Computer Science (7517) NEA projects by demonstrating:
- Object-oriented design principles
- Event-driven graphical programming with WPF
- Proper separation of concerns (UI, logic, data)
- Professional code documentation and commenting
- Implementation of design patterns (Observer pattern via IUpdateValues)
- Data persistence (high scores saved to JSON files)

---

## Game Rules

1. **The Grid**: The game board consists of a hexagonal grid of tiles. Three difficulty levels determine the grid size and bomb count:
   - **Easy**: 5×5 grid with 3 bombs
   - **Medium**: 10×10 grid with 15 bombs
   - **Hard**: 15×15 grid with 30 bombs

2. **Winning**: Reveal all non-bomb tiles without clicking on a bomb. The game records your completion time and allows you to save high scores.

3. **Losing**: Clicking on a bomb ends the game immediately. A game over screen is displayed.

4. **Flagging**: Right-click tiles to flag them as suspected bombs. Flagged tiles cannot be accidentally clicked. The remaining bomb count updates to show unflagged bombs.

5. **Number Display**: When you reveal a tile, it displays the number of bombs in the surrounding tiles (0-6 for hexagonal neighbours). Zero bombs means the tile is "empty" and is typically displayed differently.

6. **Cascading Reveals**: When you reveal a tile with 0 neighbouring bombs, all adjacent unrevealed tiles are automatically revealed in a cascading pattern.

7. **Timer**: A timer tracks elapsed time in seconds while the game is in progress. This timer can be paused and is displayed alongside the remaining bomb count.

---

## How to Play

### Starting a Game
1. Launch the application - you'll see the difficulty selection overlay
2. Click the **Easy**, **Medium**, or **Hard** button to select your difficulty level
3. The game board will appear with all tiles unrevealed

### During Gameplay
- **Left-click** on a hex tile to reveal it
- **Right-click** on a hex tile to place/remove a flag
- **Pause Button**: Click to pause the game timer (cannot click tiles while paused)
- **New Game Button**: Start a fresh game with the current difficulty
- **High Scores Button**: View the top 3 scores for each difficulty level

### Winning
- Successfully reveal all non-bomb tiles
- A dialog appears prompting you to enter your name for the high scores
- Your score is saved along with the timestamp and difficulty level

### Losing
- Click on a bomb and the game ends
- All bomb locations are revealed
- You can start a new game immediately

---

## Technologies Used

### Core Technologies
- **C# 8.0+**: Modern object-oriented programming language
- **WPF (Windows Presentation Foundation)**: Microsoft's framework for building desktop applications with rich graphical user interfaces
- **XAML (Extensible Application Markup Language)**: XML-based markup language for declaratively defining WPF user interfaces
- **.NET 8.0 (net8.0-windows)**: Latest .NET runtime platform providing modern language features and performance

### Libraries and Features
- **System.Text.Json**: For serializing and deserializing high score data to/from JSON files
- **System.Windows.Shapes.Polygon**: For rendering hexagonal tiles with precise geometric calculations
- **System.Windows.Threading.DispatcherTimer**: For the game timer implementation
- **System.IO.File**: For persistent storage of high scores

### Design Patterns
- **Observer Pattern (IUpdateValues)**: Loose coupling between GridManager and MainWindow for UI updates
- **Singleton/Static Class (GameDifficulties)**: Centralized management of difficulty configurations
- **Model-View Pattern**: Separation of game logic (GridManager, HexTile) from presentation (MainWindow)

---

## AQA Technical Skills Demonstrated

This project demonstrates the following technical skills from the AQA A-Level Computer Science specification:

### 1.1 Programming and Development
- ✓ Use of variables, constants, and data types appropriately
- ✓ Selection and iteration control structures
- ✓ Subroutine/method definitions with parameters and return values
- ✓ Object-oriented principles (encapsulation, inheritance where applicable)
- ✓ Exception handling (try-catch patterns in JSON serialization)

### 1.2 Data Structures
- ✓ Lists/Collections (List<HexTile>, List<HighScore>)
- ✓ Tuples and coordinate systems (HexCoord class)
- ✓ Multi-dimensional concepts (2D hexagonal grid)

### 1.3 Algorithms
- ✓ Searching (finding tiles by coordinate in grid)
- ✓ Pathfinding/Flood Fill (cascading tile reveal algorithm)
- ✓ Random number generation (bomb placement)
- ✓ Sorting (high scores by time taken)

### 1.4 Events and User Interaction
- ✓ Event-driven programming (button clicks, mouse interactions)
- ✓ Handling mouse events (left-click to reveal, right-click to flag)
- ✓ Timer events (game timer updates)

### 1.5 Data Persistence
- ✓ File I/O operations (loading/saving high scores)
- ✓ JSON serialization (System.Text.Json)
- ✓ Data validation and error handling

### 1.6 Testing Considerations
- ✓ Clear method purposes (documented comments)
- ✓ Input validation (difficulty selection)
- ✓ Boundary testing (grid edges, hexagon neighbours)

---

## Coding Styles and Standards

This project adheres to professional coding standards as required by the AQA specification:

### 1. Naming Conventions
- **Classes**: PascalCase (e.g., `MainWindow`, `GridManager`, `HexTile`)
- **Methods**: PascalCase (e.g., `InitialiseGame()`, `RevealTile()`)
- **Properties**: PascalCase (e.g., `HexesWide`, `IsBomb`)
- **Private Fields**: camelCase (e.g., `gridManager`, `timer`)
- **Constants**: UPPER_CASE (e.g., difficulty settings)

### 2. Code Organization
- **Logical Grouping**: Related methods grouped together with `#region` markers
- **Namespace Usage**: Single namespace `HexMinesweeper` for all classes
- **Access Modifiers**: Explicit use of `public`, `private`, `internal` for clear encapsulation

### 3. Comments and Documentation
- **XML Documentation Comments**: `/// <summary>` tags for public methods
- **Class-Level Comments**: Large comment blocks at the top of each class explaining purpose
- **Inline Comments**: Explanatory comments for complex logic
- **Method Comments**: Parameters and return values documented

### 4. Code Structure
- **Encapsulation**: Data hidden behind properties with getters/setters
- **Single Responsibility**: Each class has one clear purpose
- **Method Sizes**: Methods kept reasonably sized for readability
- **Parameter Count**: Limited parameters (max 3-4) with clear purposes

### 5. Error Handling
- **Null Checks**: Defensive programming against null references
- **Exception Handling**: Try-catch blocks around file I/O operations
- **Validation**: Input validation for user selections and data

### 6. Consistent Formatting
- **Indentation**: 4 spaces (or one tab) per level
- **Brace Placement**: Opening braces on same line (C# convention)
- **Line Length**: Lines kept under 120 characters for readability
- **Blank Lines**: Strategic use to separate logical sections

### 7. Magic Numbers Avoidance
- **Constants**: Difficulty parameters centralized in `GameDifficulties` class
- **Hex Radius**: Stored as property in `DifficultySettings`
- **Named Values**: Meaningful variable names instead of numeric literals

---

## Project Architecture

### Class Diagram Overview

```
┌─────────────────────┐
│   App (Entry)       │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐         ┌──────────────────┐
│   MainWindow (UI)   │────────▶│   IUpdateValues  │ (Interface)
└─────────┬───────────┘         └──────────────────┘
          │                               ▲
          │ manages                       │ implements
          ▼                               │
┌─────────────────────┐         ┌─────────┴───────┐
│  GridManager        │────────▶│  GridManager    │
│  (Game Logic)       │         │                 │
└─────────┬───────────┘         └─────────────────┘
          │
          │ contains
          ▼
┌─────────────────────┐
│   HexTile (Model)   │
└─────────┬───────────┘
          │
          │ uses
          ▼
┌─────────────────────┐
│   HexCoord          │
│   (Coordinate)      │
└─────────────────────┘

┌─────────────────────────┐
│ GameDifficulty (Enum)   │
└──────────┬──────────────┘
           │
           ▼
┌──────────────────────────┐
│ GameDifficulties (Static)│
└──────────┬───────────────┘
           │
           ▼
┌──────────────────────────┐
│ DifficultySettings       │
│ (Configuration)          │
└──────────────────────────┘

┌──────────────────────┐         ┌──────────────────────────┐
│  HighScore (Model)   │         │ HighScoreWindow (Dialog) │
└──────────────────────┘         └──────────────────────────┘

┌──────────────────────────────┐
│ InputHighScoreName (Dialog)   │
└──────────────────────────────┘
```

### File Structure

| File | Purpose |
|------|---------|
| `App.xaml` / `App.xaml.cs` | WPF application entry point and initialization |
| `MainWindow.xaml` / `MainWindow.xaml.cs` | Main game window, UI layout, and game event handling |
| `GridManager.cs` | Core game logic: grid management, bomb placement, cascading reveals |
| `HexTile.cs` | Individual tile model: state, visual representation, flags |
| `HexCoord.cs` | Coordinate system for hexagonal grid |
| `GameDifficulty.cs` | Enum defining difficulty levels (Easy, Medium, Hard) |
| `GameDifficulties.cs` | Static class housing difficulty configurations |
| `DifficultySettings.cs` | Data class storing configuration for each difficulty |
| `IUpdateValues.cs` | Interface for loose coupling between GridManager and MainWindow |
| `HighScore.cs` | Data model for storing high score entries |
| `HighScoreWindow.xaml` / `HighScoreWindow.xaml.cs` | Dialog window displaying top 3 scores per difficulty |
| `InputHighScoreName.xaml` / `InputHighScoreName.xaml.cs` | Dialog for inputting player name after winning |

---

## Key Design Decisions

### 1. Hexagonal Grid Representation
**Decision**: Store tiles in a flat `List<HexTile>` rather than a 2D array.
**Rationale**: 
- Cleaner neighbour calculation without edge-case complications
- Efficient iteration over all tiles
- Coordinates stored explicitly in each tile via `HexCoord`

### 2. Observer Pattern (IUpdateValues)
**Decision**: Use an interface to communicate between GridManager and MainWindow.
**Rationale**:
- Decouples game logic from UI
- GridManager doesn't directly depend on MainWindow
- Makes unit testing the GridManager easier
- Allows multiple implementations if needed

### 3. Static GameDifficulties Class
**Decision**: Use a static class with hardcoded difficulty settings.
**Rationale**:
- Simple centralized access to configurations
- Immutable settings prevent accidental modifications
- No instantiation overhead
- Clear contract: settings for each difficulty are fixed

### 4. Cascading Reveal Algorithm
**Decision**: Implement flood-fill style recursive revealing when a 0-bomb tile is clicked.
**Rationale**:
- Matches traditional Minesweeper behaviour
- Improves user experience by automating tedious revealing
- Reduces excessive clicking required
- Mathematically sound for hexagonal grids

### 5. High Score Persistence
**Decision**: Store high scores in JSON files in the application directory.
**Rationale**:
- JSON is human-readable and easy to debug
- No database required (simplicity)
- Portable - scores move with the executable
- System.Text.Json is built-in (.NET 8.0)

### 6. Hex Radius as Configuration
**Decision**: Include `HexRadius` in difficulty settings rather than hardcoding.
**Rationale**:
- Allows future difficulty levels with different sized hexagons
- Easier to scale UI for different screen resolutions
- Centralizes visual configuration

### 7. Timer Pause Functionality
**Decision**: Prevent tile clicking while timer is paused.
**Rationale**:
- Prevents cheating (pausing to think indefinitely)
- Matches traditional Minesweeper behaviour
- Clear game state: paused means no interaction possible

---

## Running the Project

### Prerequisites
- Windows OS
- .NET 8.0 runtime or SDK installed
- Visual Studio 2022 or equivalent C# IDE

### Build and Run
```bash
# Using Visual Studio
1. Open HexMinesweeper.sln
2. Press F5 to build and run

# Using dotnet CLI
dotnet build
dotnet run
```

### Building for Distribution
```bash
dotnet publish -c Release
# Output will be in bin/Release/net8.0-windows/publish/
```

---

## Conclusion

This HexMinesweeper project exemplifies professional software development practices suitable for AQA A-Level Computer Science NEA submissions. It demonstrates:

- Clear architectural design with proper separation of concerns
- Comprehensive code documentation and commenting
- Appropriate use of object-oriented principles
- Event-driven GUI programming with WPF
- Professional coding standards and naming conventions
- Practical implementation of algorithms and data structures
- Data persistence and file I/O operations

The project balances complexity with clarity, making it both a functional game and an educational exemplar for aspiring computer scientists.

---

**Author**: Created as AQA 7517 NEA Exemplar  
**Language**: C# with WPF Framework  
**Target Framework**: .NET 8.0 (net8.0-windows)  
**License**: Educational Use
