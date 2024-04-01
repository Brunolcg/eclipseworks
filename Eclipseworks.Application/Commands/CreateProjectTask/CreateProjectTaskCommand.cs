namespace Eclipseworks.Application.Commands.CreateProjectTask;

public struct CreateProjectTaskCommand : IRequest<NewResponse<Guid>>
{
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DueDate { get; init; }
    public ProjectTaskPriority Priority { get; init; }
    public Guid ProjectId { get; init; }
    public Guid? ResponsibleId { get; init; }
}