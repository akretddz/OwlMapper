namespace Shared
{
    public static class Consts
    {
        public static class ApplicationInfo
        {
            public static readonly string Name            = Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "OwlMapper";
            public static readonly string ApplicationCode = Environment.GetEnvironmentVariable("APPLICATION_IDENTIFIER") ?? "owlmapper.bootstrapper";
        }

        public static class Configuration
        {
            public static class Sections
            {
                public const string Module    = "module";
                public const string Database  = "database";
                public const string Postgres  = "postgres";
                public const string Messaging = "messaging";
                public const string Rabbit    = "rabbitMQ";

            }
            public static class Properties
            {
                public const string IsEnabled           = "isEnabled";
                public const string ConnectionString    = "connectionString";
                public const string UseDatabaseMigrator = "useDatabaseMigrator";
            }
        }

        public static readonly string[] ModuleNamesArray =
        [
            "Account",
        ];

        public static class Exceptions
        {
            public const string ValidationErrorCode = "validation_error";
            public const string InternalErrorCode   = "internal_error";
        }
    }
}