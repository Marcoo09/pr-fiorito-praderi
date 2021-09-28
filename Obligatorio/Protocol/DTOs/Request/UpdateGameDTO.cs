using Protocol.SerializationInterfaces;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTOs.Request
{
    public class UpdateGameDTO : ISerializable, IDeserializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Gender { get; set; }

        public Game ToEntity()
        {
            return new Game()
            {
                Id = Id,
                Title = Title,
                Synopsis = Synopsis,
                Gender = Gender,
            };
        }


        public void Deserialize(byte[] serializedEntity)
        {

            string[] attributes = Encoding.UTF8.GetString(serializedEntity).Split("~~");

            Id = Int32.Parse(attributes[0]);
            Title = attributes[1];
            Synopsis = attributes[2];
            Gender = attributes[3];
        }


        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Id}~~{Title}~~{Synopsis}~~{Gender}");
        }
    }
}