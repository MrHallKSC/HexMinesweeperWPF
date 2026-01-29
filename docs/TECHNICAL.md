# HexMinesweeper: Technical Development Guide for AQA NEA Students

## Overview

This document provides a comprehensive technical breakdown of the HexMinesweeper project, explaining how a professional developer approaches building this application from the ground up. It covers:

1. Initial data structure design and prototyping
2. Command-line prototype implementation
3. Transitioning to a WPF desktop application
4. Core WPF principles and concepts
5. Architecture patterns and design decisions
6. Testing and validation approaches

The goal is to show you **the journey** of software development, not just the final product.

---

## Table of Contents

1. [Phase 1: Design the Data Structures](#phase-1-design-the-data-structures)
2. [Phase 2: Prototype with Console App](#phase-2-prototype-with-console-app)
3. [Phase 3: Transition to WPF](#phase-3-transition-to-wpf)
4. [WPF Principles Explained](#wpf-principles-explained)
5. [Architecture Patterns](#architecture-patterns)
6. [Implementation Deep Dive](#implementation-deep-dive)
7. [Testing Strategy](#testing-strategy)

---

## Phase 1: Design the Data Structures

### 1.1 Understanding the Problem

Before writing ANY code, a professional developer analyzes the problem:

**Problem**: Build a Minesweeper game on a hexagonal grid

**Questions to Answer**:
- How do we represent a hexagonal grid? (2D array? List? Custom structure?)
- How do we identify neighbours in a hex grid?
- What state does each tile need?
- How do we place bombs randomly?
- How do we detect win/loss conditions?
- How do we persist high scores?

### 1.2 Data Structure Design: HexCoord

**Problem**: We need to identify positions in a hexagonal grid.

**Rectangular Grid Approach** (❌ Doesn't work well for hex):
```
[0,0] [1,0] [2,0]
[0,1] [1,1] [2,1]
[0,2] [1,2] [2,2]
```

Each cell has 8 neighbours. But in hexagonal grids, rows are offset, making neighbour calculation complex.

**Hexagonal Grid - Double-Width Coordinates** (✅ Best approach):
```
Row 0 (even):  [0,0]  [2,0]  [4,0]
Row 1 (odd):    [1,1]  [3,1]  [5,1]
Row 2 (even):  [0,2]  [2,2]  [4,2]
```

Why this works:
- Uniform neighbour offsets regardless of row
- All hexagons have exactly 6 neighbours at fixed offsets:
  - Left/Right: (-2,0), (+2,0)
  - Upper diagonal: (-1,-1), (+1,-1)
  - Lower diagonal: (-1,+1), (+1,+1)

**The HexCoord Class**:
```csharp
public class HexCoord
{
    public int X { get; private set; }  // 0 to HexesWide*2-1
    public int Y { get; private set; }  // 0 to HexesHigh-1
    
    public HexCoord(int x, int y)
    {
        X = x;
        Y = y;
    }
}
```

**Why immutable?** Once created, a tile's position never changes. Making it immutable prevents bugs.

### 1.3 Data Structure Design: HexTile

Each hexagon on the board needs to track:
- Its position (HexCoord)
- Whether it's a bomb
- Whether it's revealed
- Whether it's flagged
- How many neighboring bombs it has

```csharp
public class HexTile
{
    public HexCoord Coordinates { get; set; }
    public bool IsBomb { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int BombNeighbours { get; set; }
}
```

**Design Note**: At this stage (before WPF), no visual properties needed. The UI layer will add those later.

### 1.4 Data Structure Design: GridManager

The grid manager orchestrates the game:
- Creates and stores all HexTiles
- Manages bomb placement
- Calculates neighbors
- Detects win/loss conditions
- Implements game algorithms (flood fill)

Core data:
```csharp
public class GridManager
{
    public List<HexTile> HexGrid { get; private set; }
    public int HexesWide { get; private set; }
    public int HexesHigh { get; private set; }
    public int BombCount { get; private set; }
    
    // Methods for game logic
    public List<HexTile> GetNeighbours(HexTile tile) { ... }
    public void AllocateBombs() { ... }
    public void CalculateBombNeighbours() { ... }
    public void FloodFillReveal(HexTile tile) { ... }
    public bool CheckWin() { ... }
}
```

### 1.5 Data Structure Design: DifficultySettings

Difficulty is configurable data, not logic:

```csharp
public class DifficultySettings
{
    public string Name { get; private set; }         // "Easy", "Medium", "Hard"
    public int HexesWide { get; private set; }       // 5, 10, 15
    public int HexesHigh { get; private set; }       // 5, 10, 15
    public int Bombs { get; private set; }           // 3, 15, 30
}
```

**Why separate from GridManager?** Configuration should be independent from logic. Different difficulties can be tested independently.

### 1.6 Data Structure Design: HighScore

Players want to track their achievements:

```csharp
public class HighScore
{
    public string PlayerName { get; set; }
    public int TimeTaken { get; set; }
    public DateTime DateAchieved { get; set; }
    public string Difficulty { get; set; }
}
```

**Why DateTime?** Provides auditability and ability to see historical progression.

---

## Phase 2: Prototype with Console App

### 2.1 Why Prototype in Console?

Before investing in UI complexity, verify the **game logic works correctly**:

✓ Can we create a grid correctly?
✓ Does neighbor calculation work?
✓ Does bomb placement work?
✓ Does flood fill reveal work?
✓ Do win conditions detect correctly?

A console app isolates logic from UI concerns.

### 2.2 Console Prototype Structure

```csharp
class Program
{
    static void Main()
    {
        // Create a small test grid
        var settings = new DifficultySettings("Test", 5, 5, 3);
        var gridManager = new GridManager(settings, null);
        
        // Initialise the grid
        for (int row = 0; row < settings.HexesHigh; row++)
        {
            for (int col = 0; col < settings.HexesWide; col++)
            {
                int xCoord = row % 2 == 0 ? col * 2 : col * 2 + 1;
                int yCoord = row;
                HexTile tile = new HexTile(xCoord, yCoord);
                gridManager.AddTile(tile);
            }
        }
        
        // Place bombs and calculate neighbours
        gridManager.AllocateBombs();
        gridManager.CalculateBombNeighbours();
        
        // Display the board
        DisplayBoard(gridManager);
        
        // Test game logic
        Console.WriteLine("\nTesting neighbour calculation...");
        TestNeighbours(gridManager);
    }
    
    static void DisplayBoard(GridManager manager)
    {
        Console.WriteLine("\nGame Board:");
        foreach (var tile in manager.HexGrid)
        {
            if (tile.IsBomb)
                Console.Write("B ");
            else
                Console.Write(tile.BombNeighbours + " ");
        }
    }
    
    static void TestNeighbours(GridManager manager)
    {
        var testTile = manager.HexGrid[0];
        var neighbours = manager.GetNeighbours(testTile);
        Console.WriteLine($"Tile at ({testTile.Coordinates.X}, {testTile.Coordinates.Y}) has {neighbours.Count} neighbours");
    }
}
```

### 2.3 Testing the Prototype

Run tests to verify core algorithms:

```csharp
// Test 1: Grid creation
Console.Assert(gridManager.HexGrid.Count == 25, "5x5 grid should have 25 tiles");

// Test 2: Bomb allocation
int bombCount = gridManager.HexGrid.Count(t => t.IsBomb);
Console.Assert(bombCount == 3, "Should have exactly 3 bombs");

// Test 3: Neighbour calculation
// Each interior tile should have exactly 6 neighbours
var interiorTile = gridManager.GetTile(new HexCoord(2, 2));
var neighbours = gridManager.GetNeighbours(interiorTile);
Console.Assert(neighbours.Count == 6, "Interior tile should have 6 neighbours");

// Test 4: Bomb counting
var safeTile = gridManager.HexGrid.FirstOrDefault(t => !t.IsBomb);
if (safeTile != null)
{
    int bombNeighbours = gridManager.GetNeighbours(safeTile)
        .Count(n => n.IsBomb);
    Console.Assert(safeTile.BombNeighbours == bombNeighbours, 
        "Bomb count should match calculated neighbours");
}
```

### 2.4 Benefits of Console Prototype

✓ **Proof of Concept**: Game logic is verified to work
✓ **Algorithm Testing**: No visual distractions, pure logic
✓ **Debugging**: Easy to print debug info
✓ **Documentation**: Serves as reference for how data structures work
✓ **Foundation**: Now safely move logic to WPF UI layer

---

## Phase 3: Transition to WPF

### 3.1 The Separation of Concerns

Now that game logic is proven, we layer on presentation (UI).

**Before WPF (Console):**
- HexTile: just data (coordinates, bomb state, etc.)
- Display: print text to console

**After WPF:**
- HexTile: now holds visual reference (Polygon)
- Display: rendered on Canvas with graphics

**Key Insight**: The GridManager code doesn't change! It still manages game logic independently.

### 3.2 Adding Visual Properties to HexTile

```csharp
public class HexTile
{
    // Original data properties (unchanged)
    public HexCoord Coordinates { get; set; }
    public bool IsBomb { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int BombNeighbours { get; set; }
    
    // NEW: Visual properties for WPF
    public Polygon Hexagon { get; set; }          // The visual shape
    public TextBlock NumberText { get; set; }     // Display bomb count
    
    public void ShowNumber(int number)
    {
        if (BombNeighbours > 0)
        {
            NumberText.Text = number.ToString();
        }
        NumberText.Visibility = Visibility.Visible;
    }
    
    public void ToggleFlag()
    {
        IsFlagged = !IsFlagged;
        Hexagon.Fill = IsFlagged ? Brushes.Yellow : Brushes.LightBlue;
    }
}
```

**Design**: Visual code stays in HexTile. Logic code stays in GridManager. Clean separation.

### 3.3 The MainWindow: UI Controller

MainWindow acts as the **controller** connecting UI to logic:

```csharp
public partial class MainWindow : Window, IUpdateValues
{
    private GridManager gridManager;  // Encapsulate the game logic
    private DispatcherTimer timer;    // UI element for timing
    
    public MainWindow()
    {
        InitializeComponent();  // Build UI from XAML
        InitialiseGame();
    }
    
    private void InitialiseGame(DifficultySettings settings)
    {
        // Create the logic layer
        gridManager = new GridManager(settings, this);
        
        // Create the visual representation
        DrawHexGrid();
        
        // Initialise the game
        gridManager.AllocateBombs();
        gridManager.CalculateBombNeighbours();
    }
    
    private void LeftClick(object sender, MouseButtonEventArgs e)
    {
        // User clicked a hex
        Polygon hex = sender as Polygon;
        HexTile hexTile = hex.Tag as HexTile;
        
        if (hexTile.IsBomb)
        {
            // Loss condition
            OverlayText.Text = "Game Over!";
        }
        else
        {
            // Reveal the tile (delegate to GridManager)
            gridManager.FloodFillReveal(hexTile);
            
            // Check if we won
            if (gridManager.CheckWin())
            {
                SetWinScreen();
            }
        }
    }
}
```

**Key Pattern**: MainWindow delegates game logic to GridManager, handles only UI updates.

---

## WPF Principles Explained

### 4.1 What is WPF?

WPF (Windows Presentation Foundation) is Microsoft's framework for building **event-driven, graphically rich desktop applications**.

Key concepts:

| Concept | Explanation |
|---------|-------------|
| **XAML** | XML-based markup for defining UI layout (like HTML) |
| **Code-Behind** | C# code that responds to UI events |
| **Event-Driven** | Application reacts to user actions (clicks, keyboard, etc.) |
| **Control** | UI element (Button, Label, Canvas, etc.) |
| **Property** | Characteristic of a control (Text, Background, Visibility, etc.) |
| **Event** | Something that happens (Click, MouseEnter, etc.) |
| **Data Binding** | Connect data to UI automatically |

### 4.2 XAML: Declarative UI Definition

**XAML** (pronounced "zammel") is XML-based markup for defining UI structure:

```xml
<!-- MainWindow.xaml -->
<Window x:Class="HexMinesweeper.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        Title="HexMinesweeper" Height="600" Width="800">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*"/>    <!-- Game canvas -->
            <ColumnDefinition Width="150"/>  <!-- Control buttons -->
        </Grid.ColumnDefinitions>
        
        <!-- Canvas for drawing hex grid -->
        <Canvas x:Name="canvas" Grid.Column="0" Background="White"/>
        
        <!-- Control panel -->
        <StackPanel Grid.Column="1" Margin="10">
            <Button Name="PauseButton" Click="PauseButton_Click">Pause</Button>
            <Button Name="ResetButton" Click="ResetButton_Click">New Game</Button>
            <Label Name="lblTimer" Content="Time: 0"/>
            <Label Name="lblBombsRemaining" Content="Bombs: 3"/>
        </StackPanel>
    </Grid>
</Window>
```

**Why XAML?**
- Separates design (XAML) from logic (C#)
- Designers can edit UI without touching code
- Declarative (describe what you want, not how to build it)
- Visual Studio has a designer tool for XAML

### 4.3 Code-Behind: Event Handling

XAML specifies **what** exists and **event names**. C# code-behind specifies **how to respond**:

```csharp
// MainWindow.xaml.cs (code-behind)
public partial class MainWindow : Window
{
    // This is automatically wired up to XAML
    // <Button Click="PauseButton_Click">
    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {
        // Handle the click
        if (timer.IsEnabled)
        {
            timer.Stop();
            PauseButton.Content = "Resume";
        }
        else
        {
            timer.Start();
            PauseButton.Content = "Pause";
        }
    }
    
    // Similarly for hexagon clicks (MouseLeftButtonUp, MouseRightButtonUp)
    private void LeftClick(object sender, MouseButtonEventArgs e)
    {
        // Handle left click on hexagon
    }
    
    private void RightClick(object sender, MouseButtonEventArgs e)
    {
        // Handle right click on hexagon
    }
}
```

### 4.4 Controls and Properties

WPF provides many **controls** (UI elements) with **properties** you can set:

```csharp
// Create a control programmatically
Button myButton = new Button();
myButton.Content = "Click Me";           // What the button displays
myButton.Background = Brushes.Blue;      // Button color
myButton.Width = 100;                    // Button width
myButton.Click += MyButton_Click;        // Wire up event handler

// Create a label and update content
Label myLabel = new Label();
myLabel.Content = "Score: 100";

// Update label (common pattern in games)
int score = 150;
myLabel.Content = $"Score: {score}";
```

### 4.5 Brushes and Colors

WPF uses **brushes** to paint surfaces:

```csharp
// Predefined brushes
polygon.Fill = Brushes.LightBlue;        // Unrevealed tile
polygon.Fill = Brushes.White;            // Revealed, safe tile
polygon.Fill = Brushes.Yellow;           // Flagged tile
polygon.Fill = Brushes.Red;              // Bomb revealed

// Custom colors
SolidColorBrush customBrush = new SolidColorBrush(Color.FromArgb(255, 100, 150, 200));
polygon.Fill = customBrush;

// Alpha transparency
SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(161, 0, 0, 0));
overlay.Background = transparent;
```

### 4.6 The Canvas: Positioning Elements

The **Canvas** control lets you position child elements at exact X,Y coordinates:

```csharp
// Create a hexagon
Polygon hexagon = new Polygon();
hexagon.Points = new PointCollection { /*vertices*/ };
hexagon.Fill = Brushes.LightBlue;

// Position it at (100, 200)
Canvas.SetLeft(hexagon, 100);
Canvas.SetTop(hexagon, 200);

// Add to canvas
canvas.Children.Add(hexagon);
```

**Why use Canvas for hex grid?**
- Precise control over positioning (essential for hex grid geometry)
- Each hexagon positioned independently
- Easy to update individual hexagon appearance

### 4.7 Drawing Polygons Programmatically

A **Polygon** is a WPF shape that connects multiple points:

```csharp
// Create a hexagon by calculating 6 vertices
Polygon hexagon = new Polygon 
{ 
    Stroke = Brushes.Black,          // Border color
    StrokeThickness = 2,             // Border width
    Fill = Brushes.LightBlue         // Interior color
};

hexagon.Points = new PointCollection();

// Calculate 6 vertices for a regular hexagon
double radius = 30;                  // Distance from center to vertex
double startAngle = Math.PI / 6;    // 30 degrees (pointy-top orientation)

for (int i = 0; i < 6; i++)
{
    // Each vertex is at 60 degrees (Math.PI / 3) apart
    double angle = startAngle + Math.PI / 3 * i;
    double x = centerX + radius * Math.Cos(angle);
    double y = centerY + radius * Math.Sin(angle);
    hexagon.Points.Add(new Point(x, y));
}

// Result: Perfect regular hexagon centered at (centerX, centerY)
```

**Geometry Insight**: Regular hexagons have vertices at 60-degree intervals. Using trigonometry (sine, cosine), we calculate each vertex's position.

### 4.8 Event-Driven Programming

WPF applications are **event-driven**: respond to user actions.

```csharp
// Register event handlers
hexagon.MouseLeftButtonUp += LeftClick;   // User left-clicked
hexagon.MouseRightButtonUp += RightClick; // User right-clicked
button.Click += Button_Click;              // User clicked button

// Event handlers receive sender and event args
private void LeftClick(object sender, MouseButtonEventArgs e)
{
    // sender: the object that was clicked (the hexagon)
    // e: details about the click (which button, position, etc.)
    
    Polygon hex = sender as Polygon;
    if (hex.Tag is HexTile hexTile)
    {
        // Process the click
    }
    
    // Prevent other handlers from seeing this event
    e.Handled = true;
}
```

**Event Flow**: User action → WPF detects → fires event → your handler runs

### 4.9 DispatcherTimer: Game Loop

A **DispatcherTimer** is WPF's way to execute code repeatedly on schedule:

```csharp
// Create timer
timer = new DispatcherTimer();
timer.Interval = TimeSpan.FromSeconds(1);  // Fire every 1 second
timer.Tick += Timer_Tick;                  // Wire up event handler

// Start the timer
timer.Start();

// Event handler (called every 1 second)
private void Timer_Tick(object sender, EventArgs e)
{
    gridManager.SecondsElapsed++;
    lblTimer.Content = $"Time: {gridManager.SecondsElapsed}";
}

// Stop the timer
timer.Stop();
```

**Why not System.Timers.Timer?** DispatcherTimer runs on the UI thread, so it's safe to update UI controls directly. System.Timers.Timer runs on a background thread and would require thread synchronization.

### 4.10 Visibility: Show/Hide UI Elements

WPF controls can be shown/hidden:

```csharp
// Show a control
overlay.Visibility = Visibility.Visible;

// Hide a control
overlay.Visibility = Visibility.Collapsed;  // Hidden, doesn't take space
overlay.Visibility = Visibility.Hidden;      // Hidden, still takes space

// Toggle visibility
if (lblTimer.Visibility == Visibility.Visible)
{
    lblTimer.Visibility = Visibility.Collapsed;
}
else
{
    lblTimer.Visibility = Visibility.Visible;
}
```

---

## Architecture Patterns

### 5.1 Separation of Concerns

The project separates concerns across layers:

```
┌─────────────────────────────┐
│   PRESENTATION LAYER (UI)   │  MainWindow.xaml.cs
│   - Event handling          │  HighScoreWindow.xaml.cs
│   - Visual rendering        │  InputHighScoreName.xaml.cs
└──────────────┬──────────────┘
               │ uses
               ▼
┌─────────────────────────────┐
│   LOGIC LAYER               │  GridManager.cs
│   - Game rules              │  Flood fill algorithm
│   - Win/loss detection      │  Bomb placement
│   - Data management         │
└──────────────┬──────────────┘
               │ manipulates
               ▼
┌─────────────────────────────┐
│   DATA LAYER (MODELS)       │  HexTile.cs
│   - Game state              │  HexCoord.cs
│   - Configuration           │  GameDifficulty.cs
│   - Persistence             │  HighScore.cs
└─────────────────────────────┘
```

**Benefits**:
- **Testability**: Test logic independently of UI
- **Reusability**: Logic works with any UI (console, web, etc.)
- **Maintainability**: Change UI without touching game logic
- **Clarity**: Each layer has single responsibility

### 5.2 Observer Pattern: IUpdateValues

GridManager (logic) needs to update MainWindow (UI) without direct dependency.

**Problem**: Direct coupling
```csharp
// BAD: GridManager directly references MainWindow
public class GridManager
{
    private MainWindow window;
    
    public void UnflagTile()
    {
        window.UpdateBombCount(1);  // Tightly coupled!
    }
}
```

**Solution**: Use interface (Observer Pattern)
```csharp
// Interface defines the contract
public interface IUpdateValues
{
    void UpdateBombCount(int change);
    void StopTimer();
}

// GridManager depends on interface, not MainWindow
public class GridManager
{
    private IUpdateValues updateValues;
    
    public GridManager(DifficultySettings diff, IUpdateValues updateValues)
    {
        this.updateValues = updateValues;
    }
    
    public void UnflagTile()
    {
        updateValues.UpdateBombCount(1);  // Loosely coupled!
    }
}

// MainWindow implements the interface
public partial class MainWindow : Window, IUpdateValues
{
    public void UpdateBombCount(int change)
    {
        gridManager.RemainingBombs += change;
        lblBombsRemaining.Content = $"Bombs: {gridManager.RemainingBombs}";
    }
    
    public void StopTimer()
    {
        timer.Stop();
    }
}

// MainWindow passes itself to GridManager
gridManager = new GridManager(settings, this);
```

**Benefit**: GridManager can be tested with a mock IUpdateValues implementation.

### 5.3 Static Class: GameDifficulties Registry

Difficulty settings are global configuration:

```csharp
public static class GameDifficulties
{
    // Predefined immutable settings
    public static readonly DifficultySettings Easy = 
        new DifficultySettings("Easy", 5, 5, 3, 50);
    public static readonly DifficultySettings Medium = 
        new DifficultySettings("Medium", 10, 10, 15, 30);
    public static readonly DifficultySettings Hard = 
        new DifficultySettings("Hard", 15, 15, 30, 20);
    
    // Lookup by enum
    public static DifficultySettings GetSettings(GameDifficulty difficulty)
    {
        switch (difficulty)
        {
            case GameDifficulty.Easy: return Easy;
            case GameDifficulty.Medium: return Medium;
            case GameDifficulty.Hard: return Hard;
            default: throw new ArgumentOutOfRangeException();
        }
    }
}

// Usage:
var easySettings = GameDifficulties.GetSettings(GameDifficulty.Easy);
var gridManager = new GridManager(easySettings, this);
```

**Why static?**
- Configuration is global (doesn't need instances)
- Thread-safe (loaded once at startup)
- Prevents accidental duplication
- Clear, centralized definition

### 5.4 Data Models vs. Logic

**Data models** hold state. **Logic classes** manipulate state.

**HexTile (Data Model)**:
```csharp
public class HexTile
{
    public bool IsBomb { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int BombNeighbours { get; set; }
    // Visual properties
    public Polygon Hexagon { get; set; }
    public TextBlock NumberText { get; set; }
}
```

**GridManager (Logic Class)**:
```csharp
public class GridManager
{
    public List<HexTile> HexGrid { get; private set; }
    
    public void AllocateBombs() { /* sets IsBomb on tiles */ }
    public void CalculateBombNeighbours() { /* sets BombNeighbours */ }
    public void RevealTile(HexTile tile) { /* sets IsRevealed */ }
    public void FloodFillReveal(HexTile tile) { /* reveals multiple tiles */ }
}
```

HexTile knows nothing about the game. GridManager knows everything. Pure separation.

---

## Implementation Deep Dive

### 6.1 Flood Fill Algorithm: Cascading Reveals

When a player clicks a tile with 0 neighboring bombs, all adjacent tiles are automatically revealed:

```csharp
public void FloodFillReveal(HexTile tile)
{
    // Base case: if already revealed, stop
    if (tile.IsRevealed)
        return;
    
    // Reveal this tile
    RevealTile(tile);
    
    // If this tile has neighboring bombs, we're done
    // (cascading stops at bomb-adjacency boundary)
    if (tile.BombNeighbours == 0)
    {
        // Recursively reveal neighbors
        List<HexTile> neighbours = GetNeighbours(tile);
        foreach (HexTile neighbour in neighbours)
        {
            FloodFillReveal(neighbour);  // Recursive call
        }
    }
}
```

**Algorithm Flow Example**:

```
Initial: Click tile with 0 neighbors
         ┌─────┐
         │  0  │  ← Clicked
         └─────┘

FloodFill reveals the clicked tile, finds 0 neighbors, recurses on neighbors:
         ┌─────┐
         │  0  │  ← Revealed
         └─────┘
        / │ │ \
       1  0  0  2  ← Neighbors revealed

Continue recursing on the 0-neighbor tiles (1 and 2):
     ┌───────────┐
     │  1  0  2  │
     │  0  0  0  │  ← More neighbours revealed
     │  1  1  1  │
     └───────────┘
     
Until reaching bomb-adjacent tiles (1, 1, 1) which have neighbours > 0, stop.
```

**Time Complexity**: O(n) where n = number of tiles in connected region
**Space Complexity**: O(n) for recursion stack

### 6.2 Neighbour Calculation: Double-Width Coordinates

The hexagonal coordinate system makes neighbour finding elegant:

```csharp
public List<HexTile> GetNeighbours(HexTile tile)
{
    List<HexTile> neighbours = new List<HexTile>();
    
    // Fixed offsets work for all hexagons in double-width system
    List<Point> offsets = new List<Point>
    {
        new Point(-2, 0),   // Left
        new Point(2, 0),    // Right
        new Point(-1, -1),  // Upper-left
        new Point(1, -1),   // Upper-right
        new Point(-1, 1),   // Lower-left
        new Point(1, 1)     // Lower-right
    };
    
    foreach (Point offset in offsets)
    {
        int neighbourX = tile.Coordinates.X + (int)offset.X;
        int neighbourY = tile.Coordinates.Y + (int)offset.Y;
        
        HexCoord neighbourCoord = new HexCoord(neighbourX, neighbourY);
        
        // Check if this coordinate exists in the grid
        if (ContainsTileAt(neighbourCoord))
        {
            neighbours.Add(GetTile(neighbourCoord));
        }
    }
    
    return neighbours;
}
```

**Why this works**: The double-width coordinate system is designed so neighbour offsets are uniform. This is much simpler than axial or cube coordinates.

### 6.3 JSON Persistence: High Scores

High scores are saved to JSON files for persistence:

```csharp
public void SaveHighScores(List<HighScore> highScores)
{
    // Serialize to JSON
    string json = JsonSerializer.Serialize(highScores);
    
    // Write to file named after difficulty
    File.WriteAllText($"{currentDifficulty.Name}.json", json);
}

public List<HighScore> LoadHighScores(string difficulty)
{
    string filePath = $"{difficulty}.json";
    
    if (File.Exists(filePath))
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<HighScore>>(json);
    }
    
    return new List<HighScore>();
}
```

**Example JSON file (Easy.json)**:
```json
[
  {
    "playerName": "Alice",
    "timeTaken": 45,
    "dateAchieved": "2024-01-15T10:23:45.1234567",
    "difficulty": "Easy"
  },
  {
    "playerName": "Bob",
    "timeTaken": 52,
    "dateAchieved": "2024-01-15T11:30:12.5678901",
    "difficulty": "Easy"
  }
]
```

**Why JSON?**
- Human-readable (easy to debug)
- Built-in support (System.Text.Json)
- Portable (moves with the executable)
- No external dependencies

### 6.4 Dialog Results: Input High Score Name

WPF dialogs return information via **DialogResult**:

```csharp
public partial class InputHighScoreName : Window
{
    public InputHighScoreName(int seconds, string difficulty)
    {
        InitializeComponent();
        txtElapsed.Text = $"Difficulty: {difficulty}\nTime: {seconds}";
    }
    
    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        // Returning true means "user confirmed"
        DialogResult = true;
        // Window automatically closes
    }
    
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        // Returning false means "user cancelled"
        DialogResult = false;
    }
}

// Usage in GridManager
public string PromptForName()
{
    InputHighScoreName inputDialog = 
        new InputHighScoreName(SecondsElapsed, currentDifficulty.Name);
    
    // ShowDialog() blocks until user closes the dialog
    if (inputDialog.ShowDialog() == true)
    {
        // User clicked OK
        return inputDialog.txtName.Text;
    }
    else
    {
        // User clicked Cancel
        return null;
    }
}
```

**DialogResult Pattern**:
- Dialog code sets DialogResult property
- ShowDialog() returns that value
- Caller acts based on the result
- Clean separation of concerns

---

## Testing Strategy

### 7.1 Unit Testing the Logic Layer

Since GridManager is independent of UI, it's easily testable:

```csharp
[TestClass]
public class GridManagerTests
{
    private GridManager gridManager;
    private MockUI mockUI;
    
    [TestInitialize]
    public void Setup()
    {
        mockUI = new MockUI();
        var settings = GameDifficulties.GetSettings(GameDifficulty.Easy);
        gridManager = new GridManager(settings, mockUI);
    }
    
    [TestMethod]
    public void TestGridCreation()
    {
        // Initialize grid
        for (int i = 0; i < 25; i++)
        {
            gridManager.AddTileToGrid(new HexTile(i, i));
        }
        
        // Assert
        Assert.AreEqual(25, gridManager.HexGrid.Count);
    }
    
    [TestMethod]
    public void TestBombAllocation()
    {
        gridManager.AllocateBombs();
        int bombCount = gridManager.HexGrid.Count(t => t.IsBomb);
        Assert.AreEqual(3, bombCount);  // Easy difficulty = 3 bombs
    }
    
    [TestMethod]
    public void TestNeighborCalculation()
    {
        // Interior tile should have 6 neighbors
        var interiorTile = gridManager.GetTile(new HexCoord(2, 2));
        var neighbors = gridManager.GetNeighbours(interiorTile);
        Assert.AreEqual(6, neighbors.Count);
    }
    
    [TestMethod]
    public void TestWinCondition()
    {
        // All non-bomb tiles revealed = win
        foreach (var tile in gridManager.HexGrid)
        {
            if (!tile.IsBomb)
            {
                tile.IsRevealed = true;
            }
        }
        
        bool won = gridManager.CheckWin();
        Assert.IsTrue(won);
    }
}

// Mock UI for testing (no real UI needed)
public class MockUI : IUpdateValues
{
    public int BombUpdateCount { get; set; }
    public int TimerStopCount { get; set; }
    
    public void UpdateBombCount(int change)
    {
        BombUpdateCount++;
    }
    
    public void StopTimer()
    {
        TimerStopCount++;
    }
}
```

### 7.2 Integration Testing the UI

Test the connection between UI and logic:

```csharp
[TestClass]
public class IntegrationTests
{
    private MainWindow mainWindow;
    
    [TestInitialize]
    public void Setup()
    {
        mainWindow = new MainWindow();
    }
    
    [TestMethod]
    public void TestGameInitialization()
    {
        // Verify grid exists
        Assert.IsNotNull(mainWindow.gridManager);
        
        // Verify all tiles created
        Assert.AreEqual(25, mainWindow.gridManager.HexGrid.Count);
        
        // Verify some tiles are bombs
        int bombCount = mainWindow.gridManager.HexGrid.Count(t => t.IsBomb);
        Assert.IsTrue(bombCount > 0);
    }
    
    [TestMethod]
    public void TestUIUpdateAfterFlagging()
    {
        // Get a tile
        var tile = mainWindow.gridManager.HexGrid.First();
        
        // Flag it (this triggers UpdateBombCount)
        tile.ToggleFlag();
        mainWindow.UpdateBombCount(-1);
        
        // Verify UI updated
        var content = mainWindow.lblBombsRemaining.Content.ToString();
        Assert.IsTrue(content.Contains("Bombs:"));
    }
}
```

### 7.3 Manual Testing Checklist

For a complete exemplar, document testing approach:

```
BASIC GAMEPLAY:
☐ Click on safe tile - reveals correctly
☐ Click on bomb - game over
☐ Right-click to flag - toggles flag
☐ Cascading reveal - reveals adjacent 0-bomb tiles
☐ Timer starts on first click
☐ Pause/Resume works

DIFFICULTY LEVELS:
☐ Easy: 5x5 grid, 3 bombs
☐ Medium: 10x10 grid, 15 bombs
☐ Hard: 15x15 grid, 30 bombs
☐ Correct bomb counts

HIGH SCORES:
☐ Winning prompts for name
☐ Score saved to file
☐ High scores window shows top 3 per difficulty
☐ Scores sorted by time (fastest first)

EDGE CASES:
☐ Click bomb on first move - instant loss
☐ Flag all bombs - game still completable
☐ Pause mid-game, resume - timer continues
☐ Window resize - layout adjusts
```

---

## Summary: The Development Journey

This is how a professional developer builds HexMinesweeper:

### Phase 1: Analysis & Design (No Code)
- Understand the problem
- Sketch data structures
- Plan algorithms
- Design class relationships

### Phase 2: Console Prototype (Pure Logic)
- Implement data structures (HexCoord, HexTile, GridManager)
- Implement and test algorithms (flood fill, neighbour calculation)
- Verify game rules work
- No UI, just core game logic

### Phase 3: Transition to WPF (Add Presentation)
- Add visual properties to data models
- Build XAML UI structure
- Create event handlers in code-behind
- Connect UI to logic via clean interface

### Phase 4: Polish & Testing (Quality Assurance)
- Add features (high scores, difficulty, dialogs)
- Test thoroughly (unit tests, integration tests, manual tests)
- Handle edge cases
- Optimize performance

### Key Takeaways

1. **Separate concerns**: Logic independent from UI
2. **Design first**: Think before coding
3. **Test early**: Verify core algorithms in console
4. **Interfaces matter**: Use IUpdateValues pattern for loose coupling
5. **WPF enables rich UI**: Leverage Polygon, Canvas, DispatcherTimer
6. **Hexagonal grids are elegant**: Double-width coordinates simplify neighbour calculation
7. **Persistence is simple**: JSON files work well for high scores
8. **Architecture trumps features**: Good design beats clever code

---

## Further Reading

- **WPF Official Docs**: Microsoft's complete WPF documentation
- **XAML Reference**: Learn all XAML capabilities
- **Hexagonal Grids**: Red Blob Games has excellent hex grid resources
- **Design Patterns**: Gang of Four patterns (Observer, Registry, etc.)
- **Game Development**: General game loop and event-driven architecture
- **Testing**: Unit testing frameworks (MSTest, NUnit, xUnit)

---

**This exemplar project demonstrates professional software engineering practices suitable for AQA A-Level Computer Science NEA submissions.**
