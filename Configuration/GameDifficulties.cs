using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== GameDifficulties.cs ====================
    /// 
    /// DIFFICULTY SETTINGS REPOSITORY - STATIC CLASS
    /// 
    /// PURPOSE:
    /// GameDifficulties is a static class that serves as a centralized repository for
    /// difficulty configuration. It stores the settings for Easy, Medium, and Hard modes,
    /// and provides a method to look up settings based on a GameDifficulty enum value.
    /// 
    /// WHY A STATIC CLASS?
    /// 
    /// A static class:
    /// - Cannot be instantiated (you never write "new GameDifficulties()")
    /// - Has only static members (fields and methods)
    /// - Is accessed directly by class name (GameDifficulties.GetSettings(...))
    /// - Loads into memory once and persists for the application lifetime
    /// 
    /// This is appropriate for GameDifficulties because:
    /// - Difficulty settings are global configuration, not instance-specific
    /// - There's never a need for multiple instances
    /// - All game instances should use the same difficulty definitions
    /// - It acts as a global registry/lookup table
    /// 
    /// DESIGN PATTERN: REGISTRY PATTERN
    /// 
    /// This class implements the Registry Pattern:
    /// - Stores predefined configurations (Easy, Medium, Hard)
    /// - Provides a lookup method to retrieve configurations
    /// - Centralizes configuration management in one place
    /// - Other classes don't need to know how settings are created
    /// 
    /// HARDCODED VS. CONFIGURABLE:
    /// 
    /// Current Design (Hardcoded):
    /// - Difficulty settings are hardcoded in this class
    /// - Fast, simple, no file I/O needed
    /// - Suitable when settings rarely change
    /// - Drawback: to change settings requires recompilation
    /// 
    /// Alternative Design (External Config):
    /// - Load settings from XML, JSON, or INI file
    /// - Settings can change without recompilation
    /// - More flexible for developers or players
    /// - Adds complexity and file I/O
    /// 
    /// For an exemplar project, hardcoding is appropriate - it's simpler and clearer.
    /// 
    /// THE DIFFICULTY SETTINGS:
    /// 
    /// Each DifficultySettings includes:
    /// - Name: "Easy", "Medium", or "Hard" (string identifier)
    /// - HexesWide: number of hexagons across the grid
    /// - HexesHigh: number of hexagons down the grid
    /// - Bombs: how many bombs to place
    /// - HexRadius: pixel size of each hexagon (affects UI scaling)
    /// 
    /// DIFFICULTY PROGRESSION:
    /// 
    /// Easy:   5x5 grid,   3 bombs,  HexRadius 50px
    ///         - Small board
    ///         - Very few bombs
    ///         - Large hexagons (easy to click)
    ///         - Intended for learning the game
    /// 
    /// Medium: 10x10 grid, 15 bombs, HexRadius 30px
    ///         - Medium board
    ///         - Reasonable bomb density
    ///         - Medium hexagons
    ///         - Standard difficulty
    /// 
    /// Hard:   15x15 grid, 30 bombs, HexRadius 20px
    ///         - Large board
    ///         - High bomb density (about 13% of tiles are bombs)
    ///         - Small hexagons (harder to click accurately)
    ///         - Challenges experienced players
    /// 
    /// HOW IT'S USED:
    /// 
    /// STEP 1: User clicks "Medium" button in UI
    /// STEP 2: MainWindow calls StartGame(GameDifficulty.Medium)
    /// STEP 3: Inside StartGame(), it calls:
    ///         settings = GameDifficulties.GetSettings(GameDifficulty.Medium)
    /// STEP 4: GetSettings() switches on the enum value and returns the Medium settings
    /// STEP 5: Those settings are passed to new GridManager(settings, ...)
    /// STEP 6: GridManager uses the settings to initialise the board
    /// 
    /// EXCEPTION HANDLING:
    /// 
    /// The switch statement includes a default case that throws an exception:
    ///     default:
    ///         throw new ArgumentOutOfRangeException("invalid difficulty");
    /// 
    /// This is good defensive programming:
    /// - If somehow an invalid enum value is passed, the error is caught immediately
    /// - Error message clearly indicates what went wrong
    /// - Prevents silent failures where invalid settings would be used
    /// 
    /// Why throw here instead of returning null?
    /// - Null would need null-checking throughout the code
    /// - Throwing forces the error to be handled or crash, preventing silent bugs
    /// - Explicit failure is better than implicit failure
    /// 
    /// IMMUTABILITY:
    /// 
    /// The DifficultySettings objects are stored as public readonly fields:
    ///     public static readonly DifficultySettings Easy = ...
    /// 
    /// "readonly" means:
    /// - The field cannot be reassigned (you can't do GameDifficulties.Easy = newSettings)
    /// - The object it points to can still be modified (if DifficultySettings was mutable)
    /// - In practice, DifficultySettings is effectively immutable (properties have no setters)
    /// 
    /// This prevents accidents like:
    ///     GameDifficulties.Hard = GameDifficulties.Easy;  // COMPILE ERROR - readonly
    /// 
    /// RELATIONSHIP TO OTHER CLASSES:
    /// 
    /// - Called by: MainWindow.StartGame(GameDifficulty difficulty)
    /// - Returns: DifficultySettings object
    /// - Passed to: new GridManager(DifficultySettings, ...)
    /// - Stored in: MainWindow.settings, GridManager.currentDifficulty
    /// 
    /// BEST PRACTICES DEMONSTRATED:
    /// 
    /// 1. SINGLE RESPONSIBILITY: This class only manages difficulty settings.
    /// 2. DRY (Don't Repeat Yourself): Each difficulty defined once, reused everywhere.
    /// 3. CENTRALIZATION: All difficulty data in one place makes changes easy.
    /// 4. IMMUTABILITY: Settings can't be accidentally modified at runtime.
    /// 5. TYPE SAFETY: GetSettings() requires a GameDifficulty enum, not a string or int.
    /// 6. DEFENSIVE: Default case in switch throws exception for invalid inputs.
    /// 
    /// EXTENSIBILITY:
    /// 
    /// To add a new difficulty level "Nightmare":
    /// 
    /// 1. Add to GameDifficulty enum:
    ///    public enum GameDifficulty { Easy, Medium, Hard, Nightmare }
    /// 
    /// 2. Add to GameDifficulties class:
    ///    public static readonly DifficultySettings Nightmare = 
    ///        new DifficultySettings("Nightmare", 20, 20, 50, 15);
    /// 
    /// 3. Add case to GetSettings():
    ///    case GameDifficulty.Nightmare:
    ///        return Nightmare;
    /// 
    /// 4. Add UI button in MainWindow.xaml and event handler in MainWindow.xaml.cs
    /// 
    /// 5. Update README with new difficulty info
    /// 
    /// That's it! The rest of the code works automatically.
    /// 
    /// ================================================================
    /// </summary>

    //class to store the game game difficulty settings
    public static class GameDifficulties
    {
        //hard coded difficulty settings
        public static readonly DifficultySettings Easy = new DifficultySettings("Easy", 5, 5, 3, 50);
        public static readonly DifficultySettings Medium = new DifficultySettings("Medium", 10, 10, 15, 30);
        public static readonly DifficultySettings Hard = new DifficultySettings("Hard", 15, 15, 30, 20);

        //return the settings for the difficulty
        public static DifficultySettings GetSettings(GameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case GameDifficulty.Easy:
                    return Easy;
                case GameDifficulty.Medium:
                    return Medium;
                case GameDifficulty.Hard:
                    return Hard;
                default:
                    //something went wrong
                    throw new ArgumentOutOfRangeException("invalid difficulty");
            }
        }
    }
}
