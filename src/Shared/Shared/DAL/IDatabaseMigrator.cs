namespace Shared.DAL
{
    public interface IDatabaseMigrator
    {
        Task MigrateAsync(CancellationToken cancellationToken);
    }
}