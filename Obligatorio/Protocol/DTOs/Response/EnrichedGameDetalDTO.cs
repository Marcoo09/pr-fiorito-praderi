using System;
using System.Linq;
using System.Text;
using Protocol.SerializationInterfaces;
using Server.Domain;

namespace DTOs.Response
{
    public class EnrichedGameDetailDTO : ISerializable, IDeserializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Gender { get; set; }
        public string Path { get; set; }
        public double RatingAverage { get; set; }

        public EnrichedGameDetailDTO()
        {
        }

        public EnrichedGameDetailDTO(Game game)
        {
            Id = game.Id;
            Title = game.Title;
            Synopsis = game.Synopsis;
            Gender = game.Gender;
            Path = game.Path;
            RatingAverage = game.Reviews.Count > 0 ? game.Reviews.Select(g => g.Rating).ToList().Average() : 0.0;
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("~~");

            Id = Int32.Parse(attributes[0]);
            Title = attributes[1];
            Synopsis = attributes[2];
            Gender = attributes[3];
            Path = attributes[4];
            RatingAverage = Double.Parse(attributes[5]);
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Id}~~{Title}~~{Synopsis}~~{Gender}~~{Path}~~{RatingAverage}");
        }

        public override string ToString()
        {
            return $"Id: {Id}\n\tTitle: {Title}\n\tSynopsis: {Synopsis}\n\tGender: {Gender}\n\tCover: {Path}\n\tAverage Rating: {RatingAverage}";
        }
    }
}
