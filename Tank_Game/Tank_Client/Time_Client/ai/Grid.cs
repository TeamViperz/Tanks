﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_Client.ai
{
    class Grid : Graph<Cell>
    {
        public int width, height;
        public HashSet<Cell> walls = new HashSet<Cell>();
        public HashSet<Cell> forests = new HashSet<Cell>();

        //Square grid only allows 4 directions of movement
        public static readonly Cell[] MOVABLE_DIRECTIONS = new[]
        {
            new Cell(1, 0), // right
            new Cell(0, -1), // up
            new Cell(-1, 0), // left
            new Cell(0, 1) // down
        };

        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        
       /// <summary>
       /// check the target cell (id) is within the grid
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public bool InBounds(Cell id)
        {
            return 0 <= id.x && id.x < width && 0 <= id.y && id.y < height;
        }


        /// <summary>
        /// TO check whether the cell (id) can be traversed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Passable(Cell id)
        {
            return !walls.Contains(id);
        }

        /// <summary>
        /// Returns the cost to travel on cell b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Cost(Cell a, Cell b)
        {
            return forests.Contains(b) ? 5 : 1;
        }

        /// <summary>
        /// Returns an enumerator containing all the neighbours of the Cell-'id', which supports iteration over Cells
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Cell> Neighbors(Cell id)
        {
            foreach (var dir in MOVABLE_DIRECTIONS)
            {
                Cell next = new Cell(id.x + dir.x, id.y + dir.y);
                if (InBounds(next) && Passable(next))
                {
                    //have used yield-return to return the cells incrementally.
                    yield return next;
                }
            }
        }

    }
}
