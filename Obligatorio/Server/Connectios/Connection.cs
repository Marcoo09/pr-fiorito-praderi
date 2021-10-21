using System;
using System.IO;
using System.Net.Sockets;
using Exceptions;
using Protocol;
using Server.DataAccess.Implementations;
using Server.DataAccess.Interfaces;
using Server.Implementations;
using Server.Interfaces;

namespace Server.Connections
{
    public class Connection
    {

        private Socket _socket;
        private ProtocolHandler _protocolHandler;
        private IServiceRouter _serviceRouter;
        private State _connectionState;
        private Object _connectionStateLocker;
        private IUserRepository _userRepository;

        public Connection(Socket socket)
        {
            _socket = socket;
            _protocolHandler = new ProtocolHandler(_socket);
            _serviceRouter = new ServiceRouter();
            _connectionState = State.Down;
            _connectionStateLocker = new Object();
            _userRepository = UserRepository.GetInstance();
        }

        protected Connection() { }

        public void StartConnection()
        {
            _connectionState = State.Up;

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
            }
            catch (IOException e)
            {
                Console.WriteLine("Connection shut down by the server");
                Console.WriteLine(e.ToString());
            }
            catch (ObjectDisposedException) { }
        }

        public bool ConnectionIsUp()
        {
            lock (_connectionStateLocker)
            {
                return _connectionState == State.Up;
            }
        }

        public void ShutDown()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();

            lock (_connectionStateLocker)
            {
                _connectionState = State.Down;
            }
        }
    }
}