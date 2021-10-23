using System;
using System.Threading;
using System.Threading.Tasks;
using Server.Connections;

namespace Server
{
    class ServerProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            Thread connectionsThread = new Thread(async () => await HandleConnections());
            connectionsThread.Start();
        }

        static async Task HandleConnections()
        {
            ConnectionsHandler connectionsHandler = new ConnectionsHandler();
            Thread listiningThread = new Thread( async () => await connectionsHandler.StartListeningAsync());
            listiningThread.Start();

            Console.WriteLine("Write any key to shutdown the server");
            Console.ReadLine();
            await connectionsHandler.StartShutDownAsync();
        }
    }
}
