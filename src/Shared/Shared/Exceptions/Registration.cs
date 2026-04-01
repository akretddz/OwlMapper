using Microsoft.AspNetCore.Builder;

namespace Shared.Exceptions
{
    public static class Registration
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }
    }
}
