namespace OwlMapper.Bootstrapper.Modules;

public interface IModule
{
    string Name { get; }

    void RegisterServices(IServiceCollection services, IConfiguration configuration);

    void UseModule(IApplicationBuilder app);
}
