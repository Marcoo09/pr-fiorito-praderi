using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminServer.Models.Request;
using AdminServer.Models.Response;
using Google.Protobuf;
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

        [HttpPost("review")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewRequestModel addReviewRequestModel)
        {
            Review request = new Review()
            {
                GameId = addReviewRequestModel.GameId,
                Description = addReviewRequestModel.Description,
                Rating = addReviewRequestModel.Rating,
            };
            Message response = await _gameClient.AddReviewAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequestModel createGameRequestModel)
        {
            CreateGameRequest request = new CreateGameRequest()
            {
                CoverName = createGameRequestModel.CoverName,
                Gender = createGameRequestModel.Gender,
                Synopsis = createGameRequestModel.Synopsis,
                //FileSize = createGameRequestModel.FileSize,
                Title = createGameRequestModel.Title,
                //Data = ByteString.CopyFrom(createGameRequestModel.Data),
            };
            GameBasicInfoResponse response = await _gameClient.CreateGameAsync(request);
            return Created($"api/game/{response.Id}", response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            BasicGameRequest request = new BasicGameRequest()
            {
                GameId = id
            };

            Message response = await _gameClient.DeleteGameAsync(request);
            return Ok(response);
        }

        [HttpGet("review/{id:int}")]
        public async Task<IActionResult> GetAllReviews(int id)
        {
            BasicGameRequest basicGameRequest = new BasicGameRequest()
            {
                GameId = id
            };


            IndexReviewResponse response = await _gameClient.GetAllReviewsAsync(basicGameRequest);
            if (response.Ok)
            {
                List<ReviewDetail> reviews = response.Reviews.ToList();

                return Ok(new IndexReviewsResponseModel()
                {
                    Reviews = reviews
                });
            }
            else
            {
                Error error = response.Error;
                return BadRequest(error);
            }
        }

        [HttpGet("search/title")]
        public async Task<IActionResult> SearchGamesByTitle([FromBody] SearchMetricRequestModel searchMetricRequestModel)
        {
            SearchTitleMetric searchMetric = new SearchTitleMetric()
            {
                Title = searchMetricRequestModel.Title
            };


            SearchGameResponse response = await _gameClient.SearchGameByTitleAsync(searchMetric);
            if (response.Ok)
            {
                List<GameDetail> games = response.Games.ToList();

                return Ok(new SearchGameResponseModel()
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

        [HttpGet("search/gender")]
        public async Task<IActionResult> SearchGamesByGender([FromBody] SearchMetricRequestModel searchMetricRequestModel)
        {
            SearchGenderMetric searchMetric = new SearchGenderMetric()
            {
                Gender = searchMetricRequestModel.Gender
            };


            SearchGameResponse response = await _gameClient.SearchGameByGenderAsync(searchMetric);
            if (response.Ok)
            {
                List<GameDetail> games = response.Games.ToList();

                return Ok(new SearchGameResponseModel()
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

        [HttpGet("search/rating")]
        public async Task<IActionResult> SearchGamesByRating([FromBody] SearchMetricRequestModel searchMetricRequestModel)
        {
            SearchRatingMetric searchMetric = new SearchRatingMetric()
            {
                Rating = searchMetricRequestModel.Rating
            };


            SearchGameResponse response = await _gameClient.SearchGameByRatingAsync(searchMetric);
            if (response.Ok)
            {
                List<GameDetail> games = response.Games.ToList();

                return Ok(new SearchGameResponseModel()
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGame(int id)
        {
            BasicGameRequest basicGameRequest = new BasicGameRequest()
            {
                GameId = id
            };


            ShowGameResponse response = await _gameClient.ShowGameAsync(basicGameRequest);
            if (response.Ok)
            {
                EnrichGameDetail retrievedGame = response.Game;
                return Ok(retrievedGame);
            }
            else
            {
                Error error = response.Error;
                return NotFound(error);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGames()
        {

            IndexGameResponse response = await _gameClient.ShowGamesAsync(new Empty());
            if (response.Ok)
            {
                List<GameDetail> games = response.Games.ToList();

                return Ok(new IndexGamesModel()
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] UpdateGameRequestModel updateGameRequestModel)
        {
            UpdateGameRequest request = new UpdateGameRequest()
            {
                Id = id,
                Synopsis = updateGameRequestModel.Synopsis,
                Gender = updateGameRequestModel.Gender,
                Title = updateGameRequestModel.Title,
            };
            UpdateGameResponse response = await _gameClient.UpdateGameAsync(request);
            if (response.Ok)
            {
                GameBasicInfoResponse gameBasicInfoResponse = response.Game;
                return Ok(gameBasicInfoResponse);
            }
            else
            {
                Error error = response.Error;
                return BadRequest(error);
            }
        }

    }
}
