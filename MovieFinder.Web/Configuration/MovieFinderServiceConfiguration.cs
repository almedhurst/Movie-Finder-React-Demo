using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using MovieFinder.Infrastructure.Services;

namespace MovieFinder.Web.Configuration;

public static class MovieFinderServiceConfiguration
{
    public static IServiceCollection AddMovieFinderServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<MovieContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("MovieFinder.Infrastructure"));
        });

        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}