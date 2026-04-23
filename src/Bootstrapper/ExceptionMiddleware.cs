using Shared.Exceptions;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

using static Shared.Consts.Exceptions;

namespace Bootstrapper
{
    public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy        = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull,
        };

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

                var details = GetExceptionDetails(ex);

                httpContext.Response.StatusCode  = details.StatusCode;
                httpContext.Response.ContentType = MediaTypeNames.Application.ProblemJson;

                var body = new ErrorResponse(
                    details.ErrorCode,
                    details.Detail,
                    httpContext.TraceIdentifier,
                    details.Errors);

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOptions));
            }
        }

        private static ExceptionDetails GetExceptionDetails(Exception exception) =>
            exception switch
            {
                AppException appException => new ExceptionDetails(
                    appException.StatusCode,
                    appException.ErrorCode,
                    appException.Message,
                    null),
                ValidationException validationException => new ExceptionDetails(
                    StatusCodes.Status422UnprocessableEntity,
                    ValidationErrorCode,
                    "One or more validation errors has occurred.",
                    validationException.Errors),
                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    InternalErrorCode,
                    "An unexpected error occurred.",
                    null)
            };

        private record ExceptionDetails(int StatusCode, string ErrorCode, string Detail, object? Errors);

        private record ErrorResponse(
            string ErrorCode,
            string Detail,
            string TraceId,
            object? Errors);
    }
}
