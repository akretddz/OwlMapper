using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Modules
{
    public interface IModule
    {
        string Name { get; }
        string Path { get; }

        void Register(IServiceCollection services, IConfiguration configuration);
    }
}