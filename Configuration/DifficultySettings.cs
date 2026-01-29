using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== DifficultySettings.cs ====================
    /// 
    /// DIFFICULTY CONFIGURATION DATA CLASS
    /// 
    /// PURPOSE:
    /// DifficultySettings is a data model (sometimes called a "data transfer object" or DTO)
    /// that bundles all the configuration parameters for a specific difficulty level into
    /// a single object. It contains no logic, just data storage.
    /// 
    /// WHY A SEPARATE CLASS?
    /// 
    /// Instead of passing 5 separate parameters everywhere:
    ///     InitialiseGame(string name, int width, int height, int bombs, int radius)
    /// 
    /// We pass a single object:
    ///     InitialiseGame(DifficultySettings settings)
    /// 
    /// Benefits:
    /// - CLEANER SIGNATURES: One parameter instead of five
    /// - LOGICAL GROUPING: Related data stays together
    /// - MAINTAINABILITY: Adding new settings (e.g., starting lives) doesn't require
    ///   changing method signatures everywhere
    /// - FLEXIBILITY: The object can be modified or extended without affecting callers
    /// - TYPE SAFETY: Can't accidentally swap parameter order (new Difficulty(15, 10, 30, 20)
    ///   vs. new Difficulty(name:"Medium", hexesWide: 10, etc.)
    /// 
    /// PROPERTIES (ENCAPSULATION):
    /// 
    /// Each property has:
    /// - A getter (public read access)
    /// - A private setter (can only be set in the constructor)
    /// 
    /// This makes the object effectively immutable after construction:
    /// 
    ///     var settings = new DifficultySettings("Easy", 5, 5, 3, 50);
    ///     int width = settings.HexesWide;  // OK - reading
    ///     settings.HexesWide = 10;         // COMPILE ERROR - no public setter
    /// 
    /// Immutability prevents bugs:
    /// - Settings can't be accidentally modified mid-game
    /// - Multiple threads can safely share immutable objects
    /// - Easier to reason about code when objects don't change
    /// 
    /// THE FOUR CONFIGURATION PARAMETERS:
    /// 
    /// Name (string):
    /// - The difficulty name: "Easy", "Medium", or "Hard"
    /// - Used as identifier for saving/loading high scores
    /// - High scores are stored in files named Easy.json, Medium.json, Hard.json
    /// - Also displayed in the UI (title, high score window, etc.)
    /// 
    /// HexesWide (int):
    /// - Number of hexagons across the board (columns)
    /// - Easy: 5 hexagons wide
    /// - Medium: 10 hexagons wide
    /// - Hard: 15 hexagons wide
    /// - Used by GridManager and MainWindow to calculate total grid size
    /// - Used in the loop that creates all tiles: for (int col = 0; col < gridManager.HexesWide; col++)
    /// 
    /// HexesHigh (int):
    /// - Number of hexagons down the board (rows)
    /// - Easy: 5 hexagons high
    /// - Medium: 10 hexagons high
    /// - Hard: 15 hexagons high
    /// - Used similarly to HexesWide in board initialization
    /// - Note: For hexagonal grids, width and height are independent unlike rectangular grids
    ///   (due to hexagon offset patterns)
    /// 
    /// Bombs (int):
    /// - Total number of bombs to place on the board
    /// - Easy: 3 bombs (6% of 5x5=25 tiles)
    /// - Medium: 15 bombs (15% of 10x10=100 tiles)
    /// - Hard: 30 bombs (13% of 15x15=225 tiles)
    /// - Higher difficulty = higher bomb density (more challenging)
    /// - Passed to GridManager.AllocateBombs() to determine random placement
    /// 
    /// HexRadius (int):
    /// - Pixel size of each hexagon (measured from center to vertex)
    /// - Easy: 50 pixels (large, easy to click)
    /// - Medium: 30 pixels (medium)
    /// - Hard: 20 pixels (small, precise clicking required)
    /// - Affects:
    ///   * Visual size of hexagons on screen
    ///   * Window size (larger hexagons = larger window)
    ///   * Font size of numbers (radius is used as FontSize)
    /// - Calculated from center, so actual visible size is larger
    /// - Mathematical relationship: hexagon diameter = 2 * radius
    /// 
    /// HOW IT'S USED IN THE APPLICATION:
    /// 
    /// CREATION:
    /// - Created by GameDifficulties.GetSettings(GameDifficulty enum)
    /// - Example: GameDifficulties.GetSettings(GameDifficulty.Medium) returns the
    ///   Medium DifficultySettings instance
    /// 
    /// STORAGE:
    /// - Stored in MainWindow.settings field
    /// - Stored in GridManager.currentDifficulty property
    /// 
    /// USAGE:
    /// - Passed to GridManager constructor: new GridManager(settings, this)
    /// - Used in MainWindow.InitialiseGame() to calculate window size
    /// - Used in GridManager to initialise board dimensions
    /// - Used as identifier for high score files
    /// 
    /// FLOW EXAMPLE:
    /// 1. User clicks "Hard" button
    /// 2. MainWindow.HardButton_Click() calls StartGame(GameDifficulty.Hard)
    /// 3. StartGame() calls GameDifficulties.GetSettings(GameDifficulty.Hard)
    ///    which returns: new DifficultySettings("Hard", 15, 15, 30, 20)
    /// 4. Settings stored in MainWindow.settings
    /// 5. Settings passed to MainWindow.InitialiseGame(settings)
    /// 6. InitialiseGame() uses settings.HexesWide, settings.HexesHigh to calculate
    ///    window size and create the visual layout
    /// 7. GridManager is created with settings: new GridManager(settings, this)
    /// 8. GridManager stores settings in currentDifficulty and uses it throughout gameplay
    /// 9. When game is won, high score is saved to "Hard.json" using settings.Name
    /// 
    /// DESIGN PATTERNS DEMONSTRATED:
    /// 
    /// 1. DATA TRANSFER OBJECT (DTO) PATTERN:
    ///    - Simple class that carries data between different parts of the application
    ///    - No business logic, just storage and access
    /// 
    /// 2. IMMUTABLE OBJECT PATTERN:
    ///    - Once created, the object's state cannot change
    ///    - Thread-safe without synchronization
    ///    - Predictable behavior
    /// 
    /// 3. VALUE OBJECT PATTERN:
    ///    - Objects are compared by their values, not identity
    ///    - (Note: This implementation doesn't override Equals(), but it could)
    /// 
    /// MATHEMATICAL RELATIONSHIPS:
    /// 
    /// Easy:   5 × 5 = 25 tiles,   3 bombs   = 12% bomb density
    /// Medium: 10 × 10 = 100 tiles, 15 bombs = 15% bomb density
    /// Hard:   15 × 15 = 225 tiles, 30 bombs = 13.3% bomb density
    /// 
    /// These proportions create increasingly difficult challenges.
    /// 
    /// POSSIBLE EXTENSIONS:
    /// 
    /// If you wanted to add more features, you could extend this class:
    /// 
    ///     public class DifficultySettings
    ///     {
    ///         public string Name { get; private set; }
    ///         public int HexesWide { get; private set; }
    ///         public int HexesHigh { get; private set; }
    ///         public int Bombs { get; private set; }
    ///         public int HexRadius { get; private set; }
    ///         
    ///         // NEW FEATURES:
    ///         public int TimeLimit { get; private set; }        // Time-based challenge
    ///         public int LivesAllowed { get; private set; }     // Multiple lives
    ///         public bool CascadeReveal { get; private set; }   // Enable/disable cascade
    ///         public int StarRatingThreshold { get; private set; } // Easy 60s, Medium 120s, Hard 300s
    ///     }
    /// 
    /// SERIALIZATION NOTE:
    /// 
    /// DifficultySettings could be saved/loaded from JSON if you wanted to make
    /// difficulties configurable without recompiling. The properties are simple types
    /// that System.Text.Json can handle directly.
    /// 
    /// ================================================================
    /// </summary>

    public class DifficultySettings
    {
        public string Name { get; private set; }
        public int HexesWide { get; private set; }
        public int HexesHigh { get; private set; }
        public int Bombs { get; private set; }
        public int HexRadius { get; private set; }

        public DifficultySettings(string name, int hexesWide, int hexesHigh, int bombs, int hexRadius)
        {
            Name = name;
            HexesWide = hexesWide;
            HexesHigh = hexesHigh;
            Bombs = bombs;
            HexRadius = hexRadius;
        }
    }
}
