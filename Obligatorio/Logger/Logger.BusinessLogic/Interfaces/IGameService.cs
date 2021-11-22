using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.Domain;
using LogsServer.Domain.SearchCriteria;

namespace Logger.BusinessLogic.Interfaces
{
    public interface IGameService
    {        
        public Task<List<Log>> GetGamesLogsAsync(GameSearchCriteria criteria);
    }
}
