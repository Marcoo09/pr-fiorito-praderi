using Exceptions;

namespace Server.Domain
{
    public class Game
    {
        public string Title { get; set; }
        public string Synopsis { get; set; }

        public Game()
        {
        }

        public void ValidOrFail()
        {
            if (Title == null)
                throw new InvalidResourceException("Game must have a title");
            if (Synopsis == null)
                throw new InvalidResourceException("Game must have a posted at date");
            //if (String.IsNullOrEmpty(Image))
            //    throw new InvalidResourceException("Game must include an image");
        }

        public void Update(Game newGame)
        {
            Title = newGame.Title;
            Synopsis = newGame.Synopsis;
        }

        public override bool Equals(object? obj)
        {
            return Title == ((Game)obj).Title;
        }

    }
}
