namespace Eclipseworks.Infrastructure;

public class EclipseworksDbContext : DbContext, IEclipseworksDbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();
    public DbSet<HistoryUpdate> HistoryUpdates => Set<HistoryUpdate>();
    public DbSet<HistoryUpdateChange> HistoryUpdatesChanges => Set<HistoryUpdateChange>();
    public EclipseworksDbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }
}