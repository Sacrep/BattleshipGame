using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BattleshipGame
{
    // Gameboard class
    public class GameBoard
    {
        // Class constructor
        public GameBoard()
        {
            // Place ships randomly on board
            foreach (Ship s in ships)
            {
                
                // Loop until valid placement for the ship has been found
                while (!s.Placed)
                {
                    // Reset the obstructed parameter
                    s.Obstructed = false;

                    // Pick random start coordinates
                    int col = RandomObject.Next(10);
                    int row = RandomObject.Next(10);

                    // Pick random direction to place the ship in
                    int dir = RandomObject.Next(2);

                    //Place the ship either vertically or horizontally
                    switch (dir)
                    {
                        case 0:
                            // horizontal
                            // Check if the ship does not go out of bounds
                            if (col + s.Length > 10) continue;

                            // Check if there is another ship in the way (one of the characters is no longer the initial '\0' null state)
                            for (int x = 0; x < s.Length; x++)
                            {
                                if (board[col + x, row] != '\0') s.Obstructed = true;
                            }

                            // Place ship, marked as 'S' characters on the board
                            if (!s.Obstructed)
                            {
                                for (int x = 0; x < s.Length; x++)
                                {
                                    board[col + x, row] = s.Shorthand;                                    
                                }
                                s.Placed = true;
                            }
                            break;

                        case 1:
                            // vertical
                            // Check if the ship does not go out of bounds
                            if (row + s.Length > 10) continue;

                            // Check if there is another ship in the way (one of the characters is no longer the initial '\0' null state)
                            for (int y = 0; y < s.Length; y++)
                            {
                                if (board[col, row + y] != '\0') s.Obstructed = true;
                            }

                            // Place ship, marked as 'S' characters on the board, if it is not already occupied
                            if (!s.Obstructed)
                            {
                                for (int y = 0; y < s.Length; y++)
                                {
                                    board[col, row + y] = s.Shorthand;
                                }
                                s.Placed = true;
                            }
                            break;

                        default:
                            Console.WriteLine("Something went wrong here");
                            break;
                    }
                }
            }
        }

        //// Class properties
        // Constant string to write rows on the gameboard
        public const string Alphabet = "ABCDEFGHIJ";

        // Initialize empty board
        char[,] board = new char[10, 10];

        // Create ship game objects
        // Do the ships need to be objects? In order to track damage and if they have sunk
        Ship[] ships =
            {
                new Carrier(),
                new Battleship(),
                new Cruiser(),
                new Submarine(),
                new Destroyer()
            };


        // Initialize random number generator
        public static Random RandomObject = new Random();


        // Function to draw the gameboard in the console window
        public void DrawBoard(bool hide)
        {
            // Show column indicators
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  ¦ 1 2 3 4 5 6 7 8 9 10");
            Console.WriteLine("--+---------------------");

            // Loop through the 10 rows
            for (int row = 0; row < 10; row++)
            {
                // Show row indicator (A - J)
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(Alphabet[row]);
                Console.Write(" ¦ ");

                // Loop through the columns
                for (int col = 0; col <= 9; col++)
                {
                    // Write board character to screen
                    // TODO: check if ships should be hidden
                    char c = board[col, row];

                    // Change color depending on character to write (red for hits, white for misses)
                    if (c == 'H') Console.ForegroundColor = ConsoleColor.Red;
                    else if (c == 'x') Console.ForegroundColor = ConsoleColor.White;
                    // If ships need to be hidden, replace their characters with an empty char
                    else if (hide) c = '\0';
                    else Console.ForegroundColor = ConsoleColor.DarkGray;
                    // Write character
                    Console.Write(c + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n");
        }


        /// <summary>
        /// Method to update the board with a new hit
        /// </summary>
        public string NewShot(int Column, int Row)
        {
            string HitStatus;

            // Check if given square is valid.
            if (Column < 0 || Column >=10 || Row < 0 || Row >= 10)
            {
                // Return 'out of bounds' if given coordinates are invalid
                return "oob";
            }

            // Check current status of the square
            char CurrentStatus = board[Column, Row];
            if (CurrentStatus == 'x' || CurrentStatus == 'H')
            {
                // This spot has already been hit, don't update the board
                HitStatus = "alreadyhit"; 
            }
            else if (CurrentStatus == '\0') // It's a miss
            {
                HitStatus = "miss";
                Visualization.MessageLog.Enqueue(string.Format("It's a miss! ({0}{1})", Alphabet[Row], Column + 1));
                // Update square
                board[Column, Row] = 'x';
            }
            else // A ship was hit
            {
                HitStatus = "hit";
                Visualization.MessageLog.Enqueue(string.Format("It's a hit! ({0}{1})", Alphabet[Row], Column + 1));
                // Update square
                board[Column, Row] = 'H';

                // Check which ship has been hit
                Ship s = ships.First(x => x.Shorthand == CurrentStatus);
                // Was the ship sunk?
                s.Hits += 1;
                if (s.IsSunk)
                {
                    HitStatus = "sunk";
                    Visualization.MessageLog.Enqueue(string.Format("The {0} has been sunk!", s.Name));
                }
            }

            return HitStatus;
        }

        // Check if all ships have been sunk
        public bool CheckWin()
        {
            // Loop through the ships
            foreach (Ship s in ships )
            {
                // If one of the ships has not been sunk, return false
                if (!s.IsSunk) return false;
            }

            // If all ships have been sunk, return true
            return true;
        }
    }
}
