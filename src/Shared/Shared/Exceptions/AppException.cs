namespace Shared.Exceptions
{
    public abstract class AppException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        protected AppException(string errorCode, string message, int statusCode = 400)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
