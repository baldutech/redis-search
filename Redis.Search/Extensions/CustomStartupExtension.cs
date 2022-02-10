﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Redis.Search.Domain.Configuration;
using System;

namespace Redis.Search.Extensions
{
    internal static class CustomStartupExtension
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new()
                    {
                        Version = "v1",
                        Title = "::Api de Consulta Redis Search::"
                    });
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
               .AddRedis(connectionStringRedis.ConnectionStringRedis, name: "redis-search", tags: new[] { "readiness" }, timeout: TimeSpan.FromSeconds(5));

            return services;
        }

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ConnectionStringsOptions>(configuration.GetSection("ConnectionStrings"));

            return services;
        }
    }
}