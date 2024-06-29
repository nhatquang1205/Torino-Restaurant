using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TorinoRestaurant.API.Infrastructure.ActionResults;
using TorinoRestaurant.Application.Common.Models;
using TorinoRestaurant.Application.User.Models;
using TorinoRestaurant.Application.User.Queries;

namespace TorinoRestaurant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public sealed class UserControllers : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserControllers(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _mediator.Send(new GetUserDetailQuery(id));
            return Ok(user);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<UserEntity>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetUsersQuery request)
        {
            var users = await _mediator.Send(request);
            return Ok(users);
        }

        // [HttpPost]
        // [ProducesResponseType(typeof(CreatedResultEnvelope), StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        // public async Task<IActionResult> Post([FromBody] WeatheruserCreateDto user)
        // {
        //     var id = await _mediator.Send(new CreateWeatheruserCommand(user.TemperatureC, user.Date, user.Summary, user.LocationId));
        //     return CreatedAtAction(nameof(Get), new { id }, new CreatedResultEnvelope(id));
        // }

        // [HttpPut("{id}")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        // public async Task<IActionResult> Put(Guid id, [FromBody] WeatheruserUpdateDto user)
        // {
        //     await _mediator.Send(new UpdateWeatheruserCommand(id, user.Date));
        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> Delete(Guid id)
        // {
        //     await _mediator.Send(new DeleteWeatheruserCommand(id));
        //     return NoContent();
        // }
    }
}