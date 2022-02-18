using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Redis.Search.Properties;
using Redis.Search.Shared.Domain.Funds;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.Search.Workers
{
    public class CreateFundsCacheWorker : BackgroundService
    {
        private readonly ILogger<CreateFundsCacheWorker> _logger;

        public CreateFundsCacheWorker(
            ILogger<CreateFundsCacheWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var ff = JsonSerializer.Deserialize<IEnumerable<Fund>>(Resources.funds);


                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Erro durante execução do worker!");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
