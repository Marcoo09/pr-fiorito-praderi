using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
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
        private SemaphoreSlim _connectionStateSemaphore;
        private IUserRepository _userRepository;

        public Connection(Socket socket)
        {
            _socket = socket;
            _protocolHandler = new ProtocolHandler(_socket);
            _serviceRouter = new ServiceRouter();
            _connectionState = State.Down;
            _connectionStateSemaphore = new SemaphoreSlim(1);
            _userRepository = UserRepository.GetInstance();
        }

        protected Connection() { }

        public async Task StartConnectionAsync()
        {
            await _connectionStateSemaphore.WaitAsync();
            _connectionState = State.Up;
            _connectionStateSemaphore.Release();

            while (await ConnectionIsUp())
            {
                await HandleRequestsAsync();
            }
        }

        private async Task HandleRequestsAsync()
        {
            try
            {
                Frame receivedFrame = await _protocolHandler.ReceiveAsync();
                Frame responseFrame = await _serviceRouter.GetResponseAsync(receivedFrame);

                await _protocolHandler.SendAsync(responseFrame);
            }
            catch (ProtocolException)
            {
                Console.WriteLine("Client has disconnected");
                await ShutDownAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine("Connection shut down by the server");
                Console.WriteLine(e.ToString());
            }
            catch (ObjectDisposedException) { }
        }

        public async Task<bool> ConnectionIsUp()
        {
            bool isConnectionUp;
            await _connectionStateSemaphore.WaitAsync();
            isConnectionUp = _connectionState == State.Up;
            _connectionStateSemaphore.Release();
            return isConnectionUp;
        }

        public async Task ShutDownAsync()
        {
            try
            {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            }
            catch (ObjectDisposedException) { }

            await _connectionStateSemaphore.WaitAsync();
            _connectionState = State.Down;
            _connectionStateSemaphore.Release();
        }
    }
}