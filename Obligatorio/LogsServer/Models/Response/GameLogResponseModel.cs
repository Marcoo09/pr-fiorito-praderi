using DTOs.Response;
using LogsServer.Domain;
using LogsServer.Models.Request;
using System.Collections.Generic;

namespace LogsServer.Models.Response
{
    public class GameLogResponseModel : LogResponseModel
    {

        public GameDetailDTO Post { get; set; }
        public List<GameDetailDTO> Posts { get; set; }

        public GameLogResponseModel(Log log, PaginationRequestModel paginationRequestModel, int totalLogs)
        {
            Total = totalLogs;
            Page = paginationRequestModel.Page;
            PageSize = paginationRequestModel.PageSize;
            Tag = FormatEnumString(log.LogTag.ToString());
            CreatedAt = log.CreatedAt;

            if (log.IsEntityAList())
            {
                List<GameDetailDTO> logPosts = log.Entity as List<GameDetailDTO>;
                Posts = logPosts;
            }
            else
            {
                GameDetailDTO logPost = log.Entity as GameDetailDTO;
                Post = logPost;
            }
        }

    }
}
