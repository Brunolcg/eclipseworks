namespace Eclipseworks.Application.Queries.ListProjects;

public record struct ListProjectsViewModel
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}