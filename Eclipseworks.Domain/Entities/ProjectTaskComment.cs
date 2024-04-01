namespace Eclipseworks.Domain.Entities;

public class ProjectTaskComment : EntityBase
{
    public string Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public User User { get; init; } = null!;
    public Guid UserId { get; private set; }
    public ProjectTask ProjectTask { get; init; } = null!;
    public Guid ProjectTaskId { get; private set; }

    protected ProjectTaskComment()
    {
    }
    
    public ProjectTaskComment(
        string description,
        User user
    )
    {
        Description = description;
        CreatedAt = DateTime.Now;
        User = user;
        UserId = user.Id;
    }
}