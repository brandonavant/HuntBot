using System;
using System.IO;
using System.Threading.Tasks;
using HuntBot.Application;
using HuntBot.Infrastructure;
using HuntBot.Infrastructure.Database.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HuntBot.App
{
    class Program
    {    
        /// <summary>
        /// Entrypoint of the .NET application.
        /// </summary>
        /// <param name="args">Arguments passed into the application via command-line.</param>
        public static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();            

            try
            {
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // NOTE: The purpose of environmentName is to provide a means of feeding different
            // appsettings files for different environments. For example, when running locally,
            // you would want to utilize an appsettings.Local.json, which would NOT be checked
            // into Git. This allows you to feed AW Configuration values without fear of secrets
            // ending up in Git.
            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");            

            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    configHost.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
                    configHost.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<HuntBot>();
                    services.AddApplicationDependencies();
                    services.AddInfrastructureDependencies();
                });
        }
    }
}
