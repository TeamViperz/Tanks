using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame2.GameEngine
{
    public class Game2
    {
        // Stores the players
        public Player[] player { get; set; }
        public int brickLen { get; set; }

        // My player Number
        public int myPlayerNumber { get; set; }
        public int totalPlayers { get; set; }

        public String[] brickWalls { get; set; }
        public String[] stone { get; set; }
        public String[] water { get; set; }
        public List<coin> Coin;
        public List<lifePacket> Lifepacket;

        // Two dimentional array for the game board
        public String[,] board { get; set; }

        // Backup of initial board without player locations
        public String[,] InitialBoard { get; set; }

        public int timeCostToTarget { get; set; }
        public int gameClock { get; set; }

        public Game2()
        {
            player = new Player[5];
            gameClock = 0;
            for (int i = 0; i < 5; ++i)
            {
                player[i] = new Player();
                player[i].playerNumber = i;
            }


            board = new String[10, 10];
            InitialBoard = new String[10, 10];

            Coin = new List<coin>();
            Lifepacket = new List<lifePacket>();
        }


    }
}
