using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsGame2.GameEngine;


namespace WindowsGame2.ai
{
    class Ai
    {
        private int myPlayerNo;
        private Game2 game;

        private int timeCostToTarget ;
        int lowestTimeCostToCoinPile = 1000;
        int lowestTimeCostToLifePack = 1000;

        // store the found paths before examine the reachability
        private Stack<Cell> path;

        private Stack<Cell> pathToNearestLifePack;
        private Stack<Cell> pathToNearestCoinPile;

        private coin accuiredCoin;
        private lifePacket accuiredLifePacket;
     
        public Ai(Game2 game)
        {
            this.game = game;
            this.myPlayerNo = game.myPlayerNumber;

            path = new Stack<Cell>();
            pathToNearestLifePack = new Stack<Cell>();
            pathToNearestCoinPile = new Stack<Cell>();
        }

         void DrawPath()
        {
            foreach (Cell item in path)
            {
                Console.WriteLine(item.x.ToString() + ", " + item.y.ToString());
            }
        }

         void BuildPath(Cell start, Cell goal, Dictionary<Cell, Cell> cameFrom)
         {
            int timeCostToTarget = 0;
            
            Cell current = goal;
            path.Push(current);
            while (current.x != start.x || current.y != start.y)
            {
                current = cameFrom[current];
                path.Push(current);
                timeCostToTarget += 1;
            }
             path.Pop();
             this.timeCostToTarget = timeCostToTarget;
         }

        public Cell findPath(Game2 game)
        {
            lowestTimeCostToCoinPile = 1000;
            lowestTimeCostToLifePack = 1000;

            String[,] board = game.board;
            var grid = new Grid(10, 10);

            // set the obstacles to the Grid from the game
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    // set bricks 
                    if (board[i, j] == "B")
                    {
                        grid.brickWalls.Add(new Cell(j, i));
                    }
                    // set stones
                    else if (board[i, j] == "S")
                    {
                        grid.stone.Add(new Cell(j, i));
                    }
                    // set water
                    else if (board[i, j] == "W")
                    {
                        grid.water.Add(new Cell(j, i));
                    }
                }
            }

            
            // get my Player's current position
            var start = new Cell(game.player[myPlayerNo].playerLocationX, game.player[myPlayerNo].playerLocationY);


            //#################### Begins the Procedure to Get the proper goal to follow. #####################################

            /* tips...
            value of the coin, lifetime, whether an enemy is also targetting the coin - if he can get it soon,....
            if my health is low, high priority to health pack
            */

            /* STEPS
            1. apply A* for every pack on the board - store each path (stacks), time cost to goal (in s)
            2. for each goal, time to goal > life time of goal ? ignore; 
           (3) pro step - if enemy is goalting this and he can reach it before me ? ignore; ( this will be implemented at the final stage of the development)
           (4) now I have reachable goals. if my health is low? lifepack: coin
            5. if coin:  select the most valuable coin -- to be implemented!!!
            6. now you have a precise goal !
           
             
             /*  TO DO
             go to the most valuable coin pile from the reachable coinPile list
             */


            // ######### Life Packs search ##########
       
                foreach (var lifePack in game.Lifepacket)
                {
                    Console.WriteLine("lowestTimeCost= " + lowestTimeCostToLifePack + " timeCostToTarget= " + timeCostToTarget + " lifeTime" + lifePack.lifeTime);

                    Cell goal = new Cell(lifePack.locationX, lifePack.locationY);

                    if (start.x == goal.x && start.y == goal.y) // otherwise pathfinder will break
                    {
                        Console.WriteLine("Life Pack Accuired!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    accuiredLifePacket = lifePack;
                        continue;

                    }

                    var pathFinder = new PathFinder(grid, start, goal);
                    BuildPath(start, goal, pathFinder.cameFrom);

                    // filter the reachable life packs in time
                    if (timeCostToTarget <= lifePack.lifeTime && timeCostToTarget < lowestTimeCostToLifePack) // < or <=
                    {
                        Console.WriteLine("New lowest time cost = " + timeCostToTarget);
                    lowestTimeCostToLifePack = timeCostToTarget;

                        // keep the backup of the nearestpath sequence for now
                        if (path.Count != 0)
                        {
                        pathToNearestLifePack = new Stack<Cell>(path.Reverse());
                        }
                    }
                }

                if (game.Lifepacket.Count != 0)
                {
                    Console.WriteLine("Life Pack removed!");
                    game.Lifepacket.Remove(accuiredLifePacket);
                }

            // ######### Coin Piles search ##########

             foreach (var coinPile in game.Coin)
             {
                Console.WriteLine("lowestTimeCost= " + lowestTimeCostToCoinPile + " timeCostToTarget= " + timeCostToTarget + " lifeTime" + coinPile.lifeTime);

                Cell goal = new Cell(coinPile.locationX, coinPile.locationY);

                 if (start.x == goal.x && start.y == goal.y) // otherwise pathfinder will break
                {            
                    Console.WriteLine("Coin Accuired!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    accuiredCoin = coinPile;
                    continue;
                    
                }
                
                 var pathFinder = new PathFinder(grid, start, goal);
                 BuildPath(start, goal, pathFinder.cameFrom);

                // filter the reachable coins in time
                 if (timeCostToTarget <= coinPile.lifeTime && timeCostToTarget < lowestTimeCostToCoinPile) // < or <=
                 {
                    Console.WriteLine("New lowest time cost = "+timeCostToTarget);
                    lowestTimeCostToCoinPile = timeCostToTarget;
                    
                    // keep the backup of the nearestpath sequence for now
                     if (path.Count != 0) {
                        pathToNearestCoinPile = new Stack<Cell>(path.Reverse()); 
                      } 
                 }

             }

            if (game.Coin.Count != 0)
            {
                Console.WriteLine("Coin removed!");
                game.Coin.Remove(accuiredCoin);
            } 


            // ####### take decission to go to the Life pack or the Coin pile
            
            if (lowestTimeCostToCoinPile < lowestTimeCostToLifePack)
            {
                if (pathToNearestCoinPile.Count != 0)
                {
                    //get the next cell address to move
                    var nextCell = pathToNearestCoinPile.Pop();

                    // clear stacks
                    pathToNearestCoinPile.Clear();
                    path.Clear();

                    return nextCell;
                }
                else
                {
                    // if there aren't any coins on the board
                    path.Clear();
                    return start;
                }
            }

            if (pathToNearestLifePack.Count != 0)
            {
                //get the next cell address to move
                var nextCell = pathToNearestLifePack.Pop();

                // clear stacks
                pathToNearestLifePack.Clear();
                path.Clear();

                return nextCell;
            }
            else
            {
                // if there aren't any life packs on the board
                path.Clear();
                return start;
            }
    

        }
        
    }
}
