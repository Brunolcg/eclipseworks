namespace Eclipseworks.WebApi.Tests.Controllers.Projects;

public class DeleteProjectTests : IClassFixture<EclipseworksWebApplicationFactory<IProjectsClient>>, IAsyncLifetime
{
    private readonly EclipseworksWebApplicationFactory<IProjectsClient> _factory;

    public DeleteProjectTests(EclipseworksWebApplicationFactory<IProjectsClient> factory)
        => _factory = factory;
    
    [Fact]
    public async Task DeleteProject_ShouldReturnNoContent_WhenProjectHasDeletedWithSucess()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.Create());
            await context.SaveChangesAsync();

            return await context.Projects
                .Include(p => p.Tasks)
                .FirstAsync();
        });

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectAsync(project.Id);

        //Assert
        await _factory.UsingContextAsync(async context =>
        {
            response.Ensure(HttpStatusCode.NoContent);
            
            var projectResult = await context.Projects
                .FirstOrDefaultAsync(r => r.Id == project.Id);
            
            projectResult.Should().BeNull();
        });
    }
    
    [Fact]
    public async Task DeleteProject_ShouldReturnUnprocessableResponse_WhenProjectHasPendingTasks()
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

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectAsync(project.Id);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectHasPendingTasks);
    }
    
    [Fact]
    public async Task DeleteProject_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        //Arrange
        var project = ProjectMother.Create();

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectAsync(project.Id);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ProjectNotFound);
    }
    
    [Fact]
    public async Task DeleteProject_ShouldReturnUnprocessableResponse_WhenProjectIdIsEmpty()
    {
        //Arrange
        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.DeleteProjectAsync(Guid.Empty);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectIdIsRequired);
    }
    
    public async Task InitializeAsync()
        => await _factory.SeedDatabaseAsync();

    public async Task DisposeAsync()
        => await _factory.ResetDatabaseAsync();
}