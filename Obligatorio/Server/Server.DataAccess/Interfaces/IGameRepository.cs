using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain;

namespace Server.DataAccess.Interfaces
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllAsync();
        Task<int> InsertAsync(Game game);
        Task<Game> GetAsync(int id);
        Task UpdateAsync(int id, Game game);
        Task DeleteAsync(int id);
        Task <List<Game>> GetByAsync(Func<Game, bool> predicate);
        Task AddReviewAsync(int id, Review review);
        Task <List<Review>> GetAllReviewsAsync(int id);
    }
}