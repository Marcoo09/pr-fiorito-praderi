using System.Text;
using Protocol.SerializationInterfaces;

namespace DTOs.Response
{
    public class MessageDTO : ISerializable, IDeserializable
    {
        public string Message { get; set; }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Message}");
        }

        public void Deserialize(byte[] entity)
        {
            Message = Encoding.UTF8.GetString(entity);
        }

        public override string ToString()
        {
            return $"{Message}";
        }
    }
}