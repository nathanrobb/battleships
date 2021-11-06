using Api.Battleships.v1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Battleships.v1.Controllers
{
	[Route("v1/battleships")]
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
			// TODO: create game.
			const int gameId = 1;

			_logger.LogDebug($"New game started, gameId: {gameId}");

			return Ok(new NewGameResponse
			{
				GameId = gameId,
				BoardSize = Constants.BOARD_SIZE,
				GuessesRemaining = Constants.MAX_GUESSES,
				ShipsRemaining = Constants.PLACED_SHIP_COUNT,
			});
		}

		[AllowAnonymous]
		[HttpPatch("{gameId:int}/fire-torpedo")]
		[ProducesResponseType(typeof(FireTorpedoResponse), StatusCodes.Status200OK)]
		public IActionResult PatchFireTorpedo(int gameId, [FromBody] FireTorpedoRequest request)
		{
			// TODO: validate the game id.
			if (gameId <= 0)
			{
				_logger.LogInformation($"Invalid game id: {gameId}");
				return BadRequest("Must specify an existing game to fire a torpedo");
			}

			if (request == null)
			{
				_logger.LogInformation("Body not specified");
				return BadRequest("Must specify row and column in request body");
			}

			if (request.Row < 1 || request.Row > Constants.BOARD_SIZE)
			{
				_logger.LogInformation($"Invalid row: {request.Row}, board size: {Constants.BOARD_SIZE}");
				return BadRequest($"{nameof(request.Row)} must be in the range 1 - {Constants.BOARD_SIZE} (inclusive)");
			}

			if (request.Column < 1 || request.Column > Constants.BOARD_SIZE)
			{
				_logger.LogInformation($"Invalid column: {request.Column}, board size: {Constants.BOARD_SIZE}");
				return BadRequest($"{nameof(request.Column)} must be in the range 1 - {Constants.BOARD_SIZE} (inclusive)");
			}

			_logger.LogDebug($"Fired torpedo at {request.Row},{request.Column}");

			// TODO: actually fire the torpedo

			return Ok(new FireTorpedoResponse
			{
				GuessesRemaining = Constants.MAX_GUESSES,
				ShipsRemaining = Constants.PLACED_SHIP_COUNT,
				Distance = 4,
				ShipSunk = false,
			});
		}
	}
}
