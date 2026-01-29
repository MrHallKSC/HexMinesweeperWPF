using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMinesweeper
{
    /// <summary>
    /// ==================== HexCoord.cs ====================
    /// 
    /// HEXAGONAL GRID COORDINATE SYSTEM
    /// 
    /// PURPOSE:
    /// HexCoord is a simple data class that stores the position of a hex tile within
    /// the hexagonal grid. It holds the X and Y coordinates using a specific coordinate
    /// system designed for hex grids called "double-width" coordinates.
    /// 
    /// WHY A SEPARATE CLASS?
    /// - CLARITY: "hexTile.Coordinates.X" is more descriptive than "hexTile.XCoord"
    /// - ENCAPSULATION: Grouping related X,Y data together is object-oriented design
    /// - REUSABILITY: Any method that needs coordinates can use HexCoord instead of
    ///   passing two separate int parameters
    /// - FUTURE EXTENSIBILITY: If you add coordinate methods (like Distance calculation),
    ///   they go in one place
    /// 
    /// THE DOUBLE-WIDTH COORDINATE SYSTEM:
    /// 
    /// What is it?
    /// A clever way to represent hexagonal grids using integer coordinates that make
    /// neighbour calculations simple and intuitive. Instead of using "axial" or "cube"
    /// coordinates, double-width treats alternating rows differently.
    /// 
    /// How does it work?
    /// 
    /// VISUAL EXAMPLE (5x3 grid):
    /// 
    /// Row 0 (even):   Hex Hex Hex Hex Hex
    ///                 (0,0)(2,0)(4,0)(6,0)(8,0)
    /// 
    /// Row 1 (odd):     Hex Hex Hex Hex Hex
    ///                  (1,1)(3,1)(5,1)(7,1)(9,1)
    /// 
    /// Row 2 (even):   Hex Hex Hex Hex Hex
    ///                 (0,2)(2,2)(4,2)(6,2)(8,2)
    /// 
    /// KEY POINTS:
    /// - Even rows (0, 2, 4...): X coordinates are EVEN (0, 2, 4, 6, 8...)
    /// - Odd rows (1, 3, 5...): X coordinates are ODD (1, 3, 5, 7, 9...)
    /// - Y always goes from 0 to HexesHigh - 1
    /// - X goes from 0 to HexesWide*2 - 1 (hence "double-width")
    /// 
    /// WHY IS THIS USEFUL?
    /// 
    /// NEIGHBOUR FINDING: All hexagons have exactly 6 neighbours, and they're always
    /// at the same relative offsets:
    /// 
    /// Left and Right (always):        (-2, 0)  and  (+2, 0)
    /// Upper-Left and Upper-Right:     (-1, -1) and  (+1, -1)
    /// Lower-Left and Lower-Right:     (-1, +1) and  (+1, +1)
    /// 
    /// This is MUCH simpler than other coordinate systems! You don't need to handle
    /// different cases for even vs. odd rows - the offsets are uniform.
    /// 
    /// EXAMPLE USAGE IN CODE:
    /// 
    /// If you're at coordinate (4, 2), your 6 neighbours are at:
    /// - (2, 2)  [Left]
    /// - (6, 2)  [Right]
    /// - (3, 1)  [Upper-Left]
    /// - (5, 1)  [Upper-Right]
    /// - (3, 3)  [Lower-Left]
    /// - (5, 3)  [Lower-Right]
    /// 
    /// See GridManager.GetNeighbours() for the actual implementation.
    /// 
    /// IMMUTABILITY:
    /// - Both X and Y have private setters, so once a HexCoord is created, you can't
    ///   change its coordinates.
    /// - This prevents bugs where a tile's coordinates accidentally get modified.
    /// - This is a good practice in game development.
    /// 
    /// COMPARISON AND EQUALITY:
    /// - HexCoord uses default .Equals() based on object reference, not coordinate values.
    /// - GridManager.ContainsTileAt() manually compares X and Y values rather than using .Equals()
    /// - If you needed many coordinate comparisons, you could override .Equals() and .GetHashCode()
    /// 
    /// PRACTICAL EXAMPLE:
    /// 
    /// During board initialization, MainWindow.DrawHexGrid() creates each tile with
    /// coordinates like this:
    /// 
    ///     HexTile hexTile = new HexTile(hex, xCoord, yCoord, gridManager.HexRadius);
    /// 
    /// Later, GridManager uses these coordinates to find neighbours:
    /// 
    ///     HexCoord neighbourCoords = new HexCoord(tile.Coordinates.X + offset.X, 
    ///                                             tile.Coordinates.Y + offset.Y);
    ///     if (ContainsTileAt(neighbourCoords))
    ///     {
    ///         neighbours.Add(GetTile(neighbourCoords));
    ///     }
    /// 
    /// This shows the key benefit: coordinates are used to find tiles in the grid,
    /// which is much more efficient than searching through all tiles linearly.
    /// 
    /// FURTHER READING:
    /// If you're interested in hex grid coordinate systems, search for:
    /// - "Red Blob Games Hexagonal Grids" (excellent resource)
    /// - "Axial coordinates"
    /// - "Cube coordinates"
    /// - "Offset coordinates" (what this project uses)
    /// 
    /// ================================================================
    /// </summary>

    public class HexCoord
    {
        //class to store the coordinates of the hexagon in the grid.
        public int X { get; private set; }
        public int Y { get; private set; }

        public HexCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}

