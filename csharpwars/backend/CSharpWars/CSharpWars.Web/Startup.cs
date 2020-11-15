using CSharpWars.Common.DependencyInjection;
using CSharpWars.Web.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using static System.Convert;
using static System.Environment;

namespace CSharpWars.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigurationHelper(c =>
            {
                c.ConnectionString = _configuration.GetValue<string>("CONNECTIONSTRING");
                c.ArenaSize = ToInt32(GetEnvironmentVariable("ARENA_SIZE"));
                if (string.IsNullOrEmpty(GetEnvironmentVariable("TYE")))
                {
                    c.ValidationHost = GetEnvironmentVariable("VALIDATION_HOST");
                }
                else
                {
                    var uri = _configuration.GetServiceUri("cshwarpwars-validator");
                    c.ValidationHost = $"{uri}";
                }
                c.PointsLimit = ToInt32(GetEnvironmentVariable("POINTS_LIMIT"));
                c.BotDeploymentLimit = ToInt32(GetEnvironmentVariable("BOT_DEPLOYMENT_LIMIT"));
            });

            services.ConfigureWeb();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePathBase("/web");
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseHttpMetrics();
            app.UseCookiePolicy();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapMetrics();
            });
        }
    }
}