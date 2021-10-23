using System;
using System.Threading;

namespace Client
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            ClientUI clientUI = new ClientUI();
            Thread thread = new Thread(() => clientUI.InitAsync());
            thread.Start();
        }
    }
}
