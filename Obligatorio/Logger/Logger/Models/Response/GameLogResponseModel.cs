using Logger.Domain;
using DTOs.Response;
using System.Collections.Generic;

namespace Logger.Models.Response
{
    public class GameLogResponseModel : LogResponseModel
    {
        public GameDetailDTO Game { get; set; }
        public List<GameDetailDTO> Games { get; set; }

        public GameLogResponseModel(Log log)
        {
            Tag = FormatEnumString(log.Tag.ToString());
            CreatedAt = log.CreatedAt;

            if (log.IsEntityAList())
            {
                List<GameDetailDTO> logGames = log.Entity as List<GameDetailDTO>;
                Games = logGames;
            }
            else
            {
                GameDetailDTO logGame = log.Entity as GameDetailDTO;
                Game = logGame;
            }
        }
    }
}
