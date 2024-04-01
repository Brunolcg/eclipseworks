namespace Eclipseworks.Application.Queries.ListProjectTasks;

public record struct ListProjectTasksViewModel
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required DateTime DueDate { get; init; }
    public required ProjectTaskStatus Status { get; init; }
    public required ProjectTaskPriority Priority { get; init; }
}