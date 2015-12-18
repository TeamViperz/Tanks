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
        public brick[] bricks { get; set; }
        public int brickLen { get; set; }

        // My player Number
        public int myPlayerNumber { get; set; }
        public int totalPlayers { get; set; }

        public String[] brickWalls { get; set; }
        public String[] stone { get; set; }
        public String[] water { get; set; }
        public List<coin> Coin { get; set; }
        public List<lifePacket> Lifepacket { get; set; }

        // tempory list to kill lifepackets
        public List<lifePacket> killListLifePack;

        // tempory list to kill coin piles
        public List<coin> killListCoinPile;

        // Two dimentional array for the game board
        public String[,] board { get; set; }

        // Backup of initial board without player locations
        public String[,] InitialBoard { get; set; }

        public int timeCostToTarget { get; set; }
        public int gameClock { get; set; }

        public Game2()
        {
            player = new Player[5];
            bricks = new brick[20];
            gameClock = 1;
            for (int i = 0; i < 5; ++i)
            {
                player[i] = new Player();
                player[i].playerNumber = i;
            }

            for (int i = 0; i < 20; i++)
            {
                bricks[i] = new brick();
                bricks[i].isFull = false;
            }


            board = new String[10, 10];
            InitialBoard = new String[10, 10];

            Coin = new List<coin>();
            Lifepacket = new List<lifePacket>();
    
            killListLifePack = new List<lifePacket>();
            killListCoinPile = new List<coin>();
        }

        public void updatePacks(int currentTime)
        {
            foreach (var pack in Lifepacket)
            {
                //Console.WriteLine("current time:- "+currentTime+" start:- "+pack.appearTimeStamp+" lifeTime:- "+pack.lifeTime+" *****************----------------");
                if (currentTime >= pack.appearTimeStamp + pack.lifeTime)
                {
                    board[pack.locationY, pack.locationX] = ".";
                    killListLifePack.Add(pack);
                }
            }
            foreach (var i in killListLifePack)
            {
                Lifepacket.Remove(i);
            }

            foreach (var pack in Coin)
            {
                //Console.WriteLine("current time:- " + currentTime + " start:- " + pack.appearTimeStamp + " lifeTime:- " + pack.lifeTime + " *****************----------------");
                if (currentTime >= pack.appearTimeStamp + pack.lifeTime)
                {
                    board[pack.locationY, pack.locationX] = ".";
                    killListCoinPile.Add(pack);
                }
            }
            foreach (var i in killListCoinPile)
            {
                Coin.Remove(i);
            }

        }

        public void addPacksToBoard()
        {
            foreach (var pack in Lifepacket)
            {

                board[pack.locationY, pack.locationX] = "L";
            }

            foreach (var pack in Coin)
            {

                board[pack.locationY, pack.locationX] = "C";
            }

        }

    }
}
