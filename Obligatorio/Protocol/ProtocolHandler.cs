using System;
using System.Net.Sockets;
using Exceptions;

namespace Protocol
{
    public class ProtocolHandler
    {
        private Socket _socket;

        public ProtocolHandler(Socket socket)
        {
            _socket = socket;
        }

        public void Send(Frame frame)
        {
            SendBytesInChunks(BitConverter.GetBytes(frame.ChosenHeader), 1);
            SendBytesInChunks(BitConverter.GetBytes(frame.ChosenCommand), 1);

            if (frame.IsResponseFrame())
            {
                SendBytesInChunks(BitConverter.GetBytes(frame.ResultStatus), 1);
            }

            SendBytesInChunks(BitConverter.GetBytes(frame.DataLength), 1);
            SendBytesInChunks(frame.Data, frame.GetDataChunks());
        }

        public Frame Receive()
        {
            Frame frame = new Frame();

            frame.ChosenHeader = BitConverter.ToInt16(ReceiveBytesInChunks(2, 1));
            frame.ChosenCommand = BitConverter.ToInt16(ReceiveBytesInChunks(2, 1));

            if (frame.IsResponseFrame())
            {
                frame.ResultStatus = BitConverter.ToInt16(ReceiveBytesInChunks(2, 1));
            }

            frame.DataLength = BitConverter.ToInt32(ReceiveBytesInChunks(4, 1));
            frame.Data = ReceiveBytesInChunks(frame.DataLength, frame.GetDataChunks());

            return frame;
        }

        private void SendBytesInChunks(byte[] data, int chunks)
        {
            var sentBytes = 0;
            while (sentBytes < data.Length)
            {
                sentBytes += _socket.Send(data, sentBytes, data.Length - sentBytes, SocketFlags.None);
            }

        }

        private byte[] ReceiveBytesInChunks(int fileLength, int chunks)
        {
            byte[] buffer = new byte[fileLength];

            var iRecv = 0;
            while (iRecv < fileLength)
            {
                try
                {
                    var localRecv = _socket.Receive(buffer, iRecv, fileLength - iRecv, SocketFlags.None);
                    if (localRecv == 0) // If receive 0, the connections was closed from remote
                    {
                        throw new ProtocolException();
                    }

                    iRecv += localRecv;
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.Message);
                }
            }
            return buffer;
        }

        private static byte[] Read(int length, NetworkStream stream)
        {
            int dataReceived = 0;
            var data = new byte[length];
            while (dataReceived < length)
            {
                var received = stream.Read(data, dataReceived, length - dataReceived);
                if (received == 0)
                    throw new ProtocolException();

                dataReceived += received;
            }
            return data;
        }
    }
}