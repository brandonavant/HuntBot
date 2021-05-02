using HuntBot.Application;
using HuntBot.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuntBot.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            using var host = CreateHostBuilder().Build();
            using var serviceScope = host.Services.CreateScope();

            var services = serviceScope.ServiceProvider;

            try
            {
                var frmMain = services.GetRequiredService<FrmMain>();

                System.Windows.Forms.Application.Run(frmMain);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to create service scope in Main.");
            }
        }

        /// <summary>
        /// Prepares configuration, logging, and the dependency injection container.
        /// </summary>
        /// <returns>An initializes an instance of <see cref="HostBuilder"/> with the loaded configuration, logging, and dependency injection container.</returns>
        private static IHostBuilder CreateHostBuilder()
        {
            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            return Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Middleware
                    var serilogLogger = new LoggerConfiguration()
                        .WriteTo.File("HuntBot.log")
                        .CreateLogger();

                    services.AddLogging(loggingConfig =>
                    {
                        loggingConfig.SetMinimumLevel(LogLevel.Information);
                        loggingConfig.AddSerilog(logger: serilogLogger, dispose: true);
                    });

                    // Dependencies
                    services.AddApplicationDependencies();
                    services.AddInfrastructureDependencies();
                    services.AddScoped<FrmMain>();
                });
        }
    }
}