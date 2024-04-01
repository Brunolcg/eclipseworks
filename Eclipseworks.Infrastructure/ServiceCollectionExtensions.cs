namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("Database")
                               ?? throw new Exception("Connection String Not Found");

        services.AddScoped<EclipseworksDbContext>(provider =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();

            optionsBuilder
                .UseSqlServer(connectionString);

            optionsBuilder
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .LogTo(
                    action: Console.WriteLine,
                    categories: new[]
                    {
                        DbLoggerCategory.Database.Command.Name
                    },
                    minimumLevel: LogLevel.Information
                );

            return new EclipseworksDbContext(
                options: optionsBuilder.Options
            );
        });

        services.AddScoped<IEclipseworksDbContext>(provider => provider.GetRequiredService<EclipseworksDbContext>());

        UpdateDatabase(connectionString);
            
        return services;
    }
    
    private static void UpdateDatabase(string connectionString)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ||
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker")
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        
            var options = optionsBuilder.UseSqlServer(
                connectionString,
                sqlServerOptions => sqlServerOptions.CommandTimeout(300)).Options;

            using var context = new EclipseworksDbContext(options);
            context.Database.Migrate();

            if (!context.Users.Any())
            {
                context.Users.Add(new User(name: "Admin"));
                context.SaveChanges();
            }
        }
    }
}