using System;
using System.Text.RegularExpressions;

namespace LogsServer.Models.Response
{
    public class LogResponseModel : PaginatedResponseModel
    {
        public string Tag { get; set; }
        public DateTime CreatedAt { get; set; }

        protected string FormatEnumString(string stringEnum)
        {
            return Regex.Replace(stringEnum, "(?<=[^A-Z])(?=[A-Z])", " ");
        }
    }
}
