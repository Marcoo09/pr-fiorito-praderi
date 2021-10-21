using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Connections
{
    public class ConnectionsHandler
    {

        private Socket _socketServer;

        private IPAddress _serverIp;
        private int _serverPort;
        //private TcpListener _tcpListener;
        private List<Connection> _connections;
        private State _serverState;
        private Object _serverStateLocker;
        private Object _connectionsListLocker;


        public ConnectionsHandler()
        {
            _serverStateLocker = new Object();
            _connectionsListLocker = new Object();
            _connections = new List<Connection>();
            _serverIp = IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]);
            _serverPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            _socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socketServer.Bind(new IPEndPoint(_serverIp, _serverPort));

            //_tcpListener = new TcpListener(_serverIp, _serverPort);

            _serverState = State.Down;
        }

        public void StartListening()
        {

            _socketServer.Listen(100);
            //_tcpListener.Start(20);
            _serverState = State.Up;

            while (IsServerUp())
            {
                try
                {
                    Connection clientConnection = new Connection(_socketServer.Accept());
                    Thread clientThread = new Thread(() => clientConnection.StartConnection());

                    AddConnection(clientConnection);
                    clientThread.Start();
                    Console.WriteLine("Client accepted");
                }
                catch (SocketException)
                {
                    ShutDownConnections();
                }
            }
        }

        public void StartShutDown()
        {
            lock (_serverStateLocker)
            {
                _serverState = State.ShuttingDown;
                //_tcpListener.Stop();
                _socketServer.Close(0);
            }
        }

        private void ShutDownConnections()
        {
            lock (_serverStateLocker)
            {
                _serverState = State.Down;

                lock (_connectionsListLocker)
                {
                    for (int i = _connections.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            Connection connection = _connections.ElementAt(i);
                            connection.ShutDown();
                            _connections.RemoveAt(i);
                        }
                        catch (ObjectDisposedException) { }
                    }
                }
            }
        }

        private bool IsServerUp()
        {
            lock (_serverStateLocker)
            {
                return _serverState == State.Up;
            }
        }

        private void AddConnection(Connection connection)
        {
            lock (_connectionsListLocker)
            {
                _connections.Add(connection);
            }
        }
    }
}