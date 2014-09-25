using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.NetworkProtocol;

namespace Tests.NetworkServer.Network
{
    /// <summary>
    /// Server protocol.
    /// </summary>
    public class Server : NetServer<Player>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Server()
            : base(100)
        {
            this.Protocol.Register<MessageRequest>(this.OnChat);
            this.Protocol.Register<JoinRequest>(this.OnJoin);
        }

        /// <summary>
        /// Join packet.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="packet"></param>
        protected void OnJoin(Player client, JoinRequest packet)
        {
            Console.WriteLine("[PLAYER JOIN] Name = " + packet.Name);

            foreach (var conn in this.Connections)
            {
                if (conn.Name == null)
                {
                    continue;
                }

                if (packet.Name.ToLower() == conn.Name.ToLower() && conn.Id != client.Id)
                {
                    client.Send(new JoinResponse()
                    {
                        Success = false, 
                        Message = "This name is already taken. Please, choose another one."
                    });
                    return;
                }
            }

            this.BroadcastMessage("SYSTEM", "Player '" + packet.Name + "' connected.");

            client.Name = packet.Name;
            client.Send(new JoinResponse()
            {
                Success = true,
                Message = ""
            });
        }

        /// <summary>
        /// Chat packet.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="packet"></param>
        protected void OnChat(Player client, MessageRequest packet)
        {
            if (client.Name != null)
            {
                this.BroadcastMessage(client.Name, packet.Message);
                Console.WriteLine("[" + client.Name + "] " + packet.Message);
            }
        }

        /// <summary>
        /// Message
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        protected void BroadcastMessage(string name, string message)
        {
            foreach (var conn in this.Connections)
            {
                if (conn.Name != null) // player is logged in.
                {
                    conn.Send(new PlayerMessage()
                    {
                        Name = name,
                        Message = message
                    });
                }
            }
        }

        /// <summary>
        /// Start
        /// </summary>
        protected override void OnStart()
        {
            Console.WriteLine("[SERVER STARTED]");
        }

        /// <summary>
        /// Stopped
        /// </summary>
        protected override void OnStop()
        {
            Console.WriteLine("[SERVER STOPEED]");
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="ex"></param>
        protected override void OnError(Exception ex)
        {
            Console.WriteLine("[SERVER ERROR] " + ex.Message);
        }
    }
}
