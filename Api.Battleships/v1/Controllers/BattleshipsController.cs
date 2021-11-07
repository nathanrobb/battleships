using System;
using System.Threading.Tasks;
using Api.Battleships.Services;
using Api.Battleships.Services.Models;
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
		private readonly IGameService _gameService;
		private readonly ILogger<BattleshipsController> _logger;

		public BattleshipsController(IGameService gameService, ILogger<BattleshipsController> logger)
		{
			_gameService = gameService;
			_logger = logger;
		}

		[AllowAnonymous]
		[HttpPost("new-game")]
		[ProducesResponseType(typeof(NewGameResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> PostNewGameAsync()
		{
			// TODO: take these as post data?
			const int boardSize = Constants.BOARD_SIZE;
			const int guesses = Constants.MAX_GUESSES;
			const int placedShipCount = Constants.PLACED_SHIP_COUNT;

			var game = await _gameService.CreateGameAsync(placedShipCount, boardSize, guesses);

			_logger.LogDebug($"New game started, gameId: {game.Id}");

			return Ok(new NewGameResponse
			{
				GameId = game.Id,
				BoardSize = game.BoardSize,
				GuessesRemaining = game.TotalGuesses,
				ShipsRemaining = game.TotalShips,
			});
		}

		[AllowAnonymous]
		[HttpPatch("{gameId:int}/fire-torpedo")]
		[ProducesResponseType(typeof(FireTorpedoResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> PatchFireTorpedoAsync(int gameId, [FromBody] FireTorpedoRequest request)
		{
			var game = await _gameService.GetGameAsync(gameId);
			if (game == null)
			{
				_logger.LogInformation($"Non-existent game id: {gameId}");
				return BadRequest("Must specify an existing game to fire a torpedo");
			}

			if (request == null)
			{
				_logger.LogInformation("Body not specified");
				return BadRequest("Must specify row and column in request body");
			}

			if (request.Row < 1 || request.Row > game.BoardSize)
			{
				_logger.LogInformation($"Invalid row: {request.Row}, board size: {game.BoardSize}");
				return BadRequest($"{nameof(request.Row)} must be in the range 1 - {game.BoardSize} (inclusive)");
			}

			if (request.Column < 1 || request.Column > game.BoardSize)
			{
				_logger.LogInformation($"Invalid column: {request.Column}, board size: {game.BoardSize}");
				return BadRequest($"{nameof(request.Column)} must be in the range 1 - {game.BoardSize} (inclusive)");
			}

			var coordinate = new Coordinate(request.Row, request.Column);

			// Verify the torpedo hasn't already been fired at the specified coordinate.
			var alreadyFiredTorpedo = await _gameService.TorpedoAlreadyFiredAsync(gameId, coordinate);
			if (alreadyFiredTorpedo)
			{
				_logger.LogInformation($"Already fired at the provided coordinates row: {coordinate.Row}, column: {coordinate.Column}");
				return BadRequest("Already fired there, try somewhere new");
			}

			_logger.LogDebug($"Firing torpedo at {coordinate.Row},{coordinate.Column}");

			var torpedoResult = await _gameService.FireTorpedoAsync(game.Id, coordinate);
			if (torpedoResult == null)
			{
				_logger.LogError($"Received no torpedo result from FireTorpedo, gameId {game.Id}, row: {coordinate.Row}, column: {coordinate.Column}");
				throw new Exception("Unexpected error");
			}

			_logger.LogDebug($"Fired torpedo at {request.Row},{request.Column}, distance: {torpedoResult.Distance}");

			return Ok(new FireTorpedoResponse
			{
				GuessesRemaining = torpedoResult.GuessesRemaining,
				ShipsRemaining = torpedoResult.ShipsRemaining,
				Distance = torpedoResult.Distance,
				ShipSunk = torpedoResult.ShipSunk,
			});
		}
	}
}
