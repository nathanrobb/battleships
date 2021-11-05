using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Api.Battleships
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			try
			{
				await CreateHostBuilder(args).Build().RunAsync();
			}
			finally
			{
				// Make sure to flush our logs on shutdown.
				await Task.WhenAll(
					Task.Run(() =>
					{
						NLog.LogManager.Flush(TimeSpan.FromSeconds(10));
						NLog.LogManager.Shutdown();
					}),
					Console.Out.FlushAsync(),
					Console.Error.FlushAsync()
				);
			} 
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
