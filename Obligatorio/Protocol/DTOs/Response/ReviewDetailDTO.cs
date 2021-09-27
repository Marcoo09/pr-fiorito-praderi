using System;
using System.Text;
using Server.Domain;
using Protocol.SerializationInterfaces;

namespace DTOs.Response
{
    public class ReviewDetailDTO : ISerializable, IDeserializable
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }

        public ReviewDetailDTO()
        {
        }

        public ReviewDetailDTO(Review review)
        {
            Id = review.Id;
            Rating = review.Rating;
            Description = review.Description;
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("~~");

            Id = Int32.Parse(attributes[0]);
            Rating = Int32.Parse(attributes[1]);
            Description = attributes[2];
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Id}~~{Rating}~~{Description}");
        }

        public override string ToString()
        {
            return $"Id: {Id}\n\tRating: {Rating}\n\tDescription: {Description}";
        }
    }
}
