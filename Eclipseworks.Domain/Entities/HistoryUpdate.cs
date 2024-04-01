namespace Eclipseworks.Domain.Entities;

public class HistoryUpdate : EntityBase
{
    public HistoryUpdateType Type { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UserId { get; init; }
    public Guid EntityId { get; init; }
    private readonly List<HistoryUpdateChange> _changes = new();
    public IReadOnlyCollection<HistoryUpdateChange> Changes => _changes;

    public HistoryUpdate(
        HistoryUpdateType type, 
        Guid entityId,
        Guid userId
    )
    {
        Id = Guid.NewGuid();
        Type = type;
        EntityId = entityId;
        CreatedAt = DateTime.Now;
        UserId = userId;
    }

    public void AddChanges(HistoryUpdateChange change)
        => _changes.Add(change);

    public bool HasChanges()
        => _changes.Any();
}