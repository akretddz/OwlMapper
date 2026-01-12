using Microsoft.Extensions.Configuration;
using System.Reflection;

using static Shared.Consts;

namespace Shared.Modules
{
    public static class ModuleLoader
    {
        public static List<Assembly> LoadAssemblies(IConfiguration configuration)
        {
            var assembliesList         = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var locationsArray         = GetStaticAssembliesLocations(assembliesList);
            var moduleFilesList        = GetModuleFiles(assembliesList);
            var enabledModuleFilesList = moduleFilesList.GetEnabledModuleFiles(configuration);

            enabledModuleFilesList.ForEach(file =>
            {
                var assembly = LoadAssemblyFromFile(file);

                if (assembly is not null)
                {
                    assembliesList.Add(assembly);
                }
            });

            return assembliesList;
        }

        public static List<IModule> LoadModules(List<Assembly> assembliesList)
        => [.. assembliesList
            .SelectMany(a => a.GetTypes())
            .Where(type => typeof(IModule).IsAssignableFrom(type) && 
                           !type.IsInterface && 
                           !type.IsAbstract)
            .OrderBy(type => type.Name)
            .Select(Activator.CreateInstance)
            .Cast<IModule>()];

        private static string[] GetStaticAssembliesLocations(List<Assembly> assembliesList)
            => [.. assembliesList
                   .Where(a => a.IsDynamic is false)
                   .Select(a => a.Location)];

        private static List<string> GetModuleFiles(List<Assembly> assembliesList)
            => [.. Directory
                .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(file =>
                    assembliesList
                        .Any(assembly => string.Equals(
                             assembly.Location,
                             file,
                             StringComparison.InvariantCultureIgnoreCase)))];

        private static List<string> GetEnabledModuleFiles(
            this IEnumerable<string> moduleFiles,
            IConfiguration configuration)
        {
            var enabledModulesList = new List<string>();

            foreach (var moduleFile in moduleFiles)
            {
                if (!moduleFile.Contains(Assemblies.ModuleAssemblyPrefix))
                {
                    continue;
                }

                var moduleName = moduleFile
                    .Split(Assemblies.ModuleAssemblyPrefix)[1]
                    .Split('.')[0];

                var isEnabled = configuration
                    .GetValue<bool>($"{moduleName}:{Configuration.Properties.IsEnabled}"
                    .ToLowerInvariant());

                if (isEnabled)
                {
                    enabledModulesList.Add(moduleFile);
                }
            }

            return enabledModulesList;
        }

        private static Assembly? LoadAssemblyFromFile(string file)
        {
            try
            {
                return AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(file));
            }
            catch (FileLoadException)
            {
                return null;
            }
        }
    }
}
