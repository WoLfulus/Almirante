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
    public class ServerProtocol : Protocol<Player>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServerProtocol()
        {
            this.Register<Chat>(this.OnChat);
            this.Register<Join>(this.OnJoin);
        }

        /// <summary>
        /// Join packet.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="packet"></param>
        protected void OnJoin(Player player, Join packet)
        {

        }

        /// <summary>
        /// Chat packet.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="packet"></param>
        protected void OnChat(Player player, Chat packet)
        {

        }
    }
}
