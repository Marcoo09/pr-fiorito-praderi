using System.Threading.Tasks;

namespace Client
{
    class ClientProgram
    {
        static async Task Main(string[] args)
        {
            ClientUI clientUI = new ClientUI();
            await clientUI.InitAsync();
        }
    }
}
