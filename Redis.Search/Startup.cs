using Autofac;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redis.Search.Extensions;
using System.Text.Json.Serialization;

namespace Redis.Search
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            services
                .AddCustomSwagger()
                .AddMemoryCacheConfiguration()
                .AddCustomHealthCheck(Configuration)
                .AddCustomConfiguration(Configuration);
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory logger)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseResponseCompression();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI(config => config.UIPath = "/health-check-ui");
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("liveness")
                });
                endpoints.MapHealthChecks("/readiness", new HealthCheckOptions
                {
                    Predicate = _ => _.Tags.Contains("readiness"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
