using System;

namespace Exceptions
{
    public class ApiException : Exception
    {
        private const string _message = @"{0}";

        public int StatusCode { get; set; }
        public string Content { get; set; }

        public ApiException(Exception innerException = null) : base(null, innerException)
        {
        }

        public ApiException(int statusCode, string content, Exception innerException = null) : base(string.Format(_message, content), innerException)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
