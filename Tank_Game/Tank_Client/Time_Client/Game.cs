using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Client
{
    class Game
    {
        public Player player { get; set; }
        public String[] brickWalls { get; set; }
        public String[] stone { get; set; }
        public String[] water { get; set; }

        public Game()
        {
            player = new Player();
        }
    }
}
