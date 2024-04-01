namespace Eclipseworks.Application.Commands.DeleteProjectTask;

public struct DeleteProjectTaskCommand : IRequest<NewResponse<Unit>>
{
    public Guid ProjectId { get; init; }
    public Guid ProjectTaskId { get; init; }

    public DeleteProjectTaskCommand(
        Guid projectId, 
        Guid projectTaskId
    )
    {
        ProjectId = projectId;
        ProjectTaskId = projectTaskId;
    }
}