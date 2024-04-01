namespace Eclipseworks.WebApi.Tests.Controllers.Projects;

public class CreateProjectTaskTests : IClassFixture<EclipseworksWebApplicationFactory<IProjectsClient>>, IAsyncLifetime
{
    private readonly EclipseworksWebApplicationFactory<IProjectsClient> _factory;

    public CreateProjectTaskTests(EclipseworksWebApplicationFactory<IProjectsClient> factory)
        => _factory = factory;
    
    [Fact]
    public async Task CreateProjectTask_ShouldReturnCreatedAndProjectTaskId()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.Create());
            await context.SaveChangesAsync();

            return await context.Projects.FirstAsync();
        });
        
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new CreateProjectTaskPayload()
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            Priority = ProjectTaskPriority.High,
            ResponsibleId = responsible.Id
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskAsync(project.Id, payload);

        //Assert
        await _factory.UsingContextAsync(async context =>
        {
            response.Ensure(HttpStatusCode.Created);
            var projectTaskId = response.Content;
            
            var projectResult = await context.Projects
                .Include(p => p.Tasks.Where(t => t.Id == projectTaskId))
                .FirstOrDefaultAsync(r => r.Id == project.Id);
            
            projectResult.Should().NotBeNull();

            var projectTaskResult = projectResult!.Tasks.FirstOrDefault();
            
            projectTaskResult.Should().NotBeNull();
            
            projectTaskResult!.Id.Should().Be(projectTaskId);
            projectTaskResult.Title.Should().Be(payload.Title);
            projectTaskResult.Description.Should().Be(payload.Description);
            projectTaskResult.DueDate.Should().Be(payload.DueDate);
            projectTaskResult.Priority.Should().Be(payload.Priority);
            projectTaskResult.ResponsibleId.Should().Be(payload.ResponsibleId);
        });
    }

    [Fact]
    public async Task CreateProjectTask_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        //Arrange
        var project = ProjectMother.Create();
        
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new CreateProjectTaskPayload()
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            Priority = ProjectTaskPriority.High,
            ResponsibleId = responsible.Id
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskAsync(project.Id, payload);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ProjectNotFound);
    }
    
    [Fact]
    public async Task CreateProjectTask_ShouldReturnNotFound_WhenResponsibleDoesNotExist()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.Create());
            await context.SaveChangesAsync();

            return await context.Projects.FirstAsync();
        });
        
        var payload = new CreateProjectTaskPayload()
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            Priority = ProjectTaskPriority.High,
            ResponsibleId = Guid.NewGuid()
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskAsync(project.Id, payload);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ResponsibleNotFound);
    }
    
    [Fact]
    public async Task CreateProjectTask_ShouldReturnUnprocessableResponse_WhenProjectHasMoreThan20Tasks()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.CreateWithMaxTasks());
            await context.SaveChangesAsync();

            return await context.Projects.FirstAsync();
        });
        
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new CreateProjectTaskPayload()
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            Priority = ProjectTaskPriority.High,
            ResponsibleId = responsible.Id
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskAsync(project.Id, payload);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectCannotHaveMoreThan20Tasks);
    }
    
    [Fact]
    public async Task CreateProjectTask_ShouldReturnUnprocessableResponse_WhenProjectIdIsEmpty()
    {
        //Arrange
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new CreateProjectTaskPayload()
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            Priority = ProjectTaskPriority.High,
            ResponsibleId = responsible.Id
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskAsync(Guid.Empty, payload);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectIdIsRequired);
    }
    
    [Fact]
    public async Task CreateProjectTask_ShouldReturnUnprocessableResponse_WhenPriorityIsInvalid()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.CreateWithMaxTasks());
            await context.SaveChangesAsync();

            return await context.Projects.FirstAsync();
        });
        
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new CreateProjectTaskPayload()
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            Priority = (ProjectTaskPriority) 4,
            ResponsibleId = responsible.Id
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.CreateProjectTaskAsync(project.Id, payload);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.InvalidPriority);
    }
    
    public async Task InitializeAsync()
        => await _factory.SeedDatabaseAsync();

    public async Task DisposeAsync()
        => await _factory.ResetDatabaseAsync();
}
