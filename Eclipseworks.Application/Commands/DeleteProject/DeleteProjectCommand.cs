namespace Eclipseworks.Application.Commands.DeleteProject;

public struct DeleteProjectCommand : IRequest<NewResponse<Unit>>
{
    public Guid ProjectId { get; init; }

    public DeleteProjectCommand(Guid projectId)
        => ProjectId = projectId;
}