using System;
using System.Collections.Generic;
using System.Linq;
using Logger.Domain;
using Logger.Domain.Interfaces;
using DTOs.Response;

namespace LogsServer.Domain.SearchCriteria
{
    public class UserSearchCriteria : ISearchCriteria<Log>
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public bool MatchesCriteria(Log logUser)
        {
            bool matchesCriteria = false;

            if (logUser.EntityType == typeof(UserDetailDTO) || logUser.EntityType == typeof(List<UserDetailDTO>))
            {
                matchesCriteria = MatchesId(logUser) && MatchesName(logUser) &&
                       MatchesLogTag(logUser);
            }

            return matchesCriteria;
        }

        private bool MatchesId(Log logUser)
        {
            bool matchesId = true;

            if (Id != null)
            {
                if (logUser.IsEntityAList())
                {
                    List<UserDetailDTO> users = logUser.Entity as List<UserDetailDTO>;
                    matchesId = users.Any(t => t.Id == Id);
                }
                else
                {
                    UserDetailDTO user = logUser.Entity as UserDetailDTO;
                    matchesId = user.Id == Id;
                }
            }

            return matchesId;
        }

        private bool MatchesName(Log logUser)
        {
            bool matchesName = true;

            if (!String.IsNullOrEmpty(Name))
            {
                if (logUser.IsEntityAList())
                {
                    List<UserDetailDTO> users = logUser.Entity as List<UserDetailDTO>;
                    matchesName = users.Any(u => u.Name.ToLower().Contains(Name.ToLower()));
                }
                else
                {
                    UserDetailDTO user = logUser.Entity as UserDetailDTO;
                    matchesName = user.Name.ToLower().Contains(Name.ToLower());
                }
            }

            return matchesName;
        }

        private bool MatchesLogTag(Log logGame)
        {
            return logGame.Tag == Tag.Login || logGame.Tag == Tag.IndexUsers;
        }
    }
}