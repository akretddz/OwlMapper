using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Modules
{
    public static class Extensions
    {
        public static void AddModules(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            //TODO: 1. Załadowanie assemblów modułów z klasami impl. IModule
            //TODO: 2. iterując po nich sprawdzić odpowiednią konfigurację czy moduł ma być włączony
            //TODO: 3. Wywołać metodę Register dla każdego modułu/lub zwrócić listę modułów, potem stworzyć instacje modułów poprzez refleksję
        }
    }
}
