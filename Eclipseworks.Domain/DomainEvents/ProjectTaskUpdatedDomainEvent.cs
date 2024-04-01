namespace Eclipseworks.Domain.DomainEvents;

public class ProjectTaskUpdatedDomainEvent : DomainEvent
{
    public ProjectTask ProjectTask { get; init; }
    
    public ProjectTaskUpdatedDomainEvent(ProjectTask projectTask)
        => ProjectTask = projectTask;
}
