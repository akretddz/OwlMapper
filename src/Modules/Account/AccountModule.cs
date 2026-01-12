using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Modules;
using System.Reflection;
using Account.Identity;

namespace Account
{
    public sealed class AccountModule : IModule
    {
        private readonly Assembly _module = typeof(AccountModule).Assembly;

        public string Name => "Account";

        public string Path => "account-module";

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAccountIdentity(configuration);
        }
    }
}
