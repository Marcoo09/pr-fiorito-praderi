using System;
using System.Threading;
using Server.Connections;

namespace Server
{
    class ServerProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            Thread connectionsThread = new Thread(() => HandleConnections());
            connectionsThread.Start();
        }

        static void HandleConnections()
        {
            ConnectionsHandler connectionsHandler = new ConnectionsHandler();
            Thread listiningThread = new Thread(() => connectionsHandler.StartListening());
            listiningThread.Start();

            Console.WriteLine("Press a key to stop the server");
            Console.ReadLine();
            connectionsHandler.StartShutDown();
        }
    }
}
