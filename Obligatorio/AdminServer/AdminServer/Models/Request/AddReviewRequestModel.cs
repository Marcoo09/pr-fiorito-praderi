using System;
namespace AdminServer.Models.Request
{
    public class AddReviewRequestModel
    {
        public int GameId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
    }
}
