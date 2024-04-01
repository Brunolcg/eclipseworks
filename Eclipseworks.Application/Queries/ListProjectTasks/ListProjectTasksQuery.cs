namespace Eclipseworks.Application.Queries.ListProjectTasks;

public struct ListProjectTasksQuery : IRequest<NewResponse<IEnumerable<ListProjectTasksViewModel>>>
{
    public Guid ProjectId { get; init; }

    public ListProjectTasksQuery(Guid projectId)
    {
        ProjectId = projectId;
    }
}