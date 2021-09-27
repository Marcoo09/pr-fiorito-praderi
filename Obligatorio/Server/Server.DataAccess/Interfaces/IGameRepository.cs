using System;
using System.Collections.Generic;
using Server.Domain;

namespace Server.DataAccess.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetAll();
        int Insert(Game game);
        Game Get(int id);
        void Update(int id, Game game);
        void Delete(int id);
        public List<Game> GetBy(Func<Game, bool> predicate);
        public List<Document> GetCovers();
    }
}
