namespace Eclipseworks.Tests.Utils.ObjectMothers;

public static class ProjectTaskMother
{
    public static ProjectTask Create(
        string? title = null,
        string? description = null,
        DateTime? dueDate = null,
        ProjectTaskPriority? priority = null,
        User? responsible = null
    )
        => new ProjectTask(
            title: title ?? "Title",
            description: description ?? "Description",
            dueDate: dueDate ?? DateTime.Now,
            priority: priority ?? ProjectTaskPriority.High,
            responsible: responsible
        )
        {
            Id = Guid.NewGuid()
        };
}