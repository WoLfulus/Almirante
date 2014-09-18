using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.NetworkProtocol;

namespace Tests.NetworkServer.Server
{
    /// <summary>
    /// Server protocol.
    /// </summary>
    public class ServerProtocol : Protocol<Client>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServerProtocol()
        {
            this.Register<PacketChat>(this.OnChat);
            this.Register<PacketJoin>(this.OnJoin);
        }

        /// <summary>
        /// Join packet.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="packet"></param>
        protected void OnJoin(Client client, PacketJoin packet)
        {
            Console.WriteLine("Client #" + client.Id + " set it's name to '" + packet.Name + "'");
            client.Name = packet.Name;
        }

        /// <summary>
        /// Chat packet.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="packet"></param>
        protected void OnChat(Client client, PacketChat packet)
        {
            if (client.Name != null)
            {
                foreach (var conn in this.Server.Connections)
                {
                    conn.Send(new PacketChat() 
                    {
                        Message = "[" + client.Name + "] " + packet.Message
                    });
                }
                Console.WriteLine("[" + client.Name + "] " + packet.Message);
            }
        }
    }
}
