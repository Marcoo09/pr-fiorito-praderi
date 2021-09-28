using Protocol;

namespace Server.Interfaces
{
    public interface IGameService
    {
        Frame CreateGame(Frame requestFrame);
        Frame ShowGame(Frame requestFrame);
        Frame ShowGames();
        Frame UpdateGame(Frame requestFrame);
        Frame AddReview(Frame requestFrame);
        Frame GetAllReviews(Frame requestFrame);
        Frame SearchGameBy(Frame requestFrame);
        Frame DeleteGame(Frame requestFrame);
    }
}
