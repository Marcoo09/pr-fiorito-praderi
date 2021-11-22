using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ServerGrpc.Connections;
using ServerGrpc.Logs;

namespace ServerGrpc
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            ServerConfiguration serverConfiguration = new ServerConfiguration()
            {
                RabbitMQServerIP = config.GetSection("ServerConfiguration").GetSection("RabbitMQServerIP").Value,
                LogsQueueName = config.GetSection("ServerConfiguration").GetSection("LogsQueueName").Value,
                ServerPort = config.GetSection("ServerConfiguration").GetSection("ServerPort").Value,
                ServerIP = config.GetSection("ServerConfiguration").GetSection("ServerIP").Value,
                RabbitMQServerPort = config.GetSection("ServerConfiguration").GetSection("RabbitMQServerPort").Value,
                GrpcApiHttpPort = config.GetSection("ServerConfiguration").GetSection("GrpcApiHttpPort").Value,
                GrpcApiHttpsPort = config.GetSection("ServerConfiguration").GetSection("GrpcApiHttpsPort").Value
            };

            LogEmitter logEmitter = new LogEmitter(serverConfiguration);

            await HandleConnections(serverConfiguration);

            CreateHostBuilder(args, serverConfiguration).Build().Run();
        }

        static async Task HandleConnections(ServerConfiguration serverConfiguration)
        {
            ConnectionsHandler connectionsHandler = new ConnectionsHandler(serverConfiguration);

            var task = Task.Run(async () => await connectionsHandler.StartListeningAsync());
            Console.WriteLine("Write any key to shutdown the server");

            //await connectionsHandler.StartShutDownAsync();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args, ServerConfiguration serverConfiguration)
        {
            string httpUrl = $"http://{serverConfiguration.ServerIP}:{serverConfiguration.GrpcApiHttpPort}/";
            string httpsUrl = $"https://{serverConfiguration.ServerIP}:{serverConfiguration.GrpcApiHttpsPort}/";

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    options.ListenLocalhost(Int32.Parse(serverConfiguration.GrpcApiHttpPort), o => o.Protocols = HttpProtocols.Http2);
                });
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls(httpUrl, httpsUrl);
            });
        }

    }
}
