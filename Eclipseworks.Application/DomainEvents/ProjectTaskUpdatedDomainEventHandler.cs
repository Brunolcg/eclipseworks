namespace Microsoft.Extensions.DependencyInjection.DomainEvents;

public class ProjectTaskUpdatedDomainEventHandler : INotificationHandler<ProjectTaskUpdatedDomainEvent>
{
    private readonly IEclipseworksDbContext _context;
    private readonly RequestContext _requestContext;

    public ProjectTaskUpdatedDomainEventHandler(
        IEclipseworksDbContext context, 
        RequestContext requestContext
    )
    {
        _context = context;
        _requestContext = requestContext;
    }

    public async Task Handle(ProjectTaskUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var oldProject = await _context.Projects
            .AsNoTracking()
            .Include(t => t.Tasks.Where(pt => pt.Id == notification.ProjectTask.Id))
            .FirstAsync(p => p.Id == notification.ProjectTask.ProjectId, cancellationToken: cancellationToken);

        var oldProjectTask = oldProject.Tasks.First();

        var historyUpdate = new HistoryUpdate(
            type: HistoryUpdateType.UpdateProjectTask,
            entityId: oldProjectTask.Id,
            userId: _requestContext.LoggedUserId
        );

        var newProjectTask = notification.ProjectTask;
        
        if (newProjectTask.Title != oldProjectTask.Title)
            historyUpdate.AddChanges(new HistoryUpdateChange(
                property: nameof(ProjectTask.Title),
                newValue: newProjectTask.Title,
                oldValue: oldProjectTask.Title
            ));
        
        if (newProjectTask.Status != oldProjectTask.Status)
            historyUpdate.AddChanges(new HistoryUpdateChange(
                property: nameof(ProjectTask.Status),
                newValue: newProjectTask.Status.ToString(),
                oldValue: oldProjectTask.Status.ToString()
            ));
        
        if (newProjectTask.Description != oldProjectTask.Description)
            historyUpdate.AddChanges(new HistoryUpdateChange(
                property: nameof(ProjectTask.Description),
                newValue: newProjectTask.Description,
                oldValue: oldProjectTask.Description
            ));
        
        if (newProjectTask.ResponsibleId != oldProjectTask.ResponsibleId)
            historyUpdate.AddChanges(new HistoryUpdateChange(
                property: nameof(ProjectTask.ResponsibleId),
                newValue: $"{newProjectTask.ResponsibleId}",
                oldValue: $"{oldProjectTask.ResponsibleId}"
            ));
        
        if (newProjectTask.DueDate != oldProjectTask.DueDate)
            historyUpdate.AddChanges(new HistoryUpdateChange(
                property: nameof(ProjectTask.DueDate),
                newValue: newProjectTask.DueDate.ToString(),
                oldValue: oldProjectTask.DueDate.ToString()
            ));
        
        if (historyUpdate.HasChanges())
            await _context.HistoryUpdates.AddAsync(historyUpdate, cancellationToken: cancellationToken);
    }
}