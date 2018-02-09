using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
 
    class Program
    {

        static void Main()
        {
            // Welcome player
            Visualization.MessageLog.Enqueue("Welcome to Battleship!");

            // Create game board objects for player and AI
            // These are created with randomly placed ships, currently the player cannot chose where to place their ships
            GameBoard PlayerBoard = new GameBoard();
            GameBoard CompBoard = new GameBoard();

            //Show initial state
            Console.SetWindowSize(80, 65);
            Visualization.DrawScreen(CompBoard, PlayerBoard);

            // Draw boards again, or play
            while (true)
            {
                Console.Write("\n Do you want to re-randomize the ships? (Y/N): ");
                ConsoleKeyInfo ReadyInput = Console.ReadKey();
                if (ReadyInput.KeyChar == 'N' || ReadyInput.KeyChar == 'n')
                {
                    Console.WriteLine("\n Let's play!");
                    break;
                }
                else if (ReadyInput.KeyChar == 'Y' || ReadyInput.KeyChar == 'y')
                {
                    // Reset boards
                    PlayerBoard = new GameBoard();
                    CompBoard = new GameBoard();
                    //Show state
                    Console.Clear();
                    Visualization.DrawScreen(CompBoard, PlayerBoard);    
                }
            }

            // Determine if player of computer starts randomly
            int CurrentTurn = GameBoard.RandomObject.Next(2);

            // Initialization of game loop variables
            string BombInput = "";
            bool GameOver = false;

            // Initialize AI object
            AI CompPlayer = new AI(ref PlayerBoard);

            // Loop through player and AI turns
            while (!GameOver)
            {
                if (CurrentTurn == 0)
                {
                    // Player turn
                    Console.WriteLine("It's your turn!");
                    // Wait for correct input from player (row+column)
                    int BombCol = 0;
                    char BombRow = '\0';
                    bool CorrectInput = false;
                    while (!CorrectInput)
                    {
                        Console.Write("Please input valid row & column to bombard (from A1 to J10):");
                        // Read user input and conver it to upper case
                        BombInput = Console.ReadLine();
                        BombInput = BombInput.ToUpper();

                        // Check if valid row is given
                        if (GameBoard.Alphabet.Contains(BombInput[0]))
                        {
                            BombRow = BombInput[0];

                            // Check if valid column is given and save column to string BombCol
                            if (Int32.TryParse(BombInput.Substring(1), out BombCol))
                            {
                                CorrectInput = (BombCol <= 10) && (BombCol > 0);
                            }
                        }
                    }

                    // Pass bomb location to the board
                    Visualization.MessageLog.Enqueue("You have taken a shot");
                    string HitStatus = CompBoard.NewShot(BombCol - 1, GameBoard.Alphabet.IndexOf(BombRow));
                    // Check if a square has been chosen that was already hit
                    if (HitStatus == "alreadyhit") 
                    {
                        Console.WriteLine("The chosen square was already hit, please try again");
                        continue;
                    }

                    // Display updated state
                    Visualization.DrawScreen(CompBoard, PlayerBoard);

                    // Check win condition
                    GameOver = CompBoard.CheckWin();
                    if (GameOver)
                    {
                        Console.WriteLine("Congratulations! You have won the match!");
                    }
                    else 
                    {
                        // If the player has not won, pass the turn to the computer
                        CurrentTurn = 1;
                    }
                }
                else if (CurrentTurn == 1)
                {
                    // AI turn
                    CompPlayer.TakeTurn();

                    // Display updated state
                    Visualization.DrawScreen(CompBoard, PlayerBoard);

                    // Check win condition
                    GameOver = PlayerBoard.CheckWin();
                    if (GameOver)
                    {
                        Console.WriteLine("Oh no, you have lost... Better luck next time!");
                    }
                    else 
                    {
                        // If the computer has not won, pass the turn to the player
                        CurrentTurn = 0;
                    }

                }
                else
                {
                    // Something went wrong
                    Console.WriteLine("Something went wrong...");
                    break;
                }
            }
        }
    }
}
