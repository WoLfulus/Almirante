using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.NetworkProtocol
{
    /// <summary>
    /// Packet Ids
    /// </summary>
    public static class Packets
    {
        public const int JoinRequest = 1;
        public const int JoinResponse = 2;
        public const int ServerMessage = 3;
        public const int MessageRequest = 4;
        public const int MessageResponse = 5;
    }
}
