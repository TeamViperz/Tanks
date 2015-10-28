using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Client
{
    public class Player
    {
        public int playerNumber { get; set; }
        public int playerLocationX { get; set; }
        public int playerLocationY { get; set; }
        public int direction { get; set; }
      
        public int whetherShot { get; set; }
        public int health { get; set; }
        public int coins { get; set; }
        public int points { get; set; }

        public String toString()
        {
            return "\nPlayer Number: "+ (playerNumber).ToString() + "\nPlayer Location: " + (playerLocationX).ToString() + "," +(playerLocationY).ToString() +"\nPlayer direction: "+(direction).ToString()+"\n";
        }
    }
}
