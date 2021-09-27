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

            switch ((Command)frameRequest.ChosenCommand)
            {
                case Command.BuyGame:
                    response = _userService.BuyGame(frameRequest, user.Id);
                    break;
                case Command.IndexBoughtGames:
                    response = _userService.IndexBoughtGames(user.Id);
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
                case Command.IndexUsers:
                    response = _userService.IndexUsers();
                    break;
            }

            return response;
        }
    }
}
