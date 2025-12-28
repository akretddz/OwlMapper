namespace OwlMapper.Bootstrapper.Modules.Examples;

public class ExampleModule : IModule
{
    public string Name => "ExampleModule";

    public virtual void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    public virtual void UseModule(IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/example", () => new
            {
                module    = "ExampleModule",
                message   = "This is an example module endpoint",
                timestamp = DateTime.UtcNow
            })
            .WithName("GetExampleModuleInfo")
            .WithTags("Example");
        });
    }
}
