using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdminServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddServices(services);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        private void AddServices(IServiceCollection services)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            AdminServerConfiguration configuration = new AdminServerConfiguration()
            {
                AdminServerIP = config.GetSection("AdminServerConfiguration").GetSection("AdminServerIP").Value,
                AdminServerHttpPort = config.GetSection("AdminServerConfiguration").GetSection("AdminServerHttpPort").Value,
                AdminServerHttpsPort = config.GetSection("AdminServerConfiguration").GetSection("AdminServerHttpsPort").Value,
                GrpcServerApiHttpPort = config.GetSection("AdminServerConfiguration").GetSection("GrpcServerApiHttpPort").Value,
                GrpcServerApiHttpsPort = config.GetSection("AdminServerConfiguration").GetSection("GrpcServerApiHttpsPort").Value,
                GrpcServerIP = config.GetSection("AdminServerConfiguration").GetSection("GrpcServerIP").Value
            };
            var channel = GrpcChannel.ForAddress($"http://{configuration.GrpcServerIP}:{configuration.GrpcServerApiHttpPort}");

            services.AddScoped<UserAdminService.UserAdminServiceClient>(t => new UserAdminService.UserAdminServiceClient(channel));
            services.AddScoped<GameAdminService.GameAdminServiceClient>(t => new GameAdminService.GameAdminServiceClient(channel));

        }
    }
}