﻿using System;
using System.Collections.Generic;
using System.Linq;
using DTOs.Response;
using Protocol;
using Protocol.Serialization;

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
                case Command.IndexClients:
                    //Do sth
                    break;
                case Command.BuyGame:
                    //Do sth
                    break;
                case Command.CreateGame:
                    //Do sth
                    break;
                case Command.CreateGameReview:
                    //Do sth
                    break;
                case Command.DeleteGame:
                    //Do sth
                    break;
                case Command.GetGameReviews:
                    //Do sth
                    break;
                case Command.IndexGame:
                    //Do sth
                    break;
                case Command.IndexGamesCatalog:
                    //Do sth
                    break;
                case Command.SearchGames:
                    //Do sth
                    break;
                case Command.UpdateGame:
                    //Do sth
                    break;
            }

            return entityType;
        }

        private bool IsResponseAnArray(Command command)
        {
            bool isArray = false;

            switch (command)
            {
                case Command.IndexClients:
                    isArray = true;
                    break;
                case Command.GetGameReviews:
                    isArray = true;
                    break;
                case Command.SearchGames:
                    isArray = true;
                    break;
            }

            return isArray;
        }
    }
}