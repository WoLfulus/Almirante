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
}
