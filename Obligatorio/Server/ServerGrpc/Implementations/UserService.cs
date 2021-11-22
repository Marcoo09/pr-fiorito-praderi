using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOs.Request;
using DTOs.Response;
using Exceptions;
using Logger.Domain;
using Newtonsoft.Json;
using Protocol;
using Protocol.Serialization;
using Protocol.SerializationInterfaces;
using Server.DataAccess.Implementations;
using Server.DataAccess.Interfaces;
using Server.Domain;
using ServerGrpc.Interfaces;
using ServerGrpc.Logs;

namespace ServerGrpc.Implementations
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IGameRepository _gameRepository;
        private Serializer _serializer;
        private LogEmitter _logEmitter;

        public UserService(LogEmitter logEmitter)
        {
            _userRepository =  UserRepository.GetInstance();
            _gameRepository =  GameRepository.GetInstance();
            _serializer = new Serializer();
            _logEmitter = logEmitter;
        }

        public async Task<Frame> BuyGameAsync(Frame requestFrame, int userId)
        {
            BasicGameRequestDTO basicGameRequestDTO = new BasicGameRequestDTO();
            basicGameRequestDTO.Deserialize(requestFrame.Data);

            try
            {
                Game retrievedGame = await _gameRepository.GetAsync(basicGameRequestDTO.GameId);

                await _userRepository.BuyGameAsync(retrievedGame, userId);

                MessageDTO messageDto = new MessageDTO() { Message = "Game bought!" };

                _logEmitter.EmitLog(JsonConvert.SerializeObject(messageDto), Tag.BuyGame);

                return CreateSuccessResponse(CommandConstants.BuyGame, messageDto.Serialize());
            }
            catch (Exception e)
            {
                if (e is ResourceNotFoundException || e is InvalidResourceException)
                {
                    ErrorDTO errorDto = new ErrorDTO() { Message = e.Message };
                    return CreateErrorResponse(CommandConstants.BuyGame, errorDto.Serialize());
                }
                throw;
            }
        }

        public async Task<Frame> CreateUserAsync(Frame requestFrame)
        {
            LoginDTO loginDTO = new LoginDTO();
            loginDTO.Deserialize(requestFrame.Data);

            User currentUser;

            bool userAlreadyExist = (await _userRepository.GetAllAsync()).Any(u => u.Name == loginDTO.UserName);

            if (!userAlreadyExist)
            {
                User newUser = new User();
                newUser = loginDTO.ToEntity();
                await _userRepository.InsertAsync(newUser);
            }

            currentUser = (await _userRepository.GetAllAsync()).Find(u => u.Name == loginDTO.UserName);

            UserDetailDTO userDetailDTO = new UserDetailDTO(currentUser);

            _logEmitter.EmitLog(JsonConvert.SerializeObject(userDetailDTO), Tag.Login);

            return CreateSuccessResponse(CommandConstants.Login, userDetailDTO.Serialize());
        }

        public async Task<Frame> IndexBoughtGamesAsync(int userId)
        {
            try
            {
                User retrievedUser = await _userRepository.GetAsync(userId);

                List<GameDetailDTO> retrievedGames = retrievedUser.BoughtGames.Select(g => new GameDetailDTO(g)).ToList();

                byte[] serializedList = _serializer.SerializeEntityList(retrievedGames.Cast<ISerializable>().ToList());

                _logEmitter.EmitLog(JsonConvert.SerializeObject(retrievedGames), Tag.IndexBoughtGames);

                return new Frame()
                {
                    ChosenHeader = (short)Header.Response,
                    ChosenCommand = (short)CommandConstants.IndexBoughtGames,
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
                    return CreateErrorResponse(CommandConstants.IndexBoughtGames, errorDto.Serialize());
                }
                throw;
            }
        }

        public async Task<Frame> IndexUsersAsync()
        {
            List<UserDetailDTO> retrievedUsers = (await _userRepository.GetAllAsync()).Select(u => new UserDetailDTO(u)).ToList();
            byte[] serializedList = _serializer.SerializeEntityList(retrievedUsers.Cast<ISerializable>().ToList());

            _logEmitter.EmitLog(JsonConvert.SerializeObject(retrievedUsers), Tag.IndexUsers);

            return new Frame()
            {
                ChosenHeader = (short)Header.Response,
                ChosenCommand = (short)CommandConstants.IndexUsers,
                ResultStatus = (short)Status.Ok,
                DataLength = serializedList.Length,
                Data = serializedList,
            };
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
