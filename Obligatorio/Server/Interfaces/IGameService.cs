﻿using Protocol;

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
        Frame AddReview(Frame requestFrame);
        Frame GetAllReviews(Frame requestFrame);

        //Frame IndexGameBy(Frame requestFrame);
    }
}
