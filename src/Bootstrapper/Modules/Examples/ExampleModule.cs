namespace OwlMapper.Bootstrapper.Modules.Examples;

public class ExampleModule : IModule
{
    public string Name => "ExampleModule";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    public void UseModule(IApplicationBuilder app)
    {
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
