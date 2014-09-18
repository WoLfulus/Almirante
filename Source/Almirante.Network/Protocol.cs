using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Almirante.Network
{
    public class Protocol<T>
    {
        /// <summary>
        /// Packet handlers.
        /// </summary>
        private Dictionary<int, Action<T, object>> handlers;

        /// <summary>
        /// Constructor
        /// </summary>
        public Protocol()
        {
            this.handlers = new Dictionary<int, Action<T, object>>();
        }

        /// <summary>
        /// Registers a packet type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void Register<P>(Action<T, P> callback)
            where P : new()
        {
            var info = PacketManager.GetInformation(typeof(P));
            if (this.handlers.ContainsKey(info.Id))
            {
                throw new Exception("Packet handler already registered.");
            }
            this.handlers.Add(info.Id, new Action<T, object>((p, o) =>
            {
                callback(p, (P) o);
            }));
        }
    }
}
