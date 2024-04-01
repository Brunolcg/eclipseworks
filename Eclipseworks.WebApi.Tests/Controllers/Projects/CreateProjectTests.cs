namespace Eclipseworks.WebApi.Tests.Controllers.Projects;

public class CreateProjectTests : IClassFixture<EclipseworksWebApplicationFactory<IProjectsClient>>, IAsyncLifetime
{
    private readonly EclipseworksWebApplicationFactory<IProjectsClient> _factory;

    public CreateProjectTests(EclipseworksWebApplicationFactory<IProjectsClient> factory)
        => _factory = factory;
    
    [Fact]
    public async Task Create_ShouldReturnCreatedAndProjectId()
    {
        //Arrange
        var command = new CreateProjectCommand()
        {
            Name = "Project Name"
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectAsync(command);

        //Assert
        await _factory.UsingContextAsync(async context =>
        {
            response.Ensure(HttpStatusCode.Created);
            var projectId = response.Content;
            
            var project = await context.Projects
                .FirstOrDefaultAsync(r => r.Id == projectId);
            
            project.Should().NotBeNull();
            project!.Id.Should().Be(projectId);
            project.Name.Should().Be(command.Name);
        });
    }

    public async Task InitializeAsync()
        => await _factory.SeedDatabaseAsync();

    public async Task DisposeAsync()
        => await _factory.ResetDatabaseAsync();
}