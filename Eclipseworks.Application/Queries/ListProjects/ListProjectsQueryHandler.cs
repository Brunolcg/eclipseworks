namespace Eclipseworks.Application.Queries.ListProjects;

public class ListProjectsQueryHandler : IRequestHandler<ListProjectsQuery, NewResponse<IEnumerable<ListProjectsViewModel>>>
{
    private readonly IEclipseworksDbContext _context;

    public ListProjectsQueryHandler(IEclipseworksDbContext context)
        => _context = context;
    
    public async Task<NewResponse<IEnumerable<ListProjectsViewModel>>> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
            .OrderBy(p => p.Name)
            .Select(p => new ListProjectsViewModel
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new OkResponse<IEnumerable<ListProjectsViewModel>>(projects);
    }
}