using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using Exceptions;
//using Protocol;

namespace Client.Connections
{
    public class ConnectionsHandler
    {
        private TcpClient _tcpClient;
        private IPEndPoint _serverEndpoint;
        //private ProtocolHandler _protocolHandler;
        private ClientState _clientState;
        private IPAddress _serverIpAddress;
        private int _serverPort;
        //private SemaphoreSlim _clientStateSemaphore;

        public ConnectionsHandler()
        {
            _serverIpAddress = IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]);
            _serverPort = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);
            _tcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ClientIP"]), 0));
            //_protocolHandler = new ProtocolHandler(_tcpClient);
            _clientState = ClientState.Down;
            //_clientStateSemaphore = new SemaphoreSlim(1);
        }
    }
}
