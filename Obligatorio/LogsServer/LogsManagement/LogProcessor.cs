using LogsServer.Domain;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using DTOs.Response;

namespace LogsServer.LogsManagement
{
    public class LogProcessor
    {
        public Log ProcessLog(string receivedMessage)
        {
            string[] messageComponents = receivedMessage.Split("||");
            LogTag.TryParse(messageComponents[1], true, out LogTag parsedLogTag);
            Type entityType = ExpectedEntityType(parsedLogTag);

            return new Log()
            {
                CreatedAt = new DateTime(Int64.Parse(messageComponents[0])),
                LogTag = parsedLogTag,
                EntityType = entityType,
                Entity = JsonConvert.DeserializeObject(messageComponents[2], entityType)
            };
        }

        private Type ExpectedEntityType(LogTag logTag)
        {
            Type expectedType = null;
            switch (logTag)
            {
                case LogTag.IndexReview:
                    expectedType = typeof(List<ReviewDetailDTO>);
                    break;
                case LogTag.CreateGame:
                    expectedType = typeof(GameDetailDTO);
                    break;
                case LogTag.DeleteGame:
                    expectedType = typeof(GameDetailDTO);
                    break;
                case LogTag.ShowGame:
                    expectedType = typeof(GameDetailDTO);
                    break;
                case LogTag.UpdateGame:
                    expectedType = typeof(GameDetailDTO);
                    break;
                case LogTag.ChangeGameReview:
                    expectedType = typeof(GameDetailDTO);
                    break;
                case LogTag.CreateReview:
                    expectedType = typeof(ReviewDetailDTO);
                    break;
                case LogTag.DeleteReview:
                    expectedType = typeof(ReviewDetailDTO);
                    break;
                case LogTag.UpdateReview:
                    expectedType = typeof(ReviewDetailDTO);
                    break;
                case LogTag.IndexGamesByReview:
                    expectedType = typeof(List<GameDetailDTO>);
                    break;
            };

            return expectedType;
        }
    }
}
