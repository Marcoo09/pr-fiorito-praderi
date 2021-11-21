using System.Threading.Tasks;
using DTOs.Request;
using DTOs.Response;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Protocol;
using ServerGrpc.Implementations;

namespace ServerGrpc.Services
{
    public class UserManager: UserAdminService.UserAdminServiceBase
    {
        private ServiceRouter _serviceRouter;

        public UserManager()
        {
            _serviceRouter = new ServiceRouter();
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
    }
}