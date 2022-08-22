using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Repositories;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using MovieFinder.Infrastructure.Repositories;
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

        services.AddIdentityCore<User>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<MovieContext>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(config["JWTSettings:TokenKey"]))
                };
            });
            
            
        services.AddAuthorization();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}