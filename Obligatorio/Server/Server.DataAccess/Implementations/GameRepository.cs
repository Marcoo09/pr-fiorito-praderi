using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Exceptions;
using Server.DataAccess.Interfaces;
using Server.Domain;

namespace Server.DataAccess.Implementations
{
    public class GameRepository: IGameRepository
    {
        private static GameRepository _instance;
        private readonly List<Game> _games;
        private readonly SemaphoreSlim _gamesSemaphore;
        private static readonly SemaphoreSlim _instanceSemaphore = new SemaphoreSlim(1);
        private int _nextId;

        public GameRepository()
        {
            _gamesSemaphore = new SemaphoreSlim(1);
            _games = new List<Game>();
            _nextId = 1;
        }

        public async Task DeleteAsync(int id)
        {
            
                Game gameToRemove = await GetAsync(id);
                await _gamesSemaphore.WaitAsync();

                 _games.Remove(gameToRemove);

                _gamesSemaphore.Release();

        }

        public async Task<Game> GetAsync(int id)
        {
            await _gamesSemaphore.WaitAsync();
            Game game = _games.Find(g => g.Id == id);
            _gamesSemaphore.Release();

            if (game == null)
                    throw new ResourceNotFoundException("Game not exist");
                return game;
            
        }

        public async Task<List<Game>> GetAllAsync()
        {

            await _gamesSemaphore.WaitAsync();
            List<Game> copy = new List<Game>(_games);
            _gamesSemaphore.Release();
            return copy;
            
        }

        public async Task<List<Game>> GetByAsync(Func<Game, bool> predicate)
        {
            await _gamesSemaphore.WaitAsync();

            List <Game> games =  _games.Where(predicate).ToList();

            _gamesSemaphore.Release();

            return games;
        }       

        public async Task<int> InsertAsync(Game game)
        {
            await _gamesSemaphore.WaitAsync();

            if (_games.Any(g => g.Title == game.Title))
                    throw new InvalidResourceException("Game name need to be unique");
                game.ValidOrFail();

                Game newGame = new Game()
                {
                    Id = GetAvailableId(),
                    Title = game.Title,
                    Gender = game.Gender,
                    Synopsis = game.Synopsis,
                    Path = game.Path,
                    FileSize = game.FileSize,
                    CoverName = game.CoverName
                };
                _games.Add(newGame);

                    UpdateNextAvailableId();

                _gamesSemaphore.Release();

            return newGame.Id;
            
        }

        public async Task UpdateAsync(int id, Game game)
        {
            Game gameToBeUpdated = await GetAsync(id);

            await _gamesSemaphore.WaitAsync();

            if (_games.Any(g => g.Title == game.Title && g.Id != game.Id)) {
                _gamesSemaphore.Release();
                throw new InvalidResourceException("Game name need to be unique");
               }

            try { 
                game.Path = gameToBeUpdated.Path;
                game.ValidOrFail();

                gameToBeUpdated.Title = game.Title;
                gameToBeUpdated.Synopsis = game.Synopsis;
                gameToBeUpdated.Gender = game.Gender;
                _gamesSemaphore.Release();
            }
            catch (InvalidResourceException e)
            {
                _gamesSemaphore.Release();
                throw new InvalidResourceException(e.Message);
            }

        }

        public static GameRepository GetInstance()
        {
            _instanceSemaphore.Wait();
            if (_instance == null)
                    _instance = new GameRepository();
            
            _instanceSemaphore.Release();
            return _instance;
            

        }

        private int GetAvailableId()
        {
            return _nextId;
        }

        private void UpdateNextAvailableId()
        {
            _nextId++;
        }

        public async Task AddReviewAsync(int id, Review review)
        {
            Game gameToBeUpdated = await GetAsync(id);

            await _gamesSemaphore.WaitAsync();

            review.ValidOrFail();
                gameToBeUpdated.AddReview(review);

            _gamesSemaphore.Release();
        }

        public async Task<List<Review>> GetAllReviewsAsync(int id)
        {
            await _gamesSemaphore.WaitAsync();
            Game gameToSeeReviews =  await GetAsync (id);
            _gamesSemaphore.Release();

            return gameToSeeReviews.Reviews;
        }
    }
}
