using Confluent.Kafka;
using FabTrace.Shared;
using FabTrace.Shared.Models;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// 1. Aspire Service Defaults (Health checks, etc.)
builder.AddServiceDefaults();

// 2. Configure Kafka Producer with the Custom AOT Serializer
builder.AddKafkaProducer<string, WaferTelemetry>("messaging", settings => {
	settings.ValueSerializer = new NativeWaferSerializer();
});

// 3. Link our Source-Generated JSON Context
builder.Services.Configure<JsonOptions>(options => {
	options.SerializerOptions.TypeInfoResolver = WaferJsonContext.Default;
});

var app = builder.Build();

app.MapDefaultEndpoints();

// --- YOUR TASK STARTS HERE ---
app.MapPost("/ingest", async (WaferTelemetry wafer, IProducer<string, WaferTelemetry> producer) => {
	// TODO: 
	// 1. Create a 'new Message<string, WaferTelemetry>'
	// 2. Assign 'Key' (wafer.WaferId) and 'Value' (wafer)
	// 3. await producer.ProduceAsync("wafer-data", yourMessage)
	// 4. return Results.Accepted()
});
// --- YOUR TASK ENDS HERE ---

app.Run();

/// <summary>
/// This class is required for Native AOT. 
/// It tells Kafka exactly how to turn a WaferTelemetry object into bytes 
/// without using reflection.
/// </summary>
public class NativeWaferSerializer : ISerializer<WaferTelemetry>
{
	public byte[] Serialize(WaferTelemetry data, SerializationContext context)
	{
		return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(
			data,
			WaferJsonContext.Default.WaferTelemetry
		);
	}
}