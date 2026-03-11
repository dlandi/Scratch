using Microsoft.Extensions.Logging;

namespace GCP.PubSub;

internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Published message {MessageId}")]
    internal static partial void PublishedMessage(this ILogger logger, string messageId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to publish message")]
    internal static partial void PublishFailed(this ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Information, Message = "Received message {MessageId}: {Text}")]
    internal static partial void ReceivedMessage(this ILogger logger, string messageId, string text);

    [LoggerMessage(Level = LogLevel.Information, Message = "Subscriber started, listening for {Duration}")]
    internal static partial void SubscriberStarted(this ILogger logger, TimeSpan duration);

    [LoggerMessage(Level = LogLevel.Information, Message = "Subscriber stopped, received {Count} messages")]
    internal static partial void SubscriberStopped(this ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "Listed {Count} topics")]
    internal static partial void ListedTopics(this ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "Listed {Count} subscriptions")]
    internal static partial void ListedSubscriptions(this ILogger logger, int count);
}
