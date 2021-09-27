using System;
using System.Text;
using Protocol.SerializationInterfaces;
using Server.Domain;

namespace DTOs.Response
{
    public class UserDetailDTO : ISerializable, IDeserializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserDetailDTO()
        {
        }

        public UserDetailDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("~~");
            Id = Int32.Parse(attributes[0]);
            Name = attributes[1];
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Id}~~{Name}");
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}";
        }
    }
}
