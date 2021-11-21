using System;
using System.Collections.Generic;
using System.Text;

namespace LogsServer.Domain.SearchCriteria
{
    public interface ISearchCriteria<T>
    {
        public bool MatchesCriteria(T entity);
    }
}
