using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== GameDifficulty.cs ====================
    /// 
    /// DIFFICULTY LEVEL ENUMERATION
    /// 
    /// PURPOSE:
    /// GameDifficulty is an enumeration (enum) that defines the three difficulty levels
    /// available in HexMinesweeper: Easy, Medium, and Hard.
    /// 
    /// WHY USE AN ENUM?
    /// 
    /// Without enums, difficulty might be represented as strings ("Easy", "Medium", "Hard")
    /// or integers (0, 1, 2). This creates problems:
    /// - Strings are error-prone: typos go undetected until runtime
    /// - Magic numbers (0, 1, 2) are unclear about what they mean
    /// - No compile-time checking
    /// 
    /// Enums solve these problems by:
    /// - Restricting values to a predefined set (Easy, Medium, Hard only)
    /// - Providing type safety at compile time
    /// - Making code self-documenting
    /// - Enabling the switch statement for handling each difficulty
    /// 
    /// HOW ENUMS WORK:
    /// 
    /// An enum is a special type that defines a fixed set of named constant values.
    /// Under the hood, each name maps to an integer:
    /// - Easy = 0
    /// - Medium = 1
    /// - Hard = 2
    /// 
    /// The compiler ensures you can only use these three values. If you try to use
    /// "Insane" or "Medium-Hard", you'll get a compile error.
    /// 
    /// USAGE IN THIS PROJECT:
    /// 
    /// 1. DECLARATION IN METHODS:
    ///    When the user clicks the "Hard" button, MainWindow calls:
    ///    StartGame(GameDifficulty.Hard)
    /// 
    /// 2. SWITCH STATEMENTS:
    ///    GameDifficulties.GetSettings() uses a switch statement:
    ///    switch(difficulty)
    ///    {
    ///        case GameDifficulty.Easy: return Easy;
    ///        case GameDifficulty.Medium: return Medium;
    ///        case GameDifficulty.Hard: return Hard;
    ///    }
    ///    
    ///    The compiler ensures all cases are handled (or a default is provided).
    /// 
    /// 3. TYPE SAFETY:
    ///    You cannot accidentally pass invalid values:
    ///    StartGame(GameDifficulty.VeryEasy)  // COMPILE ERROR - doesn't exist
    ///    StartGame("Hard")                   // COMPILE ERROR - wrong type
    ///    StartGame(GameDifficulty.Hard)      // CORRECT
    /// 
    /// VALUE TYPES:
    /// - Enums are VALUE TYPES (like int, bool), not reference types (like class)
    /// - Copying an enum variable copies the value, not a reference
    /// - Enums can be compared with == (checks value equality)
    /// 
    /// RELATIONSHIP TO OTHER CLASSES:
    /// - Passed to: MainWindow.StartGame(GameDifficulty difficulty)
    /// - Converted by: GameDifficulties.GetSettings(GameDifficulty difficulty)
    ///   returns DifficultySettings object with actual configuration
    /// 
    /// BEST PRACTICES:
    /// - Enums should have singular or plural names depending on context
    ///   (Difficulty is singular for the enum, GameDifficulties is plural for the static class)
    /// - Enum values should be PascalCase (Easy, Medium, Hard)
    /// - Consider grouping related enums together
    /// 
    /// FUTURE EXTENSIBILITY:
    /// If you wanted to add a "Nightmare" difficulty level, you'd simply add it here:
    ///     public enum GameDifficulty
    ///     {
    ///         Easy,
    ///         Medium,
    ///         Hard,
    ///         Nightmare  // New level
    ///     }
    /// 
    /// Then add corresponding case in GameDifficulties.GetSettings() switch statement.
    /// 
    /// ================================================================
    /// </summary>

    //enum for choice of game difficulty
    public enum GameDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}
