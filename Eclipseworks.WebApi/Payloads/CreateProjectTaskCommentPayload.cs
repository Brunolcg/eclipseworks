namespace Microsoft.Extensions.DependencyInjection.Payloads;

public struct CreateProjectTaskCommentPayload
{
    public string Description { get; init; }

    public CreateProjectTaskCommentCommand AsCommand(Guid projectId, Guid projectTaskId)
        => new()
        {
            ProjectId = projectId,
            ProjectTaskId = projectTaskId,
            Description = Description
        };
}