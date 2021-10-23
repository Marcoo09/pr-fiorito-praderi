using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        //private static Object _instanceLocker = new Object();
        private readonly SemaphoreSlim _usersSemaphore;
        private static readonly SemaphoreSlim _instanceSemaphore = new SemaphoreSlim(1);


        public UserRepository()
        {
            _users = new List<User>();
            _usersLocker = new Object();
            _nextId = 1;
        }

        public static UserRepository GetInstanceAsync()
        {
            _instanceSemaphore.Wait();
            if (_instance == null)
                _instance = new UserRepository();
            _instanceSemaphore.Release();
            return _instance;

        }

        public async Task<int> InsertAsync(User user)
        {
            await _usersSemaphore.WaitAsync();

                User newUser = new User()
                {
                    Id = GetAvailableId(),
                    Name = user.Name,
                };

                 _users.Add(newUser);

            _usersSemaphore.Release();

            return newUser.Id;

        }

        public async Task<User> GetAsync(int id)
        {
            await _usersSemaphore.WaitAsync();

            User user = _users.Find(u => u.Id == id);

            if (user == null)
                throw new ResourceNotFoundException("User does not exist");

            _usersSemaphore.Release();

            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            await _usersSemaphore.WaitAsync();
                List <User> users = new List<User>(_users);
            _usersSemaphore.Release();

            return users;
        }

        public async Task DeleteAsync(int id)
        {
            await _usersSemaphore.WaitAsync();
                User userToRemove = await GetAsync(id);
                _users.Remove(userToRemove);
            _usersSemaphore.Release();


        }

        private int GetAvailableId()
        {
            return _nextId++;
        }

        public async Task BuyGameAsync(Game game, int userId)
        {
            await _usersSemaphore.WaitAsync();
            User userToAddGame = await GetAsync(userId);
            userToAddGame.BuyGame(game);
            _usersSemaphore.Release();

        }
    }
}
