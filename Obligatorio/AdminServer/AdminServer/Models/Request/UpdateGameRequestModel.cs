using System;
namespace AdminServer.Models.Response
{
    public class UpdateGameRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string Synopsis { get; set; }
    }
}
