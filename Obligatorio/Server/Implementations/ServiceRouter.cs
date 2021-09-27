using System;
using Protocol;
using Server.Interfaces;

namespace Server.Implementations
{
    public class ServiceRouter : IServiceRouter
    {
        private IGameService _gameService;

        public ServiceRouter()
        {
            _gameService = new GameService();
        }

        public Frame GetResponse(Frame frameRequest)
        {
            Frame response = null;

            switch ((Command)frameRequest.ChosenCommand)
            {
                case Command.BuyGame:
                    break;
                case Command.CreateGame:
                    response = _gameService.CreateGame(frameRequest);
                    break;
                case Command.CreateGameReview:
                    break;
                case Command.DeleteGame:
                    //response = _gameService.DeleteGame(frameRequest);
                    break;
                case Command.GetGameReviews:
                    break;
                case Command.IndexGame:
                    response = _gameService.ShowGame(frameRequest);
                    break;
                case Command.IndexGamesCatalog:
                    response = _gameService.ShowGames();
                    break;
                case Command.SearchGames:
                    break;
                case Command.UpdateGame:
                    response = _gameService.UpdateGame(frameRequest);
                    break;
            }

            return response;
        }
    }
}
