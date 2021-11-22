using System;
using System.Text.RegularExpressions;

namespace Logger.Models.Response
{
    public class LogResponseModel
    {
        public string Tag { get; set; }
        public DateTime CreatedAt { get; set; }

        protected string FormatEnumString(string stringEnum)
        {
            return Regex.Replace(stringEnum, "(?<=[^A-Z])(?=[A-Z])", " ");
        }
    }
}