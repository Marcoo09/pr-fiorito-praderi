using System;
using System.Threading;
using System.Threading.Tasks;
using DTOs.Response;
using Microsoft.Extensions.Configuration;
using Protocol;
using Server.Domain;
using ServerGrpc.Interfaces;
using ServerGrpc.Logs;

namespace ServerGrpc.Implementations
{
    public class ServiceRouter : IServiceRouter
    {
        private IGameService _gameService;
        private IUserService _userService;
        private User _user;
        private static ServiceRouter _instance;
        private static readonly SemaphoreSlim _instanceSemaphore = new SemaphoreSlim(1);
        private LogEmitter _logEmitter;

        public ServiceRouter()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            ServerConfiguration serverConfiguration = new ServerConfiguration()
            {
                RabbitMQServerIP = config.GetSection("ServerConfiguration").GetSection("RabbitMQServerIP").Value,
                LogsQueueName = config.GetSection("ServerConfiguration").GetSection("LogsQueueName").Value,
                ServerPort = config.GetSection("ServerConfiguration").GetSection("ServerPort").Value,
                ServerIP = config.GetSection("ServerConfiguration").GetSection("ServerIP").Value,
                RabbitMQServerPort = config.GetSection("ServerConfiguration").GetSection("RabbitMQServerPort").Value,
                GrpcApiHttpPort = config.GetSection("ServerConfiguration").GetSection("GrpcApiHttpPort").Value,
                GrpcApiHttpsPort = config.GetSection("ServerConfiguration").GetSection("GrpcApiHttpsPort").Value
            };

            _logEmitter = new LogEmitter(serverConfiguration);

            _gameService = new GameService(_logEmitter);
            _userService = new UserService(_logEmitter);
            _user = new User();
        }

        public static ServiceRouter GetInstance()
        {
            _instanceSemaphore.Wait();
            if (_instance == null)
                _instance = new ServiceRouter();

            _instanceSemaphore.Release();
            return _instance;
        }

        public async Task<Frame> GetResponseAsync(Frame frameRequest)
        {
            Frame response = null;

            switch ((CommandConstants)frameRequest.ChosenCommand)
            {
                case CommandConstants.BuyGame:
                    response = await _userService.BuyGameAsync(frameRequest, _user.Id);
                    break;
                case CommandConstants.IndexBoughtGames:
                    response = await _userService.IndexBoughtGamesAsync(_user.Id);
                    break;
                case CommandConstants.CreateGame:
                    response = await _gameService.CreateGameAsync(frameRequest);
                    break;
                case CommandConstants.CreateGameReview:
                    response = await _gameService.AddReviewAsync(frameRequest);
                    break;
                case CommandConstants.DeleteGame:
                    response = await _gameService.DeleteGameAsync(frameRequest);
                    break;
                case CommandConstants.GetGameReviews:
                    response = await _gameService.GetAllReviewsAsync(frameRequest);
                    break;
                case CommandConstants.GetGame:
                    response = await _gameService.ShowGameAsync(frameRequest);
                    break;
                case CommandConstants.IndexGamesCatalog:
                    response = await _gameService.ShowGamesAsync();
                    break;
                case CommandConstants.SearchGames:
                    response = await _gameService.SearchGameByAsync(frameRequest);
                    break;
                case CommandConstants.UpdateGame:
                    response = await _gameService.UpdateGameAsync(frameRequest);
                    break;
                case CommandConstants.IndexUsers:
                    response = await _userService.IndexUsersAsync();
                    break;
                case CommandConstants.Login:
                    response = await _userService.CreateUserAsync(frameRequest);
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
