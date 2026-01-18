using Quantum.Nats.Publisher;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<PublisherService>();

WebApplication app = builder.Build();

app.MapGet("/ping", () => "pong");

app.Run();