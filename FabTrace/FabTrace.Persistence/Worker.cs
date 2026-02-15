using Confluent.Kafka;
using FabTrace.Persistence.Data;
using FabTrace.Shared.Models;

namespace FabTrace.Persistence;

public class Worker(
	ILogger<Worker> logger,
	IConsumer<string, WaferTelemetry> consumer,
	IServiceScopeFactory scopeFactory) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		consumer.Subscribe("wafer-data");

		while(!stoppingToken.IsCancellationRequested) {
			try {
				// 1. Wait for a message from Kafka
				var result = consumer.Consume(stoppingToken);
				if(result == null)
					continue;

				var telemetry = result.Message.Value;

				// 2. Open a "Scope" to talk to the Database
				using(var scope = scopeFactory.CreateScope()) {
					var dbContext = scope.ServiceProvider.GetRequiredService<WaferDbContext>();

					// --- YOUR TASK: MAPPING ---
					// TODO: Create a 'new WaferRecord' and copy data 
					// from the 'telemetry' object into it.

					var record = new WaferRecord {
						// Assign the values here!
						Timestamp = DateTime.UtcNow
					};

					// 3. Save to Postgres
					dbContext.Wafers.Add(record);
					await dbContext.SaveChangesAsync(stoppingToken);
				}

				logger.LogInformation("Saved Wafer {Id} to Database.", telemetry.WaferId);
			}
			catch(Exception ex) {
				logger.LogError(ex, "Failed to persist wafer data.");
			}
		}
	}
}