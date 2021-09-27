using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Exceptions;
using Protocol;
using Server.DataAccess.Implementations;
using Server.DataAccess.Interfaces;
using Server.Domain;
using Server.Implementations;
using Server.Interfaces;

namespace Server.Connections
{
    public class Connection
    {
        private TcpClient _tcpClient;
        private ProtocolHandler _protocolHandler;
        private IServiceRouter _serviceRouter;
        private ConnectionsState _connectionState;
        private Object _connectionStateLocker;
        private User _user;
        private IUserRepository _userRepository;

        public Connection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _protocolHandler = new ProtocolHandler(_tcpClient);
            _serviceRouter = new ServiceRouter();
            _connectionState = ConnectionsState.Down;
            _connectionStateLocker = new Object();
            _userRepository = UserRepository.GetInstance();
        }

        protected Connection() { }

        public void StartConnection()
        {
            _connectionState = ConnectionsState.Up;

            IPEndPoint endpoint = (IPEndPoint)_tcpClient.Client.RemoteEndPoint;
            _user = new User()
            {
                Name = endpoint.Address.ToString(),
            };
            int userId = _userRepository.Insert(_user);
            _user = _userRepository.Get(userId);

            while (ConnectionIsUp())
            {
                HandleRequests();
            }
        }

        private void HandleRequests()
        {
            try
            {
                Frame receivedFrame = _protocolHandler.Receive();
                Frame responseFrame = _serviceRouter.GetResponse(receivedFrame);

                _protocolHandler.Send(responseFrame);
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

        public void ShutDown()
        {
            try
            {
                _userRepository.Delete(_user.Id);
            }
            catch (ResourceNotFoundException) { }
            _tcpClient.Close();

            lock (_connectionStateLocker)
            {
                _connectionState = ConnectionsState.Down;
            }
        }
    }
}