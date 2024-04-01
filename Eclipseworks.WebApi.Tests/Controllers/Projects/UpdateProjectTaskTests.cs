namespace Eclipseworks.WebApi.Tests.Controllers.Projects;

public class UpdateProjectTaskTests : IClassFixture<EclipseworksWebApplicationFactory<IProjectsClient>>, IAsyncLifetime
{
    private readonly EclipseworksWebApplicationFactory<IProjectsClient> _factory;

    public UpdateProjectTaskTests(EclipseworksWebApplicationFactory<IProjectsClient> factory)
        => _factory = factory;
    
    [Fact]
    public async Task UpdateProjectTask_ShouldReturnNoContent_WhenProjectTaskHasUpdatedWithSucess()
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
        
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new UpdateProjectTaskPayload
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            ResponsibleId = responsible.Id,
            Status = ProjectTaskStatus.InProgress
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.UpdateProjectTaskAsync(project.Id, projectTaskId, payload);

        //Assert
        await _factory.UsingContextAsync(async context =>
        {
            response.Ensure(HttpStatusCode.NoContent);
            
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
            projectTaskResult.Status.Should().Be(payload.Status);
            projectTaskResult.ResponsibleId.Should().Be(payload.ResponsibleId);
            
            var historyUpdate = await context.HistoryUpdates
                .FirstOrDefaultAsync(r => r.EntityId == projectTaskId);
            
            historyUpdate.Should().NotBeNull();
        });
    }
    
    [Fact]
    public async Task UpdateProjectTask_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        //Arrange
        var project = ProjectMother.CreateWithOneTask();

        var projectTaskId = project.Tasks.First().Id;
        
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new UpdateProjectTaskPayload
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            ResponsibleId = responsible.Id,
            Status = ProjectTaskStatus.InProgress
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.UpdateProjectTaskAsync(project.Id, projectTaskId, payload);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ProjectNotFound);
    }
    
    [Fact]
    public async Task UpdateProjectTask_ShouldReturnNotFound_WhenResponsibleDoesNotExist()
    {
        //Arrange
        var project = await _factory.UsingContextAsync(async context =>
        {
            await context.Projects.AddAsync(ProjectMother.CreateWithOneTask());
            await context.SaveChangesAsync();

            return await context.Projects.FirstAsync();
        });
        
        var projectTaskId = project.Tasks.First().Id;
        
        var payload = new UpdateProjectTaskPayload
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            ResponsibleId = Guid.NewGuid(),
            Status = ProjectTaskStatus.InProgress
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.UpdateProjectTaskAsync(project.Id, projectTaskId, payload);

        //Assert
        response.Ensure(HttpStatusCode.NotFound, Messages.ResponsibleNotFound);
    }
    
    [Fact]
    public async Task UpdateProjectTask_ShouldReturnUnprocessableResponse_WhenProjectIdIsEmpty()
    {
        //Arrange
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new UpdateProjectTaskPayload
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            ResponsibleId = responsible.Id,
            Status = ProjectTaskStatus.InProgress
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.UpdateProjectTaskAsync(Guid.Empty, Guid.NewGuid(), payload);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectIdIsRequired);
    }
    
    [Fact]
    public async Task UpdateProjectTask_ShouldReturnUnprocessableResponse_WhenProjectTaskIdIsEmpty()
    {
        //Arrange
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new UpdateProjectTaskPayload
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            ResponsibleId = responsible.Id,
            Status = ProjectTaskStatus.InProgress
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.UpdateProjectTaskAsync(Guid.NewGuid(), Guid.Empty, payload);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.ProjectTaskIdIsRequired);
    }
    
    [Fact]
    public async Task UpdateProjectTask_ShouldReturnUnprocessableResponse_WhenStatusIsInvalid()
    {
        //Arrange
        var responsible = await _factory.UsingContextAsync(async context => await context.Users.FirstAsync());
        
        var payload = new UpdateProjectTaskPayload
        {
            Title = "Title",
            Description = "Description",
            DueDate = DateTime.Now,
            ResponsibleId = responsible.Id,
            Status = (ProjectTaskStatus) 5
        };

        var client = _factory.CreateHttpClient();
        
        //Act
        var response = await client.UpdateProjectTaskAsync(Guid.NewGuid(), Guid.NewGuid(), payload);

        //Assert
        response.Ensure(HttpStatusCode.UnprocessableEntity, Messages.InvalidStatus);
    }
    
    public async Task InitializeAsync()
        => await _factory.SeedDatabaseAsync();

    public async Task DisposeAsync()
        => await _factory.ResetDatabaseAsync();
}