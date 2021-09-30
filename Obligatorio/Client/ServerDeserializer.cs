using System;
using System.Collections.Generic;
using System.Linq;
using DTOs.Response;
using Protocol;
using Protocol.Serialization;
using Protocol.SerializationInterfaces;

namespace Client
{
    public class ServerDeserializer
    {
        private Deserializer _deserializer;

        public ServerDeserializer()
        {
            _deserializer = new Deserializer();
        }

        public string DeserializeResponse(Frame responseFrame)
        {
            string deserializedResponse = null;
            byte[] response = responseFrame.Data;
            CommandConstants chosenCommand = (CommandConstants)responseFrame.ChosenCommand;
            Type expectedEntityTypeResponse = EntityTypeResponse(chosenCommand);

            if (responseFrame.IsSuccessful())
            {
                if (IsResponseAnArray(chosenCommand))
                {
                    List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response, expectedEntityTypeResponse);
                    List<string> entitiesToShow = entities.Select(entity => entity.ToString()).ToList();
                    deserializedResponse = String.Join("\n", entitiesToShow);
                }
                else
                {
                    IDeserializable entity = _deserializer.DeserializeEntity(response, expectedEntityTypeResponse);
                    if (IsReturningAndImage(chosenCommand))
                    {
                        EnrichedGameDetailDTO enrichedGameDetailDTO = (EnrichedGameDetailDTO)entity;
                        enrichedGameDetailDTO.WriteFile();

                    }
                    deserializedResponse = entity.ToString();
                }
            }
            else
            {
                ErrorDTO errorDto = new ErrorDTO();
                errorDto.Deserialize(responseFrame.Data);
                deserializedResponse = errorDto.ToString();
            }

            return deserializedResponse;
        }

        private Type EntityTypeResponse(CommandConstants command)
        {
            Type entityType = null;

            switch (command)
            {
                case CommandConstants.BuyGame:
                    entityType = typeof(MessageDTO);
                    break;
                case CommandConstants.CreateGame:
                    entityType = typeof(GameBasicInfoDTO);
                    break;
                case CommandConstants.CreateGameReview:
                    entityType = typeof(MessageDTO);
                    break;
                case CommandConstants.DeleteGame:
                    entityType = typeof(MessageDTO);
                    break;
                case CommandConstants.GetGameReviews:
                    entityType = typeof(ReviewDetailDTO);
                    break;
                case CommandConstants.GetGame:
                    entityType = typeof(EnrichedGameDetailDTO);
                    break;
                case CommandConstants.IndexGamesCatalog:
                    entityType = typeof(GameDetailDTO);
                    break;
                case CommandConstants.SearchGames:
                    entityType = typeof(GameDetailDTO);
                    break;
                case CommandConstants.UpdateGame:
                    entityType = typeof(GameBasicInfoDTO);
                    break;
                case CommandConstants.IndexUsers:
                    entityType = typeof(UserDetailDTO);
                    break;
                case CommandConstants.IndexBoughtGames:
                    entityType = typeof(GameDetailDTO);
                    break;
                case CommandConstants.Login:
                    entityType = typeof(UserDetailDTO);
                    break;
            }

            return entityType;
        }

        private bool IsResponseAnArray(CommandConstants command)
        {
            bool isArray = false;

            switch (command)
            {
                case CommandConstants.IndexGamesCatalog:
                    isArray = true;
                    break;
                case CommandConstants.GetGameReviews:
                    isArray = true;
                    break;
                case CommandConstants.SearchGames:
                    isArray = true;
                    break;
                case CommandConstants.IndexUsers:
                    isArray = true;
                    break;
                case CommandConstants.IndexBoughtGames:
                    isArray = true;
                    break;
            }

            return isArray;
        }

        private bool IsReturningAndImage(CommandConstants command)
        {
            bool isReturningAndImage = false;

            switch (command)
            {
                case CommandConstants.GetGame:
                    isReturningAndImage = true;
                    break;
            }

            return isReturningAndImage;
        }
    }
}