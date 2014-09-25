using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.NetworkProtocol
{
    [Packet(Packets.MessageResponse)]
    public class MessageResponse : Packet
    {
        /// <summary>
        /// Chat message.
        /// </summary>
        [Field(1)]
        public bool Success
        {
            get;
            set;
        }
    }
}
