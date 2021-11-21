namespace LogsServer.Models.Request
{
    public class PaginationRequestModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int CalculateOffset()
        {
            return (Page - 1) * PageSize;
        }
    }
}
