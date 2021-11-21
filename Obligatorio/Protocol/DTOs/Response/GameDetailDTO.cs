using System;
using System.Text;
using Protocol.SerializationInterfaces;
using Server.Domain;

namespace DTOs.Response
{
    public class GameDetailDTO : ISerializable, IDeserializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Gender { get; set; }
        public string Path { get; set; }

        //public ReviewDetailDTO Theme { get; set; } consultar si se agrega para filtar en los logs

        public GameDetailDTO()
        {
        }

        public GameDetailDTO(Game game)
        {
            Id = game.Id;
            Title = game.Title;
            Synopsis = game.Synopsis;
            Gender = game.Gender;
            Path = game.Path;
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("#");

            Id = Int32.Parse(attributes[0]);
            Title = attributes[1];
            Synopsis = attributes[2];
            Gender = attributes[3];
            Path = attributes[4];
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Id}#{Title}#{Synopsis}#{Gender}#{Path}");
        }

        public override string ToString()
        {
            return $"Id: {Id}\n\tTitle: {Title}\n\tSynopsis: {Synopsis}\n\tGender: {Gender}\n\tCover: {Path}";
        }
    }
}
