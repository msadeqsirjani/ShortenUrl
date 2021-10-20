using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace ShortUrl.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(configSettings)
                .CreateLogger();

            try
            {
                Log.Information("Application starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "The application failed to start");
                throw;
            }
            finally
            {
                Log.CloseAndFlush(); //close logs that is not written yet
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(logging => { logging.AddSerilog(); });

        }
    }
}
