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

        public Frame AddReview(Frame requestFrame)
        {
            ReviewDTO reviewDTO = new ReviewDTO();
            reviewDTO.Deserialize(requestFrame.Data);

            try
            {
                Review newReview = reviewDTO.ToEntity();
                Game gameToSendReview = _gameRepository.Get(reviewDTO.GameId);
                newReview.Game = gameToSendReview;

                _gameRepository.AddReview(reviewDTO.GameId ,newReview);

                MessageDTO messageDto = new MessageDTO() { Message = "Review added!" };

                return CreateSuccessResponse(Command.CreateGameReview, messageDto.Serialize());
            }
            catch (Exception e)
            {
                if (e is InvalidResourceException || e is ResourceNotFoundException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(Command.CreateGameReview, errorDto.Serialize());
                }
                throw;
            }
        }

        public Frame CreateGame(Frame requestFrame)
        {
            CreateGameDTO createGameDTO = new CreateGameDTO();
            createGameDTO.Deserialize(requestFrame.Data);

            try
            {
                Game newGame = createGameDTO.ToEntity();

                int newGameId = _gameRepository.Insert(newGame);
                Game createdGame = _gameRepository.Get(newGameId);
                createGameDTO.WriteFile();

                return CreateSuccessResponse(Command.CreateGame, new GameBasicInfoDTO(createdGame).Serialize());
            }
            catch (Exception e)
            {
                if (e is InvalidResourceException || e is ResourceNotFoundException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(Command.CreateGame, errorDto.Serialize());
                }
                throw;
            }
        }

        public Frame DeleteGame(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();
            basicGameRequestDTO.Deserialize(requestFrame.Data);

            try
            {
                Game gameToBeDeleted = _gameRepository.Get(basicGameRequestDTO.GameId);

                _gameRepository.Delete(basicGameRequestDTO.GameId);

                MessageDTO messageDto = new MessageDTO() { Message = "Game deleted!" };
                return CreateSuccessResponse(Command.DeleteGame, messageDto.Serialize());
            }
            catch (ResourceNotFoundException e)
            {
                ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                return CreateErrorResponse(Command.DeleteGame, errorDto.Serialize());
            }
        }

        public Frame GetAllReviews(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();
            basicGameRequestDTO.Deserialize(requestFrame.Data);

            try
            {
                Game retrievedGame = _gameRepository.Get(basicGameRequestDTO.GameId);

                List<ReviewDetailDTO> retrievedReviews = retrievedGame.Reviews.Select(r => new ReviewDetailDTO(r)).ToList();

                byte[] serializedList = _serializer.SerializeEntityList(retrievedReviews.Cast<ISerializable>().ToList());

                return new Frame()
                {
                    ChosenHeader = (short)Header.Response,
                    ChosenCommand = (short)Command.GetGameReviews,
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
                    return CreateErrorResponse(Command.GetGameReviews, errorDto.Serialize());
                }
                throw;
            }
        }

        public Frame SearchGameBy(Frame requestFrame)
        {
            SearchMetricDTO searchMetricDTO = new SearchMetricDTO();
            searchMetricDTO.Deserialize(requestFrame.Data);

            List<Game> gamesFiltered = new List<Game>();

            if (!String.IsNullOrEmpty(searchMetricDTO.Title))
            {
                gamesFiltered = _gameRepository.GetBy(g => g.Title.Contains(searchMetricDTO.Title, System.StringComparison.CurrentCultureIgnoreCase) ||
                   searchMetricDTO.Title.Contains(g.Title, System.StringComparison.CurrentCultureIgnoreCase));
            }else if (!String.IsNullOrEmpty(searchMetricDTO.Gender))
            {
                gamesFiltered = _gameRepository.GetBy(g => g.Gender.Contains(searchMetricDTO.Gender, System.StringComparison.CurrentCultureIgnoreCase) ||
                    searchMetricDTO.Gender.Contains(g.Gender, System.StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                gamesFiltered = _gameRepository.GetBy(g =>
                    (g.Reviews.Count > 0 ? g.Reviews.Select(g => g.Rating).ToList().Average() : 0) == searchMetricDTO.Rating
                );
            }

            List<GameDetailDTO> response = gamesFiltered.Select(g => new GameDetailDTO(g)).ToList();
            byte[] serializedList = _serializer.SerializeEntityList(response.Cast<ISerializable>().ToList());

            return CreateSuccessResponse(Command.SearchGames, serializedList);

        }

        public Frame ShowGame(Frame requestFrame)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();
            basicGameRequestDTO.Deserialize(requestFrame.Data);

            try
            {
                Game game = _gameRepository.Get(basicGameRequestDTO.GameId);
                EnrichedGameDetailDTO response = new EnrichedGameDetailDTO(game);
                response.ReadFile(game.Path);

                return CreateSuccessResponse(Command.IndexGame, response.Serialize());
            }
            catch (ResourceNotFoundException e)
            {
                ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                return CreateErrorResponse(Command.IndexGame, errorDto.Serialize());
            }
        }

        public Frame ShowGames()
        {
            List<GameDetailDTO> retrievedGames = _gameRepository.GetAll().Select(g => new GameDetailDTO(g)).ToList();
            byte[] serializedList = _serializer.SerializeEntityList(retrievedGames.Cast<ISerializable>().ToList());

            return new Frame()
            {
                ChosenHeader = (short)Header.Response,
                ChosenCommand = (short)Command.IndexGamesCatalog,
                ResultStatus = (short)Status.Ok,
                DataLength = serializedList.Length,
                Data = serializedList,
            };
        }

        public Frame UpdateGame(Frame requestFrame)
        {
            UpdateGameDTO updateGameDTO = new UpdateGameDTO();
            updateGameDTO.Deserialize(requestFrame.Data);

            try
            {
                Game updatedGame = updateGameDTO.ToEntity();

                _gameRepository.Update(updateGameDTO.Id, updatedGame);

                Game storedGame = _gameRepository.Get(updatedGame.Id);

                return CreateSuccessResponse(Command.UpdateGame, new GameBasicInfoDTO(storedGame).Serialize());
            }
            catch (Exception e)
            {
                if (e is ResourceNotFoundException || e is InvalidResourceException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(Command.UpdateGame, errorDto.Serialize());
                }
                throw;
            }
        }

        private Frame CreateErrorResponse(Command command, byte[] data)
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

        private Frame CreateSuccessResponse(Command command, byte[] data)
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
