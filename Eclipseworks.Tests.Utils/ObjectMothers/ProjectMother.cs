namespace Eclipseworks.Tests.Utils.ObjectMothers;

public static class ProjectMother
{
    public static Project Create(string? name = null)
        => new (name: name ?? "Project Test");

    public static Project CreateWithMaxTasks(string? name = null)
    {
        var project = Create();
        
        for(var index = 0; index < Project.MaxTasksPerProject; index++)
            project.TryAddTask(ProjectTaskMother.Create());

        return project;
    }
    
    public static Project CreateWithOneTask(string? name = null)
    {
        var project = Create();
        
        project.TryAddTask(ProjectTaskMother.Create());

        return project;
    }

    public static IEnumerable<Project> GetAll()
    {
        yield return Create();
        yield return CreateWithOneTask();
        yield return CreateWithMaxTasks();
    }
}