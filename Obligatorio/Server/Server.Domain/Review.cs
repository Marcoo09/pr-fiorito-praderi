using System;
using Exceptions;

namespace Server.Domain
{
    public class Review
    {
        public Game Game { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }

        public Review()
        {
        }

        public void ValidOrFail()
        {
            if (String.IsNullOrEmpty(Description))
                throw new InvalidResourceException("Review must have a description");
            if (Rating < 1 || Rating > 0)
                throw new InvalidResourceException("Review must be in the range 1-5");
            if (Game == null)
                throw new InvalidResourceException("Review must be associated to a game");
        }
    }
}
