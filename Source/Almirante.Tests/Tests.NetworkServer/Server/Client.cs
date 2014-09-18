using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Tests.NetworkServer.Server
{
    public class Client : Connection
    {
        /// <summary>
        /// Client name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Client()
        {
            this.Name = null;
        }

        protected override void OnConnect()
        {
            Console.WriteLine("Client #" + this.Id + " connected.");
        }

        protected override void OnDisconnect()
        {
            Console.WriteLine("Client #" + this.Id + " disconnected.");
        }

        protected override void OnError(Exception error)
        {
            Console.WriteLine("Client #" + this.Id + " error: " + error.Message);
        }
    }
}
