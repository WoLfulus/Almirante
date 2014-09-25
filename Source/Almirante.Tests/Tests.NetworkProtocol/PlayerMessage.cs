using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.NetworkProtocol
{
    [Packet(Packets.ServerMessage)]
    public class PlayerMessage : Packet
    {
        /// <summary>
        /// Chat message.
        /// </summary>
        [Field(1)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Chat message.
        /// </summary>
        [Field(2)]
        public string Message
        {
            get;
            set;
        }
    }
}
