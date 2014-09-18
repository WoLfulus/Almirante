using Almirante.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tests.NetworkProtocol;
using Tests.NetworkServer.Server;

namespace Tests.NetworkServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates an instance of protocol handler
            ServerProtocol protocol = new ServerProtocol();

            // Creates the server
            Server<Player> server = new Server<Player>(protocol, 100);

            // Register start event
            server.Started += (e, a) =>
            {
                Console.WriteLine("Server started.");
            };

            // Register stop event
            server.Stopped += (e, a) =>
            {
                Console.WriteLine("Server stopped.");
            };

            // Register error event
            server.Error += (e, a) =>
            {
                Console.WriteLine("Server error: " + a.Error.Message);
            };

            // Starts the server
            server.Start(8000);

            while (Console.ReadLine() != "quit");

            // Stops the server
            server.Stop();
        }
    }
}
