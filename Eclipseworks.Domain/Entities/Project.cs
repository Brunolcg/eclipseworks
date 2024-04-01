namespace Eclipseworks.Domain.Entities;

public class Project : EntityBase
{
    public const int MaxTasksPerProject = 20;
    
    public string Name { get; private set; } = null!;
    private readonly List<ProjectTask> _tasks = new();
    public IReadOnlyCollection<ProjectTask> Tasks => _tasks;
    
    protected Project()
    {
    }

    public Project(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Result TryAddTask(ProjectTask projectTask)
    {
        if (Tasks.Count == MaxTasksPerProject)
            return Result.Failure(error: Messages.ProjectCannotHaveMoreThan20Tasks);
        
        _tasks.Add(projectTask);
        
        return Result.Success();
    }
    
    public Result TryDeleteTask(ProjectTask projectTask)
    {
        if(_tasks.All(t => t.Id != projectTask.Id))
            return Result.Failure(error: Messages.ProjectTaskNotFound);
        
        _tasks.Remove(projectTask);
        
        return Result.Success();
    }

    public bool CanDelete()
        => _tasks.All(t => t.Status != ProjectTaskStatus.Pending);
}