using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminServer.Models.Request;
using AdminServer.Models.Response;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace AdminServer.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {

        private readonly GameAdminService.GameAdminServiceClient _gameClient;

        public GameController(GameAdminService.GameAdminServiceClient gameClient)
        {
            _gameClient = gameClient;
        }
    }
}
