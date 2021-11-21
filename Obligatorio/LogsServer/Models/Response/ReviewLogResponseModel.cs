using DTOs.Response;
using LogsServer.Domain;
using LogsServer.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsServer.Models.Response
{
    public class ReviewLogResponseModel
    {
        public ReviewDetailDTO Review { get; set; }
        public List<ReviewDetailDTO> Reviews { get; set; }

        public ReviewLogResponseModel(Log log, PaginationRequestModel paginationRequestModel, int totalLogs)
        {
            Total = totalLogs;
            Page = paginationRequestModel.Page;
            PageSize = paginationRequestModel.PageSize;
            Tag = FormatEnumString(log.LogTag.ToString());
            CreatedAt = log.CreatedAt;

            if (log.IsEntityAList())
            {
                List<ReviewDetailDTO> logThemes = log.Entity as List<ReviewDetailDTO>;
                Reviews = logThemes;
            }
            else
            {
                ReviewDetailDTO logTheme = log.Entity as ReviewDetailDTO;
                Review = logTheme;
            }
        }

    }
}
