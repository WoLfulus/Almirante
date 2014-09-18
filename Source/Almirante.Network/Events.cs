using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Almirante.Network
{
    /// <summary>
    /// Error event arguments.
    /// </summary>
    public class ErrorEventArgs : EventArgs    
    {
        public Exception Error
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Disconnected event arguments.
    /// </summary>
    public class DisconnectedEventArgs : EventArgs
    {
    }

    /// <summary>
    /// Packet event.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Packet id.
        /// </summary>
        public int Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Packet buffer content.
        /// </summary>
        public byte[] Buffer
        {
            get;
            internal set;
        }
    }
}
