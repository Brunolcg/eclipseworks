namespace Eclipseworks.WebApi.Tests.Controllers.Projects;

public class CreateProjectTaskCommentTests : IClassFixture<EclipseworksWebApplicationFactory<IProjectsClient>>, IAsyncLifetime
{
    private readonly EclipseworksWebApplicationFactory<IProjectsClient> _factory;

    public CreateProjectTaskCommentTests(EclipseworksWebApplicationFactory<IProjectsClient> factory)
        => _factory = factory;
    
    [Fact]
    public async Task CreateProjectTaskComment_ShouldReturnCreatedAndProjectTaskCommentId()
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
        
        var payload = new CreateProjectTaskCommentPayload()
        {
            Description = "Comment"
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskCommentAsync(project.Id, projectTaskId, payload);

        //Assert
        await _factory.UsingContextAsync(async context =>
        {
            response.Ensure(HttpStatusCode.Created);
            var projectTaskCommentId = response.Content;
            
            var projectResult = await context.Projects
                .Include(p => p.Tasks.Where(t => t.Id == projectTaskId))
                .ThenInclude(t => t.Comments.Where(t => t.Id == projectTaskCommentId))
                .FirstOrDefaultAsync(r => r.Id == project.Id);
            
            projectResult.Should().NotBeNull();

            var projectTaskResult = projectResult!.Tasks.FirstOrDefault();
            
            projectTaskResult.Should().NotBeNull();

            var projectTaskCommentResult = projectTaskResult!.Comments.FirstOrDefault();
            
            projectTaskCommentResult.Should().NotBeNull();
            projectTaskCommentResult!.Id.Should().Be(projectTaskCommentId);
            projectTaskCommentResult!.Description.Should().Be(payload.Description);
        });
    }

    [Fact]
    public async Task CreateProjectTaskComment_ShouldReturnNotFound_WhenProjectDoesNotExist()
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
        
        var payload = new CreateProjectTaskCommentPayload()
        {
            Description = "Comment"
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskCommentAsync(Guid.NewGuid(), projectTaskId, payload);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ProjectNotFound);
    }
    
    [Fact]
    public async Task CreateProjectTaskComment_ShouldReturnNotFound_WhenProjectTaskDoesNotExist()
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
        
        var payload = new CreateProjectTaskCommentPayload()
        {
            Description = "Comment"
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskCommentAsync(project.Id, Guid.NewGuid(), payload);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ProjectTaskNotFound);
    }
    
    [Fact]
    public async Task CreateProjectTaskComment_ShouldReturnUnprocessableResponse_WhenProjectIdIsEmpty()
    {
        //Arrange
        var payload = new CreateProjectTaskCommentPayload()
        {
            Description = "Comment"
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskCommentAsync(Guid.Empty, Guid.NewGuid(), payload);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectIdIsRequired);
    }
    
    [Fact]
    public async Task CreateProjectTaskComment_ShouldReturnUnprocessableResponse_WhenProjectTaskIdIsEmpty()
    {
        //Arrange
        var payload = new CreateProjectTaskCommentPayload()
        {
            Description = "Comment"
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskCommentAsync(Guid.NewGuid(), Guid.Empty, payload);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectTaskIdIsRequired);
    }
    
    public async Task InitializeAsync()
        => await _factory.SeedDatabaseAsync();

    public async Task DisposeAsync()
        => await _factory.ResetDatabaseAsync();
}
