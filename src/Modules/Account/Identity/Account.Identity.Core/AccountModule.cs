using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Account.Identity.Core
{
    public static class AccountModule
    {
        private static readonly Assembly _module = typeof(AccountModule).Assembly;

        public static void Register(this IServiceCollection services)
            => services.AddInfrastructure(_module);

        public static void Use(IApplicationBuilder app)
        {
            
        }
    }
}
