using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Modules
{
    public interface IModule
    {
        void Register(IServiceCollection services);
        void Use(IApplicationBuilder app);
    }
}
