using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== HexTile.cs ====================
    /// 
    /// INDIVIDUAL GAME TILE - STATE AND REPRESENTATION
    /// 
    /// PURPOSE:
    /// HexTile is a model class that represents a single hexagonal tile on the game board.
    /// It encapsulates:
    /// 1. The visual representation (Polygon shape on the canvas)
    /// 2. The text display (TextBlock for showing bomb count)
    /// 3. The game state (whether it's a bomb, flagged, revealed, etc.)
    /// 4. The position (coordinates in the hexagonal grid)
    /// 5. The neighbourhood context (how many bombs are in adjacent tiles)
    /// 
    /// WHY A SEPARATE CLASS FOR TILES?
    /// - ENCAPSULATION: All data about a tile is bundled together in one place.
    /// - MAINTAINABILITY: If tile behaviour changes, you only modify one class.
    /// - CLARITY: Code is easier to understand: hexTile.IsBomb is clearer than
    ///   searching a separate data structure for the tile's bomb status.
    /// - OBJECT-ORIENTED: Each tile is a distinct object with its own state.
    /// 
    /// HOW IT WORKS:
    /// 
    /// INITIALISATION:
    /// - Constructor receives:
    ///   * A Polygon object created by MainWindow (the visual hexagon shape)
    ///   * X, Y coordinates in the hexagonal grid
    ///   * Radius (size of the hexagon)
    /// - It stores the Polygon reference so game logic can interact with the visual element.
    /// - It creates and configures a TextBlock for displaying the bomb count number.
    /// - All properties are initialised with default values (not a bomb, not flagged, etc.)
    /// 
    /// VISUAL PROPERTIES:
    /// - Hexagon: Reference to the Polygon shape on the canvas. Its Fill (colour) changes
    ///   during gameplay:
    ///   * Light Blue (initial, unrevealed)
    ///   * White (revealed, not a bomb)
    ///   * Yellow (flagged as suspected bomb)
    ///   * Red (revealed, IS a bomb - shown on game over)
    /// - NumberText: A TextBlock positioned inside the hexagon showing the bomb count (1-6)
    ///   or empty for 0 bombs. Visibility is toggled to show/hide.
    /// 
    /// GAME STATE PROPERTIES:
    /// - IsBomb: Boolean. True if this tile contains a bomb.
    /// - IsFlagged: Boolean. True if the player right-clicked to flag it as suspected bomb.
    /// - IsRevealed: Boolean. True if the player left-clicked to reveal its contents.
    /// - BombNeighbours: Int (0-6). How many adjacent tiles contain bombs.
    ///   (Hexagons have 6 neighbours, unlike rectangles which have 8)
    /// 
    /// COORDINATE SYSTEM:
    /// - Coordinates: A HexCoord object storing X and Y in the double-width hex system.
    /// - See GridManager documentation for explanation of the coordinate system.
    /// 
    /// METHODS:
    /// 
    /// ShowNumber(int number):
    /// - Purpose: Reveal this tile by displaying its bomb count.
    /// - Logic: Only shows the number if BombNeighbours > 0 (don't show "0").
    /// - Side Effect: Makes the TextBlock visible.
    /// - Called by: GridManager.RevealTile()
    /// 
    /// ToggleFlag():
    /// - Purpose: Toggle the flagged state (flag if unflagged, unflag if flagged).
    /// - Logic: Flips IsFlagged, then changes Hexagon.Fill colour accordingly.
    /// - Colours:
    ///   * Yellow = Flagged (player thinks this is a bomb)
    ///   * Light Blue = Unflagged (back to default unrevealed colour)
    /// - Called by: MainWindow.RightClick() event handler
    /// - Note: Flagging doesn't reveal the tile, it just marks it for suspicion.
    /// 
    /// VISUAL FLOW DURING GAMEPLAY:
    /// 1. Initial State: All hexagons are Light Blue, TextBlocks are hidden.
    /// 2. Player Right-Clicks: ToggleFlag() changes colour to Yellow and sets IsFlagged = true.
    /// 3. Player Left-Clicks (if not a bomb): ShowNumber() makes TextBlock visible,
    ///    Hexagon.Fill becomes White.
    /// 4. Player Left-Clicks (if IS a bomb): Colour becomes Red (set by MainWindow), game over.
    /// 5. Cascading Reveal: If clicked tile has 0 bombs, GridManager.FloodFillReveal()
    ///    recursively reveals adjacent tiles.
    /// 
    /// TEXT BLOCK STYLING:
    /// The NumberText TextBlock is configured with:
    /// - HorizontalAlignment.Center / VerticalAlignment.Center: Text centered in hexagon
    /// - TextAlignment.Center: Text content centered (for multi-line if needed)
    /// - FontWeight.Bold: Makes numbers stand out
    /// - Foreground = Brushes.Black: Black text color
    /// - FontSize = radius: Scales text size based on hexagon size (larger hexagons = larger text)
    /// - Initial Visibility = Collapsed: Hidden until tile is revealed
    /// 
    /// RELATIONSHIP TO OTHER CLASSES:
    /// - Created by: MainWindow.DrawHexGrid() when the board is set up
    /// - Stored in: GridManager.HexGrid (List<HexTile>)
    /// - Accessed by: GridManager methods (AllocateBombs, CalculateBombNeighbours, RevealTile, etc.)
    /// - Referenced by: Polygon.Tag property (allows event handlers to find the HexTile)
    /// 
    /// COORDINATE SYSTEM INTEGRATION:
    /// - Each HexTile stores its HexCoord (position in grid)
    /// - GridManager uses these coordinates to find neighbours
    /// - Neighbours are found using hardcoded offsets specific to the double-width hex system
    /// 
    /// DESIGN NOTES:
    /// - HexTile is a simple data holder with minimal logic - mostly just getters/setters
    /// - The heavy logic (bomb placement, reveal cascading) is in GridManager
    /// - This separation keeps HexTile focused and easy to understand
    /// - All references to visual elements (Polygon, TextBlock) stay in HexTile
    ///   so if you need to change how tiles look, you know where to change it
    /// 
    /// ================================================================
    /// </summary>

    public class HexTile
    {
        public Polygon Hexagon { get; set; }
        public TextBlock NumberText { get; set; }
        public bool IsBomb { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsRevealed { get; set; }
        public HexCoord Coordinates { get; set; }
        public int BombNeighbours { get; set; }

        public HexTile(Polygon hexagon, int xCoord, int yCoord, int radius)
        {
            Hexagon = hexagon;
            Coordinates = new HexCoord(xCoord, yCoord);
            NumberText = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                FontSize = radius,
                Visibility = Visibility.Collapsed
            };
        }
        // Method to update the number and make it visible
        public void ShowNumber(int number)
        {
            if (BombNeighbours > 0)
            {
                NumberText.Text = number.ToString();
            }
            NumberText.Visibility = Visibility.Visible; // Make the TextBlock visible
        }

        public void ToggleFlag()
        {
            IsFlagged = !IsFlagged;
            if (IsFlagged)
            {
                Hexagon.Fill = Brushes.Yellow;  // Set the fill to yellow if the tile is flagged
            }
            else
            {
                Hexagon.Fill = Brushes.LightBlue;  // Set the fill to light blue if the tile is not flagged
            }
        }


    }
}
