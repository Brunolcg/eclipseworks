namespace Eclipseworks.Application.Commands.DeleteProjectTask;

public class DeleteProjectTaskCommandHandler : IRequestHandler<DeleteProjectTaskCommand, NewResponse<Unit>>
{
    private readonly IEclipseworksDbContext _context;

    public DeleteProjectTaskCommandHandler(IEclipseworksDbContext context)
        => _context = context;
    
    public async Task<NewResponse<Unit>> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var validation = await new DeleteProjectTaskCommandValidator().ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return new UnprocessableResponse<Unit>(validation.Errors.Select(x => x.ErrorMessage));
        
        var project = await _context.Projects
            .Include(p => p.Tasks.Where(t => t.Id == request.ProjectTaskId))
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken: cancellationToken);

        if (project is null)
            return new NotFoundResponse<Unit>(Messages.ProjectNotFound);

        var projectTask = project.Tasks.FirstOrDefault();
        
        if (projectTask is null)
            return new NotFoundResponse<Unit>(Messages.ProjectTaskNotFound);

        var tryDeleteResult = project.TryDeleteTask(projectTask);
        
        if (tryDeleteResult is FailureResult deleteFailureResult)
            return new UnprocessableResponse<Unit>(deleteFailureResult.Error);
        
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        
        return new NoContentResponse<Unit>();
    }
}