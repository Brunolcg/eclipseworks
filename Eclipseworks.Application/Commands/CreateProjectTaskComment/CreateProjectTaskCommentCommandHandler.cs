namespace Eclipseworks.Application.Commands.CreateProjectTaskComment;

public class CreateProjectTaskCommentCommandHandler : IRequestHandler<CreateProjectTaskCommentCommand, NewResponse<Guid>>
{
    private readonly IEclipseworksDbContext _context;
    private readonly RequestContext _requestContext;
    private readonly IMediator _mediator;

    public CreateProjectTaskCommentCommandHandler(
        IEclipseworksDbContext context, 
        RequestContext requestContext, 
        IMediator mediator
    )
    {
        _context = context;
        _requestContext = requestContext;
        _mediator = mediator;
    }

    public async Task<NewResponse<Guid>> Handle(CreateProjectTaskCommentCommand request, CancellationToken cancellationToken)
    {
        var validation = await new CreateProjectTaskCommentCommandValidator().ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return new UnprocessableResponse<Guid>(validation.Errors.Select(x => x.ErrorMessage));
        
        var project = await _context.Projects
            .Include(p => p.Tasks.Where(t => t.Id == request.ProjectTaskId))
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken: cancellationToken);

        if (project is null)
            return new NotFoundResponse<Guid>(Messages.ProjectNotFound);

        var projectTask = project.Tasks.FirstOrDefault();
        
        if (projectTask is null)
            return new NotFoundResponse<Guid>(Messages.ProjectTaskNotFound);

        var loggedUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == _requestContext.LoggedUserId);
        
        if (loggedUser is null)
            return new NotFoundResponse<Guid>(Messages.LoggedUserNotFound);

        var comment = new ProjectTaskComment(
            description: request.Description,
            user: loggedUser
        );
        
        var tryAddCommentResult = projectTask.TryAddComment(comment);

        if (tryAddCommentResult is FailureResult addCommentFailureResult)
            return new UnprocessableResponse<Guid>(addCommentFailureResult.Error);
        
        await _mediator.PublishDomainEventsAsync(projectTask.DomainEvents, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        
        return new CreatedResponse<Guid>(comment.Id);
    }
}