using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipGame
{
    /// <summary>
    /// Class to control the AI in the battleship game
    /// </summary>
    class AI 
    {
        // Class constructor
        public AI(ref GameBoard BoardObj)
        {
            // Initialize mode AI is in. Initially it searches the board randomly for ships.
            mode = "search";
            PlayerBoard = BoardObj;
        }

        // Initialize variables
        GameBoard PlayerBoard;
        string mode;
        Dictionary<string, int> HitLocation = new Dictionary<string, int>();

        // Reset search direction variables en counter
        bool upcheck; bool downcheck; bool leftcheck; bool rightcheck;
        int upcount; int downcount; int leftcount; int rightcount;


        /// <summary>
        /// Method to execute a turn for the AI
        /// </summary>
        public void TakeTurn()
        {
            // Make the program wait 3secs to simulate pc 'thinking'
            Console.WriteLine("The computer is taking his turn...");
            Thread.Sleep(1500);

            // If the AI is in search mode, take a random shot
            if (mode == "search")
            {
                Visualization.MessageLog.Enqueue("The AI is taking a random shot");
                SearchingShot();
            }

            // If the AI is in destroy mode, it takes a targeted shot around the location of the hit 
            else if (mode == "destroy")
            {
                Visualization.MessageLog.Enqueue("The AI is taking a targeted shot");
                TargetedShot();
            }
        }

        /// <summary>
        /// Take a searching shit, randomly on the board
        /// </summary>
        /// <param name="board"></param>
        void SearchingShot()
        {
            // Initialize variables
            int col, row;
            string HitType;

            // Generate random squares to hit until one is succesfull
            bool ValidShot = false;
            while (!ValidShot)
            {
                // Determine random location on the board. Shots are taken in diagonal lines across the board
                // First a random row is chosen
                row = GameBoard.RandomObject.Next(10); 
                // If the row is even, only odd columns can be chosen. Otherwise only even columns can be chosen
                col = GameBoard.RandomObject.Next(5) * 2;
                if (row % 2 == 0) col++;

                // Try to shoot chosen location
                HitType = PlayerBoard.NewShot(col, row);
                if (HitType == "alreadyhit")
                {
                    // This square has been hit already
                    ValidShot = false;
                }
                else
                {
                    // A shot has been taken
                    ValidShot = true;
                    if (HitType == "miss" || HitType == "sunk")
                    {
                        // If it was a miss, or somehow a ship was sunk, keep on searching
                        mode = "search";
                    }
                    else if (HitType == "hit")
                    {
                        // If a ship was hit, enter destroy mode
                        mode = "destroy";
                        // Save the square hit in the HitLocation dictionary
                        HitLocation.Add("col", col);
                        HitLocation.Add("row", row);

                        // Reset search direction variables
                        upcheck = true; downcheck = true; leftcheck = true; rightcheck = true;
                        // Reset counter variable
                        upcount = 1; downcount = 1; leftcount = 1; rightcount = 1;
                    }
                }
            }
        }

        /// <summary>
        /// After a ship has been it, target the squares around it until it sinks
        /// </summary>
        void TargetedShot()
        {
            
            // Try to take a shot up from the original hit
            if (upcheck)
            {
                //int[] coor = { HitLocation["row"] - upcount, HitLocation["col"] };
                // Try to take shot at adjusted coordinates
                string shotresult = PlayerBoard.NewShot(HitLocation["col"], HitLocation["row"] - upcount);

                // Check if shot is not possible, it's outside the board or the spot has been hit before
                if (shotresult == "oob" || shotresult == "alreadyhit") upcheck = false;
                
                // Otherwise, the shot has been succesful
                else
                {
                    if (shotresult == "hit") upcount++;
                    // If the ship has been sunk, return to search mode
                    else if (shotresult == "sunk")
                    {
                        // To add: if more than 1 ship has been hit in 'destroy' mode, update HitLocation to that second ship
                        mode = "search";
                        HitLocation.Clear();
                    }
                    // Shot has been taken, exit function
                    return;
                }
            }


            // Try to take a shot down from the original hit
            if (downcheck)
            {
                // Try to take shot at adjusted coordinates,
                string shotresult = PlayerBoard.NewShot(HitLocation["col"], HitLocation["row"] + downcount);

                // Check if shot is not possible, it's outside the board or the spot has been hit before
                if (shotresult == "oob" || shotresult == "alreadyhit") downcheck = false;

                // Otherwise, the shot has been succesful
                else
                {
                    if (shotresult == "hit") downcount++;
                    // If the ship has been sunk, return to search mode
                    else if (shotresult == "sunk")
                    {
                        // To add: if more than 1 ship has been hit in 'destroy' mode, update HitLocation to that second ship
                        mode = "search";
                        HitLocation.Clear();
                    }
                    // Shot has been taken, exit function
                    return;
                }
            }

            // Try to take a shot down from the original hit
            if (leftcheck)
            {
                // Try to take shot at adjusted coordinates,
                string shotresult = PlayerBoard.NewShot(HitLocation["col"] - leftcount, HitLocation["row"]);

                // Check if shot is not possible, it's outside the board or the spot has been hit before
                if (shotresult == "oob" || shotresult == "alreadyhit") leftcheck = false;

                // Otherwise, the shot has been succesful
                else
                {
                    if (shotresult == "hit") leftcount++;
                    // If the ship has been sunk, return to search mode
                    else if (shotresult == "sunk")
                    {
                        // To add: if more than 1 ship has been hit in 'destroy' mode, update HitLocation to that second ship
                        mode = "search";
                        HitLocation.Clear();
                    }
                    // Shot has been taken, exit function
                    return;
                }
            }

            // Try to take a shot down from the original hit
            if (rightcheck)
            {
                // Try to take shot at adjusted coordinates,
                string shotresult = PlayerBoard.NewShot(HitLocation["col"] + rightcount, HitLocation["row"]);

                // Check if shot is not possible, it's outside the board or the spot has been hit before
                if (shotresult == "oob" || shotresult == "alreadyhit") rightcheck = false;

                // Otherwise, the shot has been succesful
                else
                {
                    if (shotresult == "hit") rightcount++;
                    // If the ship has been sunk, return to search mode
                    else if (shotresult == "sunk")
                    {
                        // To add: if more than 1 ship has been hit in 'destroy' mode, update HitLocation to that second ship
                        mode = "search";
                        HitLocation.Clear();
                    }
                    // Shot has been taken, exit function
                    return;
                }
            }

            // Somehow no targeted shot could be taken, so take a random shot instead
            SearchingShot();
        }
    }
}
