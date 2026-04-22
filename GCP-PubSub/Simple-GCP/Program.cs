using Google.Cloud.PubSub.V1;

string? projectId = Environment.GetEnvironmentVariable("GCP_PROJECT_ID");
string? topicId = Environment.GetEnvironmentVariable("PUBSUB_TOPIC_ID");
string? subscriptionId = Environment.GetEnvironmentVariable("PUBSUB_SUBSCRIPTION_ID");

if (string.IsNullOrWhiteSpace(projectId))
{
    throw new InvalidOperationException("Set the GCP_PROJECT_ID environment variable.");
}

if (string.IsNullOrWhiteSpace(topicId))
{
    throw new InvalidOperationException("Set the PUBSUB_TOPIC_ID environment variable.");
}

if (string.IsNullOrWhiteSpace(subscriptionId))
{
    throw new InvalidOperationException("Set the PUBSUB_SUBSCRIPTION_ID environment variable.");
}

TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);
SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);

Console.WriteLine($"Project: {projectId}");
Console.WriteLine($"Topic: {topicId}");
Console.WriteLine($"Subscription: {subscriptionId}");

PublisherClient publisher = await PublisherClient.CreateAsync(topicName);
string payload = $"Hello from .NET 8 at {DateTimeOffset.UtcNow:O}";
string messageId = await publisher.PublishAsync(payload);
Console.WriteLine($"Published message ID: {messageId}");
await publisher.ShutdownAsync(TimeSpan.FromSeconds(15));

SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);
var receivedSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

Task subscriberTask = subscriber.StartAsync((message, cancellationToken) =>
{
    string text = message.Data.ToStringUtf8();
    Console.WriteLine($"Received message ID: {message.MessageId}");
    Console.WriteLine($"Received payload: {text}");

    receivedSignal.TrySetResult(true);
    return Task.FromResult(SubscriberClient.Reply.Ack);
});

using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
await using var registration = timeoutCts.Token.Register(() => receivedSignal.TrySetCanceled(timeoutCts.Token));

try
{
    await receivedSignal.Task;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Timed out waiting for a message.");
}
finally
{
    await subscriber.StopAsync(CancellationToken.None);
    await subscriberTask;
}
