using System;

namespace Api.Battleships.Services
{
	public interface IRandomGenerator
	{
		bool GetRandomBool();
		int GetRandomIntBetween(int min, int max);
	}

	public class RandomGenerator : IRandomGenerator
	{
		private readonly Random _random;

		public RandomGenerator(Random random)
		{
			_random = random;
		}

		public bool GetRandomBool()
		{
			return _random.NextDouble() >= 0.5;
		}

		public int GetRandomIntBetween(int min, int max)
		{
			return _random.Next(min, max);
		}
	}
}
