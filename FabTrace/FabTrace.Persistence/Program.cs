using FabTrace.Persistence;
using FabTrace.Persistence.Data;
using FabTrace.Shared.Models;
using Confluent.Kafka;

var builder = Host.CreateApplicationBuilder(args);

// 1. Aspire Service Defaults
builder.AddServiceDefaults();

// 2. Add Postgres (EF Core)
builder.AddNpgsqlDbContext<WaferDbContext>("database");

// 3. Add Kafka Consumer
// Note: We need a 'GroupId' so Kafka knows who is reading the data
builder.AddKafkaConsumer<string, WaferTelemetry>("messaging", settings => {
	settings.Config.GroupId = "persistence-worker-group";
	settings.Config.AutoOffsetReset = AutoOffsetReset.Earliest;
	// We use a built-in JSON deserializer for now, but will 
	// eventually need an AOT one if we go Native.
});

// 4. Register our Background Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
