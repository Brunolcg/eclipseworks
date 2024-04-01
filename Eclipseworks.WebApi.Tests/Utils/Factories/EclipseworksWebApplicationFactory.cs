namespace Eclipseworks.WebApi.Tests.Utils.Factories;

public class EclipseworksWebApplicationFactory<TClient> : WebApplicationFactory<Program>
{
    public TClient CreateHttpClient()
    {
        return RestService.For<TClient>(client: CreateClient(), settings: new RefitSettings
        {
            CollectionFormat = CollectionFormat.Multi
        });
    }
    
    public virtual async Task SeedDatabaseAsync() =>
        await UsingContextAsync(async context =>
        {
            await context.Users.AddAsync(UserMother.Admin());
            await context.SaveChangesAsync();
        });

    public virtual async Task ResetDatabaseAsync() =>
        await UsingContextAsync(async context =>
        {
            context.Projects.RemoveRange(context.Projects);
            context.HistoryUpdatesChanges.RemoveRange(context.HistoryUpdatesChanges);
            context.HistoryUpdates.RemoveRange(context.HistoryUpdates);
            
            await context.SaveChangesAsync();
        });

    public async Task UsingContextAsync(Func<EclipseworksDbContext, Task> action)
    {
        await using var scope = Services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<EclipseworksDbContext>();
        await action(context);
    }

    public async Task<TResult> UsingContextAsync<TResult>(Func<EclipseworksDbContext, Task<TResult>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<EclipseworksDbContext>();
        return await action(context);
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment(Environments.Development)
            .ConfigureTestServices(services =>
            {
                
                services.RemoveAll<DbConnection>();
                services.AddSingleton<DbConnection>(_ =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    return connection;
                });

                services.RemoveAll<EclipseworksDbContext>();
                services.AddScoped<EclipseworksDbContext>(provider =>
                {
                    var connection = provider.GetRequiredService<DbConnection>();

                    var optionsBuilder = new DbContextOptionsBuilder<DbContext>();

                    var options = optionsBuilder
                        .UseSqlite(connection)
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging()
                        .Options;

                    var context = new EclipseworksDbContext(
                        options: options
                    );

                    context.Database.EnsureCreated();

                    return context;
                });
            });
    }
}
