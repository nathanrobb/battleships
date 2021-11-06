using System;
using Api.Battleships.Services.Models;
using NUnit.Framework;

namespace Api.Battleships.Tests.Services.Models.Extensions
{
	[TestFixture]
	public class CoordinateExtensionsTests
	{
		[Test]
		public void GetManhattanDistance_NullFrom_ThrowsArgumentNullException()
		{
			// Arrange
			var to = new Coordinate(1, 1);

			// Act + Assert
			Assert.Throws<ArgumentNullException>(() => ((Coordinate)null).GetManhattanDistance(to));
		}

		[Test]
		public void GetManhattanDistance_NullTo_ThrowsArgumentNullException()
		{
			// Arrange
			var from = new Coordinate(1, 1);

			// Act + Assert
			Assert.Throws<ArgumentNullException>(() => from.GetManhattanDistance(null));
		}

		[Test]
		// Test same coordinate.
		[TestCase(1, 1, 1, 1, 0)]
		// Spec example.
		[TestCase(3, 5, 2, 7, 3)]
		// Test smaller from.
		[TestCase(1, 1, 2, 2, 2)]
		// Test smaller to.
		[TestCase(2, 2, 1, 1, 2)]
		// Max board range.
		[TestCase(1, 1, 8, 8, 14)]
		public void GetManhattanDistance_ValidCoordinates_ReturnsExpectedDistance(
			int fromRow, int fromColumn, int toRow, int toColumn, int expectedDistance)
		{
			// Arrange
			var from = new Coordinate(fromRow, fromColumn);
			var to = new Coordinate(toRow, toColumn);

			// Act
			var actual = from.GetManhattanDistance(to);

			// Assert
			Assert.AreEqual(expectedDistance, actual);
		}
	}
}
