namespace Microsoft.Extensions.DependencyInjection.Payloads;

public struct CreateProjectTaskPayload
{
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DueDate { get; init; }
    public ProjectTaskPriority Priority { get; init; }
    public Guid? ResponsibleId { get; init; }

    public CreateProjectTaskCommand AsCommand(Guid projectId)
        => new()
        {
            Title = Title,
            Description = Description,
            DueDate = DueDate,
            Priority = Priority,
            ProjectId = projectId,
            ResponsibleId = ResponsibleId
        };
}