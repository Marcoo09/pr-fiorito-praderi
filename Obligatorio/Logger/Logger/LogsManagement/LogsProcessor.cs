using System;
using System.Collections.Generic;
using System.Text;
using DTOs.Response;
using Logger.Domain;
using Newtonsoft.Json;

namespace Logger.LogsManagement
{
    public class LogProcessor
    {
        public Log ProcessLog(string receivedMessage)
        {
            string[] messageComponents = receivedMessage.Split("||");
            Tag.TryParse(messageComponents[1], true, out Tag parsedLogTag);
            Type entityType = ExpectedEntityType(parsedLogTag);

            return new Log()
            {
                CreatedAt = new DateTime(Int64.Parse(messageComponents[0])),
                Tag = parsedLogTag,
                EntityType = entityType,
                Entity = JsonConvert.DeserializeObject(messageComponents[2], entityType)
            };
        }

        private Type ExpectedEntityType(Tag tag)
        {
            Type expectedType = null;
            switch (tag)
            {
                case Tag.BuyGame:
                    expectedType = typeof(GameBasicInfoDTO);
                    break;
                case Tag.IndexBoughtGames:
                    expectedType = typeof(List<GameDetailDTO>);
                    break;
                case Tag.CreateGame:
                    expectedType = typeof(GameBasicInfoDTO);
                    break;
                case Tag.CreateGameReview:
                    expectedType = typeof(GameBasicInfoDTO);
                    break;
                case Tag.DeleteGame:
                    expectedType = typeof(GameBasicInfoDTO);
                    break;
                case Tag.GetGameReviews:
                    expectedType = typeof(GameBasicInfoDTO);
                    break;
                case Tag.GetGame:
                    expectedType = typeof(EnrichedGameDetailDTO);
                    break;
                case Tag.IndexGamesCatalog:
                    expectedType = typeof(List<GameDetailDTO>);
                    break;
                case Tag.SearchGames:
                    expectedType = typeof(List<GameDetailDTO>);
                    break;
                case Tag.UpdateGame:
                    expectedType = typeof(GameBasicInfoDTO);
                    break;
                case Tag.IndexUsers:
                    expectedType = typeof(List<UserDetailDTO>);
                    break;
                case Tag.Login:
                    expectedType = typeof(UserDetailDTO);
                    break;

            };

            return expectedType;
        }
    }
}