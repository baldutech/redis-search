using MediatR;
using Microsoft.AspNetCore.Mvc;
using Redis.Search.Features.UseCases.GetFunds.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.Search.Controllers
{
    [ApiController]
    [Produces("application/json")]
    
    public class CacheController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CacheController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("funds-cache")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetFundsAsync(
            [FromQuery] CreateFundsCacheInput input,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(input, cancellationToken);

            return Ok();
        }
    }
}
