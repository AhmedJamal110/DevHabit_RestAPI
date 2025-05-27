namespace DevHabit.API.Extensions;
public static class DatabaseExtension
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope serviceScope = app.Services.CreateScope();

       await using ApplicationDbContext applicationDbContext = serviceScope
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        try
        {
            await applicationDbContext.Database.MigrateAsync();
            app.Logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }
}
