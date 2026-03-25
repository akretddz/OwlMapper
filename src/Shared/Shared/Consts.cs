namespace Shared
{
    public static class Consts
    {
        public static class ApplicationInfo
        {
            public static readonly string Name = Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "OwlMapper";
            public static readonly string ApplicationCode = Environment.GetEnvironmentVariable("APPLICATION_IDENTIFIER") ?? "owlmapper.bootstrapper";
        }

        public static class Configuration
        {
            public static class Sections
            {
                public const string Module = "module";
                public const string Database = "database";
                public const string Postgres = "postgres";
                public const string Messaging = "messaging";
                public const string Rabbit = "rabbitMQ";

            }
            public static class Properties
            {
                public const string IsEnabled = "isEnabled";
                public const string ConnectionString = "connectionString";
            }
        }

        public static readonly string[] ModuleNamesArray =
        [
            "Account",
        ];

        public static class Databases
        {
            public static class DataTypes
            {
                public const string Nvarchar20 = "nvarchar(20)";
            }
        }
    }
}
