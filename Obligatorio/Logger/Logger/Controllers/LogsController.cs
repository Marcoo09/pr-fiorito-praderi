using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logger.BusinessLogic.Interfaces;
using Logger.Domain;
using Microsoft.AspNetCore.Mvc;
using Logger.Models.Response;
using LogsServer.Domain.SearchCriteria;

namespace Logger.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGameService _gameService;

        public LogsController(IUserService userService, IGameService gameService)
        {
            _gameService = gameService;
            _userService = userService;
        }

        [HttpGet("games")]
        public async Task<IActionResult> GetGamesLogsAsync([FromQuery] GameSearchCriteria searchCriteria)
        {
            List<Log> logs = await _gameService.GetGamesLogsAsync(searchCriteria);
            return Ok(logs.Select(l => new GameLogResponseModel(l)));
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersLogsAsync([FromQuery] UserSearchCriteria searchCriteria)
        {
            List<Log> logs = await _userService.GetUsersLogsAsync(searchCriteria);
            return Ok(logs.Select(l => new UserLogResponseModel(l)));
        }
    }
}
