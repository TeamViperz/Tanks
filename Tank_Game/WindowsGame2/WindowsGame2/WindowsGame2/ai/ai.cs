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
        public Ai(Game2 game)
        {
            this.game = game;
            this.myPlayerNo = game.myPlayerNumber;
        }
        private static Stack<Cell> path;
       


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
            path = new Stack<Cell>();
            Cell current = goal;
            path.Push(current);
            while (current.x != start.x || current.y != start.y)
            {
                current = cameFrom[current];
                path.Push(current);
                timeCostToTarget += 1;
            }
             path.Pop();
             game.timeCostToTarget = timeCostToTarget;

         }

        public Cell findPath(Game2 game)
        {
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

            
            // get ai player's my Player's current position
            var start = new Cell(game.player[myPlayerNo].playerLocationX, game.player[myPlayerNo].playerLocationY);

            // this should be a life pack or coin pack
            var goal = new Cell(9,9);  

            if (start.x == goal.x && start.y == goal.y)
            {
                game.timeCostToTarget = 0; return goal;
            }

            var patheFinder = new PathFinder(grid, start, goal);

            BuildPath(start, goal, patheFinder.cameFrom);
          
            // send this path back
            var nextCell = path.Pop();
            path.Clear();
            return nextCell;
        }
        
    }
}
