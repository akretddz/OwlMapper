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
            var locationsFileNamesList = GetLocationsFileNames(locationsArray);
            var enabledModuleFilesList = locationsFileNamesList.GetEnabledModuleFiles(configuration);

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

        private static List<string> GetLocationsFileNames(string[] locationsArray)
            => [.. Directory
                .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(file =>
                    !locationsArray
                        .Any(location => string.Equals(
                             location,
                             file,
                             StringComparison.InvariantCultureIgnoreCase)))];

        private static List<string> GetEnabledModuleFiles(
            this IEnumerable<string> moduleFiles,
            IConfiguration configuration)
        {
            var enabledModulesList = new List<string>();

            foreach (var moduleFile in moduleFiles)
            {
                if (!ModuleNamesArray.Any(moduleFile.Contains))
                {
                    continue;
                }

                var moduleFileName = Path.GetFileNameWithoutExtension(moduleFile);

                var isEnabled = configuration
                    .GetValue<bool>($"{moduleFileName}:{Configuration.Sections.Module}:{Configuration.Properties.IsEnabled}"
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