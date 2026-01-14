using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Shared.Modules
{
    public static partial class Registration
    {
        public static WebApplicationBuilder ConfigureModules(
            this WebApplicationBuilder builder
            )
        {
            var solutionRootPath = GetSolutionDirectory(builder.Environment.ContentRootPath);

            if (string.IsNullOrWhiteSpace(solutionRootPath))
            {
                return builder;
            }

            var environmentName = builder.Environment.EnvironmentName;

            foreach (var settings in GetSettings(solutionRootPath, "*.module")
                .Where(settings => ModuleConfigRegex().IsMatch(settings)))
            {
                builder.Configuration.AddJsonFile(settings, optional: false, reloadOnChange: true);
            }

            foreach (var settings in GetSettings(solutionRootPath, $"*.{environmentName}"))
            {
                builder.Configuration.AddJsonFile(settings, optional: true, reloadOnChange: true);
            }

            return builder;
        }

        private static IEnumerable<string> GetSettings(string contentRootPath, string pattern)
            => Directory.EnumerateFiles(contentRootPath,
                    $"{pattern}.json",
                    SearchOption.AllDirectories);

        private static string? GetSolutionDirectory(string currentPath)
        {
            var directory = new DirectoryInfo(currentPath);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            return directory?.FullName;
        }

        private static Regex ModuleConfigRegex() 
            => new(@"[^.]+\.module\.json$", RegexOptions.IgnoreCase);
    }
}
