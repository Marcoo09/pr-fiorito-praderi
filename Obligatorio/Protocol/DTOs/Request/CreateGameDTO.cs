using System.Text;
using Server.Domain;

namespace DTOs.Request
{
    public class CreateGameDTO
    {
        public string Title { get; set; }
        public string Gender { get; set; }
        public string Synopsis { get; set; }

        public Game ToEntity()
        {
            return new Game()
            {
                Title = Title,
                Synopsis = Synopsis,
                Gender = Gender
            };
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes($"{Title}~~{Synopsis}~~{Gender}");
        }

        public void Deserialize(byte[] serializedEntity)
        {
            string[] attributes = Encoding.UTF8.GetString(serializedEntity).Split("~~");

            Title = attributes[0];
            Synopsis = attributes[1];
            Gender = attributes[2];
        }
    }
}
