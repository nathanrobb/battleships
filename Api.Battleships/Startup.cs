using System;
using Api.Battleships.Database;
using Api.Battleships.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Api.Battleships
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder
					.ClearProviders()
					.SetMinimumLevel(LogLevel.Trace)
					.AddNLog(Configuration);
			});

			// Setup database services.

			// Singleton to keep our in-memory SQLite db connection established while the API is running otherwise our db is wiped on dispose.
			services.AddSingleton<InMemoryDbConnection>();
			services.AddDbContextFactory<BattleshipsContext, InMemoryBattleshipsContextFactory>(lifetime: ServiceLifetime.Scoped);
			services.AddScoped(s =>
			{
				var context = s.GetRequiredService<IDbContextFactory<BattleshipsContext>>().CreateDbContext();
				context.Database.EnsureCreated();
				return context;
			});

			// Setup business logic services.

			services.AddScoped<Random>();
			services.AddScoped<IRandomGenerator, RandomGenerator>();
			services.AddScoped<ShipDistanceService>();
			services.AddScoped<ShipPlacerService>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
