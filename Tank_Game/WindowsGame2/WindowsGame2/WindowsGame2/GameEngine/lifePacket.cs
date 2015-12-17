using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame2.GameEngine
{
    public class lifePacket
    {
        public int locationX { get; set; }
        public int locationY { get; set; }

        public int lifeTime { get; set; }

        public int appearTimeStamp { get; set; }


        public lifePacket(int locationX, int locationY, int lifeTime, int appearTimeStamp)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.lifeTime = lifeTime;
            this.appearTimeStamp = appearTimeStamp;
            Console.Write("Life pack Time stamp ******************************************:-  "+ appearTimeStamp);
        }
    }
}
