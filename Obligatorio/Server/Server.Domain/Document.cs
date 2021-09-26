using System;
using Server.Domain;

namespace Domain
{
    public class Document
    {
        public Game Game { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime UploadedAt { get; set; }
        public long FileSize { get; set; }
    }
}