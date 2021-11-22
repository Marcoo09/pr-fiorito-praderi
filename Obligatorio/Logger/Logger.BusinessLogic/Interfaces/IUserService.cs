using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.Domain;

namespace Logger.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        public Task<List<Log>> GetUsersLogsAsync(dynamic criteria);
    }
}
