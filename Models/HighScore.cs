using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== HighScore.cs ====================
    /// 
    /// HIGH SCORE DATA MODEL
    /// 
    /// PURPOSE:
    /// HighScore is a data model that represents a single high score entry. It stores
    /// information about one player's successful completion of the game, including
    /// their name, the time taken, when they achieved it, and what difficulty level.
    /// 
    /// WHAT IS A DATA MODEL?
    /// 
    /// A data model (or entity) is a class that represents real-world or domain objects.
    /// In this case, each HighScore object represents one completed game by one player.
    /// 
    /// Why separate from GameLogic?
    /// - Keeps game data distinct from game rules
    /// - Can be serialized/deserialized to JSON for persistence
    /// - Can be displayed in UI without exposing game logic
    /// - Follows Single Responsibility Principle
    /// 
    /// THE FOUR PROPERTIES:
    /// 
    /// 1. PlayerName (string)
    /// 
    ///    What: The name of the player who achieved this score
    ///    Format: Entered by the player via InputHighScoreName dialogue
    ///    Example: "Alice", "Bob123", "Champion"
    ///    Max Length: Not validated in current code, but could be restricted
    ///    
    ///    Use: Displayed in HighScoreWindow to show who achieved each score
    ///    
    ///    XML Serialization: Marked with [JsonPropertyName("playerName")] if using
    ///    System.Text.Json serialization attributes
    /// 
    /// 2. TimeTaken (int)
    /// 
    ///    What: The number of seconds taken to complete the game
    ///    Format: Stored as integer number of seconds
    ///    Range: 0 to 999999 (practically, 1 to 3600 for reasonable times)
    ///    Example: 42 (meaning 42 seconds), 125 (2 minutes 5 seconds)
    ///    
    ///    How Calculated: Set in GridManager.CheckWin() from GridManager.SecondsElapsed
    ///    
    ///    Use: 
    ///    - Ranking (lower is better in Minesweeper)
    ///    - Sorting high scores: .OrderBy(h => h.TimeTaken)
    ///    - Displayed in HighScoreWindow
    ///    
    ///    Sorting: High scores are displayed by fastest time (ascending order)
    /// 
    /// 3. DateAchieved (DateTime)
    /// 
    ///    What: The date and time when this score was achieved
    ///    Format: .NET DateTime object (can store year, month, day, hour, minute, second)
    ///    Set: DateTime.Now at the moment the game was won
    ///    Example: "2024-01-15 14:32:45"
    ///    
    ///    Use:
    ///    - Historical record (when was this score achieved?)
    ///    - Could be used for leaderboards showing recent scores
    ///    - Auditing (verify scores are legitimate)
    ///    
    ///    JSON Serialization: DateTime is automatically serialized as ISO 8601 format
    ///    Example JSON: "dateAchieved":"2024-01-15T14:32:45.1234567"
    /// 
    /// 4. Difficulty (string)
    /// 
    ///    What: The difficulty level at which this score was achieved
    ///    Values: "Easy", "Medium", or "Hard" (matches GameDifficulty names)
    ///    Set: DifficultySettings.Name
    ///    
    ///    Use:
    ///    - Grouping high scores by difficulty
    ///    - Comparison (only compare scores from same difficulty)
    ///    - Filtering in HighScoreWindow:
    ///      .Where(h => h.Difficulty == "Easy")
    ///    
    ///    Why String Not Enum?
    ///    - JSON serialization is simpler with strings
    ///    - Flexibility (could add new difficulties without recompilation)
    ///    - Direct match with DifficultySettings.Name
    ///    - Trade-off: Not type-safe like an enum would be
    /// 
    /// HOW HIGH SCORES ARE CREATED:
    /// 
    /// When a player wins a game, GridManager.CheckWin() creates a new HighScore:
    /// 
    ///     HighScores.Add(new HighScore
    ///     {
    ///         PlayerName = playerName,          // From input dialogue
    ///         TimeTaken = SecondsElapsed,       // From timer
    ///         DateAchieved = DateTime.Now,      // Current time
    ///         Difficulty = currentDifficulty.Name  // "Easy", "Medium", or "Hard"
    ///     });
    ///     SaveHighScores(HighScores);           // Persist to JSON
    /// 
    /// HOW HIGH SCORES ARE PERSISTED:
    /// 
    /// SAVING:
    /// - HighScore objects are converted to JSON using System.Text.Json
    /// - Stored in files: Easy.json, Medium.json, Hard.json
    /// - One file per difficulty level
    /// 
    /// Example JSON (Easy.json):
    /// [
    ///   {
    ///     "playerName": "Alice",
    ///     "timeTaken": 45,
    ///     "dateAchieved": "2024-01-15T10:23:45.1234567",
    ///     "difficulty": "Easy"
    ///   },
    ///   {
    ///     "playerName": "Bob",
    ///     "timeTaken": 52,
    ///     "dateAchieved": "2024-01-15T11:30:12.5678901",
    ///     "difficulty": "Easy"
    ///   }
    /// ]
    /// 
    /// LOADING:
    /// - JSON files are read from disk (if they exist)
    /// - Deserialized back into HighScore objects
    /// - Displayed in HighScoreWindow
    /// - Code: JsonSerializer.Deserialize<List<HighScore>>(json)
    /// 
    /// HOW HIGH SCORES ARE DISPLAYED:
    /// 
    /// HighScoreWindow shows top 3 scores for each difficulty:
    /// 
    ///     dgEasy.ItemsSource = HighScores
    ///         .Where(h => h.Difficulty == "Easy")     // Filter by difficulty
    ///         .OrderBy(h => h.TimeTaken)              // Sort by time (fastest first)
    ///         .Take(3);                                // Show only top 3
    /// 
    /// The DataGrid displays the properties as columns:
    /// - PlayerName | TimeTaken | DateAchieved | Difficulty
    /// 
    /// DESIGN CONSIDERATIONS:
    /// 
    /// 1. MUTABILITY:
    ///    HighScore properties have both getters AND setters, making it mutable.
    ///    This is necessary for JSON deserialization.
    ///    
    ///    Alternative (immutable):
    ///    public class HighScore
    ///    {
    ///        public string PlayerName { get; }
    ///        public HighScore(string name, int time, DateTime date, string difficulty)
    ///        {
    ///            PlayerName = name;
    ///            TimeTaken = time;
    ///            DateAchieved = date;
    ///            Difficulty = difficulty;
    ///        }
    ///    }
    ///    
    ///    But this requires custom JSON deserializers, adding complexity.
    /// 
    /// 2. VALIDATION:
    ///    No validation in constructor (blank parameters are allowed)
    ///    Could add validation to prevent invalid scores:
    ///    - TimeTaken >= 0
    ///    - PlayerName not null/empty
    ///    - Difficulty in {"Easy", "Medium", "Hard"}
    /// 
    /// 3. EQUALITY:
    ///    Uses default reference equality (same object in memory)
    ///    Could override Equals() for value-based equality
    /// 
    /// 4. COMPARABILITY:
    ///    No IComparable implementation
    ///    Uses LINQ .OrderBy() instead
    ///    Could implement IComparable<HighScore> for native sorting
    /// 
    /// EXEMPLAR NOTE:
    /// 
    /// This class demonstrates:
    /// - Simple data model design
    /// - Public properties for JSON serialization
    /// - Separation of data from logic
    /// - Use of DateTime for timestamps
    /// - String-based categorization
    /// - Simple class with single responsibility
    /// 
    /// ================================================================
    /// </summary>

    public class HighScore
    {
        /// <summary>
        /// The name of the player who achieved this score
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// The number of seconds taken to win the game
        /// </summary>
        public int TimeTaken { get; set; }

        /// <summary>
        /// The date and time when this high score was achieved
        /// </summary>
        public DateTime DateAchieved { get; set; }

        /// <summary>
        /// The difficulty level ("Easy", "Medium", or "Hard")
        /// </summary>
        public String Difficulty { get; set; }
    }
}
