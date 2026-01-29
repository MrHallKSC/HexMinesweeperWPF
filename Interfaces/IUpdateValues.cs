using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== IUpdateValues.cs ====================
    /// 
    /// INTERFACE FOR LOOSE COUPLING BETWEEN GAME LOGIC AND UI
    /// 
    /// PURPOSE:
    /// IUpdateValues is a C# interface that defines a contract for updating the UI
    /// from within the game logic layer. It enables GridManager (game logic) to
    /// communicate with MainWindow (UI) without having a direct dependency on it.
    /// 
    /// WHAT IS AN INTERFACE?
    /// 
    /// An interface is like a contract or specification that says:
    /// "Any class implementing this interface MUST provide these methods."
    /// 
    /// Interfaces define method signatures but not implementations:
    ///     interface IUpdateValues
    ///     {
    ///         void UpdateBombCount(int change);  // MUST implement
    ///         void StopTimer();                   // MUST implement
    ///     }
    /// 
    /// Any class that implements IUpdateValues must provide code for both methods.
    /// In this project, MainWindow implements IUpdateValues.
    /// 
    /// WHY USE AN INTERFACE?
    /// 
    /// PROBLEM WITHOUT INTERFACE:
    /// If GridManager directly referenced MainWindow:
    /// 
    ///     public class GridManager
    ///     {
    ///         private MainWindow window;  // Tight coupling!
    ///         
    ///         public void SomeGameLogic()
    ///         {
    ///             window.UpdateBombCount(-1);  // Direct dependency on MainWindow
    ///         }
    ///     }
    /// 
    /// Problems:
    /// - GridManager directly depends on MainWindow existing
    /// - Can't test GridManager without MainWindow
    /// - Can't reuse GridManager with a different UI (console, web, etc.)
    /// - Changes to MainWindow might break GridManager
    /// 
    /// SOLUTION WITH INTERFACE:
    /// 
    ///     public class GridManager
    ///     {
    ///         private IUpdateValues updateValues;  // Loose coupling!
    ///         
    ///         public void SomeGameLogic()
    ///         {
    ///             updateValues.UpdateBombCount(-1);  // Depends only on interface
    ///         }
    ///     }
    /// 
    /// Benefits:
    /// - GridManager depends only on the interface, not a specific implementation
    /// - Any class implementing IUpdateValues can be used
    /// - GridManager can be tested with a mock/fake implementation
    /// - Multiple UIs could be created without changing GridManager
    /// 
    /// THE INTERFACE METHODS:
    /// 
    /// 1. UpdateBombCount(int change)
    /// 
    ///    Purpose: Notifies the UI that the remaining bomb count has changed.
    ///    
    ///    Why needed: When a player flags/unflags a tile, GridManager needs to tell
    ///    the UI to update the "Bombs Left" label. This is a one-way communication
    ///    from logic to presentation.
    ///    
    ///    Parameter: int change
    ///    - Positive number: add bombs back to remaining count (unflagging)
    ///    - Negative number: subtract from remaining count (flagging)
    ///    - Example: UpdateBombCount(-1) means "I flagged one more bomb"
    ///    
    ///    Caller: GridManager.TriggerBombUpdate(int change)
    ///    
    ///    Implementer (MainWindow):
    ///        public void UpdateBombCount(int change)
    ///        {
    ///            gridManager.RemainingBombs += change;
    ///            lblBombsRemaining.Content = $"Bombs Left: {gridManager.RemainingBombs}";
    ///        }
    /// 
    /// 2. StopTimer()
    /// 
    ///    Purpose: Notifies the UI that the game timer should stop.
    ///    
    ///    Why needed: When the game ends (win or loss), GridManager detects this
    ///    in CheckWin() and needs to tell MainWindow to stop the timer.
    ///    
    ///    Parameters: None
    ///    
    ///    Caller: GridManager.CheckWin() - at the end of a successful game
    ///    
    ///    Implementer (MainWindow):
    ///        public void StopTimer()
    ///        {
    ///            timer.Stop();
    ///            TimerRunning = false;
    ///        }
    /// 
    /// DESIGN PATTERN: OBSERVER PATTERN
    /// 
    /// This is a simplified version of the Observer pattern:
    /// - GridManager is the subject that generates events
    /// - MainWindow is the observer that acts on those events
    /// - The interface provides the contract between them
    /// 
    /// Traditional Observer pattern uses events/delegates, but this interface
    /// approach is simpler and sufficient for this project.
    /// 
    /// DEPENDENCY INJECTION
    /// 
    /// How is the implementation passed to GridManager?
    /// 
    ///     public GridManager(DifficultySettings diff, IUpdateValues updateValues)
    ///     {
    ///         this.updateValues = updateValues;
    ///         // ...
    ///     }
    /// 
    /// MainWindow creates the GridManager and passes itself:
    ///     gridManager = new GridManager(difficulty, this);
    ///                                                  ^^^^
    ///                                    'this' implements IUpdateValues
    /// 
    /// This is called "dependency injection" - the GridManager's dependencies
    /// (things it needs to work) are "injected" via the constructor.
    /// 
    /// Why is this good?
    /// - The dependency is explicit (you see it in the constructor)
    /// - Easy to swap with a different implementation for testing
    /// - The class doesn't create its own dependencies (decoupled)
    /// 
    /// TESTING EXAMPLE:
    /// 
    /// You could create a mock implementation for testing:
    /// 
    ///     public class MockUI : IUpdateValues
    ///     {
    ///         public int BombUpdates { get; private set; }
    ///         public int TimerStops { get; private set; }
    ///         
    ///         public void UpdateBombCount(int change)
    ///         {
    ///             BombUpdates++;
    ///         }
    ///         
    ///         public void StopTimer()
    ///         {
    ///             TimerStops++;
    ///         }
    ///     }
    ///     
    ///     // Then test:
    ///     var mockUI = new MockUI();
    ///     var grid = new GridManager(GameDifficulties.Easy, mockUI);
    ///     grid.AllocateBombs();
    ///     grid.CalculateBombNeighbours();
    ///     
    ///     // Assertions about game logic work correctly
    ///     Assert.AreEqual(GameDifficulties.Easy.Bombs, 3);  // Bombs placed
    /// 
    /// ALTERNATIVE APPROACHES:
    /// 
    /// If no UI updates were needed, GridManager could work standalone without
    /// any interface. But since it needs to update the UI, the interface solves
    /// the communication problem elegantly.
    /// 
    /// RELATIONSHIP TO OTHER CLASSES:
    /// 
    /// - Implemented by: MainWindow
    /// - Used by: GridManager (receives via constructor)
    /// - Contains method signatures for: UpdateBombCount(int), StopTimer()
    /// 
    /// BEST PRACTICES DEMONSTRATED:
    /// 
    /// 1. SEPARATION OF CONCERNS: Logic and UI are separated via the interface
    /// 2. DEPENDENCY INVERSION: GridManager depends on abstraction (interface),
    ///    not concrete implementation (MainWindow)
    /// 3. TESTABILITY: Logic can be tested independently with mock implementations
    /// 4. FLEXIBILITY: Different UI implementations possible without changing logic
    /// 
    /// ================================================================
    /// </summary>

    internal interface IUpdateValues
    {
        /// <summary>
        /// Updates the UI to reflect a change in the number of flagged bombs.
        /// Called by GridManager when a tile is flagged or unflagged.
        /// </summary>
        /// <param name="change">Positive to add bombs back, negative to subtract</param>
        void UpdateBombCount(int change);

        /// <summary>
        /// Stops the game timer. Called by GridManager when the game ends.
        /// </summary>
        void StopTimer();
    }
}
