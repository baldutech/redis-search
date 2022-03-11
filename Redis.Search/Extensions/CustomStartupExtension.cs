using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Redis.Search.Domain.Configuration;
using Redis.Search.Extensions.Swagger;
using System;

namespace Redis.Search.Extensions
{
    internal static class CustomStartupExtension
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services
                .AddSwaggerGen();

            services.AddApiVersioning(o =>
            {
                o.UseApiBehavior = false;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;

                o.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddMemoryCacheConfiguration(this IServiceCollection services)
        {
            services
                .AddResponseCaching()
                .AddResponseCompression(options =>
                {
                    options.Providers.Add<BrotliCompressionProvider>();
                });

            return services;
        }

        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecksUI(setup =>
                {
                    setup.SetEvaluationTimeInSeconds(10);
                    setup.MaximumHistoryEntriesPerEndpoint(50);
                    setup.SetMinimumSecondsBetweenFailureNotifications(600);
                })
                .AddInMemoryStorage();

            var hcBuilder = services.AddHealthChecks();

            hcBuilder
                .AddCheck("liveness", () => HealthCheckResult.Healthy());

            var connectionStringRedis = configuration.GetSection("ConnectionStrings").Get<ConnectionStringsOptions>();

            hcBuilder
               .AddRedis(connectionStringRedis.ConnectionStringRedis, name: "redis-search", tags: new[] { "readiness", "database" }, timeout: TimeSpan.FromSeconds(5));

            return services;
        }

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.Configure<ConnectionStringsOptions>(configuration.GetSection("ConnectionStrings"));

            return services;
        }
    }
}
