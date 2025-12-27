namespace OwlMapper.Bootstrapper.Modules.Examples;

/// <summary>
/// Example module demonstrating module loading
/// </summary>
public class ExampleModule : IModule
{
    public string Name => "ExampleModule";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register module services here
        // Example: services.AddScoped<IExampleService, ExampleService>();
    }

    public void UseModule(IApplicationBuilder app)
    {
        // Add module to application pipeline
        // Example: app.UseMiddleware<ExampleMiddleware>();
        
        // Map example endpoint using routing
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/example", () => new
            {
                module = "ExampleModule",
                message = "This is an example module endpoint",
                timestamp = DateTime.UtcNow
            })
            .WithName("GetExampleModuleInfo")
            .WithTags("Example");
        });
    }
}
