using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using System.Text.Json;

namespace Shared.Tests.Unit
{
    internal static class TestsHelper
    {
        internal static (ExceptionMiddleware Middleware, DefaultHttpContext HttpContext, MemoryStream Body)
            BuildSut(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            var body                = new MemoryStream();
            var context             = new DefaultHttpContext();
            context.Response.Body   = body;
            context.TraceIdentifier = "test-trace-id";
            var middleware          = new ExceptionMiddleware(next, logger);

            return (middleware, context, body);
        }

        internal static async Task<JsonDocument> ReadJsonDocumentAsync(MemoryStream body)
        {
            body.Seek(0, SeekOrigin.Begin);

            return await JsonDocument.ParseAsync(body);
        }
    }
}
