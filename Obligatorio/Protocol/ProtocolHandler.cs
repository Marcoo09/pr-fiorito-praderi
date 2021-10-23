using System;
using System.Net.Sockets;
using System.Threading.Tasks;
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

        public async Task SendAsync(Frame frame)
        {
            await SendBytesInChunksAsync(BitConverter.GetBytes(frame.ChosenHeader), 1);
            await SendBytesInChunksAsync(BitConverter.GetBytes(frame.ChosenCommand), 1);

            if (frame.IsResponseFrame())
            {
                await SendBytesInChunksAsync(BitConverter.GetBytes(frame.ResultStatus), 1);
            }

            await SendBytesInChunksAsync(BitConverter.GetBytes(frame.DataLength), 1);
            await SendBytesInChunksAsync(frame.Data, frame.GetDataChunks());
        }

        public async Task<Frame> ReceiveAsync()
        {
            Frame frame = new Frame();

            frame.ChosenHeader = BitConverter.ToInt16(await ReceiveBytesInChunksAsync(2, 1));
            frame.ChosenCommand = BitConverter.ToInt16(await ReceiveBytesInChunksAsync(2, 1));

            if (frame.IsResponseFrame())
            {
                frame.ResultStatus = BitConverter.ToInt16(await ReceiveBytesInChunksAsync (2, 1));
            }

            frame.DataLength = BitConverter.ToInt32(await ReceiveBytesInChunksAsync(4, 1));
            frame.Data = await ReceiveBytesInChunksAsync(frame.DataLength, frame.GetDataChunks());

            return frame;
        }

        private async Task SendBytesInChunksAsync(byte[] data, int chunks)
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
                    await stream.WriteAsync(dataToSend);
                    offset += ProtocolConstants.MaxPacketSize;
                }
                else
                {
                    int dataLeftSize = data.Length - offset;
                    byte[] dataToSend = new byte[dataLeftSize];
                    Array.Copy(data, offset, dataToSend, 0, dataLeftSize);
                    await stream.WriteAsync(dataToSend);
                    offset += dataLeftSize;
                }
                currentChunk++;
            }
        }

        private async Task<byte[]> ReceiveBytesInChunksAsync(int fileLength, int chunks)
        {
            NetworkStream stream = _tcpClient.GetStream();
            byte[] buffer = new byte[fileLength];
            var offset = 0;
            var currentChunk = 1;

            while (offset < fileLength)
            {
                if (currentChunk < chunks)
                {
                    byte[] receivedBytes = await ReadAsync(ProtocolConstants.MaxPacketSize, stream);
                    Array.Copy(receivedBytes, 0, buffer, offset, ProtocolConstants.MaxPacketSize);
                    offset += ProtocolConstants.MaxPacketSize;
                }
                else
                {
                    int dataLeft = fileLength - offset;
                    byte[] receivedBytes = await ReadAsync(dataLeft, stream);
                    Array.Copy(receivedBytes, 0, buffer, offset, dataLeft);
                    offset += dataLeft;
                }
                currentChunk++;
            }
            return buffer;
        }

        private async Task<byte[]> ReadAsync(int length, NetworkStream stream)
        {
            int dataReceived = 0;
            var data = new byte[length];
            while (dataReceived < length)
            {
                var received = await stream.ReadAsync(data, dataReceived, length - dataReceived);
                if (received == 0)
                    throw new ProtocolException();

                dataReceived += received;
            }
            return data;
        }
    }
}