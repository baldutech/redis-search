using Autofac;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Redis.Search.Extensions;
using Redis.Search.Shared.Filters;
using Redis.Search.Shared.Modules;
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
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            services
                .AddCustomSwagger()
                .AddMediatR(typeof(Startup))
                .AddMemoryCacheConfiguration()
                .AddCustomHealthCheck(Configuration)
                .AddCustomConfiguration(Configuration);
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ModuleApplication());
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
                endpoints.MapHealthChecks("/health-check-database", new HealthCheckOptions
                {
                    Predicate = _ => _.Tags.Contains("database"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
