using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Entities;
using MovieFinder.Infrastructure.Data;

namespace MovieFinder.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

        try
        {
            await context.Database.MigrateAsync();
            var seeder = new MovieContextSeed(context, userManager, loggerFactory);
            seeder.StartMovieSeed();
            await seeder.StartUserSeed();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Problem migrating data");
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

