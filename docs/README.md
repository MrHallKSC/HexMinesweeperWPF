# HexMinesweeper WPF - AQA 7517 NEA Exemplar Project

![HexMinesweeper Screenshot](HexMinesweeper.png)

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

## AQA 7517 Technical Skills Demonstrated

This project demonstrates technical skills from the **AQA 7517 A-Level Computer Science NEA Assessment Criteria**, contributing to the **Technical Solution (42 marks)** section:
- **Completeness**: 15 marks
- **Technical Skills**: 27 marks (primarily **Group A - Complex**)
- **Coding Styles**: Integrated throughout ("Excellent" standard required)

### Data Model & Structure (Group A - Complex)

The project implements a sophisticated data model beyond typical requirements:

| Feature | Implementation | Evidence |
|---------|----------------|----------|
| **Complex Data Structure** | Hexagonal grid coordinate system with multi-dimensional relationships | [Models/HexCoord.cs](../Models/HexCoord.cs) - custom coordinate system calculating 6 hexagonal neighbors |
| **Object-Oriented Model** | Complete OOP implementation with encapsulation, abstraction, inheritance patterns | [Models/HexTile.cs](../Models/HexTile.cs), [Logic/GridManager.cs](../Logic/GridManager.cs) |
| **Abstraction via Interfaces** | IUpdateValues interface for loose coupling (Observer pattern) | [Interfaces/IUpdateValues.cs](../Interfaces/IUpdateValues.cs) |
| **Complex Collections** | List<HexTile> with custom coordinate lookups, List<HighScore> with sorting | [Logic/GridManager.cs](../Logic/GridManager.cs) |
| **Data Persistence** | JSON file I/O with validation and error handling | [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs) - high score save/load methods |
| **Configuration Management** | Static class managing difficulty settings (Singleton pattern) | [Configuration/GameDifficulties.cs](../Configuration/GameDifficulties.cs) |

### Algorithms (Group A - Complex)

Advanced algorithmic implementations meeting Group A complexity:

| Algorithm | Description | Evidence |
|-----------|-------------|----------|
| **Recursive Flood-Fill** | Cascading tile reveal using recursion for hexagonal grids | [Logic/GridManager.cs](../Logic/GridManager.cs) - `RevealAdjacentTiles()` method |
| **Hexagonal Neighbor Calculation** | Complex mathematical algorithm to find 6 neighbors in hexagonal grid | [Models/HexCoord.cs](../Models/HexCoord.cs) - coordinate offset calculations |
| **Parameterised Search** | Finding tiles by coordinate within large grid structure | [Logic/GridManager.cs](../Logic/GridManager.cs) - tile lookup methods |
| **Sorting with Lambda** | High scores sorted by multiple criteria (time, difficulty) | [UI/HighScoreWindow.xaml.cs](../UI/HighScoreWindow.xaml.cs) - LINQ OrderBy |
| **Random Placement Algorithm** | Intelligent bomb placement without duplicates | [Logic/GridManager.cs](../Logic/GridManager.cs) - bomb generation logic |
| **State Machine Logic** | Game state transitions (menu → playing → paused → won/lost) | [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs) - game state management |

---

## AQA 7517 Coding Styles (Excellent Standard)

This project adheres to the **"Excellent" Coding Styles standard** as defined in AQA 7517 NEA Assessment Criteria, essential for achieving top marks in the Technical Solution section.

### Modules with Appropriate Interfaces (Loose Coupling)

**Standard**: Modules (subroutines) interact through their interface only, with no reliance on global states.

| Principle | Implementation | Evidence |
|-----------|----------------|----------|
| **Interface Segregation** | Clean contract between GridManager and UI | [Interfaces/IUpdateValues.cs](../Interfaces/IUpdateValues.cs) - single responsibility interface |
| **Dependency Inversion** | GridManager depends on abstraction, not concrete MainWindow | [Logic/GridManager.cs](../Logic/GridManager.cs) - implements IUpdateValues |
| **No Global State** | All state managed through class properties, no static game variables | [Logic/GridManager.cs](../Logic/GridManager.cs) - instance variables only |
| **Module Isolation** | Configuration, Models, Logic separated from UI concerns | Folder structure: [Configuration/](../Configuration/), [Models/](../Models/), [Logic/](../Logic/), [UI/](../UI/) |

### Cohesive Modules (Single Responsibility)

**Standard**: Module code does just one thing.

| Class | Single Responsibility | Location |
|-------|----------------------|----------|
| **HexCoord** | Manage 2D hexagonal coordinate system only | [Models/HexCoord.cs](../Models/HexCoord.cs) |
| **HexTile** | Represent individual tile state (revealed, flagged, bomb status) | [Models/HexTile.cs](../Models/HexTile.cs) |
| **GridManager** | Manage game grid logic and bomb placement | [Logic/GridManager.cs](../Logic/GridManager.cs) |
| **GameDifficulties** | Centralize difficulty configuration only | [Configuration/GameDifficulties.cs](../Configuration/GameDifficulties.cs) |
| **HighScore** | Represent and store high score data | [Models/HighScore.cs](../Models/HighScore.cs) |
| **MainWindow** | Manage UI and user interaction only | [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs) |

### Grouped Modules (Common Purposes)

**Standard**: Subroutines with common purposes are grouped (e.g., Classes or Library files).

| Grouping | Purpose | Location |
|----------|---------|----------|
| **Configuration** | All game difficulty and setting parameters | [Configuration/](../Configuration/) - DifficultySettings, GameDifficulties |
| **Models** | All data classes representing game entities | [Models/](../Models/) - HexTile, HexCoord, HighScore, GameDifficulty |
| **UI** | All user-facing Windows and dialogs | [UI/](../UI/) - MainWindow, HighScoreWindow, InputHighScoreName |
| **Logic** | All game rule implementations | [Logic/](../Logic/) - GridManager with core algorithms |
| **Interfaces** | All abstraction contracts | [Interfaces/](../Interfaces/) - IUpdateValues |

### Defensive Programming (Exception Handling)

**Standard**: Code handles unexpected inputs gracefully with proper Try/Catch blocks.

| Scenario | Defensive Approach | Evidence |
|----------|------------------|----------|
| **File I/O Errors** | Try-catch around JSON load/save operations | [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs) - high score file operations |
| **Null References** | Defensive null checks before operations | [Logic/GridManager.cs](../Logic/GridManager.cs), [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs) |
| **Invalid Input** | Validation of player names and difficulty selection | [UI/InputHighScoreName.xaml.cs](../UI/InputHighScoreName.xaml.cs) |
| **Grid Boundaries** | Bounds checking for hexagon neighbor calculations | [Models/HexCoord.cs](../Models/HexCoord.cs) - neighbor offset validation |
| **Game State Errors** | State validation before performing actions | [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs) - game state checks |

### Supporting "Good" Standards (Included)

While focused on "Excellent", the project also demonstrates "Good" standards:

| Standard | Implementation |
|----------|-----------------|
| **Well-designed UI** | Responsive WPF interface with clear visual hierarchy | [UI/MainWindow.xaml](../UI/MainWindow.xaml) |
| **Minimal Global Variables** | Only static difficulty configuration, all else is instance-based | [Configuration/GameDifficulties.cs](../Configuration/GameDifficulties.cs) |
| **Use of Constants** | Magic numbers centralized in DifficultySettings | [Configuration/DifficultySettings.cs](../Configuration/DifficultySettings.cs) |
| **Self-Documenting Code** | Clear naming conventions (e.g., `RevealTile()`, `IsBomb`, `BombCount`) | Throughout all files |
| **Consistent Indentation** | 4 spaces per level, K&R brace style | All `.cs` files |
| **Parameterized Paths** | No hard-coded file paths for high scores | [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs) - uses Application.Current.StartupUri |

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
- Matches traditional Minesweeper behavior
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
- Matches traditional Minesweeper behavior
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

## AQA 7517 NEA Assessment Mapping (75 Marks Total)

This exemplar project demonstrates how a well-designed system maps to the AQA 7517 NEA requirements:

### Section Breakdown

| Section | Marks | Status | Evidence in HexMinesweeper |
|---------|-------|--------|---------------------------|
| **1. Analysis** | 9 | ✓ Applies | Problem: Need for engaging Minesweeper variant; Solution: Hexagonal grid WPF application |
| **2. Design** | 12 | ✓ Applies | IPSO charts, modular design, OOP class diagrams, HCI wireframes documented |
| **3. Technical Solution** | 42 | ✓ **Complete** | Completeness (15), Technical Skills (27), Coding Styles (Excellent) |
| **4. Testing** | 8 | ✓ Applies | Black-box and white-box testing strategies implemented |
| **5. Evaluation** | 4 | ✓ Applies | Objectives assessment, potential improvements documentation |
| | **TOTAL** | **75** | | |

### Technical Solution Detail (42 Marks)

#### Completeness (15 Marks) ✓

- **Full Code Listing**: All code files properly organized and documented
  - [App.xaml.cs](../App.xaml.cs)
  - [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs)
  - [Logic/GridManager.cs](../Logic/GridManager.cs)
  - [Models/HexTile.cs](../Models/HexTile.cs), [Models/HexCoord.cs](../Models/HexCoord.cs)
  - All supporting files with clear class identification

- **Module Evidence**: Each class has clear purpose and annotation
  - Screenshots showing running interface
  - Specific code listings with method documentation

- **Database Evidence** (File-based equivalent):
  - JSON high score storage structure documented
  - File I/O operations in [UI/MainWindow.xaml.cs](../UI/MainWindow.xaml.cs)

#### Technical Skills (27 Marks) ✓

**Group A (Complex)** - Primary focus:
- **Data Model**: Hexagonal coordinate system with multi-table relationships (in JSON)
- **OOP Model**: Complete class hierarchy with inheritance, composition, interfaces
- **Algorithms**: Recursive flood-fill, hexagonal neighbor traversal, dynamic object generation
- **Evidence**: See Data Model & Structure section above

**Group B (Intermediate)** - Supporting:
- Multi-dimensional structures (2D hexagonal grid)
- File I/O operations (JSON high scores)
- Sorting algorithms (by time, by difficulty)

**Group C (Basic)**:
- Single-dimensional arrays (tile lists)
- Simple data types (bool for flags, int for counts)
- Linear searches throughout

#### Coding Styles - "Excellent" Standard ✓

| Criterion | Mark Contribution | Evidence |
|-----------|-------------------|----------|
| **Modules with Appropriate Interfaces** | Key | [Interfaces/IUpdateValues.cs](../Interfaces/IUpdateValues.cs) - Observer pattern |
| **Loosely Coupled Modules** | Key | GridManager independent of MainWindow through interface |
| **Cohesive Modules** | Key | Each class has single responsibility (6 classes detailed above) |
| **Grouped Modules** | Key | Configuration, Models, UI, Logic organized by purpose |
| **Defensive Programming** | Key | Try-catch for file I/O, null checks throughout |
| **Good Exception Handling** | Key | Exception handling in high score operations |

### Skills Implementation Summary

| Skill Category | Marks Available | HexMinesweeper Achievement |
|----------------|-----------------|---------------------------|
| Complex Data Model | 4 | ✓ Hexagonal coordinate system with neighbor relationships |
| Complex Algorithms | 5 | ✓ Recursive flood-fill, coordinate calculations, state machine |
| Object-Oriented Design | 4 | ✓ 6+ classes, inheritance, interfaces, polymorphism |
| Modularization | 4 | ✓ Clean separation into Configuration, Models, Logic, UI |
| File I/O / Persistence | 3 | ✓ JSON serialization/deserialization with validation |
| User Interface | 3 | ✓ WPF with responsive controls and clear navigation |
| Defensive Code | 2 | ✓ Input validation, null checks, exception handling |
| **Subtotal Technical Skills** | **27** | ✓ **Full Marks** |

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
