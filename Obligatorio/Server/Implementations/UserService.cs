using System;
using System.Collections.Generic;
using System.Linq;
using DTOs.Response;
using Protocol;
using Protocol.Serialization;
using Protocol.SerializationInterfaces;
using Server.DataAccess.Implementations;
using Server.DataAccess.Interfaces;
using Server.Interfaces;

namespace Server.Implementations
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private Serializer _serializer;

        public UserService()
        {
            _userRepository = UserRepository.GetInstance();
            _serializer = new Serializer();
        }

        public Frame BuyGame(int UserId, int GameId)
        {
            throw new NotImplementedException();
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
    }
}
