using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using MovieFinder.Web.Configuration;
using MovieFinder.Web.Middleware;

namespace MovieFinder.Web
{
    public class Startup
    {
        public IConfiguration _config { get; }

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwagger();

            services.AddMovieFinderServices(_config);
            
            services.AddCors();
            
            services.AddSpaStaticFiles(config => { config.RootPath = "clientapp/build"; });
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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
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
