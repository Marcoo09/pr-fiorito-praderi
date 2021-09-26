﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exceptions;
using Server.DataAccess.Interfaces;
using Server.Domain;

namespace Server.DataAccess.Implementations
{
    public class GameRepository: IGameRepository
    {
        private static GameRepository _instance;
        private readonly List<Game> _games;
        private object _gameLocker;
        private static Object _instanceLocker = new Object();
        private int _nextId;

        public GameRepository()
        {
            _gameLocker = new Object();
            _games = new List<Game>();
            _nextId = 1;
        }

        public void Delete(int id)
        {
            lock (_gameLocker)
            {
                Game gameToRemove = Get(id);
                _games.Remove(gameToRemove);
            }
        }

        public Game Get(int id)
        {
            lock (_gameLocker)
            {
                Game game = _games.Find(g => g.Id == id);

                if (game == null)
                    throw new ResourceNotFoundException("Game not exist");
                return game;
            }
        }

        public List<Game> GetAll()
        {
            lock (_gameLocker)
            {
                List<Game> copy = new List<Game>(_games);
                return copy;
            }
        }

        public List<Game> GetBy(Func<Game, bool> predicate)
        {
            lock (_gameLocker)
            {
                return _games.Where(predicate).ToList();
            }
        }

        public List<Document> GetCovers()
        {
            lock (_gameLocker)
            {
                List<Document> covers = _games.Select(g => g.Cover).ToList();
                return covers.Where(c => c != null).ToList();
            }
        }

        public int Insert(Game game)
        {
            lock (_gameLocker)
            {
                if (_games.Any(g => g.Title == game.Title))
                    throw new InvalidResourceException("Game name need to be unique");
                game.ValidOrFail();

                Game newGame = new Game()
                {
                    Id = GetAvailableId(),
                    Title = game.Title,
                    Gender = game.Gender,
                    Synopsis = game.Synopsis,
                };
                _games.Add(newGame);

                UpdateNextAvailableId();

                return newGame.Id;
            }
        }

        public void Update(int id, Game game)
        {
            Game gameToBeUpdated = Get(id);

            lock (_gameLocker)
            {
                if (_games.Any(g => g.Title == game.Title && g.Id != game.Id))
                    throw new InvalidResourceException("Game name need to be unique");
                game.ValidOrFail();

                gameToBeUpdated.Title = game.Title;
                gameToBeUpdated.Synopsis = game.Synopsis;
                gameToBeUpdated.Gender = game.Gender;
                gameToBeUpdated.Cover = game.Cover;
            }
        }

        public static GameRepository GetInstance()
        {
            lock (_instanceLocker)
            {
                if (_instance == null)
                    _instance = new GameRepository();

                return _instance;
            }
        }

        private int GetAvailableId()
        {
            return _nextId;
        }

        private void UpdateNextAvailableId()
        {
            _nextId++;
        }
    }
}
