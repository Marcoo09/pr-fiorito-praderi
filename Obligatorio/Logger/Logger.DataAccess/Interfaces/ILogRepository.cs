using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.Domain;

namespace Logger.DataAccess.Interfaces
{
    public interface ILogRepository
    {
        Task StoreAsync(Log log);

        Task<List<Log>> GetLogsByAsync(Func<Log, bool> criteria);
    }
}
