using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame2.GameEngine
{
    public class coin
    {
        public int locationX { get; set; }
        public int locationY { get; set; }
        public int value { get; set; }
        public int lifeTime { get; set; }

        public coin(int locationX, int locationY, int value, int lifeTime)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.value = value;
            this.lifeTime = lifeTime;
        }

    }
}
