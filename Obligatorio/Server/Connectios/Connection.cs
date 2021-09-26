using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Exceptions;
using Protocol;

namespace Server.Connections
{
    public class Connection
    {
        private TcpClient _tcpClient;
        private ProtocolHandler _protocolHandler;
        //private IServiceRouter _serviceRouter;
        private ConnectionsState _connectionState;
        private Object _connectionStateLocker;

        protected Connection() { }
        public Connection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _protocolHandler = new ProtocolHandler(_tcpClient);
            //_serviceRouter = new ServiceRouter();
            _connectionState = ConnectionsState.Down;
            _connectionStateLocker = new Object();
        }

        public void StartConnection()
        {
            _connectionState = ConnectionsState.Up;

            while (ConnectionIsUp())
            {
                HandleRequests();
            }
        }

        public void ShutDown()
        {
            _tcpClient.Close();

            lock (_connectionStateLocker)
            {
                _connectionState = ConnectionsState.Down;
            }
        }

        private void HandleRequests()
        {
            try
            {
                Frame receivedFrame = _protocolHandler.Receive();
                //Frame responseFrame = _serviceRouter.GetResponse(receivedFrame);

                //_protocolHandler.Send(responseFrame);
            }
            catch (ProtocolException)
            {
                Console.WriteLine("Client has disconnected");
                ShutDown();
            }
            catch (IOException)
            {
                Console.WriteLine("Connection shut down by the server");
            }
            catch (ObjectDisposedException) { }
        }

        public bool ConnectionIsUp()
        {
            lock (_connectionStateLocker)
            {
                return _connectionState == ConnectionsState.Up;
            }
        }
    }
}