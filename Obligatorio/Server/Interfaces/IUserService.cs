using Protocol;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IUserService
    {
        Task<Frame> IndexUsersAsync();
        Task<Frame> BuyGameAsync(Frame requestFrame, int userId);
        Task<Frame> IndexBoughtGamesAsync(int userId);
        Task<Frame> CreateUserAsync(Frame requestFrame);
    }
}
