using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain;

namespace Server.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<int> InsertAsync(User user);
        Task<User> GetAsync(int id);
        Task<List<User>> GetAllAsync();
        Task DeleteAsync(int id);
        Task BuyGameAsync(Game game, int userId);
    }
}
