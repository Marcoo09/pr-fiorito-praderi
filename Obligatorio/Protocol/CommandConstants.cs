namespace Protocol
{
    public enum CommandConstants : short
    {
        CreateGame,//SRF4, CRF2
        GetGame, //SRF7, CRF6
        IndexGamesCatalog, //SRF2
        DeleteGame, //CRF3 
        UpdateGame, //CRF3 
        BuyGame,//SRF3 
        IndexBoughtGames, //SRF3 
        CreateGameReview,//SRF5, CRF5
        GetGameReviews, //SRF5, CRF5 
        SearchGames,//SRF6 
        IndexUsers,
        Login,
    }
}
