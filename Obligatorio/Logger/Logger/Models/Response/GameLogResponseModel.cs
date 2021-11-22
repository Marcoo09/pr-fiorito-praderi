using Logger.Domain;
using DTOs.Response;
using System.Collections.Generic;

namespace Logger.Models.Response
{
    public class GameLogResponseModel : LogResponseModel
    {
        public object Game { get; set; }
        public List<object> Games { get; set; }

        public GameLogResponseModel(Log log)
        {
            Tag = FormatEnumString(log.Tag.ToString());
            CreatedAt = log.CreatedAt;

            if (log.IsEntityAList())
            {
                Games = log.Entity as List<object>;
            }
            else
            {
                Game = log.Entity;
            }
        }
    }
}
