using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    public static class Visualization
    {

        // Initialize message log queue
        public static Queue<string> MessageLog = new Queue<string>();

        /// <summary>
        /// Method to draw the screen, with the targeting and player boards' current status
        /// </summary>
        /// <param name="OppBoard"> Gameboard object of the opponent's (AI) board </param>
        /// <param name="PlayBoard"> Gameboard object of the player's board </param>
        public static void DrawScreen(GameBoard OppBoard, GameBoard PlayBoard)
        {
            // Clear console window
            Console.Clear();

            // Draw the opponent's board first as the targeting board, then draw the player's board below that
            Console.WriteLine("\n \n     TARGETING BOARD");
            OppBoard.DrawBoard(true);   // Draw the opponent's board without the ships

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("     YOUR BOARD");
            PlayBoard.DrawBoard(false); // Draw your own board with ships showing

            // Remove log entries when it becomes too long
            while (MessageLog.Count > 8) MessageLog.Dequeue();

            // Print out the message log
            Console.ForegroundColor = ConsoleColor.White;
            foreach (string message in MessageLog)
            {
                Console.WriteLine(message);
            }
            Console.WriteLine("\n \n");
        }
    }
}
