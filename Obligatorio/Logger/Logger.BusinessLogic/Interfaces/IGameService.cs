using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.Domain;

namespace Logger.BusinessLogic.Interfaces
{
    public interface IGameService
    {
        //TODO: Improve that type
        public Task<List<Log>> GetGamesLogsAsync(dynamic criteria);
    }
}
