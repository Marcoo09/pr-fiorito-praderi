using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.BusinessLogic.Interfaces;
using Logger.DataAccess.Interfaces;
using Logger.Domain;
using LogsServer.Domain.SearchCriteria;

namespace Logger.BusinessLogic.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogRepository _logRepository;

        public UserService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<List<Log>> GetUsersLogsAsync(UserSearchCriteria criteria)
        {
            return await _logRepository.GetLogsByAsync(criteria.MatchesCriteria);
        }
    }
}
