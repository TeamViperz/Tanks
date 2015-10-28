using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Client.GameEngine;


namespace Time_Client
{
    public class Game
    {
        public Player[] player { get; set; }
        public int myPlayerNumber { get; set; }
        public int totalPlayers { get; set; }

        public String[] brickWalls { get; set; }
        public String[] stone { get; set; }
        public String[] water { get; set; }
        public String[,] board { get; set; } // two dimentional array for the game board

        public List<coin> Coin;
        public List<lifePacket> Lifepacket;

        public String[,] InitialBoard { get; set; } // initial board without player locations

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
