using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shared.Exceptions;
using System.Net.Mime;
using System.Text.Json;

namespace Shared.Tests.Unit.Exceptions
{
    [TestFixture]
    internal class ExceptionMiddlewareTests
    {
        private static (ExceptionMiddleware Middleware, DefaultHttpContext HttpContext, MemoryStream Body)
            BuildSut(RequestDelegate next)
        {
            var body = new MemoryStream();
            var context = new DefaultHttpContext();
            context.Response.Body = body;
            context.TraceIdentifier = "test-trace-id";

            var middleware = new ExceptionMiddleware(next, Substitute.For<ILogger<ExceptionMiddleware>>());

            return (middleware, context, body);
        }

        private static async Task<JsonDocument> ReadJsonDocumentAsync(MemoryStream body)
        {
            body.Seek(0, SeekOrigin.Begin);
            return await JsonDocument.ParseAsync(body);
        }

        private sealed class TestAppException(string errorCode, string message, int statusCode)
            : AppException(errorCode, message, statusCode);

        // -------------------------------------------------------------------

        [TestFixture]
        private class WhenNoExceptionThrown : ExceptionMiddlewareTests
        {
            [Test]
            public async Task Does_not_modify_response_status_code()
            {
                RequestDelegate next = _ => Task.CompletedTask;
                var (middleware, context, _) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(context.Response.StatusCode, Is.EqualTo(200));
            }

            [Test]
            public async Task Does_not_write_response_body()
            {
                RequestDelegate next = _ => Task.CompletedTask;
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(body.Length, Is.EqualTo(0));
            }
        }

        [TestFixture]
        private class WhenAppExceptionThrown : ExceptionMiddlewareTests
        {
            [Test]
            public async Task Returns_status_code_from_exception()
            {
                RequestDelegate next = _ => throw new TestAppException("some_error", "Something failed.", 400);
                var (middleware, context, _) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(context.Response.StatusCode, Is.EqualTo(400));
            }

            [Test]
            public async Task Returns_404_when_exception_has_404_status()
            {
                RequestDelegate next = _ => throw new TestAppException("not_found", "Resource not found.", 404);
                var (middleware, context, _) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(context.Response.StatusCode, Is.EqualTo(404));
            }

            [Test]
            public async Task Response_content_type_is_problem_json()
            {
                RequestDelegate next = _ => throw new TestAppException("some_error", "Something failed.", 400);
                var (middleware, context, _) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(context.Response.ContentType, Is.EqualTo(MediaTypeNames.Application.ProblemJson));
            }

            [Test]
            public async Task Response_contains_error_code()
            {
                const string expectedCode = "account_not_found";
                RequestDelegate next = _ => throw new TestAppException(expectedCode, "Not found.", 404);
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var errorCode = doc.RootElement.GetProperty("errorCode").GetString();

                Assert.That(errorCode, Is.EqualTo(expectedCode));
            }

            [Test]
            public async Task Response_contains_exception_message_as_detail()
            {
                const string expectedMessage = "Account with given email already exists.";
                RequestDelegate next = _ => throw new TestAppException("conflict", expectedMessage, 409);
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var detail = doc.RootElement.GetProperty("detail").GetString();

                Assert.That(detail, Is.EqualTo(expectedMessage));
            }

            [Test]
            public async Task Response_contains_trace_id()
            {
                RequestDelegate next = _ => throw new TestAppException("some_error", "Error.", 400);
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var traceId = doc.RootElement.GetProperty("traceId").GetString();

                Assert.That(traceId, Is.EqualTo("test-trace-id"));
            }

            [Test]
            public async Task Response_does_not_contain_errors_field()
            {
                RequestDelegate next = _ => throw new TestAppException("some_error", "Error.", 400);
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var hasErrors = doc.RootElement.TryGetProperty("errors", out _);

                Assert.That(hasErrors, Is.False);
            }
        }

        [TestFixture]
        private class WhenValidationExceptionThrown : ExceptionMiddlewareTests
        {
            private static readonly IReadOnlyDictionary<string, string[]> SampleErrors = new Dictionary<string, string[]>
            {
                { "email", ["Email is required.", "Email format is invalid."] },
                { "password", ["Password must be at least 8 characters."] },
            };

            [Test]
            public async Task Returns_422_status_code()
            {
                RequestDelegate next = _ => throw new ValidationException(SampleErrors);
                var (middleware, context, _) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(context.Response.StatusCode, Is.EqualTo(422));
            }

            [Test]
            public async Task Response_content_type_is_problem_json()
            {
                RequestDelegate next = _ => throw new ValidationException(SampleErrors);
                var (middleware, context, _) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(context.Response.ContentType, Is.EqualTo(MediaTypeNames.Application.ProblemJson));
            }

            [Test]
            public async Task ErrorCode_is_validation_error()
            {
                RequestDelegate next = _ => throw new ValidationException(SampleErrors);
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var errorCode = doc.RootElement.GetProperty("errorCode").GetString();

                Assert.That(errorCode, Is.EqualTo(ValidationException.ValidationErrorCode));
            }

            [Test]
            public async Task Response_contains_errors_dictionary()
            {
                RequestDelegate next = _ => throw new ValidationException(SampleErrors);
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var hasErrors = doc.RootElement.TryGetProperty("errors", out var errorsElement);

                Assert.That(hasErrors, Is.True);
                Assert.That(errorsElement.ValueKind, Is.EqualTo(JsonValueKind.Object));
            }

            [Test]
            public async Task Response_errors_contain_correct_fields()
            {
                RequestDelegate next = _ => throw new ValidationException(SampleErrors);
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var errorsElement = doc.RootElement.GetProperty("errors");

                Assert.That(errorsElement.TryGetProperty("email", out _), Is.True);
                Assert.That(errorsElement.TryGetProperty("password", out _), Is.True);
            }
        }

        [TestFixture]
        private class WhenSystemExceptionThrown : ExceptionMiddlewareTests
        {
            [Test]
            public async Task Returns_500_status_code()
            {
                RequestDelegate next = _ => throw new InvalidOperationException("Something crashed.");
                var (middleware, context, _) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(context.Response.StatusCode, Is.EqualTo(500));
            }

            [Test]
            public async Task Response_content_type_is_problem_json()
            {
                RequestDelegate next = _ => throw new InvalidOperationException("Something crashed.");
                var (middleware, context, _) = BuildSut(next);

                await middleware.InvokeAsync(context);

                Assert.That(context.Response.ContentType, Is.EqualTo(MediaTypeNames.Application.ProblemJson));
            }

            [Test]
            public async Task ErrorCode_is_internal_error()
            {
                RequestDelegate next = _ => throw new InvalidOperationException("Something crashed.");
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var errorCode = doc.RootElement.GetProperty("errorCode").GetString();

                Assert.That(errorCode, Is.EqualTo("internal_error"));
            }

            [Test]
            public async Task Detail_does_not_expose_stack_trace()
            {
                RequestDelegate next = _ => throw new InvalidOperationException("Something crashed.");
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var detail = doc.RootElement.GetProperty("detail").GetString();

                Assert.That(detail, Does.Not.Contain("at "));
                Assert.That(detail, Does.Not.Contain("ExceptionMiddleware"));
            }

            [Test]
            public async Task Detail_does_not_expose_internal_exception_message()
            {
                RequestDelegate next = _ => throw new InvalidOperationException("Sensitive internal crash info.");
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var detail = doc.RootElement.GetProperty("detail").GetString();

                Assert.That(detail, Does.Not.Contain("Sensitive internal crash info."));
            }

            [Test]
            public async Task Response_contains_trace_id()
            {
                RequestDelegate next = _ => throw new NullReferenceException();
                var (middleware, context, body) = BuildSut(next);

                await middleware.InvokeAsync(context);

                using var doc = await ReadJsonDocumentAsync(body);
                var traceId = doc.RootElement.GetProperty("traceId").GetString();

                Assert.That(traceId, Is.EqualTo("test-trace-id"));
            }
        }
    }
}
