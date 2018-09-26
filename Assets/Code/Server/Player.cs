namespace DFPongServer {
    public class Player {
        // Variables
        public ushort ID;
        public float Score;
        public float Y;

        // Constructor
        public Player(ushort ID, float Y) {
            this.ID = ID;
            this.Y = Y;
        }
    }
}