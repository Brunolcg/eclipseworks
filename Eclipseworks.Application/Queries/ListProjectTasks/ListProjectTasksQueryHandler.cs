namespace Eclipseworks.Application.Queries.ListProjectTasks;

public class ListProjectTasksQueryHandler : IRequestHandler<ListProjectTasksQuery, NewResponse<IEnumerable<ListProjectTasksViewModel>>>
{
    private readonly IEclipseworksDbContext _context;

    public ListProjectTasksQueryHandler(IEclipseworksDbContext context)
        => _context = context;
    
    public async Task<NewResponse<IEnumerable<ListProjectTasksViewModel>>> Handle(ListProjectTasksQuery request, CancellationToken cancellationToken)
    {
        var projectTasks = await _context.Projects
            .Where(p => p.Id == request.ProjectId)
            .SelectMany(p => p.Tasks)
            .Select(t => new ListProjectTasksViewModel
            {
                Id = t.Id,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Status = t.Status,
                Title = t.Title
            })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new OkResponse<IEnumerable<ListProjectTasksViewModel>>(projectTasks);
    }
}