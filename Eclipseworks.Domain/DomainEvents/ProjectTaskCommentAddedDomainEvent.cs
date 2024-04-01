namespace Eclipseworks.Domain.DomainEvents;

public class ProjectTaskCommentAddedDomainEvent : DomainEvent
{
    public ProjectTask ProjectTask { get; init; }
    public ProjectTaskComment ProjectTaskComment { get; init; }

    public ProjectTaskCommentAddedDomainEvent(
        ProjectTask projectTask, 
        ProjectTaskComment projectTaskComment
    )
    {
        ProjectTask = projectTask;
        ProjectTaskComment = projectTaskComment;
    }
}
