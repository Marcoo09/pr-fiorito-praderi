using System;
using System.Collections.Generic;
using System.Text;

namespace LogsServer.Domain
{
    public enum LogTag
    {
        IndexReview,
        CreateReview,
        UpdateReview,
        DeleteReview,
        IndexGamesByReview,
        CreateGame,
        ShowGame,
        UpdateGame,
        ChangeGameReview,
        DeleteGame,
    }
}
