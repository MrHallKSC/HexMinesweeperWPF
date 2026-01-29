using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== MainWindow.xaml.cs ====================
    /// 
    /// PRIMARY USER INTERFACE AND GAME CONTROLLER
    /// 
    /// PURPOSE:
    /// MainWindow is the primary window and controller of the entire HexMinesweeper game.
    /// It is responsible for:
    /// 1. Rendering the user interface (buttons, canvas for the hex grid, labels for status)
    /// 2. Managing user interactions (mouse clicks on hexagons, button clicks)
    /// 3. Controlling game flow (starting games, resetting, changing difficulty)
    /// 4. Coordinating between the UI and the game logic (GridManager)
    /// 5. Maintaining the game timer
    /// 6. Updating visual feedback (colors, numbers, bomb counts)
    /// 
    /// HOW IT WORKS - OVERALL FLOW:
    /// 1. Constructor: When the window is created, it initializes a default Easy game
    ///    and shows the difficulty overlay so the player can select their challenge level.
    /// 2. Difficulty Selection: Player clicks Easy, Medium, or Hard button, triggering
    ///    StartGame() which calls InitialiseGame() to set up the board.
    /// 3. Board Display: DrawHexGrid() creates all the hexagon polygons, positions them
    ///    on the canvas, and associates each with a HexTile object.
    /// 4. Gameplay: 
    ///    - Left-click (LeftClick handler): Reveals a tile. If it's a bomb, game over.
    ///      If it's safe, reveal the tile and recursively reveal neighbours if appropriate.
    ///    - Right-click (RightClick handler): Flags/unflags a tile as a suspected bomb.
    /// 5. Win Condition: When all non-bomb tiles are revealed, CheckWin() returns true,
    ///    the timer stops, and a dialog prompts for the player's name to save the score.
    /// 
    /// WPF CONCEPTS FOR BEGINNERS:
    /// 
    /// PARTIAL CLASSES AND CODE-BEHIND:
    /// - MainWindow is a partial class, split between MainWindow.xaml (UI design) and
    ///   this file (code logic). During compilation, they're merged into one class.
    /// - XAML defines the visual tree: buttons, canvas, labels, etc.
    /// - Code-behind (this file) defines the behaviour and logic.
    /// 
    /// EVENT HANDLERS:
    /// - Event handlers are methods that respond to user actions or system events.
    /// - Signature: void MethodName(object sender, EventArgs e)
    /// - sender: the object that triggered the event (e.g., a button)
    /// - e: contains data about the event (e.g., which mouse button was clicked)
    /// - Common events in this project: Click (buttons), MouseLeftButtonUp, MouseRightButtonUp (hexagons)
    /// 
    /// DISPATCHER TIMER:
    /// - The DispatcherTimer is WPF's way of running code repeatedly on a schedule.
    /// - Unlike System.Timers.Timer, DispatcherTimer works on the UI thread, so it's
    ///   safe to update UI controls directly (like changing button text or label content).
    /// - Interval: time between ticks (we use 1 second for the game timer).
    /// - Tick event: fires each time the interval elapses (every 1 second in our case).
    /// 
    /// DATA BINDING AND CONTENT:
    /// - WPF controls like buttons and labels have Content properties that define what's displayed.
    /// - lblTimer.Content = "Time: 42" updates the label text.
    /// - This is more flexible than winForms where you'd use .Text property.
    /// 
    /// CANVAS AND DRAWING:
    /// - Canvas is a WPF container that lets you position child elements at exact X,Y coordinates.
    /// - Unlike Grid or StackPanel, Canvas doesn't auto-arrange children - you control positions.
    /// - Canvas.SetLeft() and Canvas.SetTop() set the position of a child element.
    /// - We use Canvas to draw our hexagon grid at precise coordinates.
    /// 
    /// POLYGON FOR HEXAGONS:
    /// - Polygon is a WPF shape that draws a multi-sided shape given a list of points.
    /// - We calculate the 6 vertices of each hexagon using trigonometry (sine, cosine, angles).
    /// - Polygon.Points is a PointCollection containing all the vertices.
    /// - Polygon.Fill controls the color (brush) of the interior.
    /// - We attach event handlers to Polygon objects to detect clicks.
    /// 
    /// COMMAND PATTERN / EVENT HANDLERS:
    /// - Clicking a hexagon triggers a MouseLeftButtonUp or MouseRightButtonUp event.
    /// - The event handler receives the Polygon that was clicked as the sender.
    /// - We extract the HexTile from the Polygon.Tag property to access game state.
    /// 
    /// INTERFACE IMPLEMENTATION (IUpdateValues):
    /// - MainWindow implements IUpdateValues to allow GridManager to call back to the UI.
    /// - This decouples the game logic from the presentation layer.
    /// - GridManager doesn't know it's updating a MainWindow - it just calls methods on
    ///   an IUpdateValues interface, which could be any implementation.
    /// 
    /// ARCHITECTURE DECISION:
    /// Why does MainWindow implement IUpdateValues? 
    /// - GridManager needs to update the UI when bombs are flagged/unflagged.
    /// - Direct coupling (GridManager knowing about MainWindow) would be bad design.
    /// - Using an interface allows GridManager to be tested independently and reused
    ///   with different UI implementations.
    /// 
    /// ================================================================
    /// </summary>

    public partial class MainWindow : Window, IUpdateValues
    {
        // attributes for the game

        private GridManager gridManager;
        private DispatcherTimer timer;
        private bool TimerRunning = false;
        public DifficultySettings settings = GameDifficulties.GetSettings(GameDifficulty.Easy); // start on easy

        /// <summary>
        /// This is the constructor of the MainWindow class. 
        /// It initialises the game (passing the selected difficulty)
        /// and the timer when a new instance of MainWindow is created.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitialiseGame(settings);
            OverlayText.Text = "Pick your difficulty";
            Overlay.Visibility = Visibility.Visible;
            InitialiseTimer();
        }

        /// <summary>
        /// This method initialises the game with the given difficulty settings. 
        /// It sets up the grid, window size, and other game parameters, 
        /// and then draws the hex grid and allocates the bombs, calculating the bomb neighbours.
        /// </summary>
        /// <param name="difficulty"></param>
        private void InitialiseGame(DifficultySettings difficulty)
        {
            Overlay.Visibility = Visibility.Collapsed;
            PauseButton.IsEnabled = true;
            //create the gridmanager with the appropriate settings
            gridManager = new GridManager(difficulty, this);

            //work out the size of the hexgrid
            double hexGridWidth = (gridManager.HexesWide + 0.5) * (gridManager.HexRadius * Math.Sqrt(3));
            double hexGridHeight = gridManager.HexesHigh * (gridManager.HexRadius * Math.Sqrt(3));
            double widthOfButtons = 150;
            double padding = 20;

            //set the column widths
            MainGrid.ColumnDefinitions[0].Width = new GridLength(hexGridWidth + 2 * padding);
            MainGrid.ColumnDefinitions[1].Width = new GridLength(widthOfButtons + padding);

            //set the window size as I can't set the row height

            // Set the window height
            this.Height = hexGridHeight + (padding * 2) + SystemParameters.MenuBarHeight;
            this.Width = hexGridWidth + widthOfButtons + (padding * 4); // no idea why I need to multiply by 4 instead of 2 here

            //clear the canvas
            canvas.Children.Clear();

            //reset the grid
            gridManager.ResetGrid();
            lblBombsRemaining.Content = $"Bombs: {gridManager.BombCount}";

            //redraw the grid
            DrawHexGrid();

            //allocate the bombs & recalculate the bomb neighbours
            gridManager.AllocateBombs();
            gridManager.CalculateBombNeighbours();

        }

        /// <summary>
        /// This method starts a new game with the selected difficulty. It resets the timer and starts it.
        /// </summary>
        /// <param name="difficulty"></param>
        private void StartGame(GameDifficulty difficulty)
        {
            settings = GameDifficulties.GetSettings(difficulty);
            InitialiseGame(settings);
            ResetTimer();
            TimerRunning = false;
        }


        #region Timer stuff
        /// <summary>
        /// This method initialises the timer, setting it to tick every second and start immediately.
        /// </summary>
        private void InitialiseTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick; // Add the Timer_Tick method as the event handler

        }

        /// <summary>
        /// This method is the event handler for the timer's Tick event. 
        /// It increments the secondsElapsed variable and updates the timer label every second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            gridManager.SecondsElapsed++;
            lblTimer.Content = $"Time: {gridManager.SecondsElapsed}";
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void StartTimer()
        {
            timer.Start();
            TimerRunning = true;
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void StopTimer()
        {
            timer.Stop();
            TimerRunning = false;
        }

        /// <summary>
        /// Resets the timer and updates the label back to 0
        /// </summary>
        public void ResetTimer()
        {
            StopTimer();
            TimerRunning = false;
            gridManager.SecondsElapsed = 0;
            lblTimer.Content = "Time: 0";
        }
        #endregion

        #region Button Event Handlers

        /// <summary>
        /// Pauses the timer when the pause button is clicked, resumes when clicked again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                TimerRunning = false;
                PauseButton.Content = "Resume"; // Change the button text to "Resume"
                Overlay.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                OverlayText.Text = "Paused"; // Show the "Paused" text on the overlay
                Overlay.Visibility = Visibility.Visible;
            }
            else
            {
                timer.Start();
                TimerRunning = true;
                PauseButton.Content = "Pause"; // Change the button text back to "Pause"
                Overlay.Visibility = Visibility.Collapsed;
                Overlay.Background = new SolidColorBrush(Color.FromArgb(161, 0, 0, 0));
            }
        }

        /// <summary>
        /// When the user clicks the Show Timer button, it toggles the visibility of the timer label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleTimer_Click(object sender, RoutedEventArgs e)
        {
            if (lblTimer.Visibility == Visibility.Visible)
            {
                lblTimer.Visibility = Visibility.Collapsed;
                ToggleTimerMenuItem.Header = "Show Timer";
            }
            else
            {
                lblTimer.Visibility = Visibility.Visible;
                ToggleTimerMenuItem.Header = "Hide Timer";
            }
        }
        /// <summary>
        /// Show the high scores window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHighScores_Click(object sender, RoutedEventArgs e)
        {
            if (TimerRunning) //only pause if the timer is running
            {
                PauseButton_Click(sender, e);
            }

            HighScoreWindow highScoresWindow = new HighScoreWindow(gridManager.LoadAllHighScores());
            highScoresWindow.ShowDialog();
        }

        /// <summary>
        /// Exit the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// This method is the event handler for the left click event on a hexagon.
        /// When the user left clicks on a hexagon, it checks if the hexagon is a bomb or not.
        /// If it is a bomb it changes it to be red and pops up a message box saying "Game Over".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftClick(object sender, MouseButtonEventArgs e)
        {
            //start timer after first click if not already
            if (TimerRunning == false) { StartTimer(); TimerRunning = true; }
            //work out which hexagon was clicked
            Polygon hex = sender as Polygon;
            //check it's a hexagon and it has a hextile object tagged
            if (hex != null && hex.Tag is HexTile hexTile)
            {
                //check if the hexagon is a bomb - if so change it red
                if (hexTile.IsBomb)
                {
                    hex.Fill = Brushes.Red;
                    //game over
                    StopTimer();
                    OverlayText.Text = "Game Over!";
                    Overlay.Visibility = Visibility.Visible;
                    PauseButton.IsEnabled = false;
                }
                else
                {
                    //if not a bomb reveal tile and floodfill
                    gridManager.FloodFillReveal(hexTile);
                    //check if won
                    if (gridManager.CheckWin())
                    {
                        SetWinScreen();
                    }
                }
            }
        }

        /// <summary>
        /// Changes the overlay to show the win screen and stops the timer.
        /// </summary>
        public void SetWinScreen()
        {
            StopTimer();
            OverlayText.Text = $"You Win!\n{gridManager.SecondsElapsed} seconds";
            Overlay.Visibility = Visibility.Visible;
            PauseButton.IsEnabled = false;
        }

        /// <summary>
        /// This is the event handler for the right click event on a hexagon.
        /// When the user right-clicks on a hexagon, it toggles the flag on the hexagon.
        /// It also updates the bomb count label to show the number of bombs remaining.
        /// If the hexagon is flagged, it decreases the bomb count, and if it is unflagged, it increases the bomb count.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightClick(object sender, MouseButtonEventArgs e)
        {
            Polygon hex = sender as Polygon;
            if (hex != null && hex.Tag is HexTile hexTile)
            {
                //toggle the flag
                hexTile.ToggleFlag();
                //check if i need to give them a bomb back
                if (hexTile.IsFlagged)
                {
                    UpdateBombCount(-1);  // If the tile is flagged, decrease the bomb count
                }
                else
                {
                    UpdateBombCount(1);   // else increment the bomb count to give them back the flag
                }
            }
        }

        /// <summary>
        /// This method is the event handler for the Reset button.
        /// It calls InitialiseGame with the current settings to reset the game.
        /// It then resets the timer and starts it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            //reset the game with teh current settings
            InitialiseGame(settings);
            ResetTimer();
            TimerRunning = false;
        }

        /// <summary>
        /// Event handler for the Easy button.
        /// It starts a new game with the Easy difficulty settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame(GameDifficulty.Easy);
        }

        /// <summary>
        /// Event handler for the Medium button.
        /// It starts a new game with the Medium difficulty settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame(GameDifficulty.Medium);
        }

        /// <summary>
        /// Event handler for the Hard button.
        /// It starts a new game with the Hard difficulty settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame(GameDifficulty.Hard);
        }
        #endregion

        #region Draw / Update UI
        /// <summary>
        /// This method received teh amount to change the bomb count by, updates it
        /// and then updates the label to show the new bomb count.
        /// </summary>
        /// <param name="change"></param>
        public void UpdateBombCount(int change)
        {
            gridManager.RemainingBombs += change;
            lblBombsRemaining.Content = $"Bombs Left: {gridManager.RemainingBombs}";
        }


        /// <summary>
        /// This method creates a hexagon polygon with the given size and center coordinates.
        /// It returns the hexagon polygon object
        /// </summary>
        /// <param name="size"></param>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <returns>Polygon</returns>
        public Polygon CreateHexagon(double size, double centerX, double centerY)
        {
            Polygon hexagon = new Polygon { Stroke = Brushes.Black, Fill = Brushes.LightBlue };
            hexagon.Points = new PointCollection();
            //start at 30 degrees for pointy-top hexagon
            double startAngle = Math.PI / 6;

            for (int i = 0; i < 6; i++) //surely I don't need to create a constant for number of sides?
            {
                double angle = startAngle + Math.PI / 3 * i; //60 degrees in radians
                int x = (int)(centerX + size * Math.Cos(angle));
                int y = (int)(centerY + size * Math.Sin(angle));
                hexagon.Points.Add(new Point(x, y)); //add each point to the collection
            }
            return hexagon;
        }

        /// <summary>
        /// This method draws the hex grid on the canvas.
        /// It uses the gridManager to get the number of hexes wide and high, and the radius of the hexes.
        /// It draws the Hexagon polygons created by CreateHexagon and also create a HexTile object for each one.
        /// The coordinates of the hexagon are calculated based on the row and column of the hexagon using double width coordinates and assigned to the hextile object.
        /// The textblock to show the number is added to the canvas and positioned in the center of the hexagon.
        /// The HexTile object is tagged to the polygon and the event handlers are set for left and right clicks
        /// </summary>
        public void DrawHexGrid()
        {

            //work out width and height of the hex
            double hexWidth = Math.Sqrt(3) * gridManager.HexRadius; // flat side to flat side
            double hexHeight = 2 * gridManager.HexRadius; // point to point
            double horizontalSpacing = hexWidth;
            double verticalSpacing = hexHeight * 0.75;
            const double padding = 20;

            //loopy loop
            for (int row = 0; row < gridManager.HexesHigh; row++)
            {
                for (int col = 0; col < gridManager.HexesWide; col++)
                {
                    double x = padding + gridManager.HexRadius + (col * horizontalSpacing);
                    double y = padding + gridManager.HexRadius + (row * verticalSpacing);

                    //offset every other row
                    if (row % 2 == 1)
                    {
                        x += horizontalSpacing / 2;
                    }

                    // create a hexgagon at those coordinates
                    Polygon hex = CreateHexagon(gridManager.HexRadius, x, y);

                    // Calculate the double-width coordinate
                    int xCoord;
                    if (row % 2 == 0)
                    {
                        xCoord = col * 2;
                    }
                    else
                    {
                        xCoord = col * 2 + 1;
                    }
                    int yCoord = row;

                    // Create a HexTile object and assign it to the Tag property of the polygon
                    HexTile hexTile = new HexTile(hex, xCoord, yCoord, gridManager.HexRadius);

                    // setup the textblock for the number
                    hexTile.NumberText.Width = hexWidth;
                    hexTile.NumberText.Height = hexHeight;
                    Canvas.SetLeft(hexTile.NumberText, x - hexWidth / 2); // fiddled around till it looked good
                    Canvas.SetTop(hexTile.NumberText, y - hexHeight / 3);

                    // Add the hexagon and the text to the canvas
                    canvas.Children.Add(hex);
                    canvas.Children.Add(hexTile.NumberText);

                    // tag the hexagon with the hextile
                    hex.Tag = hexTile;

                    //add the hextile to the gridmanager
                    gridManager.AddTileToGrid(hexTile);

                    // set the event handler
                    hex.MouseLeftButtonUp += LeftClick;
                    hex.MouseRightButtonUp += RightClick;

                }
            }
        }
        #endregion
    }
}