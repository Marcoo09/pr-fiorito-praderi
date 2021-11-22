using System;
namespace AdminServer.Models.Request
{
    public class SearchMetricRequestModel
    {
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Gender { get; set; }
    }
}
