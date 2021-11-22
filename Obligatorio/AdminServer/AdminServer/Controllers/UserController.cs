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
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserAdminService.UserAdminServiceClient _userClient;

        public UserController(UserAdminService.UserAdminServiceClient userClient)
        {
            _userClient = userClient;
        }

        [HttpPost("buy/{id:int}")]
        public async Task<IActionResult> BuyGame(int id)
        {
            BasicGameRequest request = new BasicGameRequest() { GameId = id };
            Message response = await _userClient.BuyGameAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel createUserRequestModel)
        {
            Login request = new Login()
            {
                UserName = createUserRequestModel.UserName
            };
            UserDetail response = await _userClient.CreateUserAsync(request);
            return Created($"api/user/{response.Id}",response);
        }

        [HttpGet("buy/{id:int}")]
        public async Task<IActionResult> IndexBoughtGames(int id)
        {
            BasicUserRequest basicUserRequest = new BasicUserRequest()
            {
                Id = id
            };


            IndexGameResponse response = await _userClient.IndextBoughtGamesAsync(basicUserRequest);
            if (response.Ok)
            {
                List<GameDetail> games = response.Games.ToList();

                return Ok(new IndextBoughtGamesResponseModel()
                {
                    Games = games
                });
            }
            else
            {
                Error error = response.Error;
                return BadRequest(error);
            }
        }

        [HttpGet]
        public async Task<IActionResult> IndexUsers()
        {
            IndexUsersResponse response = await _userClient.IndexUsersAsync(new Empty());
            if (response.Ok)
            {
                List<UserDetail> users = response.Users.ToList();

                return Ok(new IndexUsersResponseModel()
                {
                    Users = users
                });
            }
            else
            {
                Error error = response.Error;
                return BadRequest(error);
            }
        }
    }
}