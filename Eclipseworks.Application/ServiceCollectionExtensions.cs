namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssembly(assembly)
        );
        
        services.TryAddTransient(provider =>
        {
            var dbContext = provider.GetRequiredService<IEclipseworksDbContext>();
            var loggedUser = dbContext.Users.FirstOrDefault();

            return new RequestContext
            {
                LoggedUserId = loggedUser?.Id ?? Guid.Empty
            };
        });
        
        return services;
    }
}