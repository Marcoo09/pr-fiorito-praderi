using DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogsServer.Domain.SearchCriteria
{
    public class GameSearchCriteria
    {
        public int? GameId { get; set; }
        public int? ReviewId { get; set; }
        public string GameName { get; set; }

        public bool MatchesCriteria(Log logGame)
        {
            bool matchesCriteria = false;

            if (logGame.EntityType == typeof(GameDetailDTO) || logGame.EntityType == typeof(List<GameDetailDTO>))
            {
                matchesCriteria = MatchesGameId(logGame) && MatchesReviewId(logGame) && MatchesGameName(logGame) &&
                       MatchesLogTag(logGame);
            }

            return matchesCriteria;
        }

        private bool MatchesGameId(Log logGame)
        {
            bool matchesGameId = true;

            if (GameId != null)
            {
                if (logGame.IsEntityAList())
                {
                    List<GameDetailDTO> game = logGame.Entity as List<GameDetailDTO>;
                    matchesGameId = game.Any(g => g.Id == GameId);
                }
                else
                {
                    GameDetailDTO game = logGame.Entity as GameDetailDTO;
                    matchesGameId = game.Id == GameId;
                }
            }

            return matchesGameId;
        }

        private bool MatchesGameName(Log logGame)
        {
            bool matchesGameName = true;

            if (!String.IsNullOrEmpty(GameName))
            {
                if (logGame.IsEntityAList())
                {
                    List<GameDetailDTO> game = logGame.Entity as List<GameDetailDTO>;
                    matchesGameName = game.Any(g => g.Title.ToLower().Contains(GameName.ToLower()));
                }
                else
                {
                    GameDetailDTO game = logGame.Entity as GameDetailDTO;
                    matchesGameName = game.Title.ToLower().Contains(GameName.ToLower());
                }
            }

            return matchesGameName;
        }

        private bool MatchesReviewId(Log logGame)
        {
            bool matchesReviewId = true;

            if (ReviewId != null)
            {
                if (logGame.IsEntityAList())
                {
                    List<GameDetailDTO> games = logGame.Entity as List<GameDetailDTO>;
                    //TODO    matchesReviewId = games.Any(g => g.Review.Id == ReviewId); 
                }
                else
                {
                    GameDetailDTO game = logGame.Entity as GameDetailDTO;
                    //TODO   matchesReviewId = game.Review.Id == ReviewId;
                }
            }

            return matchesReviewId;
        }

        private bool MatchesLogTag(Log logGame)
        {
            return logGame.LogTag == LogTag.CreateGame || logGame.LogTag == LogTag.DeleteGame ||
                   logGame.LogTag == LogTag.ShowGame || logGame.LogTag == LogTag.UpdateGame ||
                   logGame.LogTag == LogTag.ChangeGameReview || logGame.LogTag == LogTag.IndexGamesByReview;
        }
    }
}
