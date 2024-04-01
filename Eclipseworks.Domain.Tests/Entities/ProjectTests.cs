namespace Eclipseworks.Domain.Tests.Entities;

public class ProjectTests
{
    [Fact]
    public void Constructor_ShouldCreateProjectWithSuccess()
    {
        var name = "Project Name";

        var project = new Project(name: name);

        project.Name.Should().Be(name);
    }
    
    [Fact]
    public void TryAddTask_ShouldReturnSuccess()
    {
        var project = ProjectMother.Create();
        var projectTask = ProjectTaskMother.Create();
        var projectTasks = new List<ProjectTask> { projectTask };
        
        var result = project.TryAddTask(projectTask);

        result.Should().BeOfType<SuccessResult>();
        project.Tasks.Should().BeEquivalentTo(projectTasks);
    }
    
    [Fact]
    public void TryAddTask_ShouldReturnFailure_WhenProjectAlready20Tasks()
    {
        var project = ProjectMother.CreateWithMaxTasks();
        var projectTask = ProjectTaskMother.Create();
        
        var result = project.TryAddTask(projectTask);

        result.Should().BeOfType<FailureResult>()
            .Which.Error
            .Should().Be(Messages.ProjectCannotHaveMoreThan20Tasks);
    }
    
    [Fact]
    public void TryDeleteTask_ShouldReturnSuccess()
    {
        var project = ProjectMother.CreateWithOneTask();
        var projectTask = project.Tasks.First();
        
        var result = project.TryDeleteTask(projectTask);

        result.Should().BeOfType<SuccessResult>();
        project.Tasks.Any().Should().BeFalse();
    }
    
    [Fact]
    public void TryDeleteTask_ShouldReturnFailure_WhenProjectTaskNotFound()
    {
        var project = ProjectMother.CreateWithOneTask();
        var projectTask = ProjectTaskMother.Create();
        
        var result = project.TryDeleteTask(projectTask);

        result.Should().BeOfType<FailureResult>()
            .Which.Error
            .Should().Be(Messages.ProjectTaskNotFound);
    }
}