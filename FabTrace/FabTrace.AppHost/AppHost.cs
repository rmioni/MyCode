var builder = DistributedApplication.CreateBuilder(args);

// 1. Define the Database (Postgres)
// This will automatically pull the Postgres Docker image and start it
var postgres = builder.AddPostgres("postgres")
					  .WithDataBindMount("./data/postgres") // Keeps your data even if you restart
					  .AddDatabase("telemetry-db");

// 2. Define the Message Broker (Kafka)
// This starts Kafka and a helpful UI to see your messages in the browser
var kafka = builder.AddKafka("messaging")
				   .WithKafkaUI();

// 3. Define the Gateway (Ingestion)
// We tell the Gateway where Kafka is
var gateway = builder.AddProject<Projects.FabTrace_Gateway>("gateway")
					 .WithReference(kafka);

// 4. Define the Persistence Service (The Archiver)
// We tell it where to find both Kafka (to read) and Postgres (to write)
builder.AddProject<Projects.FabTrace_Persistence>("persistence")
	   .WithReference(kafka)
	   .WithReference(postgres);

// 5. Define the Simulator (The Factory Floor)
// We tell it where the Gateway is so it can start sending data
builder.AddProject<Projects.FabTrace_Simulator>("simulator")
	   .WithReference(gateway);

builder.Build().Run();
