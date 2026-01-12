using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public static class Extensions
    {
        public static IServiceCollection AddShared(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddModules(configuration);

            return services;
        }
    }
}
