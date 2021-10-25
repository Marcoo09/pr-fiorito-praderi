using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Connections
{
    public class ConnectionsHandler
    {

        private Socket _socketServer;
        private IPAddress _serverIp;
        private int _serverPort;
        private List<Connection> _connections;
        private State _serverState;
        private SemaphoreSlim _connectionsListSemaphore;
        private SemaphoreSlim _serverStateSemaphore;
        private bool _isShuttingDown = false;


        public ConnectionsHandler()
        {

            _serverStateSemaphore = new SemaphoreSlim(1);
            _connectionsListSemaphore = new SemaphoreSlim(1);
            _connections = new List<Connection>();
            _serverIp = IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]);
            _serverPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);
            _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socketServer.Bind(new IPEndPoint(_serverIp, _serverPort));
            _serverState = State.Down;
        }

        public async Task StartListeningAsync()
        {

            _socketServer.Listen(100);
            await _serverStateSemaphore.WaitAsync();
            _serverState = State.Up;
            _serverStateSemaphore.Release();

            while (await IsServerUp())
            {
                try
                {
                    Connection clientConnection = new Connection(_socketServer.Accept());
                    Task clientTask = new Task(async () => await clientConnection.StartConnectionAsync());

                    await AddConnectionAsync(clientConnection);
                    clientTask.Start();

                    if (!_isShuttingDown)
                    {
                        Console.WriteLine("Client accepted");
                    }
                }
                catch (Exception)
                {
                    await ShutDownConnectionsAsync();
                }
            }
            Console.WriteLine("Exiting....");
        }

        public async Task StartShutDownAsync()
        {
            await _serverStateSemaphore.WaitAsync();
             _serverState = State.ShuttingDown;
             _socketServer.Close(0);
            _serverStateSemaphore.Release();

            await ShutDownConnectionsAsync();

            _isShuttingDown = true;

            var fakeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            fakeSocket.Connect(_serverIp, _serverPort);
        }

        private async Task ShutDownConnectionsAsync()
        {
            await _serverStateSemaphore.WaitAsync();
            _serverState = State.Down;
            _serverStateSemaphore.Release();

            await _connectionsListSemaphore.WaitAsync();
            for (int i = _connections.Count - 1; i >= 0; i--)
            {
                try
                {
                    Connection connection = _connections.ElementAt(i);
                    await connection.ShutDownAsync();
                    _connections.RemoveAt(i);
                }
                catch (ObjectDisposedException) { }
            }
            _connectionsListSemaphore.Release();

        }

        private async Task<bool> IsServerUp()
        {
            bool isServerUp;
            await _serverStateSemaphore.WaitAsync();
            isServerUp = _serverState == State.Up;
            _serverStateSemaphore.Release();
            return isServerUp;
        }

        private async Task AddConnectionAsync(Connection connection)
        {

            await _connectionsListSemaphore.WaitAsync();
            _connections.Add(connection);
            _connectionsListSemaphore.Release();

        }
    }
}