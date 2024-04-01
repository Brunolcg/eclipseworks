namespace Eclipseworks.WebApi.Tests.Controllers.Projects;

public class ListProjectsTests : IClassFixture<EclipseworksWebApplicationFactory<IProjectsClient>>, IAsyncLifetime
{
    private readonly EclipseworksWebApplicationFactory<IProjectsClient> _factory;

    public ListProjectsTests(EclipseworksWebApplicationFactory<IProjectsClient> factory)
        => _factory = factory;
    
    [Fact]
    public async Task ListProjects_ShouldReturnOk()
    {
        //Arrange
        var projects = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddRangeAsync(ProjectMother.GetAll());
            await context.SaveChangesAsync();

            return await context.Projects.ToListAsync();
        });
        
        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.ListProjectAsync();

        //Assert
        response.Ensure(HttpStatusCode.OK);
        
        var projectsResponse = response.Content;
        
        projectsResponse.Should().NotBeNull();
        projectsResponse!.Should().BeEquivalentTo(
            projects.Select(p => new ListProjectsViewModel()
            {
                Id = p.Id,
                Name = p.Name
            }));
    }

    public async Task InitializeAsync()
        => await _factory.SeedDatabaseAsync();

    public async Task DisposeAsync()
        => await _factory.ResetDatabaseAsync();
}