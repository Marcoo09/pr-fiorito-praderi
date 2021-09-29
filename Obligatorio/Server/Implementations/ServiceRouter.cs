using System;
using Protocol;
using Server.Domain;
using Server.Interfaces;

namespace Server.Implementations
{
    public class ServiceRouter : IServiceRouter
    {
        private IGameService _gameService;
        private IUserService _userService;

        public ServiceRouter()
        {
            _gameService = new GameService();
            _userService = new UserService();
        }

        public Frame GetResponse(Frame frameRequest, User user)
        {
            Frame response = null;

            switch ((CommandConstants)frameRequest.ChosenCommand)
            {
                case CommandConstants.BuyGame:
                    response = _userService.BuyGame(frameRequest, user.Id);
                    break;
                case CommandConstants.IndexBoughtGames:
                    response = _userService.IndexBoughtGames(user.Id);
                    break;
                case CommandConstants.CreateGame:
                    response = _gameService.CreateGame(frameRequest);
                    break;
                case CommandConstants.CreateGameReview:
                    response = _gameService.AddReview(frameRequest);
                    break;
                case CommandConstants.DeleteGame:
                    response = _gameService.DeleteGame(frameRequest);
                    break;
                case CommandConstants.GetGameReviews:
                    response = _gameService.GetAllReviews(frameRequest);
                    break;
                case CommandConstants.GetGame:
                    response = _gameService.ShowGame(frameRequest);
                    break;
                case CommandConstants.IndexGamesCatalog:
                    response = _gameService.ShowGames();
                    break;
                case CommandConstants.SearchGames:
                    response = _gameService.SearchGameBy(frameRequest);
                    break;
                case CommandConstants.UpdateGame:
                    response = _gameService.UpdateGame(frameRequest);
                    break;
                case CommandConstants.IndexUsers:
                    response = _userService.IndexUsers();
                    break;
            }

            return response;
        }
    }
}
