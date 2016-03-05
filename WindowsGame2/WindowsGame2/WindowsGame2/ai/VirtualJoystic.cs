using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsGame2.serverClientConnection;

namespace WindowsGame2.ai
{
    class VirtualJoystic
    {
        public void move(ConnectionToServer conn, Cell nextMove, int currentX, int currentY)
        {
            
            if (nextMove.x == currentX + 1) { conn.sendData("RIGHT#"); }
            else if (nextMove.x == currentX - 1) { conn.sendData("LEFT#"); }
            else if (nextMove.y == currentY + 1) { conn.sendData("DOWN#"); }
            else if (nextMove.y == currentY - 1) { conn.sendData("UP#"); }

            Thread.Sleep(1000);

        }
    }
}
