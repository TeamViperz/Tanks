using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank_Client.ai
{
    struct Cell
    {
        // cell location on the grid
        public readonly int x, y;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
