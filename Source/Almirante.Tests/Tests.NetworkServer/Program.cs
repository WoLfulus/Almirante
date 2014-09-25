using Almirante.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tests.NetworkProtocol;
using Tests.NetworkServer.Network;

namespace Tests.NetworkServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates the server
            Server server = new Server();

            // Starts the server
            server.Start(8000);

            while (Console.ReadLine() != "quit");

            // Stops the server
            server.Stop();
        }
    }
}
