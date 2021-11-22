using System.Collections.Generic;
using DTOs.Response;
using Logger.Domain;

namespace Logger.Models.Response
{
    public class UserLogResponseModel : LogResponseModel
    {
        public UserDetailDTO User { get; set; }
        public List<UserDetailDTO> Users { get; set; }

        public UserLogResponseModel(Log log)
        {
            Tag = FormatEnumString(log.Tag.ToString());
            CreatedAt = log.CreatedAt;

            if (log.IsEntityAList())
            {
                List<UserDetailDTO> logUsers = log.Entity as List<UserDetailDTO>;
                Users = logUsers;
            }
            else
            {
                UserDetailDTO logUser = log.Entity as UserDetailDTO;
                User = logUser;
            }
        }
    }

}
