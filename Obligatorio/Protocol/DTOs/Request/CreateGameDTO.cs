using System;
using System.Text;
using Server.Domain;

namespace DTOs.Request
{
    public class CreateGameDTO
    {
        public string Title { get; set; }
        public string Synopsis { get; set; }
        //public string Image { get; set; }

        public Game ToEntity()
        {
            return new Game()
            {
                Title = Title,
                Synopsis = Synopsis
            };
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Title}~~{Synopsis}");
        }

        public void Deserialize(byte[] serializedEntity)
        {
            string[] attributes = Encoding.UTF8.GetString(serializedEntity).Split("~~");

            Title = attributes[0];
            Synopsis = attributes[1];
        }
    }
}
