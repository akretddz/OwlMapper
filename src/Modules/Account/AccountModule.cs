using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Modules;

namespace Account
{
    public sealed class AccountModule : IModule
    {
        public string Name => "Account";

        public string Path => "account-module";

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            return;
        }
    }
}
