using Eclipseworks.Domain.DomainEvents;

namespace Eclipseworks.Domain.Entities;

public class ProjectTask : EntityBase
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime DueDate { get; private set; }
    public ProjectTaskStatus Status { get; private set; }
    public ProjectTaskPriority Priority { get; private set; }
    public Project Project { get; init; } = null!;
    public Guid ProjectId { get; private set; }
    public User? Responsible { get; private set; }
    public Guid? ResponsibleId { get; private set; }
    public DateTime? ConcludedDate { get; private set; }
    
    private readonly List<ProjectTaskComment> _comments = new();
    public IReadOnlyCollection<ProjectTaskComment> Comments => _comments;
    
    protected ProjectTask()
    {
    }
    
    public ProjectTask(
        string title, 
        string description, 
        DateTime dueDate, 
        ProjectTaskPriority priority,
        User? responsible
    )
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = ProjectTaskStatus.Pending;
        Priority = priority;
        Responsible = responsible;
        ResponsibleId = responsible?.Id;
    }
    
    public Result TryUpdate(
        string title, 
        string description, 
        DateTime dueDate, 
        ProjectTaskStatus status,
        User? responsible
    )
    {
        Title = title;
        Description = description;
        DueDate = dueDate;

        if (Status != ProjectTaskStatus.Concluded && status == ProjectTaskStatus.Concluded)
            ConcludedDate = DateTime.Now;
        
        Status = status;
        Responsible = responsible;
        ResponsibleId = responsible?.Id;
        
        DomainEvents.Enqueue(new ProjectTaskUpdatedDomainEvent(
            projectTask: this
        ));
        
        return Result.Success();
    }

    public Result TryAddComment(ProjectTaskComment projectTaskComment)
    {
        _comments.Add(projectTaskComment);
        
        DomainEvents.Enqueue(new ProjectTaskCommentAddedDomainEvent(
            projectTask: this,
            projectTaskComment: projectTaskComment
        ));
        
        return Result.Success();
    }
}