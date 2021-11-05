using Api.Battleships.v1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Battleships.v1.Controllers
{
	[Route("v1/[controller]")]
	[Authorize]
	[ApiController]
	public class BattleshipsController : ControllerBase
	{
		private readonly ILogger<BattleshipsController> _logger;

		public BattleshipsController(ILogger<BattleshipsController> logger)
		{
			_logger = logger;
		}

		[AllowAnonymous]
		[HttpPost("new-game")]
		[ProducesResponseType(typeof(NewGameResponse), StatusCodes.Status200OK)]
		public IActionResult PostNewGame()
		{
			// TODO: actually start a new game

			return Ok(new NewGameResponse
			{
				Guesses = 20,
				Ships = 2,
			});
		}

		[AllowAnonymous]
		[HttpPatch("fire-torpedo")]
		[ProducesResponseType(typeof(FireTorpedoResponse), StatusCodes.Status200OK)]
		public IActionResult PatchFireTorpedo([FromBody] FireTorpedoRequest request)
		{
			if (request == null || string.IsNullOrWhiteSpace(request.Coordinate))
				return BadRequest("Must specify coordinates in request body");

			_logger.LogDebug($"Fired torpedo at {request.Coordinate}");

			// TODO: actually fire the torpedo

			return Ok(new FireTorpedoResponse
			{
				GuessesRemaining = 20,
				ShipsRemaining = 2,
				ShipProximity = "",
				ShipSunk = false,
			});
		}
	}
}
