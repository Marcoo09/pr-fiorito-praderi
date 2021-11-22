using Logger.Domain;
using DTOs.Response;
using System.Collections.Generic;
using System.Collections;

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
                List<object> list = new List<object>();

                IList collection = (IList)log.Entity;
                foreach (object item in collection)
                {
                    list.Add(item);
                }
                Games = list;
            }
            else
            {
                Game = log.Entity;
            }
        }
    }
}
