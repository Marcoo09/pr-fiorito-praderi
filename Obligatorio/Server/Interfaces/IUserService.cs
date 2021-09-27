using Protocol;

namespace Server.Interfaces
{
    public interface IUserService
    {
        Frame IndexUsers();
        Frame BuyGame(int UserId, int GameId);
    }
}
