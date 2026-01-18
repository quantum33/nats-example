using NATS.Client.JetStream;
using NATS.Net;

namespace Quantum.Nats.Publisher;

public class PublisherService(ILogger<PublisherService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            await using NatsClient nc = new NatsClient();
            INatsJSContext js = nc.CreateJetStreamContext();

            // Publish new order messages
            // Notice we're using JetStream context to publish and receive ACKs
            logger.LogInformation($"Publishing order {i}...");
            var ack = await js.PublishAsync($"orders.new.{i}", new Order { Id = i }, cancellationToken: stoppingToken);
            ack.EnsureSuccess();

            logger.LogInformation($"DONE order {i}...");
            i++;
            await Task.Delay(1000, stoppingToken);
        }
    }
}

public record Order
{
    public int Id { get; init; }
}

