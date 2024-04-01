namespace Microsoft.Extensions.DependencyInjection.DomainEvents;

public class ProjectTaskCommentAddedDomainEventHandler : INotificationHandler<ProjectTaskCommentAddedDomainEvent>
{
    private readonly IEclipseworksDbContext _context;
    private readonly RequestContext _requestContext;

    public ProjectTaskCommentAddedDomainEventHandler(
        IEclipseworksDbContext context, 
        RequestContext requestContext
    )
    {
        _context = context;
        _requestContext = requestContext;
    }
    
    public async Task Handle(ProjectTaskCommentAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        var historyUpdate = new HistoryUpdate(
            type: HistoryUpdateType.AddProjectTaskComment,
            entityId: notification.ProjectTask.Id,
            userId: _requestContext.LoggedUserId
        );
        
        historyUpdate.AddChanges(new HistoryUpdateChange(
            property: $"{nameof(ProjectTaskComment)}.{nameof(ProjectTaskComment.Description)}",
            newValue: notification.ProjectTaskComment.Description
        ));
        
        await _context.HistoryUpdates.AddAsync(historyUpdate, cancellationToken: cancellationToken);
    }
}