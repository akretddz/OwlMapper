using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Exceptions
{
    public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private const string InternalErrorCode = "internal_error";

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                logger.LogError(
                    "Validation exception [{ErrorCode}]: {Errors}",
                    ex.ErrorCode,
                    ex.Errors);

                await WriteResponseAsync(context, ex.StatusCode, new ExceptionResponse(
                    Status: ex.StatusCode,
                    Title: GetTitle(ex.StatusCode),
                    Detail: ex.Message,
                    ErrorCode: ex.ErrorCode,
                    TraceId: context.TraceIdentifier,
                    Errors: ex.Errors));
            }
            catch (AppException ex)
            {
                logger.LogError(
                    "Business exception [{ErrorCode}]: {Message}",
                    ex.ErrorCode,
                    ex.Message);

                await WriteResponseAsync(context, ex.StatusCode, new ExceptionResponse(
                    Status: ex.StatusCode,
                    Title: GetTitle(ex.StatusCode),
                    Detail: ex.Message,
                    ErrorCode: ex.ErrorCode,
                    TraceId: context.TraceIdentifier,
                    Errors: null));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                await WriteResponseAsync(context, 500, new ExceptionResponse(
                    Status: 500,
                    Title: "Internal Server Error",
                    Detail: "An unexpected error occurred.",
                    ErrorCode: InternalErrorCode,
                    TraceId: context.TraceIdentifier,
                    Errors: null));
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, int statusCode, ExceptionResponse response)
        {
            context.Response.ContentType = MediaTypeNames.Application.ProblemJson;
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, SerializerOptions));
        }

        private static string GetTitle(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            422 => "Unprocessable Entity",
            _ => "Error",
        };

        private sealed record ExceptionResponse(
            int Status,
            string Title,
            string Detail,
            string ErrorCode,
            string TraceId,
            [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            IReadOnlyDictionary<string, string[]>? Errors);
    }
}
