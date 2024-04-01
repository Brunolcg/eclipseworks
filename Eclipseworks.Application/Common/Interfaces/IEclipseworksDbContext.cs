namespace Eclipseworks.Application.Common.Interfaces;

public interface IEclipseworksDbContext
{
    public DbSet<Project> Projects { get; }
    public DbSet<User> Users { get; }
    public DbSet<HistoryUpdate> HistoryUpdates { get; }
    public DbSet<HistoryUpdateChange> HistoryUpdatesChanges { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}