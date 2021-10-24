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
            // Thread connectionsThread = new Thread(async () => await HandleConnections());
            //connectionsThread.Start();
            try
            {
                await HandleConnections();
            }
            catch {

                throw new Exception("Server is down! Please try again");
                Console.WriteLine("Server is down! Please try again");
            }
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
