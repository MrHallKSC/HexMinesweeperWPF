using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== HighScoreWindow.xaml.cs ====================
    /// 
    /// HIGH SCORE DISPLAY DIALOG
    /// 
    /// PURPOSE:
    /// HighScoreWindow is a secondary WPF window (dialog) that displays the high scores
    /// for all three difficulty levels. It shows the top 3 fastest times for each difficulty
    /// in separate DataGrid controls.
    /// 
    /// WHAT IS A DIALOG WINDOW?
    /// 
    /// A dialog is a secondary window that:
    /// - Appears on top of the main window
    /// - Often requires user action (clicking a button) to close
    /// - Can prevent interaction with the main window while open (modal)
    /// - Is different from the main application window
    /// 
    /// In WPF, any Window can be shown as a dialog using window.ShowDialog()
    /// (unlike regular windows shown with window.Show())
    /// 
    /// HOW IT WORKS:
    /// 
    /// OPENING THE DIALOG:
    /// From MainWindow, when user clicks "High Scores" button:
    /// 
    ///     HighScoreWindow highScoresWindow = new HighScoreWindow(gridManager.LoadAllHighScores());
    ///     highScoresWindow.ShowDialog();
    /// 
    /// The constructor receives a List<HighScore> containing all high scores from all
    /// difficulties, loaded from the JSON files.
    /// 
    /// INITIALIZATION - CONSTRUCTOR:
    /// 
    ///     public HighScoreWindow(List<HighScore> HighScores)
    ///     {
    ///         InitialiseComponent();  // Build the WPF UI from XAML
    ///         
    ///         // Populate Easy scores
    ///         dgEasy.ItemsSource = HighScores
    ///             .Where(h => h.Difficulty == "Easy")     // Filter for Easy only
    ///             .OrderBy(h => h.TimeTaken)              // Sort by time ascending (fastest first)
    ///             .Take(3);                                // Show top 3 only
    ///         
    ///         // Similar for Medium and Hard...
    ///     }
    /// 
    /// The constructor does all the data processing:
    /// - Filters high scores by difficulty
    /// - Sorts by time taken
    /// - Takes only top 3
    /// - Assigns to DataGrid ItemsSource (data binding)
    /// 
    /// WHY FILTER, SORT, AND LIMIT?
    /// 
    /// FILTERING (.Where()):
    /// - The combined HighScores list contains Easy, Medium, and Hard scores
    /// - Each DataGrid should show only scores from its difficulty
    /// - .Where(h => h.Difficulty == "Easy") keeps only Easy scores
    /// 
    /// SORTING (.OrderBy()):
    /// - In Minesweeper, faster times are better
    /// - Sort by TimeTaken in ascending order (smallest first)
    /// - Results in fastest time appearing first
    /// 
    /// LIMITING (.Take(3)):
    /// - High scores typically show top 3, top 10, top 100, etc.
    /// - This project shows top 3 per difficulty
    /// - .Take(3) returns only the first 3 items from the sorted list
    /// 
    /// LINQ QUERIES:
    /// 
    /// The code uses LINQ (Language Integrated Query) - a C# feature for querying collections:
    /// 
    ///     HighScores.Where(h => h.Difficulty == "Easy")
    ///     
    /// Breakdown:
    /// - HighScores: the source list
    /// - .Where(): filters items matching a condition
    /// - h => h.Difficulty == "Easy": lambda expression
    ///   * h: represents each item being evaluated
    ///   * =&gt;: arrow means "such that"
    ///   * h.Difficulty == "Easy": the condition each item must satisfy
    /// 
    /// Combined query:
    ///     HighScores
    ///         .Where(h => h.Difficulty == "Easy")
    ///         .OrderBy(h => h.TimeTaken)
    ///         .Take(3)
    /// 
    /// This is equivalent to SQL:
    ///     SELECT TOP 3 * FROM HighScores 
    ///     WHERE Difficulty = 'Easy' 
    ///     ORDER BY TimeTaken ASC
    /// 
    /// DATA BINDING:
    /// 
    /// What is ItemsSource?
    /// - ItemsSource is a WPF DataGrid property that specifies what data to display
    /// - It expects a collection (IEnumerable<T>)
    /// - When you assign to ItemsSource, the DataGrid automatically creates rows
    /// 
    /// Example flow:
    /// 1. Query returns 3 HighScore objects
    /// 2. dgEasy.ItemsSource = [HighScore#1, HighScore#2, HighScore#3]
    /// 3. DataGrid displays 3 rows
    /// 4. Columns are auto-generated from HighScore properties
    /// 5. Headers: PlayerName, TimeTaken, DateAchieved, Difficulty
    /// 6. Each row contains data from one HighScore object
    /// 
    /// CLOSING THE WINDOW:
    /// 
    /// When user clicks the "Close" button:
    /// 
    ///     private void btnClose_Click(object sender, RoutedEventArgs e)
    ///     {
    ///         this.Close();
    ///     }
    /// 
    /// - this.Close() closes the window
    /// - Control returns to MainWindow
    /// - Allows main game to continue
    /// 
    /// WINDOW MODALITY:
    /// 
    /// The window is shown as a modal dialog:
    /// 
    ///     highScoresWindow.ShowDialog();
    ///     
    /// vs.
    ///     
    ///     highScoresWindow.Show();
    /// 
    /// ShowDialog(): Modal - user MUST close this window before using MainWindow
    /// Show(): Modeless - user can switch between windows
    /// 
    /// Modal is appropriate here because:
    /// - High score window is temporary (just view info)
    /// - User should return to main game when done
    /// - Prevents confusion of multiple windows
    /// 
    /// EDGE CASES:
    /// 
    /// What if no high scores exist?
    /// - Query returns empty list
    /// - DataGrid displays empty (no rows)
    /// - Headers still visible
    /// - Current code handles this gracefully
    /// 
    /// What if only 1-2 high scores exist?
    /// - .Take(3) returns all available (might be less than 3)
    /// - DataGrid shows 1-2 rows instead of 3
    /// - Works correctly with no additional code needed
    /// 
    /// WPF CONCEPTS:
    /// 
    /// PARTIAL CLASSES (again):
    /// - HighScoreWindow is partial, split between XAML and code-behind
    /// - XAML (HighScoreWindow.xaml) defines the visual layout
    /// - Code-behind (this file) defines the behaviour
    /// 
    /// EVENT HANDLERS:
    /// - btnClose_Click is triggered when user clicks the Close button
    /// - Signature: void MethodName(object sender, RoutedEventArgs e)
    /// - sender: the button that was clicked
    /// - e: event data (not used in this handler)
    /// 
    /// DataGrid Control:
    /// - WPF control for displaying tabular data
    /// - Auto-generates columns from object properties
    /// - Handles scrolling, sizing, selection automatically
    /// - Powerful for displaying lists of data
    /// 
    /// DESIGN DECISIONS:
    /// 
    /// 1. Why three separate DataGrids instead of one?
    /// - Clearer presentation (Easy / Medium / Hard visually separated)
    /// - Easier to label each section
    /// - Could be combined into one DataGrid with grouping if desired
    /// 
    /// 2. Why top 3 scores?
    /// - Traditional for arcade games
    /// - Small enough to fit on screen
    /// - Recognizable format
    /// - Could be configurable (top 10, top 20) if desired
    /// 
    /// 3. Why sort by time ascending?
    /// - Faster times are better in Minesweeper
    /// - User sees best time first
    /// - Intuitive ordering
    /// 
    /// 4. Why filter by difficulty?
    /// - Fair comparison (Easy vs Hard are different challenges)
    /// - User sees their Easy scores separately from Hard
    /// - Prevents confusion
    /// 
    /// EXEMPLAR NOTE:
    /// 
    /// This class demonstrates:
    /// - Secondary WPF window creation
    /// - Modal dialog usage
    /// - LINQ query syntax
    /// - Data binding to UI controls
    /// - Event handling
    /// - Simple, focused responsibility (display data only)
    /// 
    /// ================================================================
    /// </summary>

    public partial class HighScoreWindow : Window
    {
        /// <summary>
        /// Constructor for the HighScoreWindow.
        /// Receives all high scores and populates three DataGrids (Easy, Medium, Hard)
        /// with the top 3 scores for each difficulty, sorted by fastest time.
        /// </summary>
        /// <param name="HighScores">List of all HighScore objects from all difficulties</param>
        public HighScoreWindow(List<HighScore> HighScores)
        {
            InitializeComponent();

            // Filter, sort, and display Easy scores
            // .Where() filters by difficulty, .OrderBy() sorts by time, .Take(3) limits to top 3
            dgEasy.ItemsSource = HighScores
                .Where(h => h.Difficulty == "Easy")
                .OrderBy(h => h.TimeTaken)
                .Take(3);

            // Same process for Medium difficulty
            dgMedium.ItemsSource = HighScores
                .Where(h => h.Difficulty == "Medium")
                .OrderBy(h => h.TimeTaken)
                .Take(3);

            // Same process for Hard difficulty
            dgHard.ItemsSource = HighScores
                .Where(h => h.Difficulty == "Hard")
                .OrderBy(h => h.TimeTaken)
                .Take(3);
        }

        /// <summary>
        /// Event handler for the Close button.
        /// Closes this window and returns control to the MainWindow.
        /// </summary>
        /// <param name="sender">The button that was clicked</param>
        /// <param name="e">Event data</param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
