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
    [Packet(Packets.Join)]
    public class Join : Packet
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

        /// <summary>
        /// Test field
        /// </summary>
        [Field(2)]
        public uint Test
        {
            get;
            set;
        }
    }
}
