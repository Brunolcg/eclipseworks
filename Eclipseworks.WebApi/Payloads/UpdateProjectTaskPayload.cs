namespace Microsoft.Extensions.DependencyInjection.Payloads;

public struct UpdateProjectTaskPayload
{
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DueDate { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public Guid? ResponsibleId { get; init; }

    public UpdateProjectTaskCommand AsCommand(Guid projectId, Guid projectTaskId)
        => new()
        {
            Title = Title,
            Description = Description,
            DueDate = DueDate,
            Status = Status,
            ProjectId = projectId,
            ProjectTaskId = projectTaskId,
            ResponsibleId = ResponsibleId
        };
}