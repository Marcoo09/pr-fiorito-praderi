using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminServer.Models.Request;
using AdminServer.Models.Response;
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

            //To Improve
            //if (response.Ok)
            //{
            //    PostDetailInfo retrievedPost = response.Post;
            //    return Ok(retrievedPost);
            //}
            //else
            //{
            //    Error error = response.Error;
            //    return NotFound(error);
            //}
        }

        //[HttpPost]
        //public async Task<IActionResult> CreatePost([FromBody] CreatePostRequestModel createPostRequest)
        //{
        //    CreatePostRequest request = new CreatePostRequest()
        //    {
        //        Name = createPostRequest.Name,
        //        ThemeId = createPostRequest.ThemeId,
        //        PostedAt = DateTime.Now.Ticks,
        //        ClientIp = Request.HttpContext.Connection.RemoteIpAddress.ToString()
        //    };
        //    CreatePostResponse response = await _postClient.CreatePostAsync(request);
        //    if (response.Ok)
        //    {
        //        PostBasicInfo createdPost = response.CreatedPost;
        //        return Created($"api/posts/{createdPost.Id}", createdPost);
        //    }
        //    else
        //    {
        //        Error error = response.Error;
        //        return BadRequest(error);
        //    }
        //}

        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostRequestModel updatePostModel)
        //{
        //    UpdatePostRequest request = new UpdatePostRequest()
        //    {
        //        Id = id,
        //        Name = updatePostModel.Name
        //    };
        //    UpdatePostResponse response = await _postClient.UpdatePostAsync(request);
        //    if (response.Ok)
        //    {
        //        PostBasicInfo postBasicInfo = response.Post;
        //        return Ok(postBasicInfo);
        //    }
        //    else
        //    {
        //        Error error = response.Error;
        //        return BadRequest(error);
        //    }
        //}

        //[HttpPut("{id:int}/theme")]
        //public async Task<IActionResult> UpdatePost(int id, [FromBody] ChangePostThemeRequestModel changePostThemeRequestModel)
        //{
        //    ChangePostThemeRequest request = new ChangePostThemeRequest()
        //    {
        //        PostId = id,
        //        ThemeId = changePostThemeRequestModel.ThemeId
        //    };
        //    ChangePostThemeResponse response = await _postClient.ChangePostThemeAsync(request);
        //    if (response.Ok)
        //    {
        //        PostDetailInfo postDetailInfo = response.Post;
        //        return Ok(postDetailInfo);
        //    }
        //    else
        //    {
        //        Error error = response.Error;
        //        return BadRequest(error);
        //    }
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> DeletePost(int id)
        //{
        //    DeletePostRequest request = new DeletePostRequest()
        //    {
        //        Id = id,
        //    };
        //    DeletePostResponse response = await _postClient.DeletePostAsync(request);
        //    if (response.Ok)
        //    {
        //        Message message = response.Message;
        //        return Ok(message);
        //    }
        //    else
        //    {
        //        Error error = response.Error;
        //        return BadRequest(error);
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> IndexPostsByTheme([FromQuery] PaginationRequestModel paginationRequestModel,
        //    [FromQuery] IndexPostsByThemeRequest indexPostsByThemeRequest)
        //{
        //    IndexPostsByThemeResponse response = await _postClient.IndexPostsByThemeAsync(indexPostsByThemeRequest);
        //    if (response.Ok)
        //    {
        //        List<PostDetailInfo> posts = response.Posts.ToList();
        //        int offset = paginationRequestModel.CalculateOffset();

        //        return Ok(new IndexPostsByThemeResponseModel()
        //        {
        //            Page = paginationRequestModel.Page,
        //            PageSize = paginationRequestModel.PageSize,
        //            Total = posts.Count,
        //            Posts = posts.Skip(offset).Take(paginationRequestModel.PageSize).ToList()
        //        });
        //    }
        //    else
        //    {
        //        Error error = response.Error;
        //        return BadRequest(error);
        //    }
        //}
    }
}