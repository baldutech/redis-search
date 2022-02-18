using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Redis.Search.Features.UseCases.GetFunds.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.Search.Workers
{
    public class CreateFundsCacheWorker : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateFundsCacheWorker> _logger;

        public CreateFundsCacheWorker(
            IMediator mediator,
            ILogger<CreateFundsCacheWorker> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _mediator.Send(new CreateFundsCacheInput(), stoppingToken);
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
