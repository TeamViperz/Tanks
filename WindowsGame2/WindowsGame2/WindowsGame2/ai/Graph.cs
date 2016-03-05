using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsGame2.ai
{
    interface Graph<L>
    {
        int Cost(Cell a, Cell b);
        IEnumerable<Cell> Neighbors(Cell id);
    }
}
