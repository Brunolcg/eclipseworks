namespace Eclipseworks.WebApi.Tests.Controllers.Projects;

public class DeleteProjectTaskTests : IClassFixture<EclipseworksWebApplicationFactory<IProjectsClient>>, IAsyncLifetime
{
    private readonly EclipseworksWebApplicationFactory<IProjectsClient> _factory;

    public DeleteProjectTaskTests(EclipseworksWebApplicationFactory<IProjectsClient> factory)
        => _factory = factory;
    
    [Fact]
    public async Task DeleteProjectTask_ShouldReturnNoContent_WhenProjectTaskHasDeletedWithSucess()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.CreateWithOneTask());
            await context.SaveChangesAsync();

            return await context.Projects
                .Include(p => p.Tasks)
                .FirstAsync();
        });

        var projectTaskId = project.Tasks.First().Id;
        
        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectTaskAsync(project.Id, projectTaskId);

        //Assert
        await _factory.UsingContextAsync(async context =>
        {
            response.Ensure(HttpStatusCode.NoContent);
            
            var projectResult = await context.Projects
                .Include(p => p.Tasks.Where(t => t.Id == projectTaskId))
                .FirstOrDefaultAsync(r => r.Id == project.Id);
            
            projectResult.Should().NotBeNull();

            var projectTaskResult = projectResult!.Tasks.FirstOrDefault();
            
            projectTaskResult.Should().BeNull();
        });
    }
    
    [Fact]
    public async Task DeleteProjectTask_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        //Arrange
        var project = ProjectMother.CreateWithOneTask();

        var projectTaskId = project.Tasks.First().Id;

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectTaskAsync(project.Id, projectTaskId);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ProjectNotFound);
    }
    
    [Fact]
    public async Task DeleteProjectTask_ShouldReturnNotFound_WhenProjectTaskDoesNotExist()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.CreateWithOneTask());
            await context.SaveChangesAsync();

            return await context.Projects.FirstAsync();
        });
        
        var projectTaskId = Guid.NewGuid();
        
        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectTaskAsync(project.Id, projectTaskId);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ProjectTaskNotFound);
    }
    
    [Fact]
    public async Task DeleteProjectTask_ShouldReturnUnprocessableResponse_WhenProjectIdIsEmpty()
    {
        //Arrange
        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectTaskAsync(Guid.Empty, Guid.NewGuid());

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectIdIsRequired);
    }
    
    [Fact]
    public async Task DeleteProjectTask_ShouldReturnUnprocessableResponse_WhenProjectTaskIdIsEmpty()
    {
        //Arrange
        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectTaskAsync(Guid.NewGuid(), Guid.Empty);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectTaskIdIsRequired);
    }
    
    public async Task InitializeAsync()
        => await _factory.SeedDatabaseAsync();

    public async Task DisposeAsync()
        => await _factory.ResetDatabaseAsync();
}