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

        public Frame GetCoverFromGame(Frame requestFrame)
        {
            throw new NotImplementedException();
        }

        public Frame ShowGame(Frame requestFrame)
        {
            int gameIdToShow = BitConverter.ToInt32(requestFrame.Data);

            try
            {
                Game game = _gameRepository.Get(gameIdToShow);
                return CreateSuccessResponse(Command.IndexGame, new GameDetailDTO(game).Serialize());
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
            throw new NotImplementedException();
        }

        public Frame UploadCoverToGame(Frame requestFrame)
        {
            throw new NotImplementedException();
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
