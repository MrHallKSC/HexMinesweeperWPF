using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;
using System.Windows;


namespace HexMinesweeper
{
    /// <summary>
    /// ==================== GridManager.cs ====================
    /// 
    /// GAME LOGIC AND STATE MANAGEMENT
    /// 
    /// PURPOSE:
    /// GridManager is the "engine" of the HexMinesweeper game. It encapsulates all game logic
    /// and state management, completely separate from the UI. It is responsible for:
    /// 1. Creating and managing the collection of HexTile objects that form the game board
    /// 2. Placing bombs randomly on the grid
    /// 3. Calculating neighbour relationships in the hexagonal grid
    /// 4. Counting bombs adjacent to each tile
    /// 5. Revealing tiles and implementing the cascading reveal algorithm (flood fill)
    /// 6. Checking win/loss conditions
    /// 7. Managing high scores (loading, saving, displaying)
    /// 8. Communicating UI updates back to MainWindow through the IUpdateValues interface
    /// 
    /// WHY SEPARATE LOGIC FROM UI?
    /// - TESTABILITY: You can test all game logic without any UI. Just create a GridManager,
    ///   call methods, and verify the state of HexTiles.
    /// - REUSABILITY: GridManager could be used with a console UI, web UI, or any other UI.
    /// - MAINTAINABILITY: Changes to game rules don't require UI changes and vice versa.
    /// - CLARITY: The code is easier to understand when UI complexity doesn't obscure logic.
    /// 
    /// HOW IT WORKS:
    /// 
    /// INITIALISATION:
    /// - Constructor receives DifficultySettings (grid size, bomb count, etc.) and a reference
    ///   to an IUpdateValues object (implemented by MainWindow).
    /// - It initialises an empty HexGrid list that will hold all HexTile objects.
    /// - It loads any existing high scores for the selected difficulty from JSON files.
    /// 
    /// BUILDING THE GRID:
    /// - MainWindow calls CreateHexagon() repeatedly to create Polygon shapes.
    /// - For each hexagon created, MainWindow creates a HexTile and passes it to AddTileToGrid().
    /// - After all tiles are added, GridManager doesn't interact with the visual Polygon
    ///   objects - the HexTiles hold references to them but the logic focuses on HexTile state.
    /// 
    /// HEXAGONAL COORDINATE SYSTEM:
    /// - The project uses "double-width" coordinates for the hexagonal grid.
    /// - This is a clever coordinate system for hex grids:
    ///   * X coordinate: varies from 0 to (HexesWide * 2 - 1)
    ///   * Y coordinate: varies from 0 to HexesHigh - 1
    ///   * Even rows have x-coordinates 0, 2, 4, 6, ... (even numbers)
    ///   * Odd rows have x-coordinates 1, 3, 5, 7, ... (odd numbers)
    /// - This makes neighbour calculation straightforward: the 6 neighbours are always at
    ///   fixed offsets: (-2,0), (2,0), (-1,-1), (1,-1), (-1,1), (1,1)
    /// - See GetNeighbours() for how this is used.
    /// 
    /// BOMB PLACEMENT AND COUNTING:
    /// - AllocateBombs(): Randomly selects tiles and marks them as bombs. If a tile is
    ///   already a bomb, it retries (to ensure no tile is marked as bomb twice).
    /// - CalculateBombNeighbours(): For each tile, finds all neighbours and counts how many
    ///   are bombs. This count is stored in tile.BombNeighbours (0-6 for a hex grid).
    /// 
    /// REVEALING AND FLOOD FILL:
    /// - RevealTile(): Sets a tile's IsRevealed flag, changes its colour to white, and
    ///   displays the bomb count (if > 0).
    /// - FloodFillReveal(): Recursively reveals tiles. When you click a 0-bomb tile,
    ///   it reveals all neighbours. Any neighbour that also has 0 bombs reveals ITS neighbours,
    ///   and so on. This is the standard Minesweeper "automatic reveal" behaviour.
    /// - Prevents re-revealing already-revealed tiles via the base case check.
    /// 
    /// WIN CONDITION:
    /// - CheckWin(): Returns true if all non-bomb tiles are revealed.
    /// - Also checks if the current time beats the previous high scores.
    /// - If it does, prompts the player for their name and saves the new high score.
    /// 
    /// HIGH SCORE PERSISTENCE:
    /// - LoadHighScores(difficulty): Loads a JSON file (Easy.json, Medium.json, Hard.json)
    ///   containing high scores for that difficulty.
    /// - SaveHighScores(): Serializes the high score list to JSON and writes to a file.
    /// - LoadAllHighScores(): Loads high scores for all three difficulties at once.
    /// 
    /// INTERFACE USAGE (IUpdateValues):
    /// - GridManager holds a reference to an IUpdateValues object (the MainWindow).
    /// - When a tile is unflagged, it calls updateValues.UpdateBombCount(1) to update
    ///   the remaining bomb count displayed in the UI.
    /// - This allows GridManager to request UI updates without directly referencing MainWindow.
    /// 
    /// KEY DATA STRUCTURES:
    /// - HexGrid: List<HexTile> - all tiles on the board
    /// - HexesWide, HexesHigh: int - grid dimensions
    /// - HexRadius: int - size of each hexagon
    /// - BombCount, RemainingBombs: int - bomb tracking
    /// - HighScores: List<HighScore> - high score entries
    /// 
    /// ALGORITHMS USED:
    /// - RANDOM PLACEMENT: AllocateBombs() uses Random.Next() to select tiles.
    /// - NEIGHBOUR FINDING: GetNeighbours() uses coordinate offsets specific to hex grids.
    /// - FLOOD FILL: FloodFillReveal() uses recursion to reveal connected regions.
    /// - SEARCH/FILTER: LINQ queries to find tiles by coordinate or filter high scores.
    /// - SORTING: .OrderBy() and .Take(3) to get top 3 high scores per difficulty.
    /// 
    /// EXCEPTION HANDLING:
    /// - LoadHighScores() checks if file exists before reading (avoids FileNotFoundException).
    /// - Try-catch could be added for JSON deserialisation if needed.
    /// 
    /// BEST PRACTICES DEMONSTRATED:
    /// - ENCAPSULATION: Properties expose data, methods control behaviour.
    /// - SINGLE RESPONSIBILITY: GridManager handles game logic only.
    /// - DRY (Don't Repeat Yourself): GetNeighbours() is reused in CalculateBombNeighbours
    ///   and FloodFillReveal() rather than duplicating the logic.
    /// - CLEAR NAMING: Method names describe what they do (AllocateBombs, FloodFillReveal, etc.)
    /// 
    /// ================================================================
    /// </summary>

    internal class GridManager
    {

        private IUpdateValues updateValues;
        public int HexesWide { get; private set; }
        public int HexesHigh { get; private set; }
        public int HexRadius { get; private set; }
        public int BombCount { get; private set; }
        public int RemainingBombs { get; set; }
        public List<HexTile> HexGrid { get; private set; }
        public int SecondsElapsed { get; set; }
        public List<HighScore> HighScores { get; set; }
        public DifficultySettings currentDifficulty { get; set; }


        // Constructor
        public GridManager(DifficultySettings diff, IUpdateValues updateValues)
        {
            currentDifficulty = diff;
            HighScores = LoadHighScores(diff.Name); //load the highscores for the set difficulty
            this.HexGrid = new List<HexTile>();
            this.HexesWide = currentDifficulty.HexesWide;
            this.HexesHigh = currentDifficulty.HexesHigh;
            this.BombCount = currentDifficulty.Bombs;
            this.RemainingBombs = currentDifficulty.Bombs; // start remaining at starting bomb count
            this.HexRadius = currentDifficulty.HexRadius;
            this.updateValues = updateValues;

        }

        /// <summary>
        /// Method to add a tile to the grid
        /// </summary>
        /// <param name="tile">Tile to add</param>
        public void AddTileToGrid(HexTile tile)
        {
            HexGrid.Add(tile);
        }

        /// <summary>
        /// Method to get a tile from the grid
        /// </summary>
        /// <param name="givenHexCoord">The HexCoord that you want the Hextile for</param>
        /// <returns>Hextile from coord specified</returns>
        public HexTile GetTile(HexCoord givenHexCoord)
        {
            foreach (HexTile tile in HexGrid)
            {
                if (tile.Coordinates.X == givenHexCoord.X && tile.Coordinates.Y == givenHexCoord.Y)
                {
                    return tile;
                }
            }
            return null!;
        }

        /// <summary>
        /// method to return whether a tile exists at the given coordinates
        /// </summary>
        /// <param name="coordinates">The Hexcoord that </param>
        /// <returns>bool</returns>

        public bool ContainsTileAt(HexCoord coordinates)
        {
            foreach (HexTile tile in HexGrid)
            {
                if (tile.Coordinates.X == coordinates.X && tile.Coordinates.Y == coordinates.Y)
                {
                    return true;  // Return true as soon as a matching tile is found
                }
            }
            return false;  // Return false if no matching tile is found
        }

        /// <summary>
        /// Reset the grid by clearing the list of tiles "Hexgrid"
        /// </summary>
        public void ResetGrid()
        {
            HexGrid.Clear();
        }

        /// <summary>
        /// Given a HexTile, return a list of its neighbours
        /// </summary>
        /// <param name="tile">Hextile</param>
        /// <returns>List of Hextiles</returns>
        public List<HexTile> GetNeighbours(HexTile tile)
        {
            List<HexTile> neighbours = new List<HexTile>();

            // Define the possible offsets for neighbouring tiles
            List<System.Drawing.Point> offsets = new List<System.Drawing.Point>
            {
                new System.Drawing.Point(-2, 0), new System.Drawing.Point(2, 0), // Left and right
                new System.Drawing.Point(-1, -1), new System.Drawing.Point(1, -1), // Upper left and upper right
                new System.Drawing.Point(-1, 1), new System.Drawing.Point(1, 1) // Lower left and lower right
            };

            foreach (System.Drawing.Point offset in offsets)
            {
                HexCoord neighbourCoords = new HexCoord(tile.Coordinates.X + offset.X, tile.Coordinates.Y + offset.Y);
                if (ContainsTileAt(neighbourCoords))
                {
                    neighbours.Add(GetTile(neighbourCoords));
                }
            }
            return neighbours;
        }

        /// <summary>
        /// method to randomly place bombs on the grid
        /// </summary>
        public void AllocateBombs()
        {
            Random random = new Random();
            for (int i = 0; i < BombCount; i++)
            {
                //randomly pick a tile from the hexgrid
                HexTile tile = HexGrid[random.Next(HexGrid.Count)];
                //check if the tile is already a bomb, if so, pick another tile
                if (tile.IsBomb)
                {
                    i--;
                }
                else
                {
                    tile.IsBomb = true;
                }

            }
        }

        /// <summary>
        /// method to calculate the number of bomb neighbours for each tile and assign it to the tile's property
        /// </summary>
        public void CalculateBombNeighbours()
        {
            foreach (HexTile tile in HexGrid)
            {
                List<HexTile> neighbours = GetNeighbours(tile);
                int bombCount = 0;
                foreach (HexTile neighbour in neighbours)
                {
                    if (neighbour.IsBomb)
                    {
                        bombCount++;  // Increment the bomb count if the neighbour is a bomb
                    }
                }
                tile.BombNeighbours = bombCount;  // Assign the count of bomb neighbours to the tile's property
            }
        }

        /// <summary>
        /// returns bool to show if the game is won 
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckWin()
        {
            foreach (HexTile tile in HexGrid)
            {
                // If any tile is not revealed and not a bomb, then the player has not won yet
                if (!tile.IsRevealed && !tile.IsBomb)
                {
                    return false;  // Return false as soon as we find a tile that doesn't meet the condition
                }
            }

            // Check if the current score beats the high score for the current difficulty.
            var bestTime = HighScores.Where(h => h.Difficulty == currentDifficulty.Name);
            //MessageBox.Show($"Best time: {(bestTime.Any() ? bestTime.Max(h => h.TimeTaken) : 0)}");
            if (!bestTime.Any() || SecondsElapsed < bestTime.Min(h => h.TimeTaken))
            {
                updateValues.StopTimer();
                //MessageBox.Show($"You have the best time! of {SecondsElapsed}");
                // Prompt the user for their name.
                string playerName = PromptForName();

                if (playerName != null)
                {
                    // Save the new high score.
                    HighScores.Add(new HighScore
                    {
                        PlayerName = playerName,
                        TimeTaken = SecondsElapsed,
                        DateAchieved = DateTime.Now,
                        Difficulty = currentDifficulty.Name
                    });
                    SaveHighScores(HighScores);
                }
            }
            return true;  // If all tiles meet the condition, return true
        }

        /// <summary>
        /// method to reveal a tile by setting it's colour to white and adding the textblock to the canvas
        /// </summary>
        /// <param name="tile"></param>
        public void RevealTile(HexTile tile)
        {
            if (tile.IsFlagged) { TriggerBombUpdate(1); }
            tile.ShowNumber(tile.BombNeighbours);
            tile.IsRevealed = true;
            tile.Hexagon.Fill = Brushes.White;

        }
        /// <summary>
        /// update the bomb count by the given change
        /// Triggers the bomb update event in mainwindow
        /// had to use interface to access it.
        /// </summary>
        /// <param name="change"></param>
        public void TriggerBombUpdate(int change)
        {
            updateValues.UpdateBombCount(change);
        }

        /// <summary>
        /// Method to flood fill reveal the grid 
        /// Uses recusion to reveal all tiles that have no bomb neighbours
        /// </summary>
        /// <param name="tile">The tile to reveal / floodfill from</param>
        public void FloodFillReveal(HexTile tile)
        {
            if (tile.IsRevealed) { return; } //base case
            RevealTile(tile);
            //if the tile has no bomb neighbours, reveal all neighbours recursively
            if (tile.BombNeighbours == 0)
            {
                List<HexTile> neighbours = GetNeighbours(tile);
                foreach (HexTile neighbour in neighbours)
                {
                    if (neighbour.BombNeighbours == 0)
                    {
                        FloodFillReveal(neighbour);
                    }
                    else
                    {

                        RevealTile(neighbour);
                    }
                }
            }
        }

        /// <summary>
        /// passed the difficulty this will return a list of highscores for that difficulty
        /// </summary>
        /// <param name="diff">a string of the difficulty name</param>
        /// <returns>A list of Highscores</returns>
        public List<HighScore> LoadHighScores(string diff)
        {
            if (File.Exists($"{diff}.json"))

            {
                string json = File.ReadAllText($"{diff}.json");
                return JsonSerializer.Deserialize<List<HighScore>>(json);
            }
            else
            {
                return new List<HighScore>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="highScores"></param>
        public void SaveHighScores(List<HighScore> highScores)
        {
            string json = JsonSerializer.Serialize(highScores);
            File.WriteAllText($"{currentDifficulty.Name}.json", json);
        }

        /// <summary>
        /// Loops through all the difficulties and loads the highscores for each
        /// Returns a list of all highscores
        /// </summary>
        /// <returns>A list containing all highscores</returns>
        public List<HighScore> LoadAllHighScores()
        {
            List<HighScore> allHighScores = new List<HighScore>();

            // Load the high scores for each difficulty
            foreach (string difficulty in new[] { "Easy", "Medium", "Hard" })
            {

                allHighScores.AddRange(LoadHighScores(difficulty));
            }

            return allHighScores;
        }

        /// <summary>
        /// Method to prompt the user for their name
        /// SHows the InputHighScoreName dialog 
        /// </summary>
        /// <returns>string of the name provided or null</returns>
        public string PromptForName()
        {
            // Create a new input dialog

            InputHighScoreName inputDialog = new InputHighScoreName(SecondsElapsed, currentDifficulty.Name);
            if (inputDialog.ShowDialog() == true)
            {
                return inputDialog.txtName.Text;
            }
            else
            {
                return null;
            }
        }

    }
}
