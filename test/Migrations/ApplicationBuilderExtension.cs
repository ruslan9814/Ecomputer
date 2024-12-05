using Microsoft.EntityFrameworkCore;
/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class ApplicationBuilderExtension
{
    /// <summary>
    /// Migrates the database for the given <typeparamref name="TDbContext"/>.
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="builder"> The <see cref="IApplicationBuilder" />.</param>
    /// <returns> The <see cref="IApplicationBuilder" />.</returns>
    public static IApplicationBuilder MigrateDbContext<TDbContext>(this IApplicationBuilder builder)
        where TDbContext : DbContext
    {
        using var scope = builder.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        dbContext.Database.Migrate();

        return builder;
    }
}
