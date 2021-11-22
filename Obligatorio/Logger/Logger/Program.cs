using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logger.LogsManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Logger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Logs server is starting...");
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            {
                LogsQueueName = config.GetSection("LogsServerConfiguration").GetSection("LogsQueueName").Value,
                WebApiHttpPort = config.GetSection("LogsServerConfiguration").GetSection("WebApiHttpPort").Value,
                WebApiHttpsPort = config.GetSection("LogsServerConfiguration").GetSection("WebApiHttpsPort").Value,
                RabbitMQServerIP = config.GetSection("LogsServerConfiguration").GetSection("RabbitMQServerIP").Value,
                RabbitMQServerPort = config.GetSection("LogsServerConfiguration").GetSection("RabbitMQServerPort").Value,
            };

            LogReceiver logReceiver = new LogReceiver(loggerConfiguration);
            logReceiver.ReceiveServerLogs();

            CreateHostBuilder(args, loggerConfiguration).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, LoggerConfiguration configuration)
        {

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

    }
}
