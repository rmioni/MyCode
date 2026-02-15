namespace FabTrace.Shared.Models;

public record WaferTelemetry(
	string MachineId,     // Which machine sent it?
	string WaferId,       // Unique ID of the wafer
	double Temperature,   // Degrees Celsius
	double Pressure,      // Bar/PSI
	DateTime Timestamp    // When was it measured?
);