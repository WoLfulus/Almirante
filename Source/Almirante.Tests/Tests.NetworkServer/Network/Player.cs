using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Tests.NetworkServer.Network
{
    /// <summary>
    /// Player class.
    /// </summary>
    public class Player : NetConnection
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
        /// Position X
        /// </summary>
        public int X
        {
            get;
            set;
        }

        /// <summary>
        /// Position Y
        /// </summary>
        public int Y
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Player()
        {
            this.Name = null;
            this.X = 0;
            this.Y = 0;
        }

        /// <summary>
        /// Connected
        /// </summary>
        protected override void OnConnect()
        {
            Console.WriteLine("Player #" + this.Id + " connected.");
        }

        /// <summary>
        /// Disconnected
        /// </summary>
        protected override void OnDisconnect()
        {
            Console.WriteLine("Player #" + this.Id + " disconnected.");
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="error"></param>
        protected override void OnError(Exception error)
        {
            Console.WriteLine("Player #" + this.Id + " error: " + error.Message);
        }
    }
}
