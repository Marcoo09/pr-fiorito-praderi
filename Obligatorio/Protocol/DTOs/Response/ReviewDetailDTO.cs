using System;
using System.Text;
using Server.Domain;
using Protocol.SerializationInterfaces;

namespace DTOs.Response
{
    public class ReviewDetailDTO : ISerializable, IDeserializable
    {
        public int Rating { get; set; }
        public string Description { get; set; }

        public ReviewDetailDTO()
        {
        }

        public ReviewDetailDTO(Review review)
        {
            Rating = review.Rating;
            Description = review.Description;
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("#");

            Rating = Int32.Parse(attributes[0]);
            Description = attributes[1];
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Rating}#{Description}");
        }

        public override string ToString()
        {
            return $"Review:\n\tRating: {Rating}\n\tDescription: {Description}";
        }
    }
}
