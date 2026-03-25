namespace Bootstrapper
{
    public static class Consts
    {
        public static class HealthChecks
        {
            public const string Default  = "Default";
            public const string Postgres = "Postgres";
            public const string RabbitMQ = "RabbitMQ";

            public const string PostgresDbCheckQuery = "SELECT 1";
        }
    }
}