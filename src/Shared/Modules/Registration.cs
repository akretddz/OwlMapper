using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.RegularExpressions;

namespace Shared.Modules
{
    public static partial class Registration
    {
        public static WebApplicationBuilder ConfigureModules(
            this WebApplicationBuilder builder
            )
        {
#pragma warning disable ASP0013 // Suggest switching from using Configure methods to WebApplicationBuilder.Configuration
            builder.Host.ConfigureAppConfiguration((context, config) =>
            {
                var solutionRootPath = GetSolutionDirectory(context.HostingEnvironment.ContentRootPath);

                if (string.IsNullOrWhiteSpace(solutionRootPath))
                {
                    return;
                }

                var environmentName = context.HostingEnvironment.EnvironmentName;

                foreach (var settings in GetSettings(solutionRootPath, "*.module")
                    .Where(settings => ModuleConfigRegex().IsMatch(settings)))
                {
                    config.AddJsonFile(settings, optional: false, reloadOnChange: true);
                }

                foreach (var settings in GetSettings(solutionRootPath, $"*.{environmentName}"))
                {
                    config.AddJsonFile(settings, optional: true, reloadOnChange: true);
                }
            });
#pragma warning restore ASP0013 // Suggest switching from using Configure methods to WebApplicationBuilder.Configuration

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
