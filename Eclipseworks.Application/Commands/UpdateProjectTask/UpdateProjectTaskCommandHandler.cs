namespace Eclipseworks.Application.Commands.UpdateProjectTask;

public class UpdateProjectTaskCommandHandler : IRequestHandler<UpdateProjectTaskCommand, NewResponse<Unit>>
{
    private readonly IEclipseworksDbContext _context;
    private readonly IMediator _mediator;

    public UpdateProjectTaskCommandHandler(
        IEclipseworksDbContext context, 
        IMediator mediator
    )
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<NewResponse<Unit>> Handle(UpdateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var validation = await new UpdateProjectTaskCommandValidator().ValidateAsync(request, cancellationToken);
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

        var responsible = request.ResponsibleId is not null 
            ? await _context.Users
                .FirstOrDefaultAsync(p => p.Id == request.ResponsibleId, cancellationToken: cancellationToken)
            : null;
        
        if (request.ResponsibleId is not null && responsible is null)
            return new NotFoundResponse<Unit>(Messages.ResponsibleNotFound);

        var tryUpdateResult = projectTask.TryUpdate(
            title: request.Title,
            description: request.Description,
            dueDate: request.DueDate,
            status: request.Status,
            responsible: responsible
        );

        if (tryUpdateResult is FailureResult updateFailureResult)
            return new UnprocessableResponse<Unit>(updateFailureResult.Error);
        
        await _mediator.PublishDomainEventsAsync(projectTask.DomainEvents, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        
        return new NoContentResponse<Unit>();
    }
}