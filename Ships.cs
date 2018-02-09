using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame
{
    // Abstract ship class to keep track of which ship has been hit / sunk
    public abstract class Ship
    {
        public string Name { get; set; }
        public char Shorthand { get; set; }
        public int Length { get; set; }
        public int Hits { get; set; }
        public bool Placed = false;
        public bool Obstructed = false;
        public bool IsSunk => (Hits >= Length);
    }

    // Subclasses of ship for the different types of different lengths
    public class Destroyer : Ship
    {
        public Destroyer()
        {
            Name = "Destroyer";
            Shorthand = 'D';
            Length = 2;
        }
    }

    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Shorthand = 'S';
            Length = 3;
        }
    }

    public class Cruiser : Ship
    {
        public Cruiser()
        {
            Name = "Cruiser";
            Shorthand = 'C';
            Length = 3;
        }
    }

    public class Battleship : Ship
    {
        public Battleship()
        {
            Name = "Battleship";
            Shorthand = 'B';
            Length = 4;
        }
    }

    public class Carrier : Ship
    {
        public Carrier()
        {
            Name = "Aircraft Carrier";
            Shorthand = 'A';
            Length = 5;
        }
    }

}
