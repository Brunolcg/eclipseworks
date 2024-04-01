namespace Eclipseworks.Core.Domain.Base;

public abstract class EntityBase
{
    public Guid Id { get; set; }
    
    [NotMapped]
    public Queue<DomainEvent> DomainEvents { get; private set; } = new();
}