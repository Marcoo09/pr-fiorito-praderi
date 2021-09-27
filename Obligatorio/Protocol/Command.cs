namespace Protocol
{
    public enum Command : short
    {
        IndexGamesCatalog, //SRF2 -> DONE
        BuyGame,//SRF3 -> Marco
        CreateGame,//SRF4, CRF2 Marco -> DONE ->
        CreateGameReview,//SRF5, CRF5
        GetGameReviews, //
        SearchGames,//SRF6 By title, gender, rating -> Marco
        IndexGame, //SRF7, CRF6-> Not completed -> Marco
        DeleteGame, //CRF3 -> Nico
        UpdateGame //CRF3 -> Nico
    }
}
