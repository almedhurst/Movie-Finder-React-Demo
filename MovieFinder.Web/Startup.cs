using System.Text;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using MovieFinder.Web.Configuration;
using MovieFinder.Web.Middleware;

namespace MovieFinder.Web
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public IWebHostEnvironment _env { get; }

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwagger();

            services.AddMovieFinderServices(_config);
            
            services.AddCors();

            if (_env.IsDevelopment())
            {
                services.AddSpaStaticFiles(config => { config.RootPath = "clientapp/build"; });
            }
            
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseCors(opt =>
            {
                opt.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                if (!env.IsDevelopment())
                {
                    endpoints.MapFallbackToController("Index", "Fallback");                    
                }
            });

            if (env.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "clientapp";
                    if (env.IsDevelopment())
                    {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
            }
            
        }
    }
}
