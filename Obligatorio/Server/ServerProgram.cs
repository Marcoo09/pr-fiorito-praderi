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

            var task = Task.Run(async () => await connectionsHandler.StartListeningAsync());
            Console.WriteLine("Write any key to shutdown the server");

            Console.ReadLine();
            
            await connectionsHandler.StartShutDownAsync();
        }
    }
}
