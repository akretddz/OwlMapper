using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OwlMapper.Shared;

namespace OwlMapper.Modules.Sample;

/// <summary>
/// Sample module demonstrating how to create a module for the OwlMapper Bootstrapper.
/// This module is disabled by default and serves as a template/example.
/// </summary>
public class SampleModule : IModule
{
    public string Name => "SampleModule";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register your services here
        // Example:
        // services.AddScoped<ISampleService, SampleService>();
        // services.Configure<SampleModuleOptions>(configuration.GetSection("Modules:SampleModule"));
    }

    public void ConfigureApplication(IApplicationBuilder app)
    {
        // Configure your middleware and endpoints here
        // Example:
        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapGet("/sample", () => "Sample module endpoint");
        // });
    }
}
