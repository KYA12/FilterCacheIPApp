using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using FilterIPApp.Logging;
using FilterIPApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FilterIPApp 
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public ILoggerFactory _loggerFactory;
        public IPTableContext context;
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<IPTableContext>(options => options.UseSqlServer(ConnectionString));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));//Creating logging system 
            var logger = loggerFactory.CreateLogger("FileLogger");

            app.Use(async (context, next) =>
            {
                await next.Invoke();

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)//Implementing custom error handler
                {
                    await context.Response.WriteAsync("Woops! You are not allowed to see this page");
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                    routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
