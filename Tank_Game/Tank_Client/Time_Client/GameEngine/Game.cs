using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank_Client.GameEngine;
using Time_Client.GameEngine;


namespace Tank_Client
{
    public class Game
    {
        // Stores the players
        public Player[] player { get; set; }

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

        public Game()
        {
            player = new Player[5];

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
