﻿namespace LogsServer.Models.Response
{
    public class PaginatedResponseModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}
