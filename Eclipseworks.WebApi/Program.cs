var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddWebApi(builder.Configuration);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory());

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger(moduleName: "Eclipseworks");

app.UseRouting();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithExposedHeaders(HeaderNames.ContentDisposition)
);

app.MapControllers();

app.Run();

public partial class Program
{
}

