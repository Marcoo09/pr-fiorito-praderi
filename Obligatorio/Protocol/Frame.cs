namespace Protocol
{
    public class Frame
    {
        public short ChosenHeader { get; set; }
        public short ResultStatus { get; set; }
        public short ChosenCommand { get; set; }
        public byte[] Data { get; set; }
        public int DataLength { get; set; }

        public Frame()
        {
            DataLength = 0;
            Data = new byte[0];
        }

        public bool IsResponseFrame()
        {
            return ChosenHeader == (short) Header.Response;
        }

        public bool IsSuccessful()
        {
            return ResultStatus == (short) Status.Ok;
        }

        public int GetDataChunks()
        {
            long chunks = DataLength / ProtocolConstants.MaxPacketSize;
            return DataLength % ProtocolConstants.MaxPacketSize == 0 ? (int)chunks : (int)chunks + 1;
        }
    }
}
