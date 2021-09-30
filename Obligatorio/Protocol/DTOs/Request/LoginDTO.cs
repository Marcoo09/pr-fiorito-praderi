using System;
using System.Text;
using Protocol.SerializationInterfaces;
using Server.Domain;

namespace DTOs.Request
{
    public class LoginDTO : ISerializable, IDeserializable
    {
        public string UserName { get; set; }

        public User ToEntity()
        {
            return new User()
            {
                Name = UserName
            };
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{UserName}"); ;
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("#");

            UserName = attributes[0];
        }
    }
}
