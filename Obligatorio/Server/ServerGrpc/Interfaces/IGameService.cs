using Protocol;
using System.Threading.Tasks;

namespace ServerGrpc.Interfaces
{
    public interface IGameService
    {
        Task<Frame> CreateGameAsync(Frame requestFrame);
        Task<Frame> ShowGameAsync(Frame requestFrame);
        Task<Frame> ShowGamesAsync();
        Task<Frame> UpdateGameAsync(Frame requestFrame);
        Task<Frame> AddReviewAsync(Frame requestFrame);
        Task<Frame> GetAllReviewsAsync(Frame requestFrame);
        Task<Frame> SearchGameByAsync(Frame requestFrame);
        Task<Frame> DeleteGameAsync(Frame requestFrame);
    }
}
