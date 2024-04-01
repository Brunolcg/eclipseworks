namespace Eclipseworks.Application.Commands.UpdateProjectTask;

public struct UpdateProjectTaskCommand : IRequest<NewResponse<Unit>>
{
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DueDate { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public Guid ProjectId { get; init; }
    public Guid ProjectTaskId { get; init; }
    public Guid? ResponsibleId { get; init; }
}