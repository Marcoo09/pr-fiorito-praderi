using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Protocol;

namespace Client.Connections
{
    public class ConnectionsHandler
    {
        private TcpClient _tcpClient;
        private IPEndPoint _serverEndpoint;
        private ProtocolHandler _protocolHandler;
        private ClientState _clientState;
        private SemaphoreSlim _clientStateSemaphore;
        private IPAddress _serverIpAddress;
        private int _serverPort;

        public ConnectionsHandler()
        {
            //_serverEndpoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]),
            _serverPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);
            _tcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ClientIP"]), 0));
            _protocolHandler = new ProtocolHandler(_tcpClient);
            _clientState = ClientState.Down;
            _clientStateSemaphore = new SemaphoreSlim(1);
            _serverIpAddress = IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]);
        }

        public async Task ConnectToServerAsync()
        {
            await _tcpClient.ConnectAsync(_serverIpAddress, _serverPort);
            await _clientStateSemaphore.WaitAsync();

            _clientState = ClientState.Up;
            _clientStateSemaphore.Release();
        }

        public async Task ShutDownAsync()
        {
            _clientState = ClientState.ShuttingDown;
            _tcpClient.Close();
            _clientState = ClientState.Down;

            _clientStateSemaphore.Release();
        }

        public bool IsClientStateUp()
        {
            return _clientState == ClientState.Up;
        }

        public async Task<Frame> SendRequestAsync(Frame requestFrame)
        {
            try
            {
                await _protocolHandler.SendAsync(requestFrame);
                Frame response = await _protocolHandler.ReceiveAsync();
                return response;
            }
            catch (IOException)
            {
                Console.WriteLine("Server is down! Please try again");
                await ShutDownAsync();
                return null;
            }
        }

    }
}
