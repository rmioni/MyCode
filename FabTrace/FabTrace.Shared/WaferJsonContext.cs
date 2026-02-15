using System.Text.Json.Serialization;
using FabTrace.Shared.Models;

namespace FabTrace.Shared;

// The [JsonSerializable] attributes tell the compiler: 
// "Hey, I need you to build a high-performance, non-reflection 
//  JSON logic for these specific types at compile-time."

[JsonSerializable(typeof(WaferTelemetry))]
[JsonSerializable(typeof(List<WaferTelemetry>))]
public partial class WaferJsonContext : JsonSerializerContext
{
}
