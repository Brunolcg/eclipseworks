namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string moduleName)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "swagger";
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{moduleName} Documentation");
            c.DocExpansion(DocExpansion.None);
            c.DefaultModelsExpandDepth(-1);
        });

        return app;
    }
}