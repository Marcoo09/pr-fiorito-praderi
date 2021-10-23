using System;
using System.Net.Sockets;
using System.Threading.Tasks;
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
            var sentBytes = 0;
            while (sentBytes < data.Length)
            {
                sentBytes += _socket.Send(data, sentBytes, data.Length - sentBytes, SocketFlags.None);
            }

        }

        private async Task<byte[]> ReceiveBytesInChunksAsync(int fileLength, int chunks)
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