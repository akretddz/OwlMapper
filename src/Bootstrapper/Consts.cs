using System.Reflection;

namespace Bootstrapper
{
    public static class Consts
    {
        public static readonly Assembly[] ModulesAssembliesArray =
        [
            typeof(AccountModule).Assembly,
        ];

        public static class HealthChecks
        {
            public const string Default  = "Default";
            public const string Postgres = "Postgres";
            public const string RabbitMQ = "RabbitMQ";
        }
    }
}
