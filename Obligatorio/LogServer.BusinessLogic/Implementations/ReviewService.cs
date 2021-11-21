using LogsServer.BusinessLogic.Interfaces;
using LogsServer.DataAccess.Interfaces;
using LogsServer.Domain;
using LogsServer.Domain.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsServer.BusinessLogic.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly ILogRepository _logRepository;

        public ReviewService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<List<Log>> GetReviewLogsByAsync(ReviewSearchCriteria criteria)
        {
            return await _logRepository.GetLogsByAsync(criteria.MatchesCriteria);
        }
    }
}
