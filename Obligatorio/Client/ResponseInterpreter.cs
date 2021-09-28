using System;
using System.Collections.Generic;
using System.Linq;
using DTOs.Response;
using Protocol;
using Protocol.Serialization;
using Protocol.SerializationInterfaces;

namespace Client
{
    public class ResponseInterpreter
    {
        private Deserializer _deserializer;

        public ResponseInterpreter()
        {
            _deserializer = new Deserializer();
        }

        public string InterpretResponse(Frame responseFrame)
        {
            string interpretedResponse = null;
            byte[] response = responseFrame.Data;
            Command chosenCommand = (Command)responseFrame.ChosenCommand;
            Type expectedEntityTypeResponse = EntityTypeResponse(chosenCommand);

            if (responseFrame.IsSuccessful())
            {
                if (IsResponseAnArray(chosenCommand))
                {
                    List<IDeserializable> entities = _deserializer.DeserializeArrayOfEntities(response, expectedEntityTypeResponse);
                    List<string> entitiesToShow = entities.Select(entity => entity.ToString()).ToList();
                    interpretedResponse = String.Join("\n", entitiesToShow);
                }
                else
                {
                    IDeserializable entity = _deserializer.DeserializeEntity(response, expectedEntityTypeResponse);
                    interpretedResponse = entity.ToString();
                }
            }
            else
            {
                ErrorDTO errorDto = new ErrorDTO();
                errorDto.Deserialize(responseFrame.Data);
                interpretedResponse = errorDto.ToString();
            }

            return interpretedResponse;
        }

        private Type EntityTypeResponse(Command command)
        {
            Type entityType = null;

            switch (command)
            {
                case Command.BuyGame:
                    entityType = typeof(MessageDTO);
                    break;
                case Command.CreateGame:
                    entityType = typeof(GameBasicInfoDTO);
                    break;
                case Command.CreateGameReview:
                    entityType = typeof(MessageDTO);
                    break;
                case Command.DeleteGame:
                    //Do sth
                    break;
                case Command.GetGameReviews:
                    entityType = typeof(ReviewDetailDTO);
                    break;
                case Command.IndexGame:
                    entityType = typeof(GameDetailDTO);
                    break;
                case Command.IndexGamesCatalog:
                    entityType = typeof(GameDetailDTO);
                    break;
                case Command.SearchGames:
                    //Do sth
                    break;
                case Command.UpdateGame:
                    //Do sth
                    break;
                case Command.IndexUsers:
                    entityType = typeof(UserDetailDTO);
                    break;
                case Command.IndexBoughtGames:
                    entityType = typeof(GameDetailDTO);
                    break;
            }

            return entityType;
        }

        private bool IsResponseAnArray(Command command)
        {
            bool isArray = false;

            switch (command)
            {
                case Command.IndexGamesCatalog:
                    isArray = true;
                    break;
                case Command.GetGameReviews:
                    isArray = true;
                    break;
                case Command.SearchGames:
                    isArray = true;
                    break;
                case Command.IndexUsers:
                    isArray = true;
                    break;
                case Command.IndexBoughtGames:
                    isArray = true;
                    break;
            }

            return isArray;
        }
    }
}