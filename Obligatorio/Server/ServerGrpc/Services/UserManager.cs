using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOs.Request;
using DTOs.Response;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Protocol;
using Protocol.Serialization;
using Protocol.SerializationInterfaces;
using ServerGrpc.Implementations;

namespace ServerGrpc.Services
{
    public class UserManager: UserAdminService.UserAdminServiceBase
    {
        private ServiceRouter _serviceRouter;
        private readonly IDeserializer _deserializer;


        public UserManager()
        {
            _serviceRouter = new ServiceRouter();
            _deserializer = new Deserializer();
        }

        public override async Task<Message> BuyGame(BasicGameRequest request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.BuyGame,
                Data = new BasicGameRequestDTO()
                    {
                        GameId = request.GameId
                    }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);

            MessageDTO messageDto = new MessageDTO();
            messageDto.Deserialize(response.Data);

            return new Message()
            {
                Value = messageDto.Message
            };
        }

        public override async Task<UserDetail> CreateUser(Login request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.Login,
                Data = new LoginDTO()
                {
                    UserName = request.UserName
                }.Serialize()

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);

            UserDetailDTO userDetailDTO = new UserDetailDTO();
            userDetailDTO.Deserialize(response.Data);

            return new UserDetail() {
                Id = userDetailDTO.Id,
                Name = userDetailDTO.Name,
            };
        }

        public override async Task<IndexGameResponse> IndextBoughtGames(BasicUserRequest request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.IndexBoughtGames,
                Data = BitConverter.GetBytes(request.Id)

            };
            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);
            if (response.IsSuccessful())
            {
                List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response.Data, typeof(GameDetailDTO));
                List<GameDetailDTO> games = entities.Cast<GameDetailDTO>().ToList();

                IndexGameResponse mappedResponse = new IndexGameResponse() { Ok = true };
                games.ForEach(g => mappedResponse.Games.Add(new GameDetail()
                {
                    Id = g.Id,
                    Synopsis = g.Synopsis,
                    Gender = g.Gender,
                    Title = g.Title,
                    Path = g.Path,
                }));

                return mappedResponse;
            }
            else
            {

                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new IndexGameResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }
        }

        public override async Task<IndexUsersResponse> IndexUsers(Empty request, ServerCallContext context)
        {
            Frame requestFrame = new Frame()
            {
                ChosenCommand = (short)CommandConstants.IndexUsers,

            };

            Frame response = await _serviceRouter.GetResponseAsync(requestFrame);


            if (response.IsSuccessful())
            {
                List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response.Data, typeof(UserDetailDTO));
                List<UserDetailDTO> users = entities.Cast<UserDetailDTO>().ToList();

                IndexUsersResponse mappedResponse = new IndexUsersResponse() { Ok = true };
                users.ForEach(u => mappedResponse.Users.Add(new UserDetail()
                {
                    Id = u.Id,
                    Name = u.Name,
                }));

                return mappedResponse;
            }
            else
            {
                MessageDTO messageDto = new MessageDTO();
                messageDto.Deserialize(response.Data);

                return new IndexUsersResponse()
                {
                    Ok = false,
                    Error = new Error()
                    {
                        Message = messageDto.Message
                    }
                };
            }
        }
    }
}
