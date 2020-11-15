using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using static System.Environment;

namespace CSharpWars.Validator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    if (string.IsNullOrEmpty(GetEnvironmentVariable("TYE")))
                    {
                        webBuilder.ConfigureKestrel((context, options) =>
                        {
                            options.Listen(IPAddress.Any, 5000, configure => configure.Protocols = HttpProtocols.Http2);
                        });
                    }
                    webBuilder.UseStartup<Startup>();
                });
    }
}