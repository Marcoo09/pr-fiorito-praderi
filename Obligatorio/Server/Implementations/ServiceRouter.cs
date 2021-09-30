using System;
using DTOs.Response;
using Protocol;
using Server.Domain;
using Server.Interfaces;

namespace Server.Implementations
{
    public class ServiceRouter : IServiceRouter
    {
        private IGameService _gameService;
        private IUserService _userService;
        private User _user;

        public ServiceRouter()
        {
            _gameService = new GameService();
            _userService = new UserService();
            _user = new User();
        }

        public Frame GetResponse(Frame frameRequest)
        {
            Frame response = null;

            switch ((CommandConstants)frameRequest.ChosenCommand)
            {
                case CommandConstants.BuyGame:
                    response = _userService.BuyGame(frameRequest, _user.Id);
                    break;
                case CommandConstants.IndexBoughtGames:
                    response = _userService.IndexBoughtGames(_user.Id);
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
                case CommandConstants.Login:
                    response = _userService.CreateUser(frameRequest);
                    UpdateCurrentUser(response);
                    break;
            }

            return response;
        }

        private void UpdateCurrentUser(Frame responseFrame)
        {
            UserDetailDTO userDetailDTO = new UserDetailDTO();
            userDetailDTO.Deserialize(responseFrame.Data);

            _user.Id = userDetailDTO.Id;
            _user.Name = userDetailDTO.Name;

        }

    }
}
