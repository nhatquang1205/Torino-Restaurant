using MediatR;
using Microsoft.AspNetCore.Mvc;
using TorinoRestaurant.API.Infrastructure.ActionResults;
using TorinoRestaurant.Application.Authentication.Command;
using TorinoRestaurant.Application.Authentication.Models;

namespace TorinoRestaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthenticatedResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthenticatedResult>> LoginAsync([FromBody] LoginRequestEntity request)
        {
            return await _mediator.Send(new LoginCommand(request.Username, request.Password));
        }
    }
}