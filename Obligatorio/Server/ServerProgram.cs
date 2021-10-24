using System;
using System.Threading;
using System.Threading.Tasks;
using Server.Connections;

namespace Server
{
    class ServerProgram
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");

            await HandleConnections();
        }

        static async Task HandleConnections()
        {
            ConnectionsHandler connectionsHandler = new ConnectionsHandler();
            Console.WriteLine("Write any key to shutdown the server");

            await connectionsHandler.StartListeningAsync();

            Console.ReadLine();
            
            await connectionsHandler.StartShutDownAsync();
        }
    }
}
