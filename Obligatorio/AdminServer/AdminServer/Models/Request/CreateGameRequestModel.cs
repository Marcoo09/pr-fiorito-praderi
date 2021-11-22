using System;
namespace AdminServer.Models.Request
{
    public class CreateGameRequestModel
    {
        public string Title { get; set; }
        public string Gender { get; set; }
        public string Synopsis { get; set; }
        public string CoverName { get; set; }
        public byte[] Data { get; set; }
        public long FileSize { get; set; }
    }
}
