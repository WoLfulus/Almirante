using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.NetworkProtocol
{
    /// <summary>
    /// Join packet.
    /// </summary>
    [Packet(Packets.JoinResponse)]
    public class JoinResponse : Packet
    {
        /// <summary>
        /// Join success.
        /// </summary>
        [Field(1)]
        public bool Success
        {
            get;
            set;
        }

        /// <summary>
        /// Message content.
        /// </summary>
        [Field(2)]
        public string Message
        {
            get;
            set;
        }
    }
}
