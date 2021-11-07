using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Battleships.Database;
using Api.Battleships.Database.Models;
using Api.Battleships.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Battleships.Services
{
	public interface IGameService
	{
		/// <summary>
		/// Returns the game with the given id, or null if a game with the provided <see cref="gameId"/> does not exist.
		/// </summary>
		Task<Game> GetGameAsync(int gameId);
		/// <summary>
		/// Creates a new battleships game and saves it to the database.
		/// </summary>
		/// <returns>The created game's id.</returns>
		/// <remarks>Calls save changes internally.</remarks>
		Task<Game> CreateGameAsync(int shipCount, int boardSize, int guesses);
		/// <summary>
		/// Returns true if a torpedo has already been fired at the provided coordinate.
		/// </summary>
		Task<bool> TorpedoAlreadyFiredAsync(int gameId, Coordinate torpedoCoordinate);
		/// <summary>
		/// Fires a torpedo on the board.
		/// </summary>
		/// <param name="gameId">The game to fire the torpedo in.</param>
		/// <param name="torpedoCoordinate">The coordinate to fire the torpedo at.</param>
		/// <returns>The result of firing the torpedo.</returns>
		Task<TorpedoResult> FireTorpedoAsync(int gameId, Coordinate torpedoCoordinate);
	}

	public class GameService : IGameService
	{
		private readonly BattleshipsContext _battleshipsContext;
		private readonly ShipPlacerService _shipPlacerService;
		private readonly ShipDistanceService _shipDistanceService;
		private readonly ILogger<GameService> _logger;

		public GameService(
			BattleshipsContext battleshipsContext,
			ShipPlacerService shipPlacerService,
			ShipDistanceService shipDistanceService,
			ILogger<GameService> logger)
		{
			_battleshipsContext = battleshipsContext;
			_shipPlacerService = shipPlacerService;
			_shipDistanceService = shipDistanceService;
			_logger = logger;
		}

		public async Task<Game> GetGameAsync(int gameId)
		{
			if (gameId <= 0)
				return null;

			return await _battleshipsContext.Games.FindAsync(gameId);
		}

		public async Task<Game> CreateGameAsync(int shipCount, int boardSize, int guesses)
		{
			var shipPlacements = _shipPlacerService.GetShipPlacements(shipCount, boardSize);

			var game = CreateGame(boardSize, guesses, shipCount, shipPlacements);

			await _battleshipsContext.SaveChangesAsync();

			return game;
		}

		public async Task<bool> TorpedoAlreadyFiredAsync(int gameId, Coordinate torpedoCoordinate)
		{
			return await _battleshipsContext.Torpedoes
				.Where(t => t.GameId == gameId)
				.AnyAsync(t => t.Row == torpedoCoordinate.Row && t.Column == torpedoCoordinate.Column);
		}

		public async Task<TorpedoResult> FireTorpedoAsync(int gameId, Coordinate torpedoCoordinate)
		{
			var game = await _battleshipsContext.Games
				.SingleOrDefaultAsync(g => g.Id == gameId);

			if (game == null)
				throw new ArgumentNullException(nameof(game));

			var guessCount = await _battleshipsContext.Torpedoes
				.Where(t => t.GameId == gameId)
				.CountAsync();

			var remainingShipCells = await _battleshipsContext.ShipCells
				.Where(c => c.Ship.GameId == gameId)
				.Where(c => c.TorpedoId == null)
				.OrderBy(c => c.Id)
				.ToListAsync();

			// Already won...
			if (remainingShipCells.Count == 0)
			{
				return new TorpedoResult
				{
					Distance = -1,
					ShipSunk = false,
					GuessesRemaining = game.TotalGuesses - guessCount,
					ShipsRemaining = 0,
				};
			}

			// Add the new torpedo to the games fired torpedoes.
			guessCount++;
			var torpedo = new Torpedo
			{
				Row = torpedoCoordinate.Row,
				Column = torpedoCoordinate.Column,

				Game = game,
			};

			_battleshipsContext.Torpedoes.Add(torpedo);

			var minDistance =_shipDistanceService.GetClosestShipCell(torpedoCoordinate, remainingShipCells);

			// 0 distance means the torpedo hit the ship.
			var shipSunk = false;
			if (minDistance.Distance == 0)
			{
				minDistance.ShipCell.HitByTorpedo = torpedo;

				// This cell is no longer 'remaining', remove it from the list.
				remainingShipCells.Remove(minDistance.ShipCell);

				shipSunk = remainingShipCells.All(c => c.ShipId != minDistance.ShipCell.ShipId);
			}

			await _battleshipsContext.SaveChangesAsync();

			return new TorpedoResult
			{
				Distance = minDistance.Distance,
				ShipSunk = shipSunk,
				GuessesRemaining = game.TotalGuesses - guessCount,
				ShipsRemaining = remainingShipCells.Select(c => c.ShipId).Distinct().Count(),
			};
		}

		public Game CreateGame(int boardSize, int guesses, int shipCount, IEnumerable<ShipPlacement> shipPlacements)
		{
			var game = new Game
			{
				BoardSize = boardSize,
				TotalGuesses = guesses,
				TotalShips = shipCount,
			};

			_battleshipsContext.Games.Add(game);

			foreach (var placement in shipPlacements)
			{
				var ship = new Ship
				{
					Game = game,
				};

				_battleshipsContext.Ships.Add(ship);

				foreach (var coordinate in placement.Coordinates)
				{
					var cell = new ShipCell
					{
						Row = coordinate.Row,
						Column = coordinate.Column,

						Ship = ship,
					};
					
					_battleshipsContext.ShipCells.Add(cell);
				}

				_logger.LogDebug($"Placing ship at ({string.Join(")(", ship.ShipCells.Select(c => $"{c.Row},{c.Column}"))})");
			}

			return game;
		}
	}
}
