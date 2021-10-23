using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class ClientProgram
    {
        static async Task Main(string[] args)
        {
            ClientUI clientUI = new ClientUI();
            Thread thread = new Thread(async () => await clientUI.InitAsync());
            thread.Start();
        }
    }
}
