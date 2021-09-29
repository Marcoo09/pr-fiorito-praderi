using System;
using System.Text;
using Protocol.SerializationInterfaces;

namespace DTOs.Request
{
    public class BasicGameRequestDTO : ISerializable, IDeserializable
    {
        public int GameId { get; set; }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{GameId}"); ;
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("#");

            GameId = Int32.Parse(attributes[0]);
        }
    }
}
