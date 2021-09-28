using System;
using System.Collections.Generic;
using Exceptions;

namespace Server.Domain
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Gender { get; set; }

        public string Path { get; set; }
        public string CoverName { get; set; }
        public long FileSize { get; set; }

        public List<Review> Reviews {get;set;}

        public Game()
        {
            Reviews = new List<Review>();
        }

        public void ValidOrFail()
        {
            if (String.IsNullOrEmpty(Title))
                throw new InvalidResourceException("Game must have a title");
            if (String.IsNullOrEmpty(Synopsis))
                throw new InvalidResourceException("Game must have a Synopsis");
            if(String.IsNullOrEmpty(Gender))
                throw new InvalidResourceException("Game must have a gender");
            if (String.IsNullOrEmpty(Path))
                throw new InvalidResourceException("Game must have a cover");
        }

        public void Update(Game newGame)
        {
            Title = newGame.Title;
            Synopsis = newGame.Synopsis;
            Gender = newGame.Gender;
        }

        public override bool Equals(object? obj)
        {
            return Title == ((Game)obj).Title;
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);
        }

    }
}
