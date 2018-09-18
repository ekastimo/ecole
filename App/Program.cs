using System;
using App.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace App
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(@"Logs\application.log",rollingInterval: RollingInterval.Hour)
                .CreateLogger();
            
            try
            {
                Log.Information("Starting web host");
                var host = BuildWebHost(args);
                Seed.EnsureSeedData(host.Services).GetAwaiter().GetResult();
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Set up container here...

            return services.BuildServiceProvider(validateScopes: false);
        }
    }
}
