using System;
using Protocol.SerializationInterfaces;
using System.Text;
using Server.Domain;

namespace DTOs.Request
{
    public class ReviewDTO : ISerializable, IDeserializable
    {
        public int GameId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }

        public Review ToEntity()
        {
            return new Review()
            {
                Rating = Rating,
                Description = Description
            };
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("~~");

            GameId = Int32.Parse(attributes[0]);
            Rating = Int32.Parse(attributes[1]);
            Description = attributes[2];
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{GameId}~~{Rating}~~{Description}");
        }
    }
}
