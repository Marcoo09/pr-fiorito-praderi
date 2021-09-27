using Protocol;

namespace Server.Interfaces
{
    public interface IGameService
    {
        Frame CreateGame(Frame requestFrame);
        Frame GetCoverFromGame(Frame requestFrame);
        Frame ShowGame(Frame requestFrame);
        Frame ShowGames();
        Frame UpdateGame(Frame requestFrame);
        Frame UploadCoverToGame(Frame requestFrame);


        //Frame UpdateGameCover(Frame requestFrame);
        //Frame IndexCovers(Frame requestFrame);
        //Frame IndexGameBy(Frame requestFrame);
    }
}
