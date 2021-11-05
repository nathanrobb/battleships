using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Api.Battleships.Tests.Helpers
{
	public static class ActionResultExtensions
	{
		/// <summary>
		/// Asserts this <see cref="IActionResult"/> is a BadRequest result and gets the response message.
		/// </summary>
		/// <param name="actionResult">The <see cref="IActionResult"/> to get the BadRequest response message from.</param>
		/// <returns>The response message of the request.</returns>
		public static string AssertBadRequestGetMessage(this IActionResult actionResult)
		{
			Assert.AreEqual(typeof(BadRequestObjectResult), actionResult.GetType());
			var okActionResult = (BadRequestObjectResult) actionResult;

			Assert.AreEqual(400, okActionResult.StatusCode);

			Assert.AreEqual(typeof(string), okActionResult.Value.GetType());
			return (string)okActionResult.Value;
		}

		/// <summary>
		/// Asserts this <see cref="IActionResult"/> is an OK result and gets the response body of type <see cref="T"/>.
		/// </summary>
		/// <typeparam name="T">The expected response body type</typeparam>
		/// <param name="actionResult">The <see cref="IActionResult"/> to get the OK response body from.</param>
		/// <returns>The response body of the request.</returns>
		public static T AssertOkGetValue<T>(this IActionResult actionResult)
		{
			Assert.AreEqual(typeof(OkObjectResult), actionResult.GetType());
			var okActionResult = (OkObjectResult) actionResult;

			Assert.AreEqual(200, okActionResult.StatusCode);

			Assert.AreEqual(typeof(T), okActionResult.Value.GetType());
			return (T)okActionResult.Value;
		}
	}
}
