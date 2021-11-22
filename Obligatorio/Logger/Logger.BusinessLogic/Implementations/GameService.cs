using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.BusinessLogic.Interfaces;
using Logger.DataAccess.Interfaces;
using Logger.Domain;

namespace Logger.BusinessLogic.Implementations
{
    public class GameService : IGameService
    {
        private readonly ILogRepository _logRepository;

        public GameService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<List<Log>> GetGamesLogsAsync(dynamic criteria)
        {
            return await _logRepository.GetLogsByAsync(criteria.MatchesCriteria);
        }
    }
}
