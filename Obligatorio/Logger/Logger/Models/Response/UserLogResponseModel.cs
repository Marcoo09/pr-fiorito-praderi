using System.Collections.Generic;
using DTOs.Response;
using Logger.Domain;

namespace Logger.Models.Response
{
    public class UserLogResponseModel : LogResponseModel
    {
        public object User { get; set; }
        public List<object> Users { get; set; }

        public UserLogResponseModel(Log log)
        {
            Tag = FormatEnumString(log.Tag.ToString());
            CreatedAt = log.CreatedAt;

            if (log.IsEntityAList())
            {
                Users = log.Entity as List<object>;
            }
            else
            {
                User = log.Entity;
            }
        }
    }

}
