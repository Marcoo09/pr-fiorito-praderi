namespace Protocol
{
    public enum Command : short
    {
        IndexGamesCatalog, //SRF2 -> DONE
        BuyGame,//SRF3 -> Marco -> DONE
        IndexBoughtGames, //SRF3 -> Marco -> DONE
        CreateGame,//SRF4, CRF2 Marco -> DONE 
        CreateGameReview,//SRF5, CRF5
        GetGameReviews, //SRF5, CRF5
        SearchGames,//SRF6 By title, gender, rating 
        IndexGame, //SRF7, CRF6-> Not completed
        DeleteGame, //CRF3 -> Nico
        UpdateGame, //CRF3 -> Nico
        IndexUsers, // -> DONE
    }
}
