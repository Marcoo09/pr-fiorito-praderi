using System;
using System.Text;
using Protocol.SerializationInterfaces;
using Server.Domain;

namespace DTOs.Response
{
    public class GameBasicInfoDTO: ISerializable, IDeserializable
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public GameBasicInfoDTO()
        {
        }

        public GameBasicInfoDTO(Game game)
        {
            Id = game.Id;
            Title = game.Title;
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Id}~~{Title}");
        }

        public void Deserialize(byte[] entity)
        {
            string[] attributes = Encoding.UTF8.GetString(entity).Split("~~");

            Id = Int32.Parse(attributes[0]);
            Title = attributes[1];
        }
    }
}
