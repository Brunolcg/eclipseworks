namespace Eclipseworks.Application.Commands.CreateProjectTask;

public class CreateProjectTaskCommandHandler : IRequestHandler<CreateProjectTaskCommand, NewResponse<Guid>>
{
    private readonly IEclipseworksDbContext _context;

    public CreateProjectTaskCommandHandler(IEclipseworksDbContext context)
        => _context = context;
    
    public async Task<NewResponse<Guid>> Handle(CreateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var validation = await new CreateProjectTaskCommandValidator().ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return new UnprocessableResponse<Guid>(validation.Errors.Select(x => x.ErrorMessage));
        
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId);

        if (project is null)
            return new NotFoundResponse<Guid>(Messages.ProjectNotFound);
        
        var responsible = request.ResponsibleId is not null 
            ? await _context.Users
                .FirstOrDefaultAsync(p => p.Id == request.ResponsibleId, cancellationToken: cancellationToken)
            : null;
        
        if (request.ResponsibleId is not null && responsible is null)
            return new NotFoundResponse<Guid>(Messages.ResponsibleNotFound);
        
        var projectTask = new ProjectTask(
            title: request.Title,
            description: request.Description,
            dueDate: request.DueDate,
            priority: request.Priority,
            responsible: responsible
        );

        if (project.TryAddTask(projectTask: projectTask) is FailureResult updateFailureResult)
            return new UnprocessableResponse<Guid>(updateFailureResult.Error);
        
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        
        return new CreatedResponse<Guid>(projectTask.Id);
    }
}