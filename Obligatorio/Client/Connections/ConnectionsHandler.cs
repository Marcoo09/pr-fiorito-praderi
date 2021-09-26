﻿using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Protocol;

namespace Client.Connections
{
    public class ConnectionsHandler
    {
        private TcpClient _tcpClient;
        private IPEndPoint _serverEndpoint;
        private ProtocolHandler _protocolHandler;
        private ClientState _clientState;

        public ConnectionsHandler()
        {
            _serverEndpoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]),
                    Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]));
            _tcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ClientIP"]), 0));
            _protocolHandler = new ProtocolHandler(_tcpClient);
            _clientState = ClientState.Down;
        }

        public void ConnectToServer()
        {
            _tcpClient.Connect(_serverEndpoint);
            _clientState = ClientState.Up;
        }

        public void ShutDown()
        {
            _clientState = ClientState.ShuttingDown;
            _tcpClient.Close();
            _clientState = ClientState.Down;
        }

        public bool IsClientStateUp()
        {
            return _clientState == ClientState.Up;
        }

        public Frame SendRequest(Frame requestFrame)
        {
            try
            {
                _protocolHandler.Send(requestFrame);
                Frame response = _protocolHandler.Receive();
                return response;
            }
            catch (IOException)
            {
                Console.WriteLine("Server is down! Please try again");
                ShutDown();
                return null;
            }
        }

    }
}
