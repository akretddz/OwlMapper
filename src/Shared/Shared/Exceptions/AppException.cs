using System.Net;

namespace Shared.Exceptions
{
    public abstract class AppException(string errorCode, string message, int statusCode) : Exception(message)
    {
        public int StatusCode   { get; } = statusCode;
        public string ErrorCode { get; } = errorCode;
    }
}
