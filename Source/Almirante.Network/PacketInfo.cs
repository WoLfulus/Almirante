using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Almirante.Network
{
    /// <summary>
    /// Packet information.
    /// </summary>
    public class PacketInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Writer
        /// </summary>
        public Action<BinaryWriter, Packet> Writer
        {
            get;
            internal set;
        }

        /// <summary>
        /// Reader
        /// </summary>
        public Action<BinaryReader, Packet> Reader
        {
            get;
            internal set;
        }

        /// <summary>
        /// Reader
        /// </summary>
        public Func<Packet> Constructor
        {
            get;
            internal set;
        }
    }
}
