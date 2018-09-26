using DarkRift;
using DarkRift.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFPongServer
{
    public class PlayerManager : DarkRift.Server.Plugin {
        // Fields
        Dictionary<IClient, Player> players = new Dictionary<IClient, Player>();

        // Constructor
        public PlayerManager(PluginLoadData data) : base(data) {
            ClientManager.ClientConnected += ClientConnected;
        }

        // Overrides
        public override bool ThreadSafe => false;
        public override Version Version => new Version(1, 0, 0);

        // Events
        void ClientConnected(object sender, ClientConnectedEventArgs e) {
            //float x = (players.Count == 0) ? -8f : 8f;
            //Player player = new Player(e.Client.ID, x, 0f);

            //// Spawn new player for existing players
            //using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
            //    // Create message
            //    writer.Write(player.ID);
            //    writer.Write(player.X);
            //    writer.Write(player.Y);
            //    Message message = Message.Create(Tags.SpawnPlayer, writer);

            //    // Send message
            //    foreach (IClient client in ClientManager.GetAllClients().Where(b => b != e.Client))
            //        client.SendMessage(message, SendMode.Reliable);
            //}

            //// Spawn existing players for new player
            //players.Add(e.Client, player);
            //using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
            //    // Create message
            //    foreach (Player buffer in players.Values) {
            //        writer.Write(buffer.ID);
            //        writer.Write(buffer.X);
            //        writer.Write(buffer.Y);
            //    }
            //    Message message = Message.Create(Tags.SpawnPlayer, writer);

            //    // Send message
            //    e.Client.SendMessage(message, SendMode.Reliable);
            //}

            // Store player info
            Player player = new Player(e.Client.ID, 0f);
            players.Add(e.Client, player);

            // Setup listener
            e.Client.MessageReceived += MessageReceived;

            // Let's get the ball rolling!
            ResetBall();
        }
        void MessageReceived(object sender, MessageReceivedEventArgs e) {
            // Get message/reader
            Message m = e.GetMessage();
            DarkRiftReader r = m.GetReader();

            // Filter through tags
            switch (m.Tag) {
                case Tags.MovePlayer:
                    MovePlayer(e.Client, m, r);
                    break;
                case Tags.ScoreUpdate:
                    ScoreUpdate(e.Client);
                    break;
            }
        }

        // Methods
        void MovePlayer(IClient client, Message m, DarkRiftReader r) {
            // Read new position
            float y = r.ReadSingle();
            players[client].Y = y;

            // Send clients position data
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(y);
            m.Serialize(writer);
            foreach (IClient c in ClientManager.GetAllClients().Where(b => b != client))
                c.SendMessage(m, SendMode.Unreliable);
        }
        void ResetBall() {
            // Calculate new velocity and speed
            float[] x = { Utils.RandomFloat(-1f, 1f), 0f };
            x[1] = x[0] * -1;
            float y = Utils.RandomFloat(-1f, 1f);
            float speed = Utils.RandomFloat(6f, 9f);

            // Get clients
            IClient[] clients = ClientManager.GetAllClients();

            // Loop through clients
            for (int i = 0; i < clients.Count(); i++) {
                // Create message
                DarkRiftWriter writer = DarkRiftWriter.Create();
                writer.Write(x[i]);
                writer.Write(y);
                writer.Write(speed);

                // Send message
                Message message = Message.Create(Tags.ResetBall, writer);
                clients[i].SendMessage(message, SendMode.Unreliable);
            }
        }
        void ScoreUpdate(IClient client) {
            // Increment player score
            players[client].Score++;

            Console.WriteLine("Player #" + players[client].ID + " has just scored! Score: " + players[client].Score);

            // Start new round
            ResetBall();
        }
    }
}