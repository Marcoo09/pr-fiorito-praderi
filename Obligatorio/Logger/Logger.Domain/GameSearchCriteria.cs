using System;
using System.Collections.Generic;
using System.Linq;
using Logger.Domain;
using Logger.Domain.Interfaces;
using DTOs.Response;

namespace LogsServer.Domain.SearchCriteria
{
    public class GameSearchCriteria : ISearchCriteria<Log>
    {
        public int? Id { get; set; }
        public string Title { get; set; }

        public bool MatchesCriteria(Log logGame)
        {
            bool matchesCriteria = false;

            if (logGame.EntityType == typeof(GameDetailDTO) || logGame.EntityType == typeof(List<GameBasicInfoDTO>)
                    || logGame.EntityType == typeof(GameBasicInfoDTO) || logGame.EntityType == typeof(List<GameDetailDTO>))
            {
                matchesCriteria = MatchesId(logGame) && MatchesTitle(logGame) &&
                       MatchesLogTag(logGame);
            }

            return matchesCriteria;
        }

        private bool MatchesId(Log logGame)
        {
            bool matchesId = true;

            if (Id != null)
            {
                if (logGame.IsEntityAList())
                {
                    List<GameBasicInfoDTO> themes = logGame.Entity as List<GameBasicInfoDTO>;
                    matchesId = themes.Any(t => t.Id == Id);
                }
                else
                {
                    GameBasicInfoDTO theme = logGame.Entity as GameBasicInfoDTO;
                    matchesId = theme.Id == Id;
                }
            }

            return matchesId;
        }

        private bool MatchesTitle(Log logGame)
        {
            bool matchesTitle = true;

            if (!String.IsNullOrEmpty(Title))
            {
                if (logGame.IsEntityAList())
                {
                    List<GameBasicInfoDTO> games = logGame.Entity as List<GameBasicInfoDTO>;
                    matchesTitle = games.Any(t => t.Title.ToLower().Contains(Title.ToLower()));
                }
                else
                {
                    GameBasicInfoDTO game = logGame.Entity as GameBasicInfoDTO;
                    matchesTitle = game.Title.ToLower().Contains(Title.ToLower());
                }
            }

            return matchesTitle;
        }

        private bool MatchesLogTag(Log logGame)
        {
            return logGame.Tag == Tag.CreateGame || logGame.Tag == Tag.IndexBoughtGames ||
                   logGame.Tag == Tag.GetGame || logGame.Tag == Tag.IndexGamesCatalog ||
                   logGame.Tag == Tag.SearchGames || logGame.Tag == Tag.BuyGame ||
                   logGame.Tag == Tag.CreateGameReview || logGame.Tag == Tag.DeleteGame;
        }
    }
}