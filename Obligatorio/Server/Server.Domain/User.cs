using System;
using System.Collections.Generic;
using Exceptions;

namespace Server.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Game> BoughtGames { get; set; }

        public User()
        {
            BoughtGames = new List<Game>();
        }

        public void BuyGame(Game game)
        {
            BoughtGames.Add(game);
        }
    }
}
