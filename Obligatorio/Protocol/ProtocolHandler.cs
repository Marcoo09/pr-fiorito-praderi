using System;
using System.Net.Sockets;
using Exceptions;

namespace Protocol
{
    public class ProtocolHandler
    {
        private TcpClient _tcpClient;

        public ProtocolHandler(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
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
            NetworkStream stream = _tcpClient.GetStream();
            int offset = 0;
            int currentChunk = 1;

            while (offset < data.Length)
            {
                if (currentChunk < chunks)
                {
                    var dataToSend = new byte[ProtocolConstants.MaxPacketSize];
                    Array.Copy(data, offset, dataToSend, 0, ProtocolConstants.MaxPacketSize);
                    stream.Write(dataToSend);
                    offset += ProtocolConstants.MaxPacketSize;
                }
                else
                {
                    int dataLeftSize = data.Length - offset;
                    byte[] dataToSend = new byte[dataLeftSize];
                    Array.Copy(data, offset, dataToSend, 0, dataLeftSize);
                    stream.Write(dataToSend);
                    offset += dataLeftSize;
                }
                currentChunk++;
            }
        }

        private byte[] ReceiveBytesInChunks(int fileLength, int chunks)
        {
            NetworkStream stream = _tcpClient.GetStream();
            byte[] buffer = new byte[fileLength];
            var offset = 0;
            var currentChunk = 1;

            while (offset < fileLength)
            {
                if (currentChunk < chunks)
                {
                    byte[] receivedBytes = Read(ProtocolConstants.MaxPacketSize, stream);
                    Array.Copy(receivedBytes, 0, buffer, offset, ProtocolConstants.MaxPacketSize);
                    offset += ProtocolConstants.MaxPacketSize;
                }
                else
                {
                    int dataLeft = fileLength - offset;
                    byte[] receivedBytes = Read(dataLeft, stream);
                    Array.Copy(receivedBytes, 0, buffer, offset, dataLeft);
                    offset += dataLeft;
                }
                currentChunk++;
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