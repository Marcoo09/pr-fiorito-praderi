﻿using LogsServer.Domain;
using LogsServer.Domain.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsServer.BusinessLogic.Interfaces
{
    public interface IGameService
    {
        public Task<List<Log>> GetGameLogsByAsync(GameSearchCriteria criteria);
    }
}
