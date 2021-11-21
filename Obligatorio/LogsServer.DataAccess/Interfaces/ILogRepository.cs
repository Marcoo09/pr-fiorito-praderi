using LogsServer.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace LogsServer.DataAccess.Interfaces
{
    public interface ILogRepository
    {
        Task StoreAsync(Log log);

        Task<List<Log>> GetLogsByAsync(Func<Log, bool> criteria);
    }
}
