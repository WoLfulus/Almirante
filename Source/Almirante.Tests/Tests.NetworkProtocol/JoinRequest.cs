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
    [Packet(Packets.JoinRequest)]
    public class JoinRequest : Packet
    {
        /// <summary>
        /// Name field.
        /// </summary>
        [Field(1)]
        public string Name
        {
            get;
            set;
        }
    }
}
