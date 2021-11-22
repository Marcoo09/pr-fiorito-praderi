using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.BusinessLogic.Interfaces;
using Logger.DataAccess.Interfaces;
using Logger.Domain;
using LogsServer.Domain.SearchCriteria;

namespace Logger.BusinessLogic.Implementations
{
    public class GameService : IGameService
    {
        private readonly ILogRepository _logRepository;

        public GameService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<List<Log>> GetGamesLogsAsync(GameSearchCriteria criteria)
        {
            return await _logRepository.GetLogsByAsync(criteria.MatchesCriteria);
        }
    }
}
