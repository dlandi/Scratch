using Google.Cloud.PubSub.V1;
using System.Text.Json;

// 1. Load appsettings.json from the app's output directory.
JsonElement appSettings = LoadAppSettings();

// 1. Ensure Google credentials are available before creating any Pub/Sub clients.
EnsureGoogleApplicationCredentials(appSettings);

// 2. Read required Pub/Sub settings from environment variables or appsettings.json.
string projectId = GetRequiredSetting(appSettings, "GCP_PROJECT_ID", "GoogleCloud", "ProjectId");
string topicId = GetRequiredSetting(appSettings, "PUBSUB_TOPIC_ID", "PubSub", "TopicId");
string subscriptionId = GetRequiredSetting(appSettings, "PUBSUB_SUBSCRIPTION_ID", "PubSub", "SubscriptionId");

// 3. Fail fast if any required setting is missing.
// 3. Build strongly typed topic and subscription names for the Google client library.
TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);
SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);

// 4. Echo the resolved configuration to the console.
Console.WriteLine($"Project: {projectId}");
Console.WriteLine($"Topic: {topicId}");
Console.WriteLine($"Subscription: {subscriptionId}");

// 5. Create a publisher, publish one message, then shut the publisher down cleanly.
//PublisherClient publisher = await PublisherClient.CreateAsync(topicName);
//string payload = $"Hello from .NET 8 at {DateTimeOffset.UtcNow:O}";
//string messageId = await publisher.PublishAsync(payload);
//Console.WriteLine($"Published message ID: {messageId}");
//await publisher.ShutdownAsync(TimeSpan.FromSeconds(15));

// 6. Create a subscriber and a signal that completes when one message is received.
SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);
var receivedSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

// 7. Start the subscriber callback that prints and acknowledges the first received message.
Task subscriberTask = subscriber.StartAsync((message, cancellationToken) =>
{
    string text = message.Data.ToStringUtf8();
    Console.WriteLine($"Received message ID: {message.MessageId}");
    Console.WriteLine($"Received payload: {text}");

    receivedSignal.TrySetResult(true);
    return Task.FromResult(SubscriberClient.Reply.Ack);
});

// 8. Wait up to 30 seconds for the message to arrive.
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
    // 9. Stop the subscriber and wait for its background task to finish.
    await subscriber.StopAsync(CancellationToken.None);
    await subscriberTask;
}

static JsonElement LoadAppSettings()
{
    string settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
    if (!File.Exists(settingsPath))
    {
        throw new InvalidOperationException($"Provide appsettings.json in {AppContext.BaseDirectory}.");
    }

    using FileStream settingsStream = File.OpenRead(settingsPath);
    using JsonDocument settings = JsonDocument.Parse(settingsStream);

    return settings.RootElement.Clone();
}

static void EnsureGoogleApplicationCredentials(JsonElement appSettings)
{
    const string credentialVariableName = "GOOGLE_APPLICATION_CREDENTIALS";

    // If the environment variable already exists, keep using it.
    string? credentialPath = Environment.GetEnvironmentVariable(credentialVariableName);
    if (!string.IsNullOrWhiteSpace(credentialPath))
    {
        return;
    }

    // Read the configured credential file path from GoogleCloud:GoogleApplicationCredentials.
    if (!TryGetString(appSettings, out string? configuredPath, "GoogleCloud", "GoogleApplicationCredentials"))
    {
        throw new InvalidOperationException($"Set the {credentialVariableName} environment variable or configure GoogleCloud:GoogleApplicationCredentials in appsettings.json.");
    }

    if (string.IsNullOrWhiteSpace(configuredPath))
    {
        throw new InvalidOperationException("Configure a non-empty GoogleCloud:GoogleApplicationCredentials value in appsettings.json.");
    }

    string resolvedPath = Path.IsPathRooted(configuredPath)
        ? configuredPath
        : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuredPath));

    // Validate the configured file and publish it into the process environment.
    if (!File.Exists(resolvedPath))
    {
        throw new InvalidOperationException($"The configured Google application credentials file was not found: {resolvedPath}");
    }

    Environment.SetEnvironmentVariable(credentialVariableName, resolvedPath);
}

static string GetRequiredSetting(JsonElement appSettings, string environmentVariableName, params string[] settingPath)
{
    string? environmentValue = Environment.GetEnvironmentVariable(environmentVariableName);
    if (!string.IsNullOrWhiteSpace(environmentValue))
    {
        return environmentValue;
    }

    if (TryGetString(appSettings, out string? configuredValue, settingPath) && !string.IsNullOrWhiteSpace(configuredValue))
    {
        return configuredValue;
    }

    throw new InvalidOperationException($"Set the {environmentVariableName} environment variable or configure {string.Join(':', settingPath)} in appsettings.json.");
}

static bool TryGetString(JsonElement element, out string? value, params string[] propertyPath)
{
    JsonElement current = element;
    foreach (string propertyName in propertyPath)
    {
        if (!current.TryGetProperty(propertyName, out current))
        {
            value = null;
            return false;
        }
    }

    value = current.GetString();
    return true;
}
