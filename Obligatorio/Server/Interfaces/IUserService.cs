using Protocol;

namespace Server.Interfaces
{
    public interface IUserService
    {
        Frame IndexUsers();
        Frame BuyGame(Frame requestFrame, int userId);
        Frame IndexBoughtGames(int userId);
        Frame CreateUser(Frame requestFrame);
    }
}
