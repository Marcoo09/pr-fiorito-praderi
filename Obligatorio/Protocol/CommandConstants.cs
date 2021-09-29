namespace Protocol
{
    public enum CommandConstants : short
    {
        IndexGamesCatalog, //SRF2
        BuyGame,//SRF3 -> Marco 
        IndexBoughtGames, //SRF3 
        CreateGame,//SRF4, CRF2 Marco
        CreateGameReview,//SRF5, CRF5
        GetGameReviews, //SRF5, CRF5 
        SearchGames,//SRF6 
        GetGame, //SRF7, CRF6
        DeleteGame, //CRF3 
        UpdateGame, //CRF3 
        IndexUsers, 
    }
}
