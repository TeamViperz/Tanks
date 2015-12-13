namespace Tank_Client.GameEngine
{
    public class lifePacket
    {
        public int locationX{get; set;}
        public int locationY { get; set; }
        
        public int lifeTime { get; set; }

        public lifePacket(int locationX, int locationY,  int lifeTime) {
            this.locationX = locationX;
            this.locationY = locationY;
            this.lifeTime = lifeTime;
        }
    }
}
