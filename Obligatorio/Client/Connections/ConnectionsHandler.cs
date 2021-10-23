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
        private IPEndPoint _serverEndpoint;
        private ProtocolHandler _protocolHandler;
        private ClientState _clientState;
        private SemaphoreSlim _clientStateSemaphore;
        private IPAddress _serverIpAddress;
        private int _serverPort;

        private Socket _socket;

        public ConnectionsHandler()
        {
            _serverEndpoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]),
                    Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]));
            _clientState = ClientState.Down;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ClientIP"]), 0));
            _protocolHandler = new ProtocolHandler(_socket);

        }

        public async Task ConnectToServerAsync()
        {
            try
            {
                _socket.Connect(_serverEndpoint);
                _clientState = ClientState.Up;
                Console.WriteLine("Connected to server.");
            }
            catch (SocketException)
            {
                Console.WriteLine("Server is down! Please try again");
            }

        }

        public async Task ShutDownAsync()
        {
            _clientState = ClientState.ShuttingDown;
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
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
            catch (SocketException)
            {
                Console.WriteLine("Server is down! Please try again");
                await ShutDownAsync();
                return null;
            }
        }

    }
}
