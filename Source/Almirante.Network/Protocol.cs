using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Almirante.Network
{

    /// <summary>
    /// Protocol manager.
    /// </summary>
    public class NetClientProtocol
    {
        /// <summary>
        /// Packet handlers.
        /// </summary>
        private Dictionary<int, List<Action<byte[]>>> handlers;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetClientProtocol()
        {
            this.handlers = new Dictionary<int, List<Action<byte[]>>>();
        }

        /// <summary>
        /// Registers a packet type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void Subscribe<P>(Action<P> callback)
            where P : Packet, new()
        {
            var info = PacketManager.GetInformation(typeof(P));

            List<Action<byte[]>> handlers = null;
            if (this.handlers.TryGetValue(info.Id, out handlers))
            {
                
            }
            else
            {
                var list = new List<Action<byte[]>>();
                list.Add(new Action<byte[]>((data) =>
                {
                    P packet = info.Constructor() as P;
                    if (packet != null)
                    {
                        packet.Read(data);
                    }
                    callback(packet);
                }));
                this.handlers.Add(info.Id, list);
            }
        }

        /// <summary>
        /// Packet handler.
        /// </summary>
        internal void Handle(int id, byte[] payload)
        {
            List<Action<byte[]>> handlers = null;
            if (this.handlers.TryGetValue(id, out handlers))
            {
                foreach (var handler in handlers)
                {
                    handler(payload);
                }
            }
            else
            {
                throw new Exception("Packet handlers not found for packet id #" + id);
            }
        }
    }

    /// <summary>
    /// Protocol manager.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NetServerProtocol<T>
        where T : NetConnection, new()
    {
        /// <summary>
        /// Packet handlers.
        /// </summary>
        private Dictionary<int, Action<T, byte[]>> handlers;

        /// <summary>
        /// Server client.
        /// </summary>
        public NetServer<T> Server
        {
            get;
            internal set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NetServerProtocol()
        {
            this.handlers = new Dictionary<int, Action<T, byte[]>>();
        }

        /// <summary>
        /// Registers a packet type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void Register<P>(Action<T, P> callback)
            where P : Packet, new()
        {
            var info = PacketManager.GetInformation(typeof(P));
            if (this.handlers.ContainsKey(info.Id))
            {
                throw new Exception("Packet handler already registered.");
            }
            this.handlers.Add(info.Id, new Action<T, byte[]>((p, data) =>
            {
                P packet = info.Constructor() as P;
                if (packet != null)
                {
                    packet.Read(data);
                }
                callback(p, packet);
            }));
        }

        /// <summary>
        /// Packet handler.
        /// </summary>
        internal void Handle(T connection, int id, byte[] payload)
        {
            Action<T, byte[]> handler = null;
            if (this.handlers.TryGetValue(id, out handler))
            {
                handler(connection, payload);
            }
            else
            {
                throw new Exception("Packet handler not found for packet id #" + id);
            }
        }
    }
}
