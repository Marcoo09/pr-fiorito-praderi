using System;
using System.Collections.Generic;
using System.Linq;
using DTOs.Request;
using DTOs.Response;
using Exceptions;
using Protocol;
using Protocol.Serialization;
using Protocol.SerializationInterfaces;
using Server.DataAccess.Implementations;
using Server.DataAccess.Interfaces;
using Server.Domain;
using Server.Interfaces;

namespace Server.Implementations
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IGameRepository _gameRepository;
        private Serializer _serializer;

        public UserService()
        {
            _userRepository = UserRepository.GetInstance();
            _gameRepository = GameRepository.GetInstance();
            _serializer = new Serializer();
        }

        public Frame BuyGame(Frame requestFrame, int userId)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();
            basicGameRequestDTO.Deserialize(requestFrame.Data);

            try
            {
                Game retrievedGame = _gameRepository.Get(basicGameRequestDTO.GameId);

                _userRepository.BuyGame(retrievedGame, userId);

                MessageDTO messageDto = new MessageDTO() { Message = "Game bought!" };

                return CreateSuccessResponse(Command.BuyGame, messageDto.Serialize());
            }
            catch (Exception e)
            {
                if (e is ResourceNotFoundException || e is InvalidResourceException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(Command.BuyGame, errorDto.Serialize());
                }
                throw;
            }
        }

        public Frame IndexBoughtGames(int userId)
        {
            try
            {
                User retrievedUser = _userRepository.Get(userId);

                List<GameDetailDTO> retrievedGames = retrievedUser.BoughtGames.Select(g => new GameDetailDTO(g)).ToList();

                byte[] serializedList = _serializer.SerializeEntityList(retrievedGames.Cast<ISerializable>().ToList());

                return new Frame()
                {
                    ChosenHeader = (short)Header.Response,
                    ChosenCommand = (short)Command.IndexBoughtGames,
                    ResultStatus = (short)Status.Ok,
                    DataLength = serializedList.Length,
                    Data = serializedList,
                };
            }
            catch (Exception e)
            {
                if (e is ResourceNotFoundException || e is InvalidResourceException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(Command.BuyGame, errorDto.Serialize());
                }
                throw;
            }
        }

        public Frame IndexUsers()
        {
            List<UserDetailDTO> retrievedUsers = _userRepository.GetAll().Select(u => new UserDetailDTO(u)).ToList();
            byte[] serializedList = _serializer.SerializeEntityList(retrievedUsers.Cast<ISerializable>().ToList());

            return new Frame()
            {
                ChosenHeader = (short)Header.Response,
                ChosenCommand = (short)Command.IndexUsers,
                ResultStatus = (short)Status.Ok,
                DataLength = serializedList.Length,
                Data = serializedList,
            };
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
