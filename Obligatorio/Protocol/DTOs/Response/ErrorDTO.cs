using System.Text;
using Protocol.Serialization;

namespace DTOs.Response
{
    public class ErrorDTO : IDeserializable, ISerializable
    {
        public string Message { get; set; }

        public void Deserialize(byte[] serializedEntity)
        {
            Message = Encoding.UTF8.GetString(serializedEntity);
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Message}");
        }

        public override string ToString()
        {
            return $"Error: {Message}";
        }
    }
}