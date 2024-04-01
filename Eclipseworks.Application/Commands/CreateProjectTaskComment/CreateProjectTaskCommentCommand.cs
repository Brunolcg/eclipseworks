namespace Eclipseworks.Application.Commands.CreateProjectTaskComment;

public struct CreateProjectTaskCommentCommand : IRequest<NewResponse<Guid>>
{
    public string Description { get; init; }
    public Guid ProjectId { get; init; }
    public Guid ProjectTaskId { get; init; }
}