namespace Eclipseworks.Application.Commands.CreateProject;

public struct CreateProjectCommand : IRequest<NewResponse<Guid>>
{
    public string Name { get; init; }
}