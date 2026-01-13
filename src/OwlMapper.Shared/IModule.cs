using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OwlMapper.Shared;

/// <summary>
/// Interface representing a module in the application.
/// Modules can be loaded dynamically or statically and can be enabled/disabled via configuration.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Registers services in the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    void RegisterServices(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Configures the application pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    void ConfigureApplication(IApplicationBuilder app);
}
