﻿using Almirante.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.NetworkProtocol
{
    [Packet(Packets.MessageRequest)]
    public class MessageRequest : Packet
    {
        /// <summary>
        /// Chat message.
        /// </summary>
        [Field(1)]
        public string Message
        {
            get;
            set;
        }
    }
}
