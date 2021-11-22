using System;
namespace Logger.Domain
{
    public enum Tag : short
    {
        CreateGame,
        GetGame,
        IndexGamesCatalog,
        DeleteGame,
        UpdateGame,
        BuyGame,
        IndexBoughtGames,
        CreateGameReview,
        GetGameReviews,
        SearchGames,
        IndexUsers,
        Login,
    }
}
