using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_Client.ai
{
    class Test
    {
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
                    if (grid.walls.Contains(id)) { Console.Write("##"); }
                    else if (ptr.x == x + 1) { Console.Write("\u2192 "); } // right arrow
                    else if (ptr.x == x - 1) { Console.Write("\u2190 "); } // left arrow
                    else if (ptr.y == y + 1) { Console.Write("\u2193 "); } // down arrow
                    else if (ptr.y == y - 1) { Console.Write("\u2191 "); } // up arrow
                    else { Console.Write("* "); }
                }
                Console.WriteLine();
            }
        }

        static void DrawInitialGrid(Grid grid)
        {
            // Print out the cameFrom array
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    Cell id = new Cell(x, y);
                    Cell ptr = id;

                    if (grid.walls.Contains(id)) { Console.Write("##"); }
                    else if (grid.forests.Contains(id)) { Console.Write("@@"); }
                    else { Console.Write("* "); }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            // Make "diagram 4" from main article
            var grid = new Grid(10, 10);
            for (var x = 1; x < 4; x++)
            {
                for (var y = 7; y < 9; y++)
                {
                    grid.walls.Add(new Cell(x, y));
                }
            }
            grid.forests = new HashSet<Cell>
            {
                new Cell(3, 4), new Cell(3, 5),
                new Cell(4, 1), new Cell(4, 2),
                new Cell(4, 3), new Cell(4, 4),
                new Cell(4, 5), new Cell(4, 6),
                new Cell(4, 7), new Cell(4, 8),
                new Cell(5, 1), new Cell(5, 2),
                new Cell(5, 3), new Cell(5, 4),
                new Cell(5, 5), new Cell(5, 6),
                new Cell(5, 7), new Cell(5, 8),
                new Cell(6, 2), new Cell(6, 3),
                new Cell(6, 4), new Cell(6, 5),
                new Cell(6, 6), new Cell(6, 7),
                new Cell(7, 3), new Cell(7, 4),
                new Cell(7, 5)
            };

            // Run A*
            DrawInitialGrid(grid);
               
            var patheFinder = new PathFinder(grid, new Cell(1, 4),
                                        new Cell(8, 5));

            DrawGrid(grid, patheFinder);
            int a = Console.Read();
        }
        
    }
}
