using NATS.Client.JetStream;
using NATS.Net;

namespace Quantum.Nats.Consumer;

public class JetStreamService(ILogger<JetStreamService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Background service has started.");
        await using NatsClient nc = new();
        INatsJSContext js = nc.CreateJetStreamContext();

        await js.CreateStreamAsync(new(name: "SHOP_ORDERS", subjects: ["orders.>"]), stoppingToken);
        INatsJSConsumer consumer = await js.CreateOrUpdateConsumerAsync(
            stream: "SHOP_ORDERS",
            new("order_processor"),
            cancellationToken: stoppingToken);

        await foreach (NatsJSMsg<Order> msg in consumer.ConsumeAsync<Order>().WithCancellation(stoppingToken))
        {
            Order? order = msg.Data;
            logger.LogInformation($"Processing {msg.Subject} {order}...");
            await msg.AckAsync(cancellationToken: stoppingToken);
        }
    }
}

public record Order
{
    public int Id { get; init; }
}