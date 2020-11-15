using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using static System.Environment;

namespace CSharpWars.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    var keyVault = GetEnvironmentVariable("KEY_VAULT");
                    var clientId = GetEnvironmentVariable("CLIENT_ID");
                    var clientSecret = GetEnvironmentVariable("CLIENT_SECRET");
                    configurationBuilder.AddAzureKeyVault(keyVault, clientId, clientSecret);
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();

                    if (string.IsNullOrEmpty(GetEnvironmentVariable("TYE")))
                    {
                        webBuilder.ConfigureKestrel((hostContext, options) =>
                        {
                            options.Listen(IPAddress.Any, 5000);
                        });
                    }

                    webBuilder.UseStartup<Startup>();
                });
    }
}