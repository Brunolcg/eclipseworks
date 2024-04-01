namespace Eclipseworks.Application.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, NewResponse<Guid>>
{
    private readonly IEclipseworksDbContext _context;

    public CreateProjectCommandHandler(IEclipseworksDbContext context)
        => _context = context;
    
    public async Task<NewResponse<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(name: request.Name);
        
        await _context.Projects.AddAsync(project, cancellationToken: cancellationToken);
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        
        return new CreatedResponse<Guid>(project.Id);
    }
}