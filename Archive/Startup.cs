using Archive.Logs;
using Archive.Services;
using HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoConsumerLibary.MongoConnection;
using MongoConsumerLibary.MongoConnection.Collections;
using MongoConsumerLibary.MongoConnection.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Archive", Version = "v1" });
            });
            StartSingleTones(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Archive v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public void StartSingleTones(IServiceCollection services)
        {
            MongoSettings mongoSettings = Configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
            HealthCheckSettings healthCheckSettings = Configuration.GetSection(nameof(HealthCheckSettings)).Get<HealthCheckSettings>();
            services.AddSingleton(mongoSettings);
            services.AddSingleton(new ArchiveLogger());
            services.AddSingleton<MongoConnection>();
            services.AddSingleton(new ZlibCompression());
            services.AddSingleton<IFrameService, FrameService>();
            services.AddSingleton(new PointReducer());
            HealthCheckEndPoint healthCheck = new HealthCheckEndPoint();
            Task.Run(() => { healthCheck.StartUp(healthCheckSettings); });
            // force start mongoconnection on start
            using (IServiceScope scope = services.BuildServiceProvider().CreateScope()) 
            {
                ArchiveLogger logger = scope.ServiceProvider.GetRequiredService<ArchiveLogger>();
                MongoConnection mongoConnection = scope.ServiceProvider.GetRequiredService<MongoConnection>();
            }
        }
    }
}
