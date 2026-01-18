using Quantum.Nats.Consumer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<JetStreamService>();

var app = builder.Build();

app.MapGet("/ping", () => "pong");

app.Run();