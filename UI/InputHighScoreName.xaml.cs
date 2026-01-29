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
    /// ==================== InputHighScoreName.xaml.cs ====================
    /// 
    /// HIGH SCORE PLAYER NAME INPUT DIALOG
    /// 
    /// PURPOSE:
    /// InputHighScoreName is a WPF dialog window that appears when a player successfully
    /// completes a game. It prompts the player to enter their name so it can be saved
    /// with the high score. It displays the difficulty and time taken as context.
    /// 
    /// WHEN DOES THIS APPEAR?
    /// 
    /// In GridManager.CheckWin(), when game completion is detected:
    /// 
    ///     if (!bestTime.Any() || SecondsElapsed < bestTime.Min(h => h.TimeTaken))
    ///     {
    ///         // Player's time beats existing records
    ///         string playerName = PromptForName();
    ///         // ... save the high score
    ///     }
    /// 
    /// PromptForName() creates and shows this dialog.
    /// 
    /// DIALOG RESULT PATTERN:
    /// 
    /// This dialog uses the DialogResult pattern for communication between the dialog
    /// and the caller:
    /// 
    /// OPENING:
    ///     InputHighScoreName inputDialog = new InputHighScoreName(SecondsElapsed, difficulty);
    ///     if (inputDialog.ShowDialog() == true)
    ///     {
    ///         string playerName = inputDialog.txtName.Text;
    ///         // Use the name...
    ///     }
    ///     else
    ///     {
    ///         // User cancelled, don't save
    ///     }
    /// 
    /// ShowDialog() returns:
    /// - true: User clicked OK (wants to save the score)
    /// - false: User clicked Cancel (doesn't want to save)
    /// - null: User closed the window (treat as Cancel)
    /// 
    /// WHY USE DialogResult?
    /// 
    /// DialogResult provides a clean way to:
    /// - Return a status (OK/Cancel) from the dialog
    /// - Wait for dialog to close before continuing main program
    /// - Know whether the user confirmed or cancelled
    /// - Get data from the dialog (txtName.Text)
    /// 
    /// THE CONSTRUCTOR:
    /// 
    ///     public InputHighScoreName(int seconds, string difficulty)
    ///     {
    ///         InitialiseComponent();  // Build UI from XAML
    ///         txtElapsed.Text = $"Difficulty: {difficulty}\\nTime: {seconds}";
    ///     }
    /// 
    /// Parameters:
    /// - seconds: The elapsed time (how fast they won)
    /// - difficulty: The difficulty level (Easy/Medium/Hard)
    /// 
    /// The constructor displays this information in a read-only text box so the
    /// player sees their achievement context before entering their name.
    /// 
    /// WHY SHOW TIME AND DIFFICULTY?
    /// 
    /// Context and Motivation:
    /// - Shows the player what achievement they're recording
    /// - Motivates them to enter their name
    /// - Provides verification (player can see if time is correct)
    /// 
    /// User Experience:
    /// - Player completes game
    /// - Sees "You Win! 45 seconds"
    /// - Dialog opens with "Difficulty: Easy" and "Time: 45"
    /// - Confirms they want their name associated with this score
    /// 
    /// DISPLAY FORMAT:
    /// 
    ///     txtElapsed.Text = $"Difficulty: {difficulty}\\nTime: {seconds}";
    /// 
    /// Result on screen:
    ///     Difficulty: Medium
    ///     Time: 87
    /// 
    /// The \\n is a newline character (string escape sequence):
    /// - \n: newline in strings
    /// - \\n: literal backslash-n in comments/documentation
    /// 
    /// USER INTERACTION:
    /// 
    /// After seeing the dialog, player can:
    /// 
    /// 1. ENTER NAME AND CLICK OK:
    ///    - Player types "AliceWins" in txtName TextBox
    ///    - Clicks OK button
    ///    - DialogResult = true
    ///    - Dialog closes
    ///    - Score is saved with name "AliceWins"
    /// 
    /// 2. CLICK CANCEL:
    ///    - Player changes mind about saving
    ///    - Clicks Cancel button
    ///    - DialogResult = false
    ///    - Dialog closes
    ///    - Score is NOT saved
    ///    - Game ends
    /// 
    /// 3. CLOSE WINDOW:
    ///    - Player clicks X button to close window
    ///    - DialogResult remains unset (null)
    ///    - Treated same as Cancel (score not saved)
    /// 
    /// THE BUTTON HANDLERS:
    /// 
    /// OK BUTTON:
    ///     private void btnOk_Click(object sender, RoutedEventArgs e)
    ///     {
    ///         DialogResult = true;  // Signal success
    ///     }
    /// 
    /// Purpose: Set DialogResult to true, window automatically closes
    /// Does NOT need to call this.Close() explicitly - WPF handles it
    /// 
    /// CANCEL BUTTON:
    ///     private void btnCancel_Click(object sender, RoutedEventArgs e)
    ///     {
    ///         DialogResult = false;  // Signal cancellation
    ///     }
    /// 
    /// Purpose: Set DialogResult to false, window automatically closes
    /// 
    /// INPUT VALIDATION:
    /// 
    /// Current Implementation:
    /// - No validation in this dialog
    /// - Empty names are allowed
    /// - Very long names are allowed
    /// - Any characters are allowed
    /// 
    /// Possible Enhancements:
    ///     private void btnOk_Click(object sender, RoutedEventArgs e)
    ///     {
    ///         if (string.IsNullOrWhiteSpace(txtName.Text))
    ///         {
    ///             MessageBox.Show("Please enter a name");
    ///             return;  // Don't close, let user try again
    ///         }
    ///         if (txtName.Text.Length > 20)
    ///         {
    ///             MessageBox.Show("Name must be 20 characters or less");
    ///             return;
    ///         }
    ///         DialogResult = true;
    ///     }
    /// 
    /// TEXT BOX PROPERTIES:
    /// 
    /// The dialog has two text elements:
    /// 
    /// 1. txtElapsed (read-only):
    ///    - Displays "Difficulty: X" and "Time: Y"
    ///    - Not editable (IsReadOnly = true or similar)
    ///    - Shows the context of the achievement
    /// 
    /// 2. txtName (editable):
    ///    - Where player enters their name
    ///    - Editable TextBox
    ///    - Accessed by caller as: inputDialog.txtName.Text
    /// 
    /// WORKFLOW INTEGRATION:
    /// 
    /// Here's how this fits in the full win sequence:
    /// 
    /// 1. Player clicks on safe tile (not a bomb)
    /// 2. GridManager.FloodFillReveal() reveals tiles
    /// 3. GridManager.CheckWin() detects all non-bomb tiles revealed
    /// 4. If time is better than existing scores:
    ///    GridManager.PromptForName() creates this dialog
    /// 5. Player enters name and clicks OK
    /// 6. Dialog returns name to GridManager
    /// 7. New HighScore object created with name, time, date, difficulty
    /// 8. HighScore saved to JSON file
    /// 9. Dialog closes
    /// 10. MainWindow shows win screen
    /// 
    /// WPF CONCEPTS:
    /// 
    /// DIALOG RESULT:
    /// - Special property on Window for modal dialog communication
    /// - Setting DialogResult automatically closes the window
    /// - ShowDialog() returns the DialogResult value
    /// - Enables clean separation of UI from logic
    /// 
    /// TEXT BOX CONTROL:
    /// - TextBox: WPF control for single or multi-line text input
    /// - Text property: Gets/sets the content
    /// - IsReadOnly: Makes text box display-only
    /// - placeholder text: Hint text (can be added with behaviour)
    /// 
    /// PARTIAL CLASS & CODE-BEHIND:
    /// - Again, split between XAML (layout) and C# (logic)
    /// - XAML: <TextBox Name="txtName" />
    /// - C#: txtName.Text accesses the TextBox
    /// 
    /// EVENT HANDLERS:
    /// - Two button click handlers
    /// - Both set DialogResult (which closes window)
    /// - Could add validation logic before setting DialogResult
    /// 
    /// PARAMETER PASSING:
    /// - Constructor receives seconds and difficulty
    /// - Stores them by displaying in txtElapsed
    /// - Caller retrieves name via txtName.Text property
    /// - This is a simple alternative to using custom properties
    /// 
    /// ALTERNATIVE APPROACHES:
    /// 
    /// Instead of accessing txtName.Text from outside:
    /// 
    ///     // Current way:
    ///     string name = inputDialog.txtName.Text;
    ///     
    ///     // Better way - add public property:
    ///     public string PlayerName => txtName.Text;
    ///     
    ///     // Then use:
    ///     string name = inputDialog.PlayerName;
    /// 
    /// The property approach is cleaner encapsulation.
    /// 
    /// DESIGN NOTES:
    /// 
    /// This is a simple, focused dialog:
    /// - Single purpose: get player name
    /// - Single responsibility
    /// - No game logic (doesn't validate difficulty, time, etc.)
    /// - No file I/O (just returns the name)
    /// - Testable: logic is minimal
    /// 
    /// EXEMPLAR NOTE:
    /// 
    /// This class demonstrates:
    /// - WPF secondary window (dialog)
    /// - DialogResult pattern for user confirmation
    /// - Event handlers for buttons
    /// - String formatting with interpolation
    /// - Simple data input UI
    /// - Separation of concerns (UI for input, logic elsewhere)
    /// 
    /// ================================================================
    /// </summary>

    public partial class InputHighScoreName : Window
    {
        /// <summary>
        /// Constructor for the name input dialog.
        /// Displays the game time and difficulty level that was just completed.
        /// </summary>
        /// <param name="seconds">The number of seconds taken to win</param>
        /// <param name="difficulty">The difficulty level (Easy, Medium, or Hard)</param>
        public InputHighScoreName(int seconds, string difficulty)
        {
            InitializeComponent();
            // Display the game results for context
            // The player sees their time and difficulty before entering their name
            txtElapsed.Text = $"Difficulty: {difficulty}\nTime: {seconds}";
        }

        /// <summary>
        /// Event handler for the OK button.
        /// Confirms the player wants to save their high score with the entered name.
        /// Sets DialogResult to true, which closes the window.
        /// </summary>
        /// <param name="sender">The OK button that was clicked</param>
        /// <param name="e">Event data</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            // Save the name and close the dialog.
            // Setting DialogResult to true signals the caller that the player confirmed
            DialogResult = true;
        }

        /// <summary>
        /// Event handler for the Cancel button.
        /// Indicates the player does not want to save their score.
        /// Sets DialogResult to false, which closes the window.
        /// </summary>
        /// <param name="sender">The Cancel button that was clicked</param>
        /// <param name="e">Event data</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog without saving the name.
            // Setting DialogResult to false signals the caller that the player cancelled
            DialogResult = false;
        }
    }
}
