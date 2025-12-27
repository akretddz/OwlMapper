namespace OwlMapper.Bootstrapper.Modules;

/// <summary>
/// Interface for application modules
/// </summary>
public interface IModule
{
    /// <summary>
    /// Module name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Register module services in DI container
    /// </summary>
    void RegisterServices(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Add module to application pipeline
    /// </summary>
    void UseModule(IApplicationBuilder app);
}
