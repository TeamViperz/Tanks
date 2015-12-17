using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Tank_Client.ai;
using Tank_Client.Gui;
using Tank_Client.serverClientConnection;

namespace Tank_Client.GameEngine
{
    public class GameConsole
    {
        private static Stack<Cell> path;

        static void BuildPath(Cell start, Cell goal, Dictionary<Cell, Cell> cameFrom)
        {
            path = new Stack<Cell>();
            Cell current = goal;
            path.Push(current);
            while (current.x != start.x || current.y != start.y)
            {
                current = cameFrom[current];
                path.Push(current);
            }

        }

        static void DrawGrid(Grid grid, PathFinder patheFinder)
        {
            // Print out the cameFrom array
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    Cell id = new Cell(x, y);
                    Cell ptr = id;

                    // Here the 'out' modifier is used to pass the parameter 'ptr', without assigning an initial value to it
                    if (!patheFinder.cameFrom.TryGetValue(id, out ptr))
                    {
                        ptr = id;
                    }
                    if (grid.brickWalls.Contains(id)) { Console.Write("B "); }
                    else if (grid.stone.Contains(id)) { Console.Write("S "); }
                    else if (grid.water.Contains(id)) { Console.Write("W "); }
                    else if (grid.lifePacks.Contains(id)) { Console.Write("L "); }
                    else if (path.Contains(id)) { Console.Write("* "); }
                    /*   else if (ptr.x == x + 1) { Console.Write("\u2192 "); } // right arrow
                       else if (ptr.x == x - 1) { Console.Write("\u2190 "); } // left arrow
                       else if (ptr.y == y + 1) { Console.Write("\u2193 "); } // down arrow
                       else if (ptr.y == y - 1) { Console.Write("\u2191 "); } // up arrow*/
                    else { Console.Write(". "); }
                }
                Console.WriteLine();

            }

            /*  foreach (KeyValuePair<Cell, Cell> entry in patheFinder.cameFrom)
              {
                  Console.WriteLine(entry.Key.x.ToString()+", "+ entry.Key.y.ToString() + " <-- " + entry.Value.x.ToString()+", "+ entry.Value.y.ToString());

              }*/
        }

        static void Main(string[] args)
        {
            // Make a new Game
            Game game = new Game();


          

            // Create a new gateway to communicate with the server
            ConnectionToServer myConnection = new ConnectionToServer(game);

            Console.Title = "Client";

            // Send initial join request to the server.
            myConnection.sendJOINrequest();


            // Create a new thread to handle incoming traffic from server
            Thread thread = new Thread(new ThreadStart(() => myConnection.receiveData()));
            thread.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Joystick(myConnection));


            /* sr.Close();

             // path finding demo
             var grid = new Grid(10, 10);

             for (var x = 1; x < 4; x++)
             {
                 grid.brickWalls.Add(new Cell(x, 2));
             }

             for (var x = 1; x < 4; x++)
             {
                 for (var y = 7; y < 9; y++)
                 {
                     grid.brickWalls.Add(new Cell(x, y));
                 }
             }

             Cell start = new Cell(0, 0); // get this from server messege
             Cell goal = new Cell(8, 7); // this should be a coin, life packet - read from server messege
             var patheFinder = new PathFinder(grid, start, goal);

             BuildPath(start, goal, patheFinder.cameFrom); // save the best path to the Stack 'path'
             DrawGrid(grid, patheFinder);
             //travel the path
             // use a while loop*/
        }

    }
}
