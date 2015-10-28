using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Client.client;
using Time_Client.GameEngine;

namespace Time_Client.utils
{
    public class Tokenizer
    {
        private Game game;

        public Tokenizer(Game game)
        {
            this.game = game;
        }

        public int Acceptance(String text)
        {
            try
            {
                text = text.Remove(text.Length - 1);
            
                String[] tokens = text.Split(';');
                game.myPlayerNumber = int.Parse(tokens[0].Substring(3, 1));
                
                game.player[game.myPlayerNumber].playerLocationX = int.Parse(tokens[1].Substring(0, 1));
                game.player[game.myPlayerNumber].playerLocationY = int.Parse(tokens[1].Substring(2, 1));
                game.player[game.myPlayerNumber].direction = int.Parse(tokens[2]);

                //Console.WriteLine(game.player[game.myPlayerNumber].toString());
                return 0;
            }
            catch (Exception e)
            {
                
                Console.WriteLine("Error in messege sent from server :- " + e.Message);
                return -1;
            }
        }
        public int Initiation(String text)
        {
            text = text.Remove(0, 2);
            text = text.Remove(text.Length - 1);

            
            String[] tokens = text.Split(':');

            if (tokens[0].Substring(1, 1).Equals(game.player[game.myPlayerNumber].playerNumber.ToString()))
            {
                clearBoard();
                game.brickWalls = tokens[1].Split(';');
                game.stone = tokens[2].Split(';');
                game.water = tokens[3].Split(';');
                for (int i = 0; i < game.brickWalls.Length; i++)
                {
                    String[] j = game.brickWalls[i].Split(',');
                    game.board[int.Parse(j[1]), int.Parse(j[0])] = "B";

                }
                for (int i = 0; i < game.stone.Length; i++)
                {
                    String[] j = game.stone[i].Split(',');
                    game.board[int.Parse(j[1]), int.Parse(j[0])] = "S";

                }
                for (int i = 0; i < game.water.Length; i++)
                {
                    String[] j = game.water[i].Split(',');
                    game.board[int.Parse(j[1]), int.Parse(j[0])] = "W";

                }
                backupBoard(); // keep a coppy of the initial board
                
            }
            else
            {
                Console.WriteLine("Player name mismatch while initializing");
            }

            return 0;
        }
        public int MovingAnDshooting(String text)
        {
            // restore the board with initial map details
            restoreBoard();

            text = text.Remove(text.Length - 1);
            text = text.Remove(0, 2);
            String[] tokens = text.Split(':');

            game.totalPlayers = tokens.Length;
           
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].StartsWith("P"))
                {
                    
                    string str = tokens[i].Remove(0, 3);
                    
                    string[] tokens2 = str.Split(';');
                   

            
                    for (int j = 0; j < tokens2.Length; j++)
                    {
                        
                        if (j == 0)
                        {
                            
                            game.player[i].playerLocationX = int.Parse(tokens2[j].Substring(0,1));

                            game.player[i].playerLocationY = int.Parse(tokens2[j].Substring(2, 1));
                        }
                        
                        // update the map
                        game.board[game.player[i].playerLocationY, game.player[i].playerLocationX] = i.ToString();

                        if (j == 1)
                        {
                            game.player[i].direction = int.Parse(tokens2[j]);
                        }
                        if (j == 2)
                        {
                            game.player[i].whetherShot = int.Parse(tokens2[j]);
                        }
                        if (j == 3)
                        {
                            game.player[i].health = int.Parse(tokens2[j]);
                        }
                        if (j == 4)
                        {
                            game.player[i].coins = int.Parse(tokens2[j]);
                        }
                        if (j == 5)
                        {
                            game.player[i].points = int.Parse(tokens2[j]);
                        }
                        
                    }
                }
            }
           
            return 0;
        }

        public int Coins(String text)
        {
            
            text = text.Remove(text.Length - 1);
            text = text.Remove(0, 2);
            string[] tokens = text.Split(':');
            coin Coin = new coin(int.Parse(tokens[0].Substring(0,1)), int.Parse(tokens[0].Substring(2,1)), int.Parse(tokens[1]), int.Parse(tokens[2]));
            game.Coin.Add(Coin);
            return 0;
        }

        public int lifePacks(String text)
        {
            text = text.Remove(text.Length - 1);
            text = text.Remove(0, 2);
            string[] tokens = text.Split(':');
            lifePacket LifePacket = new lifePacket(int.Parse(tokens[0].Substring(0, 1)), int.Parse(tokens[0].Substring(2, 1)), int.Parse(tokens[1]));
            game.Lifepacket.Add(LifePacket);
            return 0;
        }

        public int Rejection(String text)
        {
            switch (text)
            {
                case "PLAYERS_FULL#":   
                    return 1; 
                case "ALREADY_ADDED#": 
                    return 2;
                case "GAME_ALREADY_STARTED#": 
                    return 3;

                case "OBSTACLE#": 
                    return 4;
                case "CELL_OCCUPIED#":
                    return 5;
                case "DEAD": 
                    return 6;
                case "TOO_QUICK":
                    return 7;
                case "INVALID_CELL": 
                    return 8;
                case "GAME_HAS_FINISHED":
                    return 9;
                case "GAME_NOT_STARTED_YET":
                    return 10;
                case "NOT_A_VALID_CONTESTANT":
                    return 11;
                default: return 0;
            }    
        }

        public void printBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(game.board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void clearBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game.board[i, j] = "X";
                }
            }
        }

        public void backupBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game.InitialBoard[i, j] = game.board[i, j];
                }
            }
        }

        public void restoreBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    game.board[i, j] = game.InitialBoard[i, j];
                }
            }
        }
    }
}
