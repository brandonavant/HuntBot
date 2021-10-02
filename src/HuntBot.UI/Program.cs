using HuntBot.Application;
using HuntBot.Domain.HuntBotGames.GameState;
using HuntBot.Domain.HuntBotGamesGameState;
using HuntBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.WinForms;
using System;
using System.Windows.Forms;

namespace HuntBot.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
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
                Log.Logger.Error(ex, "Unable to start main form.");
            }
        }

        /// <summary>
        /// Prepares configuration, logging, and the dependency injection container.
        /// </summary>
        /// <returns>An initializes an instance of <see cref="HostBuilder"/> with the loaded configuration, logging, and dependency injection container.</returns>
        private static IHostBuilder CreateHostBuilder()
        {
            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            // ConcurrentDictionary provide a global state across multiple threads with which
            // processes can check the state of a running HuntBot game session.
            var gameStateLookup = new GameStateLookup();

            gameStateLookup.TryAdd(GameStateLookupKeys.GameStatus, GameStatus.PendingLogin);

            return Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Middleware
                    var serilogLogger = new LoggerConfiguration()
                        .WriteTo.File("HuntBot.log")
                        .WriteToGridView()
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
                    services.AddSingleton(gameStateLookup);
                });
        }
    }
}