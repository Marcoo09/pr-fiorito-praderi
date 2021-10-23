using System;
using System.Collections.Generic;
using DTOs.Request;
using DTOs.Response;
using Exceptions;
using Protocol;
using Protocol.Serialization;
using Server.DataAccess.Implementations;
using Server.DataAccess.Interfaces;
using Server.Domain;
using Server.Interfaces;
using System.Linq;
using Protocol.SerializationInterfaces;
using System.Threading.Tasks;

namespace Server.Implementations
{
    public class GameService : IGameService
    {
        private IGameRepository _gameRepository;
        private Serializer _serializer;

        public GameService()
        {
            _gameRepository = GameRepository.GetInstance();
            _serializer = new Serializer();
        }

        public async Task<Frame> AddReviewAsync(Frame requestFrame)
        {
            ReviewDTO reviewDTO = new ReviewDTO();
            reviewDTO.Deserialize(requestFrame.Data);

            try
            {
                Review newReview = reviewDTO.ToEntity();
                Game gameToSendReview = await _gameRepository.GetAsync(reviewDTO.GameId);
                newReview.Game = gameToSendReview;

                _gameRepository.AddReviewAsync(reviewDTO.GameId ,newReview);

                MessageDTO messageDto = new MessageDTO() { Message = "Review added!" };

                return CreateSuccessResponse(CommandConstants.CreateGameReview, messageDto.Serialize());
            }
            catch (Exception e)
            {
                if (e is InvalidResourceException || e is ResourceNotFoundException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(CommandConstants.CreateGameReview, errorDto.Serialize());
                }
                throw;
            }
        }

        public async Task<Frame> CreateGameAsync(Frame requestFrame)
        {
            CreateGameDTO createGameDTO = new CreateGameDTO();
            createGameDTO.Deserialize(requestFrame.Data);

            try
            {
                Game newGame = createGameDTO.ToEntity();

                int newGameId = await _gameRepository.InsertAsync(newGame);
                Game createdGame = await _gameRepository.GetAsync(newGameId);
                createGameDTO.WriteFile();

                return CreateSuccessResponse(CommandConstants.CreateGame, new GameBasicInfoDTO(createdGame).Serialize());
            }
            catch (Exception e)
            {
                if (e is InvalidResourceException || e is ResourceNotFoundException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(CommandConstants.CreateGame, errorDto.Serialize());
                }
                throw;
            }
        }

        public async Task<Frame> DeleteGameAsync(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();
            basicGameRequestDTO.Deserialize(requestFrame.Data);

            try
            {
                Game gameToBeDeleted = await _gameRepository.GetAsync(basicGameRequestDTO.GameId);

                await _gameRepository.DeleteAsync(basicGameRequestDTO.GameId);

                MessageDTO messageDto = new MessageDTO() { Message = "Game deleted!" };
                return CreateSuccessResponse(CommandConstants.DeleteGame, messageDto.Serialize());
            }
            catch (ResourceNotFoundException e)
            {
                ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                return CreateErrorResponse(CommandConstants.DeleteGame, errorDto.Serialize());
            }
        }

        public async Task<Frame> GetAllReviewsAsync(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();
            basicGameRequestDTO.Deserialize(requestFrame.Data);

            try
            {
                Game retrievedGame = await _gameRepository.GetAsync(basicGameRequestDTO.GameId);

                List<ReviewDetailDTO> retrievedReviews = retrievedGame.Reviews.Select(r => new ReviewDetailDTO(r)).ToList();

                byte[] serializedList = _serializer.SerializeEntityList(retrievedReviews.Cast<ISerializable>().ToList());

                return new Frame()
                {
                    ChosenHeader = (short)Header.Response,
                    ChosenCommand = (short)CommandConstants.GetGameReviews,
                    ResultStatus = (short)Status.Ok,
                    DataLength = serializedList.Length,
                    Data = serializedList,
                };
            }
            catch (Exception e)
            {
                if (e is InvalidResourceException || e is ResourceNotFoundException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(CommandConstants.GetGameReviews, errorDto.Serialize());
                }
                throw;
            }
        }

        public async Task<Frame> SearchGameByAsync(Frame requestFrame)
        {
            SearchMetricDTO searchMetricDTO = new SearchMetricDTO();
            searchMetricDTO.Deserialize(requestFrame.Data);

            List<Game> gamesFiltered = new List<Game>();

            if (!String.IsNullOrEmpty(searchMetricDTO.Title))
            {
                gamesFiltered = await _gameRepository.GetByAsync(g => g.Title.Contains(searchMetricDTO.Title, System.StringComparison.CurrentCultureIgnoreCase) ||
                   searchMetricDTO.Title.Contains(g.Title, System.StringComparison.CurrentCultureIgnoreCase));
            }else if (!String.IsNullOrEmpty(searchMetricDTO.Gender))
            {
                gamesFiltered = await _gameRepository.GetByAsync(g => g.Gender.Contains(searchMetricDTO.Gender, System.StringComparison.CurrentCultureIgnoreCase) ||
                    searchMetricDTO.Gender.Contains(g.Gender, System.StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                gamesFiltered = await _gameRepository.GetByAsync(g =>
                    (g.Reviews.Count > 0 ? g.Reviews.Select(g => g.Rating).ToList().Average() : 0) == searchMetricDTO.Rating
                );
            }

            List<GameDetailDTO> response = gamesFiltered.Select(g => new GameDetailDTO(g)).ToList();
            byte[] serializedList = _serializer.SerializeEntityList(response.Cast<ISerializable>().ToList());

            return CreateSuccessResponse(CommandConstants.SearchGames, serializedList);

        }

        public async Task<Frame> ShowGameAsync(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();
            basicGameRequestDTO.Deserialize(requestFrame.Data);

            try
            {
                Game game = await _gameRepository.GetAsync(basicGameRequestDTO.GameId);
                EnrichedGameDetailDTO response = new EnrichedGameDetailDTO(game);
                response.ReadFile(game.Path);

                return CreateSuccessResponse(CommandConstants.GetGame, response.Serialize());
            }
            catch (ResourceNotFoundException e)
            {
                ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                return CreateErrorResponse(CommandConstants.GetGame, errorDto.Serialize());
            }
        }

        public async Task<Frame> ShowGamesAsync()
        {
            List<GameDetailDTO> retrievedGames = (await _gameRepository.GetAllAsync()).Select(g => new GameDetailDTO(g)).ToList();
            byte[] serializedList = _serializer.SerializeEntityList(retrievedGames.Cast<ISerializable>().ToList());

            return new Frame()
            {
                ChosenHeader = (short)Header.Response,
                ChosenCommand = (short)CommandConstants.IndexGamesCatalog,
                ResultStatus = (short)Status.Ok,
                DataLength = serializedList.Length,
                Data = serializedList,
            };
        }

        public async Task<Frame> UpdateGameAsync(Frame requestFrame)
        {
            UpdateGameDTO updateGameDTO = new UpdateGameDTO();
            updateGameDTO.Deserialize(requestFrame.Data);

            try
            {
                Game updatedGame = updateGameDTO.ToEntity();

                await _gameRepository.UpdateAsync(updateGameDTO.Id, updatedGame);

                Game storedGame = await _gameRepository.GetAsync(updatedGame.Id);

                return CreateSuccessResponse(CommandConstants.UpdateGame, new GameBasicInfoDTO(storedGame).Serialize());
            }
            catch (Exception e)
            {
                if (e is ResourceNotFoundException || e is InvalidResourceException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(CommandConstants.UpdateGame, errorDto.Serialize());
                }
                throw;
            }
        }

        private Frame CreateErrorResponse(CommandConstants command, byte[] data)
        {
            return new Frame()
            {
                ChosenHeader = (short)Header.Response,
                ChosenCommand = (short)command,
                ResultStatus = (short)Status.Error,
                DataLength = data.Length,
                Data = data,
            };
        }

        private Frame CreateSuccessResponse(CommandConstants command, byte[] data)
        {
            return new Frame()
            {
                ChosenHeader = (short)Header.Response,
                ChosenCommand = (short)command,
                ResultStatus = (short)Status.Ok,
                DataLength = data.Length,
                Data = data,
            };
        }
    }
}
