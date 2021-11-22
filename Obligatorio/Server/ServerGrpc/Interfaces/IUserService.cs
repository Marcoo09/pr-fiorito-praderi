using Protocol;
using System.Threading.Tasks;

namespace ServerGrpc.Interfaces
{
    public interface IUserService
    {
        Task<Frame> IndexUsersAsync();
        Task<Frame> BuyGameAsync(Frame requestFrame, int userId);
        Task<Frame> IndexBoughtGamesAsync(int userId);
        Task<Frame> CreateUserAsync(Frame requestFrame);
    }
}
