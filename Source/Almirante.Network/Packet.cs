using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Almirante.Network
{
    /// <summary>
    /// Packet class.
    /// </summary>
    public class Packet 
    {
        /// <summary>
        /// Packet id.
        /// </summary>
        public int Id
        {
            get
            {
                return PacketManager.GetInformation(this.GetType()).Id;
            }
        }

        /// <summary>
        /// Writes the packet to a buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] Write()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    PacketInfo info = PacketManager.GetInformation(this.GetType());
                    info.Writer(writer, this);
                    writer.Flush();
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Reads the packet from a buffer.
        /// </summary>
        /// <param name="buffer"></param>
        public void Read(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    PacketInfo info = PacketManager.GetInformation(this.GetType());
                    info.Reader(reader, this);
                }
            }
        }
    }


    /// <summary>
    /// Packet attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PacketAttribute : Attribute
    {
        /// <summary>
        /// Packet id.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id.</param>
        public PacketAttribute(int id)
        {
            this.Id = id;
        }
    }

    /// <summary>
    /// Packet attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// Packet id.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id.</param>
        public FieldAttribute(int id)
        {
            this.Id = id;
        }
    }
}
