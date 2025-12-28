using System.Reflection;

namespace OwlMapper.Bootstrapper.Modules;

public class ModuleLoader
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ModuleLoader> _logger;
    private readonly List<IModule> _modules = new();

    public ModuleLoader(IConfiguration configuration, ILogger<ModuleLoader> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public virtual void LoadModules()
    {
        var moduleConfig = new ModuleConfiguration();
        _configuration.GetSection("Modules").Bind(moduleConfig);

        var moduleTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var moduleType in moduleTypes)
        {
            try
            {
                var module = (IModule)Activator.CreateInstance(moduleType)!;
                
                if (moduleConfig.EnabledModules.TryGetValue(module.Name, out var isEnabled) && !isEnabled)
                {
                    _logger.LogInformation("Module {ModuleName} is disabled in configuration", module.Name);
                    continue;
                }

                _modules.Add(module);
                _logger.LogInformation("Loaded module: {ModuleName}", module.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load module of type {ModuleType}", moduleType.Name);
            }
        }
    }

    public virtual void RegisterModuleServices(IServiceCollection services)
    {
        foreach (var module in _modules)
        {
            try
            {
                _logger.LogInformation("Registering services for module: {ModuleName}", module.Name);
                module.RegisterServices(services, _configuration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register services for module {ModuleName}", module.Name);
            }
        }
    }

    public virtual void UseModules(IApplicationBuilder app)
    {
        foreach (var module in _modules)
        {
            try
            {
                _logger.LogInformation("Adding module to pipeline: {ModuleName}", module.Name);
                module.UseModule(app);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add module {ModuleName} to pipeline", module.Name);
            }
        }
    }

    public IReadOnlyList<IModule> LoadedModules => _modules.AsReadOnly();
}
