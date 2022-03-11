using MediatR;
using Microsoft.AspNetCore.Mvc;
using Redis.Search.Features.UseCases.GetFunds.Models;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Redis.Search.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    
    public class FundsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FundsController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("funds")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetFundsAsync(
            [FromQuery] GetFundsInput input,
            CancellationToken cancellationToken)
        {
            if (!input.IsValid())
            {
                return BadRequest();
            }

            var result = await _mediator.Send(input, cancellationToken);

            if (result == null || !result.Any())
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}
