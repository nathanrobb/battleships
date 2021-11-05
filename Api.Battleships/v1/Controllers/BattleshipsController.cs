using Api.Battleships.v1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Battleships.v1.Controllers
{
	[Route("v1/[controller]")]
	[Authorize]
	[ApiController]
	public class BattleshipsController : ControllerBase
	{
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
	}
}
