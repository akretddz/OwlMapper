namespace OwlMapper.Bootstrapper.Modules;

/// <summary>
/// Configuration for module loading
/// </summary>
public class ModuleConfiguration
{
    public Dictionary<string, bool> EnabledModules { get; set; } = new();
}
