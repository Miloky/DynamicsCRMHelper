using System;
using DynamicsHelper.Dynamics;
using DynamicsHelper.Interfaces;
using DynamicsHelper.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicsHelper
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        // TODO: Add cashing, webpack reloads too often and it consume too much time (Redis)
        // TODO: Add proxy for portal queries
        // TODO: Optimize performance
        // TODO: Crete webpack alternative(I'm too lazy to start this app every time I need make changes to this )

        public Startup(IConfiguration configuration) => _configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDynamicsTokenService, DynamicsTokenService>();
            var settingsName = _configuration.GetValue<string>("SettingsName");
            Console.WriteLine(settingsName);
            if(string.IsNullOrWhiteSpace(settingsName)){
                throw new Exception("Please, provide settings name!");
            }
            services.Configure<DynamicsSettings>(x => _configuration.GetSection($"DynamicsSettings:{settingsName}").Bind(x));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCaching();
            app.UseMiddleware<DynamicsApiProxyMiddleware>();
        }
    }
}
