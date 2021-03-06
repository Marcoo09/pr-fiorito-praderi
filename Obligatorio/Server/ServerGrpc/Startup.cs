using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerGrpc.Implementations;
using ServerGrpc.Services;

namespace ServerGrpc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserManager>();
                endpoints.MapGrpcService<GameManager>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            ServerConfiguration serverConfiguration = new ServerConfiguration()
            {
                RabbitMQServerIP = config.GetSection("ServerConfiguration").GetSection("RabbitMQServerIP").Value,
                RabbitMQServerPort = config.GetSection("ServerConfiguration").GetSection("RabbitMQServerPort").Value,
                LogsQueueName = config.GetSection("ServerConfiguration").GetSection("LogsQueueName").Value,
                ServerPort = config.GetSection("ServerConfiguration").GetSection("ServerPort").Value,
                ServerIP = config.GetSection("ServerConfiguration").GetSection("ServerIP").Value,
                GrpcApiHttpPort = config.GetSection("ServerConfiguration").GetSection("GrpcApiHttpPort").Value,
                GrpcApiHttpsPort = config.GetSection("ServerConfiguration").GetSection("GrpcApiHttpsPort").Value
            };

            services.AddScoped<ServerConfiguration>(s => serverConfiguration);
            services.AddScoped<ServiceRouter, ServiceRouter>(s => ServiceRouter.GetInstance());

        }
    }
}
