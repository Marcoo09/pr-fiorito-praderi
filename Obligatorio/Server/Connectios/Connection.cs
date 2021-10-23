using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
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
        private State _connectionState;
        private SemaphoreSlim _connectionStateSemaphore;
        private IUserRepository _userRepository;

        public Connection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _protocolHandler = new ProtocolHandler(_tcpClient);
            _serviceRouter = new ServiceRouter();
            _connectionState = State.Down;
            _connectionStateSemaphore = new SemaphoreSlim(1);
            _userRepository = UserRepository.GetInstanceAsync();
        }

        protected Connection() { }

        public async Task StartConnectionAsync()
        {
            _connectionState = State.Up;

            while (ConnectionIsUp())
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

        public bool ConnectionIsUp()
        {
            
             return _connectionState == State.Up;
            
        }

        public async Task ShutDownAsync()
        {
            _tcpClient.Close();

            await _connectionStateSemaphore.WaitAsync();
            _connectionState = State.Down;
            _connectionStateSemaphore.Release();
        }
    }
}