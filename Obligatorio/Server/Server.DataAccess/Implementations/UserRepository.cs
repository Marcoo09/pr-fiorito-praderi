using System;
using System.Collections.Generic;
using Exceptions;
using Server.DataAccess.Interfaces;
using Server.Domain;

namespace Server.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users;
        private Object _usersLocker;
        private int _nextId;
        private static UserRepository _instance;
        private static Object _instanceLocker = new Object();

        public UserRepository()
        {
            _users = new List<User>();
            _usersLocker = new Object();
            _nextId = 1;
        }

        public static UserRepository GetInstance()
        {
            lock (_instanceLocker)
            {
                if (_instance == null)
                    _instance = new UserRepository();

                return _instance;
            }
        }

        public int Insert(User user)
        {
            lock (_usersLocker)
            {
                User newUser = new User()
                {
                    Id = GetAvailableId(),
                    Name = user.Name,
                };
                _users.Add(newUser);

                return newUser.Id;
            }
        }

        public User Get(int id)
        {
            lock (_usersLocker)
            {
                User user = _users.Find(u => u.Id == id);

                if (user == null)
                    throw new ResourceNotFoundException("User does not exist");
                return user;
            }
        }

        public List<User> GetAll()
        {
            lock (_usersLocker)
            {
                return new List<User>(_users);
            }
        }

        public void Delete(int id)
        {
            lock (_usersLocker)
            {
                User userToRemove = Get(id);
                _users.Remove(userToRemove);
            }
        }

        private int GetAvailableId()
        {
            return _nextId++;
        }

        public void BuyGame(Game game, int userId)
        {
            lock (_usersLocker)
            {
                User userToAddGame = Get(userId);
                userToAddGame.BuyGame(game);
            }
        }
    }
}
