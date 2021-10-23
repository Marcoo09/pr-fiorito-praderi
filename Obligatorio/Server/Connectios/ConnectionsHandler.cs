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
        private IPAddress _serverIp;
        private int _serverPort;
        private TcpListener _tcpListener;
        private List<Connection> _connections;
        private State _serverState;
        private SemaphoreSlim _connectionsListSemaphore;
        private SemaphoreSlim _serverStateSemaphore;


        public ConnectionsHandler()
        {

            _serverStateSemaphore = new SemaphoreSlim(1);
            _connectionsListSemaphore = new SemaphoreSlim(1);
            _connections = new List<Connection>();
            _serverIp = IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]);
            _serverPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);
            _tcpListener = new TcpListener(_serverIp, _serverPort);
            _serverState = State.Down;
        }

        public async Task StartListeningAsync()
        {
            _tcpListener.Start(20);
            _serverState = State.Up;

            while (IsServerUp())
            {
                try
                {
                    Connection clientConnection = new Connection(await _tcpListener.AcceptTcpClientAsync());
                    clientConnection.StartConnectionAsync();

                    await AddConnectionAsync(clientConnection);
                    Console.WriteLine("Client accepted");
                }
                catch (SocketException)
                {
                    await ShutDownConnectionsAsync();
                }
            }
        }

        public async Task StartShutDownAsync()
        {

            await _serverStateSemaphore.WaitAsync();
            _serverState = State.ShuttingDown;
            _tcpListener.Stop();
            _serverStateSemaphore.Release();
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

        private bool IsServerUp()
        {
                return _serverState == State.Up;
        }

        private async Task AddConnectionAsync(Connection connection)
        {

            await _connectionsListSemaphore.WaitAsync();
            _connections.Add(connection);
            _connectionsListSemaphore.Release();

        }
    }
}