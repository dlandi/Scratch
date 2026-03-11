using GCP.PubSub;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddGcpPubSub(); // binds "PubSub" section from appsettings.json
    })
    .Build();

await host.StartAsync();

var admin = host.Services.GetRequiredService<IPubSubAdmin>();
var publisher = host.Services.GetRequiredService<IPubSubPublisher>();
var subscriber = host.Services.GetRequiredService<IPubSubSubscriber>();

// List available topics and subscriptions
Console.WriteLine("Available topics:");
var topics = await admin.ListTopicsAsync();
foreach (var topic in topics)
    Console.WriteLine($"  {topic}");

Console.WriteLine("\nAvailable subscriptions:");
var subscriptions = await admin.ListSubscriptionsAsync();
foreach (var sub in subscriptions)
    Console.WriteLine($"  {sub}");

// Publish a single message
Console.WriteLine("Publishing a single message...");
var messageId = await publisher.PublishAsync("Hello from Console!");
Console.WriteLine($"Published message ID: {messageId}");

// Publish a batch
Console.WriteLine("\nPublishing a batch of messages...");
var messages = new[] { "Batch message 1", "Batch message 2", "Batch message 3" };
var count = await publisher.PublishBatchAsync(messages);
Console.WriteLine($"Published {count} messages.");

// Subscribe
Console.WriteLine("\nListening for messages (10 seconds)...");
var received = await subscriber.PullMessagesAsync(
    async (text, attributes, ct) =>
    {
        Console.WriteLine($"  Received: {text}");
        return true; // ACK
    },
    TimeSpan.FromSeconds(10));

Console.WriteLine($"Received {received} messages total.");
await host.StopAsync();
