namespace Eclipseworks.Application.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, NewResponse<Unit>>
{
    private readonly IEclipseworksDbContext _context;

    public DeleteProjectCommandHandler(IEclipseworksDbContext context)
        => _context = context;
    
    public async Task<NewResponse<Unit>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var validation = await new DeleteProjectCommandValidator().ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return new UnprocessableResponse<Unit>(validation.Errors.Select(x => x.ErrorMessage));
        
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken: cancellationToken);

        if (project is null)
            return new NotFoundResponse<Unit>(Messages.ProjectNotFound);

        if (!project.CanDelete())
            return new UnprocessableResponse<Unit>(Messages.ProjectHasPendingTasks);

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        
        return new NoContentResponse<Unit>();
    }
}