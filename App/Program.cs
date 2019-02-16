using System;
using App.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;
using Serilog.Events;

namespace App
{
    public class Program
    {
        public static int Main(string[] args)
        {
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(BsonType.String));
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
                .UseKestrel()
                .Build();

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Set up container here...

            return services.BuildServiceProvider(validateScopes: false);
        }
    }
}
