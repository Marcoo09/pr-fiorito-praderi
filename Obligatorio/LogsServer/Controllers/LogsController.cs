using LogsServer.BusinessLogic.Interfaces;
using LogsServer.Domain;
using LogsServer.Domain.SearchCriteria;
using LogsServer.Models;
using LogsServer.Models.Request;
using LogsServer.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LogsServer.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IReviewService _reviewService;

        public LogsController(IGameService postService, IReviewService themeService)
        {
            _gameService = postService;
            _reviewService = themeService;
        }

        [HttpGet("review")]
        public async Task<IActionResult> GetThemeLogsAsync([FromQuery] ReviewSearchCriteria searchCriteria,
            [FromQuery] PaginationRequestModel paginationRequestModel)
        {
            int offset = paginationRequestModel.CalculateOffset();
            List<Log> retrievedLogs = await _reviewService.GetReviewLogsByAsync(searchCriteria);
            List<Log> logsToReturn = retrievedLogs.Skip(offset).Take(paginationRequestModel.PageSize).ToList();
            return Ok(logsToReturn.Select(l => new ReviewLogResponseModel(l, paginationRequestModel, retrievedLogs.Count)));
        }

        [HttpGet("game")]
        public async Task<IActionResult> GetPostsLogsAsync([FromQuery] GameSearchCriteria searchCriteria,
            [FromQuery] PaginationRequestModel paginationRequestModel)
        {
            int offset = paginationRequestModel.CalculateOffset();
            List<Log> retrievedLogs = await _gameService.GetGameLogsByAsync(searchCriteria);
            List<Log> logsToReturn = retrievedLogs.Skip(offset).Take(paginationRequestModel.PageSize).ToList();
            return Ok(logsToReturn.Select(l => new GameLogResponseModel(l, paginationRequestModel, retrievedLogs.Count)));
        }
    }
}
