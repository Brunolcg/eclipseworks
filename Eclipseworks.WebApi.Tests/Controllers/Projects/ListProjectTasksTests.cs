namespace Eclipseworks.WebApi.Tests.Controllers.Projects;

public class ListProjectTasksTests : IClassFixture<EclipseworksWebApplicationFactory<IProjectsClient>>, IAsyncLifetime
{
    private readonly EclipseworksWebApplicationFactory<IProjectsClient> _factory;

    public ListProjectTasksTests(EclipseworksWebApplicationFactory<IProjectsClient> factory)
        => _factory = factory;
    
    [Fact]
    public async Task ListProjectTasks_ShouldReturnOk()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.CreateWithMaxTasks());
            await context.SaveChangesAsync();

            return await context.Projects
                .Include(p => p.Tasks)
                .FirstAsync();
        });
        
        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.ListProjectTasksAsync(project.Id);

        //Assert
        response.Ensure(HttpStatusCode.OK);
        
        var projectTasksResponse = response.Content;
        
        projectTasksResponse.Should().NotBeNull();
        projectTasksResponse!.Should().BeEquivalentTo(
            project.Tasks.Select(t => new ListProjectTasksViewModel()
            {
                Id = t.Id,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Status = t.Status,
                Title = t.Title
            }));
    }

    public async Task InitializeAsync()
        => await _factory.SeedDatabaseAsync();

    public async Task DisposeAsync()
        => await _factory.ResetDatabaseAsync();
}
