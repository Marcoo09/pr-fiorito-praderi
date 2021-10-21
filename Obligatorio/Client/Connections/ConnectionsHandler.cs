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
        private IPEndPoint _serverEndpoint;
        private ProtocolHandler _protocolHandler;
        private ClientState _clientState;

        private Socket _socket;

        public ConnectionsHandler()
        {
            _serverEndpoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ServerIP"]),
                    Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]));
            _clientState = ClientState.Down;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["ClientIP"]), 0));
            _protocolHandler = new ProtocolHandler(_socket);

        }

        public void ConnectToServer()
        {
            try
            {
                _socket.Connect(_serverEndpoint);
                _clientState = ClientState.Up;
                Console.WriteLine("Connected to server.");
            }
            catch (SocketException)
            {
                Console.WriteLine("Server is down! Please try again");
            }

        }

        public void ShutDown()
        {
            _clientState = ClientState.ShuttingDown;
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
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
            catch (SocketException)
            {
                Console.WriteLine("Server is down! Please try again");
                ShutDown();
                return null;
            }
        }

    }
}
